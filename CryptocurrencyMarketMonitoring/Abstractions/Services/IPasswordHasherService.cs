using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions
{
    public interface IPasswordHasherService
    {
        string Hash(string password);

        (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
    }
}
