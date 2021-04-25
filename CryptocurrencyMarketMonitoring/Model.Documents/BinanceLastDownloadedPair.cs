using CryptocurrencyMarketMonitoring.Model.Documents.Attributes;
using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Model.Documents
{
    [CollectionName("binance_last_downloaded")]
    [ConnectionName("NoSql")]
    public class BinanceLastDownloadedPair : MongoDocumentBase
    {
        public string Pair { get; set; }
        public IntervalType Interval { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
