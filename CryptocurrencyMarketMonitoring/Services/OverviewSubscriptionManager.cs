using CryptocurrencyMarketMonitoring.Abstractions.Managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Services
{
    public class OverviewSubscriptionManager : IOverviewSubscriptionManager
    {
        public void Subscribe(string clientId, string currency)
        {
            _clientSubscriptions.AddOrUpdate(clientId, currency, (key, value) => currency);
        }

        public Dictionary<string, string> GetClientSubscriptions()
        {
            var retval = new Dictionary<string, string>();

            var keys = _clientSubscriptions.Keys;

            foreach (var key in keys)
            {
                if (_clientSubscriptions.TryGetValue(key, out var value))
                    retval.Add(key, value);
            }

            return retval;
        }

        public void Unsubscribe(string clientId)
        {
            _clientSubscriptions.TryRemove(clientId, out _);
        }


        ConcurrentDictionary<string, string> _clientSubscriptions = new ConcurrentDictionary<string, string>();
    }
}
