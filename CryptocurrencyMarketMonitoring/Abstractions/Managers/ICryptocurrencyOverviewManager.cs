using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions.Managers
{
    public interface ICryptocurrencyOverviewManager
    {
        Task<IEnumerable<CryptocurrencyOverviewDto>> GetCryptocurrencyOverviewAllAsync();
    }
}
