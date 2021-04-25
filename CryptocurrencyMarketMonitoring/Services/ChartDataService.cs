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

namespace CryptocurrencyMarketMonitoring.Services
{
    public class ChartDataService : IChartDataService
    {

        public ChartDataService(IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }

        public async Task<IEnumerable<ChartDataDto>> GetChartDataAsync(string currency, string vsCurrency, IntervalType intervalType)
        {
            return await GetBinanceChartDataAsync(currency, vsCurrency, intervalType);
        }

        private async Task<IEnumerable<ChartDataDto>> GetBinanceChartDataAsync(string currency, string vsCurrency, IntervalType intervalType)
        {
            using (var unit = DIContainer.BeginScopeService<IBinanceChartDataUnit<BinanceChartData>>())
            {
                return await unit.GetAsync(x => true, 0, 1000, currency, vsCurrency, intervalType);
            }
        }

        IBinanceClient _binanceClient;
    }
}
