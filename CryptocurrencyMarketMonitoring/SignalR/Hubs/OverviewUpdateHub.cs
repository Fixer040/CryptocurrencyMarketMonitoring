using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.SignalR.Hubs
{
    public class OverviewUpdateHub : Hub<IOverviewUpdateClient>
    {
        public async Task SendUpdateAsync(IEnumerable<OverviewUpdateDto> updates)
        {
            await Clients.Others.ReceiveUpdate(updates);
        }

    }
}
