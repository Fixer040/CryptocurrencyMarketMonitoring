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

namespace CryptocurrencyMarketMonitoring.Services
{

    //TODO: IHostedService
    public class CryptocurrencyOverviewManager : ICryptocurrencyOverviewManager
    {

        public CryptocurrencyOverviewManager(IHubContext<CryptocurrencyOverviewUpdateHub, ICryptocurrencyOverviewUpdateHub> updateHub, ICoinGeckoClient coinGeckoClient, IBinanceClient binanceClient)
        {
            _updateHub = updateHub;
            _binanceClient = binanceClient;
            _coinGeckoClient = coinGeckoClient;

            _ = InitAsync();
        }


        public async Task<IEnumerable<CryptocurrencyOverviewDto>> GetCryptocurrencyOverviewAllAsync()
        {
            _waitHandle.WaitOne();
            return _currentCryptocurrencyOverviewData.Values.OrderBy(x => x.Ranking).ToList();
        }

        private async Task InitAsync()
        {
            await UpdateDataAsync();

            _waitHandle.Set();

            _ = Task.Run(DoUpdateCycleAsync);
        }

        private async Task DoUpdateCycleAsync()
        {
            while (true)
            {
                await UpdateDataAsync();
                await Task.Delay(_updatecyclePeriod);
            }

        }
       

        private async Task UpdateDataAsync()
        {
            var coinMarkets = await _coinGeckoClient.CoinsClient.GetCoinMarkets("usd", Array.Empty<string>(), "market_cap_desc", 100, 1, true, "7d,24h", null);

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




        private ConcurrentDictionary<string, CryptocurrencyOverviewDto> _currentCryptocurrencyOverviewData = new();
        private readonly IBinanceClient _binanceClient;
        private readonly ICoinGeckoClient _coinGeckoClient;
        private int _updatecyclePeriod = 60000;
        private IHubContext<CryptocurrencyOverviewUpdateHub, ICryptocurrencyOverviewUpdateHub> _updateHub;
        private EventWaitHandle _waitHandle = new(false, EventResetMode.ManualReset);
    }
}
