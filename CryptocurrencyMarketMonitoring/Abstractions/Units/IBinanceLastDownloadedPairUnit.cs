using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions.Units
{
    public interface IBinanceLastDownloadedPairUnit<TDocument> : IDisposable
    {
        Task<IEnumerable<TDocument>> GetAllAsync(Expression<Func<TDocument, bool>> filter);
        Task CreateAsync(TDocument lastDownloadedPair);
        Task UpdateAsync(string id, TDocument lastDownloadedPair);
    }
}
