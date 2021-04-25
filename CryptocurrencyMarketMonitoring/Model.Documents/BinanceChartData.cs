using CryptocurrencyMarketMonitoring.Model.Documents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Model.Documents
{
    [CollectionName("chart_binance_{0}/{1}_{2}")]
    [ConnectionName("NoSql")]
    public class BinanceChartData : MongoDocumentBase
    {
        public DateTime OpenTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal BaseVolume { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal QuoteVolume { get; set; }
        public int TradeCount { get; set; }
        public decimal TakerBuyBaseVolume { get; set; }
        public decimal TakerBuyQuoteVolume { get; set; }
    }
}
