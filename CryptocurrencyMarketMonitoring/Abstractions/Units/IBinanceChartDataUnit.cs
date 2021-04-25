using Binance.Net.Interfaces;
using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions.Units
{
    public interface IBinanceChartDataUnit<TDocument> : IDisposable
    {
        Task<IEnumerable<ChartDataDto>> GetAsync(Expression<Func<TDocument, bool>> filter, int page, int pageSize, string currency, string vsCurrency, IntervalType intervalType);
        Task CreateManyAsync(IEnumerable<IBinanceKline> binanceKlines, string currency, string vsCurrency, IntervalType intervalType);
    }
}
