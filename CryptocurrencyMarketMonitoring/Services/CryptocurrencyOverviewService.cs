using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CryptocurrencyMarketMonitoring.Abstractions.Managers;

namespace CryptocurrencyMarketMonitoring.Services
{
    public class CryptocurrencyOverviewService : ICryptocurrencyOverviewService
    {

        public CryptocurrencyOverviewService(ICryptocurrencyOverviewManager cryptocurrencyOverviewManager)
        {
            _cryptocurrencyOverviewManager = cryptocurrencyOverviewManager;
        }

        public async Task<IEnumerable<CryptocurrencyOverviewDto>> GetCryptocurrencyListAsync()
        {
            return await _cryptocurrencyOverviewManager.GetCryptocurrencyOverviewAllAsync();
        }

        ICryptocurrencyOverviewManager _cryptocurrencyOverviewManager;
    }
}
