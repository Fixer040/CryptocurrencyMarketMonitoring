using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CryptocurrencyMarketMonitoring.Abstractions.Managers;

namespace CryptocurrencyMarketMonitoring.Services
{
    public class OverviewService : IOverviewService
    {

        public OverviewService(IOverviewManager cryptocurrencyOverviewManager)
        {
            _cryptocurrencyOverviewManager = cryptocurrencyOverviewManager;
        }

        public IEnumerable<string> GetSupportedCurrencies()
        {
            return _cryptocurrencyOverviewManager.GetSupportedCurrencies();
        }

        public IEnumerable<OverviewDto> GetOverviewAll(string currency)
        {
            return _cryptocurrencyOverviewManager.GetOverviewAll(currency);
        }

        public OverviewDto GetOverview(string currency, string vsCurrency)
        {
            return _cryptocurrencyOverviewManager.GetOverview(currency, vsCurrency);
        }

        IOverviewManager _cryptocurrencyOverviewManager;
    }
}
