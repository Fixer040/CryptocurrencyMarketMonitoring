using AutoMapper;
using Binance.Net.Interfaces;
using CryptocurrencyMarketMonitoring.Abstractions.Units;
using CryptocurrencyMarketMonitoring.Model.Documents;
using CryptocurrencyMarketMonitoring.Model.Repository;
using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Model.Units
{
    public class BinanceChartDataUnit : UnitOfWorkMongoBase, IBinanceChartDataUnit<BinanceChartData>
    {
        public BinanceChartDataUnit(ILoggerFactory loggerFactory, IMongoRepositoryLocator locator, IMapper mapper) : base(loggerFactory, locator)
        {
            _logger = loggerFactory.CreateLogger<BinanceChartDataUnit>();
            _mapper = mapper;
        }


        public async Task<IEnumerable<ChartDataDto>> GetAsync(Expression<Func<BinanceChartData, bool>> filter, int page, int pageSize, string currency, string vsCurrency, IntervalType intervalType)
        {
            return await ExecuteCommandAsync<BinanceChartData, ChartDataDto>(async locator =>
            {
                var binanceChartData = await locator.FindAsync(filter, (x => x.OpenTime), page, pageSize, true, currency, vsCurrency, intervalType.ToString());
                var result = binanceChartData.Select(x => _mapper.Map<ChartDataDto>(x));

                return result;
            });
        }

        public async Task CreateManyAsync(IEnumerable<IBinanceKline> binanceKlines, string currency, string vsCurrency, IntervalType intervalType)
        {
            if (binanceKlines == null) return;

            await ExecuteCommandAsync<BinanceChartData>(async locator =>
            {
                var convertedKlines = binanceKlines.Select(x => _mapper.Map<BinanceChartData>(x));

                await locator.InsertAsync(convertedKlines, currency, vsCurrency, intervalType.ToString());

            });
        }

        private ILogger<BinanceChartDataUnit> _logger;
        private IMapper _mapper;
    }
}
