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

    public class OverviewManager : BackgroundService, IOverviewManager
    {

        public OverviewManager(IHubContext<OverviewUpdateHub, IOverviewUpdateClient> updateHub, ICoinGeckoClient coinGeckoClient, IBinanceClient binanceClient)
        {
            _updateHub = updateHub;
            _binanceClient = binanceClient;
            _coinGeckoClient = coinGeckoClient;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await UpdateDataAsync(sendUpdate: false);
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


        public async Task<IEnumerable<OverviewDto>> GetCryptocurrencyOverviewAllAsync()
        {
            _waitHandle.WaitOne();

            return _currentCryptocurrencyOverviewData.Values.OrderBy(x => x.Ranking).ToList();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateDataAsync(true);

                await Task.Delay(_updateCyclePeriod, stoppingToken);
            }
        }

        private async Task UpdateDataAsync(bool sendUpdate = false)
        {
            var coinMarkets = await _coinGeckoClient.CoinsClient.GetCoinMarkets("usd", Array.Empty<string>(), "market_cap_desc", 250, 1, true, "7d,24h", null);

            var oldValues = _currentCryptocurrencyOverviewData.Values.Select(x => x.DeepClone()).ToList();
            var newValues = new List<OverviewDto>();
            foreach (var coinMarket in coinMarkets)
            {
                var cryptocurrency = new OverviewDto()
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

                _currentCryptocurrencyOverviewData.AddOrUpdate(cryptocurrency.Name, cryptocurrency, (key, value) => cryptocurrency);
                newValues.Add(cryptocurrency);
            }

            foreach (var key in _currentCryptocurrencyOverviewData.Keys)
            {
                if (!newValues.Any(x => x.Name == key))
                {
                    _currentCryptocurrencyOverviewData.TryRemove(key, out _);
                }
            }

            if (sendUpdate)
            {
                var updates = GetCryptocurrencyOverviewUpdates(oldValues, newValues);

                await _updateHub.Clients.All.ReceiveUpdate(updates);
            }
        }

        private IEnumerable<OverviewUpdateDto> GetCryptocurrencyOverviewUpdates(IEnumerable<OverviewDto> oldValues, IEnumerable<OverviewDto> newValues)
        {
            var updates = new List<OverviewUpdateDto>();
            foreach (var oldValue in oldValues)
            {
                var matchingNewValue = newValues.FirstOrDefault(x => x.Name == oldValue.Name);

                var update = new OverviewUpdateDto();
                if (matchingNewValue != null)
                {
                    var compareLogic = new CompareLogic();
                    var comparisonResult = compareLogic.Compare(oldValue, matchingNewValue);

                    if (!comparisonResult.AreEqual)
                    {
                        update.UpdateType = OverviewUpdateType.None;
                    }
                    else
                    {
                        update.UpdateType = OverviewUpdateType.Update;
                        update.Data = matchingNewValue;
                    }
                }
                else
                {
                    update.UpdateType = OverviewUpdateType.Delete;
                    update.Data = oldValue;
                }

                if (update.UpdateType != OverviewUpdateType.None)
                    updates.Add(update);
            }

            var completelyNewValues = newValues.Where(x => !oldValues.Any(y => y.Name == x.Name));

            foreach (var completelyNewValue in completelyNewValues)
            {
                var update = new OverviewUpdateDto()
                {
                    UpdateType = OverviewUpdateType.Create,
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


        private ConcurrentDictionary<string, OverviewDto> _currentCryptocurrencyOverviewData = new();
        private readonly IBinanceClient _binanceClient;
        private readonly ICoinGeckoClient _coinGeckoClient;
        private int _updateCyclePeriod = 60000;
        private readonly IHubContext<OverviewUpdateHub, IOverviewUpdateClient> _updateHub;
        private EventWaitHandle _waitHandle = new(false, EventResetMode.ManualReset);
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new();
    }
}
