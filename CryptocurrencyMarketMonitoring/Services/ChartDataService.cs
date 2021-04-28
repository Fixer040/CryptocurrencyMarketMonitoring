using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CryptocurrencyMarketMonitoring.Abstractions.Managers;
using Binance.Net.Interfaces;
using Binance.Net.Enums;
using CryptocurrencyMarketMonitoring.Abstractions;
using CryptocurrencyMarketMonitoring.Abstractions.Units;
using CryptocurrencyMarketMonitoring.Model.Documents;
using AutoMapper;

namespace CryptocurrencyMarketMonitoring.Services
{
    public class ChartDataService : IChartDataService
    {

        public ChartDataService(IBinanceClient binanceClient, IMapper mapper)
        {
            _binanceClient = binanceClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChartDataDto>> GetChartDataAsync(string currency, string vsCurrency, IntervalType intervalType)
        {
            var localDbData = await GetBinanceChartDataAsync(currency, vsCurrency, intervalType);

            if (localDbData == null || !localDbData.Any())
            {
                var binanceVsCurrency = vsCurrency.Replace("USD", "USDT");

                var klines = await _binanceClient.Spot.Market.GetKlinesAsync($"{currency}{binanceVsCurrency}", (KlineInterval)intervalType, limit: 1000);

                //try to switch currency order - for crypto pairs
                if (klines.Data == null || !klines.Data.Any())
                    klines = await _binanceClient.Spot.Market.GetKlinesAsync($"{binanceVsCurrency}{currency}", (KlineInterval)intervalType, limit: 1000);

                var binanceChartData = klines?.Data?.Select(x => _mapper.Map<BinanceChartData>(x));

                return binanceChartData?.Select(x => _mapper.Map<ChartDataDto>(x));
            }

            return localDbData;
        }

        private async Task<IEnumerable<ChartDataDto>> GetBinanceChartDataAsync(string currency, string vsCurrency, IntervalType intervalType)
        {
            using (var unit = DIContainer.BeginScopeService<IBinanceChartDataUnit<BinanceChartData>>())
            {
                return await unit.GetAsync(x => true, 0, 1000, currency, vsCurrency, intervalType);
            }
        }

        IBinanceClient _binanceClient;
        IMapper _mapper;
    }
}
