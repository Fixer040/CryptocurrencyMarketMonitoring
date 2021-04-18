using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CryptocurrencyMarketMonitoring.Shared
{
    public class OverviewUpdateDto
    {
        public OverviewUpdateType UpdateType { get; set; }
        public OverviewDto Data { get; set; }

    }


    public enum OverviewUpdateType
    {
        None = 0,
        Update = 1,
        Create = 2,
        Delete = 3
    }
}
