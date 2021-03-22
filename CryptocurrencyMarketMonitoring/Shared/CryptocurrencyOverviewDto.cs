using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CryptocurrencyMarketMonitoring.Shared
{
    public class CryptocurrencyOverviewDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Ranking { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public decimal PriceUSD { get; set; }
        public decimal LastDayPercentageMovement { get; set; }
        public decimal LastWeekPercentageMovement { get; set; }
        public decimal MarketCapUSD { get; set; }
        public decimal VolumeUSD { get; set; }
        public Uri IconSrc { get; set; }

    }
}
