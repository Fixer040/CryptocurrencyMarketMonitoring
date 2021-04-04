using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions.Managers
{
    public interface ICryptocurrencyOverviewManager : IHostedService, IDisposable
    {
        Task<IEnumerable<CryptocurrencyOverviewDto>> GetCryptocurrencyOverviewAllAsync();
    }
}
