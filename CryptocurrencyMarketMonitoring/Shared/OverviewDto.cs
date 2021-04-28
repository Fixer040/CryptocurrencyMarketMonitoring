using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CryptocurrencyMarketMonitoring.Shared
{
    public class OverviewDto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long MarketCapRank { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PriceChangePercentage24HInCurrency { get; set; }
        public decimal PriceChangePercentage7DInCurrency { get; set; }
        public decimal MarketCap { get; set; }
        public decimal TotalVolume { get; set; }
        public Uri Image { get; set; }
        public SparklineDto Sparkline { get; set; }

    }

    public class SparklineDto
    {
        public IEnumerable<SparklineValueDto> SparklineValues {get;set;}
    }

    public class SparklineValueDto
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
    }
}
