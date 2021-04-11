using Binance.Net;
using CryptocurrencyMarketMonitoring.Shared;
using CryptoExchange.Net.RateLimiter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinGecko.Clients;
using CoinGecko.Interfaces;
using System.Collections.Concurrent;
using CryptocurrencyMarketMonitoring.Abstractions.Managers;
using Microsoft.AspNetCore.SignalR;
using CryptocurrencyMarketMonitoring.SignalR.Hubs;
using Binance.Net.Interfaces;
using System.Threading;
using Force.DeepCloner;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Extensions.Hosting;

namespace CryptocurrencyMarketMonitoring.Services
{

    public class CryptocurrencyOverviewManager : BackgroundService, ICryptocurrencyOverviewManager
    {

        public CryptocurrencyOverviewManager(IHubContext<CryptocurrencyOverviewUpdateHub, ICryptocurrencyOverviewUpdateHub> updateHub, ICoinGeckoClient coinGeckoClient, IBinanceClient binanceClient)
        {
            _updateHub = updateHub;
            _binanceClient = binanceClient;
            _coinGeckoClient = coinGeckoClient;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await UpdateDataAsync();

            _waitHandle.Set();

            _executingTask = ExecuteAsync(_stoppingCts.Token);

            return;
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


        public async Task<IEnumerable<CryptocurrencyOverviewDto>> GetCryptocurrencyOverviewAllAsync()
        {
            _waitHandle.WaitOne();

            return _currentCryptocurrencyOverviewData.Values.OrderBy(x => x.Ranking).ToList();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateDataAsync();

                await Task.Delay(_updateCyclePeriod, stoppingToken);
            }
        }

        private async Task UpdateDataAsync()
        {
            var coinMarkets = await _coinGeckoClient.CoinsClient.GetCoinMarkets("usd", Array.Empty<string>(), "market_cap_desc", 250, 1, true, "7d,24h", null);

            var oldValues = _currentCryptocurrencyOverviewData.Values.Select(x => x.DeepClone()).ToList();
            var newValues = new List<CryptocurrencyOverviewDto>();
            foreach (var coinMarket in coinMarkets)
            {
                var cryptocurrency = new CryptocurrencyOverviewDto()
                {
                    Ranking = coinMarket.MarketCapRank ?? 0,
                    Name = coinMarket.Name,
                    Ticker = coinMarket.Symbol.ToUpper(),
                    LastDayPercentageMovement = coinMarket.PriceChangePercentage24HInCurrency / 100 ?? 0,
                    LastWeekPercentageMovement = coinMarket.PriceChangePercentage7DInCurrency / 100 ?? 0,
                    PriceUSD = coinMarket.CurrentPrice ?? 0,
                    VolumeUSD = coinMarket.TotalVolume ?? 0,
                    MarketCapUSD = coinMarket.MarketCap ?? 0,
                    IconSrc = coinMarket.Image
                };



                _currentCryptocurrencyOverviewData.AddOrUpdate(cryptocurrency.Ticker, cryptocurrency, (key, value) => cryptocurrency);
                newValues.Add(cryptocurrency);
            }

            foreach (var key in _currentCryptocurrencyOverviewData.Keys)
            {
                if (!newValues.Any(x => x.Ticker == key))
                {
                    _currentCryptocurrencyOverviewData.TryRemove(key, out _);
                }
            }

            var updates = GetCryptocurrencyOverviewUpdates(oldValues, newValues);
            await _updateHub.Clients.All.SendUpdateAsync(updates);

        }

        private IEnumerable<CryptocurrencyOverviewUpdateDto> GetCryptocurrencyOverviewUpdates(IEnumerable<CryptocurrencyOverviewDto> oldValues, IEnumerable<CryptocurrencyOverviewDto> newValues)
        {
            var updates = new List<CryptocurrencyOverviewUpdateDto>();
            foreach (var oldValue in oldValues)
            {
                var matchingNewValue = newValues.FirstOrDefault(x => x.Ticker == oldValue.Ticker);

                var update = new CryptocurrencyOverviewUpdateDto();
                if (matchingNewValue != null)
                {
                    var compareLogic = new CompareLogic();
                    var comparisonResult = compareLogic.Compare(oldValue, matchingNewValue);

                    if (!comparisonResult.AreEqual)
                    {
                        update.UpdateType = CryptocurrencyOverviewUpdateType.None;
                    }
                    else
                    {
                        update.UpdateType = CryptocurrencyOverviewUpdateType.Update;
                        update.Data = matchingNewValue;
                    }
                }
                else
                {
                    update.UpdateType = CryptocurrencyOverviewUpdateType.Delete;
                    update.Data = oldValue;
                }

                if (update.UpdateType != CryptocurrencyOverviewUpdateType.None)
                    updates.Add(update);
            }

            var completelyNewValues = newValues.Where(x => !oldValues.Any(y => y.Ticker == x.Ticker));

            foreach (var completelyNewValue in completelyNewValues)
            {
                var update = new CryptocurrencyOverviewUpdateDto()
                {
                    UpdateType = CryptocurrencyOverviewUpdateType.Create,
                    Data = completelyNewValue
                };
            }

            return updates;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            _stoppingCts.Cancel();
        }


        private ConcurrentDictionary<string, CryptocurrencyOverviewDto> _currentCryptocurrencyOverviewData = new();
        private readonly IBinanceClient _binanceClient;
        private readonly ICoinGeckoClient _coinGeckoClient;
        private int _updateCyclePeriod = 60000;
        private readonly IHubContext<CryptocurrencyOverviewUpdateHub, ICryptocurrencyOverviewUpdateHub> _updateHub;
        private EventWaitHandle _waitHandle = new(false, EventResetMode.ManualReset);
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new();
    }
}
