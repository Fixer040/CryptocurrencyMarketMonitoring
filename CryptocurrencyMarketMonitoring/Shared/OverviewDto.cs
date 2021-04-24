using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CryptocurrencyMarketMonitoring.Shared
{
    public class OverviewDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Ranking { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public decimal LastDayPercentageMovement { get; set; }
        public decimal LastWeekPercentageMovement { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Volume { get; set; }
        public Uri IconSrc { get; set; }

    }
}
