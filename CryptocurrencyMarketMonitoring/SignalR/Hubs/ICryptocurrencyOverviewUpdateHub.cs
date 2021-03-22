using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.SignalR.Hubs
{
    public interface ICryptocurrencyOverviewUpdateHub
    {
        Task SendUpdateAsync(IEnumerable<CryptocurrencyOverviewDto> cryptocurrencyUpdates);
    }
}
