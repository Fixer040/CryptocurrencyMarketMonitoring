using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.SignalR.Hubs
{
    public interface IOverviewUpdateClient
    {
        Task ReceiveUpdate(IEnumerable<OverviewUpdateDto> cryptocurrencyUpdates);
    }
}
