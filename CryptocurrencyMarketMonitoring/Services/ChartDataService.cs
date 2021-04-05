using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CryptocurrencyMarketMonitoring.Abstractions.Managers;
using Binance.Net.Interfaces;

namespace CryptocurrencyMarketMonitoring.Services
{
    public class ChartDataService : IChartDataService
    {

        public ChartDataService(IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }

        public async Task<IEnumerable<ChartDataDto>> GetChartDataAsync(string ticker)
        {
            var binanceKLines = await _binanceClient.Spot.Market.GetKlinesAsync($"{ticker}USDT", Binance.Net.Enums.KlineInterval.OneDay);

            var retval = new List<ChartDataDto>();

            foreach (var kLine in binanceKLines.Data)
            {
                var chartDataDto = new ChartDataDto()
                {
                    Close = kLine.Close,
                    Date = kLine.CloseTime,
                    High = kLine.High,
                    Low = kLine.Low,
                    Open = kLine.Open,
                    Volume = kLine.BaseVolume
                };

                retval.Add(chartDataDto);
            }

            return retval;

        }

        IBinanceClient _binanceClient;
    }
}
