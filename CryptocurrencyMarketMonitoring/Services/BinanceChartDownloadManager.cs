using Binance.Net.Enums;
using Binance.Net.Interfaces;
using CoinGecko.Interfaces;
using CryptocurrencyMarketMonitoring.Abstractions;
using CryptocurrencyMarketMonitoring.Abstractions.Managers;
using CryptocurrencyMarketMonitoring.Abstractions.Units;
using CryptocurrencyMarketMonitoring.Model.Documents;
using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Services
{
    public class BinanceChartDownloadManager : BackgroundService, IBinanceChartDownloadManager
    {

        public BinanceChartDownloadManager(ICoinGeckoClient coinGeckoClient, IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
            _coinGeckoClient = coinGeckoClient;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var supportedCurrencies = await _coinGeckoClient.CoinsClient.GetCoinList();
            var supportedVsCurrencies = await _coinGeckoClient.SimpleClient.GetSupportedVsCurrencies();

            _supportedCurrencies = supportedCurrencies.Where(x => _allowedCurrencies.Contains(x.Symbol.ToUpperInvariant())).Select(x => x.Symbol.ToUpperInvariant());
            _supportedVsCurrencies = supportedVsCurrencies.Where(x => _allowedVsCurrencies.Contains(x.ToUpperInvariant())).Select(x => x.ToUpperInvariant());

            using (var unit = DIContainer.BeginScopeService<IBinanceLastDownloadedPairUnit<BinanceLastDownloadedPair>>())
            {
                var allLastPairs = await unit.GetAllAsync(x => true);

                var allPossiblePairs = new List<string>();

                foreach (var supportedCurrency in _supportedCurrencies)
                {
                    foreach (var supportedVsCurrency in _supportedVsCurrencies)
                    {
                        foreach (var allowedInterval in _allowedIntervalTypes)
                        {
                            var pair = $"{supportedCurrency}{supportedVsCurrency}";
                            var dictionaryKey = $"{pair}{allowedInterval}";

                            var savedPair = allLastPairs.FirstOrDefault(x => x.Pair == pair && x.Interval == allowedInterval);

                            if (savedPair == null)
                            {
                                var newPair = new BinanceLastDownloadedPair()
                                {
                                    Id = ObjectId.GenerateNewId(),
                                    Pair = pair,
                                    Interval = allowedInterval,
                                    Timestamp = DateTime.UnixEpoch.ToUniversalTime()
                                };

                                await unit.CreateAsync(newPair);

                                _lastDownloadedStamps[dictionaryKey] = newPair;
                            }
                            else
                            {
                                _lastDownloadedStamps[dictionaryKey] = savedPair;
                            }
                        }

                    }
                }
            }

            _executingTask = ExecuteAsync(_stoppingCts.Token);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
                return;

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tasks = new List<Task>();
                foreach (var supportedCurrency in _supportedCurrencies)
                {
                    foreach (var supportedVsCurrency in _supportedVsCurrencies)
                    {
                        foreach (var allowedIntervalType in _allowedIntervalTypes)
                        {
                            var task = DownloadDataAsync(supportedCurrency, supportedVsCurrency, allowedIntervalType, stoppingToken);
                            tasks.Add(task);
                        }
                    }
                }

                await Task.WhenAll(tasks);

                await Task.Delay(_downloadCyclePeriod, stoppingToken);
            }
        }

        private async Task DownloadDataAsync(string currency, string vsCurrency, IntervalType intervalType, CancellationToken stoppingToken)
        {
            if (_lastDownloadedStamps.TryGetValue($"{currency}{vsCurrency}{intervalType}", out var lastStamp))
            {
                //hack for binance - uses USDT instead of USD
                var binanceVsCurrency = vsCurrency.Replace("USD", "USDT");

                var klines = await _binanceClient.Spot.Market.GetKlinesAsync($"{currency}{binanceVsCurrency}", (KlineInterval)intervalType, startTime: lastStamp.Timestamp, limit: 1000, ct: stoppingToken);

                if (klines.Data != null && klines.Data.Any())
                {
                    using (var unit = DIContainer.BeginScopeService<IBinanceChartDataUnit<BinanceChartData>>())
                    {
                        await unit.CreateManyAsync(klines.Data, currency, vsCurrency, intervalType);
                    }

                    using (var unit = DIContainer.BeginScopeService<IBinanceLastDownloadedPairUnit<BinanceLastDownloadedPair>>())
                    {
                        lastStamp.Timestamp = klines.Data.LastOrDefault().CloseTime;
                        await unit.UpdateAsync(lastStamp.Id.ToString(), lastStamp);
                    }
                }
            }

        }

        private readonly IBinanceClient _binanceClient;
        private readonly ICoinGeckoClient _coinGeckoClient;
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new();
        private IEnumerable<string> _supportedCurrencies;
        private IEnumerable<string> _supportedVsCurrencies;
        private int _downloadCyclePeriod = 60000;
        private readonly string[] _allowedCurrencies = { "BTC", "ETH" };
        private readonly string[] _allowedVsCurrencies = { "USD" };
        private readonly IntervalType[] _allowedIntervalTypes = { IntervalType.one_hour, IntervalType.two_hours, IntervalType.four_hours, IntervalType.twelve_hours, IntervalType.one_day, IntervalType.one_month };
        private ConcurrentDictionary<string, BinanceLastDownloadedPair> _lastDownloadedStamps = new();

    }
}
