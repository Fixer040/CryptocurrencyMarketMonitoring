using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CryptocurrencyMarketMonitoring.Shared
{
    public class CryptocurrencyOverviewUpdateDto
    {
        public CryptocurrencyOverviewUpdateType UpdateType { get; set; }
        public CryptocurrencyOverviewDto Data { get; set; }

    }


    public enum CryptocurrencyOverviewUpdateType
    {
        Update = 0,
        Create = 1,
        Delete = 2
    }
}
