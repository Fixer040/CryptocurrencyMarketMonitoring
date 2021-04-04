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
        None = 0,
        Update = 1,
        Create = 2,
        Delete = 3
    }
}
