using CryptocurrencyMarketMonitoring.Abstractions.Managers;
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
        public OverviewUpdateHub(IOverviewSubscriptionManager subscriptionManager)
        {
            _subscriptionManager = subscriptionManager;
        }

        public void Subscribe(string clientId, string currency)
        {
            _subscriptionManager.Subscribe(clientId, currency);
        }


        IOverviewSubscriptionManager _subscriptionManager;
    }
}
