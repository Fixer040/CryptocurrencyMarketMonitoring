using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions.Services
{
    public interface IOverviewService
    {
        IEnumerable<string> GetSupportedCurrencies();

        IEnumerable<OverviewDto> GetOverviewAll(string currency);

        OverviewDto GetOverview(string currency, string vsCurrency);

    }
}
