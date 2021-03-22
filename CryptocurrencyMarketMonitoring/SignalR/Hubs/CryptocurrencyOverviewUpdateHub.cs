using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.SignalR.Hubs
{
    public class CryptocurrencyOverviewUpdateHub : Hub<ICryptocurrencyOverviewUpdateHub>, ICryptocurrencyOverviewUpdateHub
    {
        public async Task SendUpdateAsync(IEnumerable<CryptocurrencyOverviewDto> cryptocurrencyUpdates)
        {
            await Clients.All.SendUpdateAsync(cryptocurrencyUpdates);
        }
    }
}
