using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int RestoreTokenDurationMin { get; set; }
        public int ExpiresDurationMin { get; set; }
        public string Secret { get; set; }
    }
}
