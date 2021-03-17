using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CryptocurrencyMarketMonitoring.Shared
{
    public class Cryptocurrency : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int? CryptocurrencyId { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public decimal PriceUSD { get; set; }
        public double LastDayPercentageMovement { get; set; }
        public double LastWeekPercentageMovement { get; set; }
        public int MarketCapUSD { get; set; }
        public int VolumeUSD { get; set; }
        public int CirculatingSupply { get; set; }

    }
}
