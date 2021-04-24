using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions.Managers
{
    public interface IOverviewSubscriptionManager
    {
        void Subscribe(string clientId, string currency);
        Dictionary<string, string> GetClientSubscriptions();
        void Unsubscribe(string clientId);
    }
}
