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
using AutoMapper;
using CoinGecko.Entities.Response.Coins;

namespace CryptocurrencyMarketMonitoring.Services
{

    public class OverviewManager : BackgroundService, IOverviewManager
    {

        public OverviewManager(IHubContext<OverviewUpdateHub, IOverviewUpdateClient> updateHub, ICoinGeckoClient coinGeckoClient, IOverviewSubscriptionManager subscriptionManager, IMapper mapper)
        {
            _updateHub = updateHub;
            _coinGeckoClient = coinGeckoClient;
            _subscriptionManager = subscriptionManager;
            _mapper = mapper;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var supportedCurrencies = await _coinGeckoClient.SimpleClient.GetSupportedVsCurrencies();


            //Temporary currency limit in place for testing purposes
            _supportedCurrencies = supportedCurrencies.Select(x => x.ToUpperInvariant()).Take(5).ToList();

            if (!_supportedCurrencies.Contains("USD"))
                _supportedCurrencies.Add("USD");

            if (!_supportedCurrencies.Contains("EUR"))
                _supportedCurrencies.Add("EUR");

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

        public IEnumerable<string> GetSupportedCurrencies()
        {
            _waitHandle.WaitOne();

            return _supportedCurrencies;
        }

        public IEnumerable<OverviewDto> GetOverviewAll(string currency)
        {
            _waitHandle.WaitOne();

            if (_overviewData.TryGetValue(currency, out var currencyData))
            {
                var retval = currencyData.Values.OrderBy(x => x.MarketCapRank).ToList();

                return retval;
            }

            return null;
        }

        public OverviewDto GetOverview(string currency, string vsCurrency)
        {
            _waitHandle.WaitOne();

            if (_overviewData.TryGetValue(vsCurrency, out var vsCurrencyData))
            {
                if (vsCurrencyData.TryGetValue(currency, out var overview))
                    return overview;
            }

            return null;
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
            foreach (var supportedCurrency in _supportedCurrencies)
            {
                if (!_overviewData.TryGetValue(supportedCurrency, out var supportedCurrencyData))
                {
                    supportedCurrencyData = new ConcurrentDictionary<string, OverviewDto>();
                    _overviewData.TryAdd(supportedCurrency, supportedCurrencyData);
                }

                var coinMarkets = await _coinGeckoClient.CoinsClient.GetCoinMarkets(supportedCurrency, Array.Empty<string>(), "market_cap_desc", 250, 1, true, "7d,24h", null);
                await Task.Delay(500);

                var oldValues = supportedCurrencyData.Values.Select(x => x.DeepClone()).ToList();
                var newValues = new List<OverviewDto>();
                foreach (var coinMarket in coinMarkets)
                {
                    var cryptocurrency = _mapper.Map<CoinMarkets, OverviewDto>(coinMarket);

                    cryptocurrency.Sparkline = new SparklineDto();

                    var sparklineValues = new List<SparklineValueDto>();

                    for (int i = 0; i < coinMarket.SparklineIn7D.Price.Length; i++)
                    {

                        var sparklinevalue = new SparklineValueDto()
                        {
                            Id = i,
                            Value = coinMarket.SparklineIn7D.Price[i]
                        };

                        sparklineValues.Add(sparklinevalue);
                    }

                    cryptocurrency.Sparkline.SparklineValues = sparklineValues;


                    supportedCurrencyData.AddOrUpdate(cryptocurrency.Name, cryptocurrency, (key, value) => cryptocurrency);
                    newValues.Add(cryptocurrency);
                }

                foreach (var key in supportedCurrencyData.Keys)
                {
                    if (!newValues.Any(x => x.Name == key))
                    {
                        supportedCurrencyData.TryRemove(key, out _);
                    }
                }

                if (sendUpdate)
                {
                    _ = SendOverviewUpdatesToClients(supportedCurrency, oldValues, newValues);
                }

            }

        }

        private async Task SendOverviewUpdatesToClients(string currency, IEnumerable<OverviewDto> oldValues, IEnumerable<OverviewDto> newValues)
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

                    if (comparisonResult.AreEqual)
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

            var subscriptions = _subscriptionManager.GetClientSubscriptions();

            foreach (var subscription in subscriptions)
            { 
                if (subscription.Value == currency)
                {
                    await _updateHub.Clients.Client(subscription.Key).ReceiveUpdate(updates);
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            _stoppingCts.Cancel();
        }


        private ConcurrentDictionary<string, ConcurrentDictionary<string, OverviewDto>> _overviewData = new();
        private readonly ICoinGeckoClient _coinGeckoClient;
        private int _updateCyclePeriod = 60000;
        private readonly IHubContext<OverviewUpdateHub, IOverviewUpdateClient> _updateHub;
        private EventWaitHandle _waitHandle = new(false, EventResetMode.ManualReset);
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new();
        private List<string> _supportedCurrencies;
        private IOverviewSubscriptionManager _subscriptionManager;
        private IMapper _mapper;
    }
}
