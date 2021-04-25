using AutoMapper;
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
    public class BinanceLastDownloadedPairUnit : UnitOfWorkMongoBase, IBinanceLastDownloadedPairUnit<BinanceLastDownloadedPair>
    {
        public BinanceLastDownloadedPairUnit(ILoggerFactory loggerFactory, IMongoRepositoryLocator locator, IMapper mapper) : base(loggerFactory, locator)
        {
            _logger = loggerFactory.CreateLogger<BinanceLastDownloadedPairUnit>();
            _mapper = mapper;
        }


        public async Task<IEnumerable<BinanceLastDownloadedPair>> GetAllAsync(Expression<Func<BinanceLastDownloadedPair, bool>> filter)
        {
            return await ExecuteCommandAsync<BinanceLastDownloadedPair, BinanceLastDownloadedPair>(async locator =>
            {
                return await locator.FindAsync(filter);
            });
        }


        public async Task CreateAsync(BinanceLastDownloadedPair lastdownloadedPair)
        {
            if (lastdownloadedPair == null) return;

            await ExecuteCommandAsync<BinanceLastDownloadedPair>(async locator =>
            {
                await locator.InsertAsync(lastdownloadedPair);

            });
        }

        public async Task UpdateAsync(string id, BinanceLastDownloadedPair lastDownloadedPair)
        {
            if (lastDownloadedPair == null) return;

            await ExecuteCommandAsync<BinanceLastDownloadedPair>(async locator =>
            {

                var u = GetUpdater(locator, lastDownloadedPair);

                if (await locator.UpdateAsync<BinanceLastDownloadedPair>(ObjectId.Parse(id), u))
                {
                }
            });

       }

        UpdateDefinition<BinanceLastDownloadedPair>[] GetUpdater(IMongoRepositoryLocator locator, BinanceLastDownloadedPair lastDownloadedPair)
        {
            UpdateDefinition<BinanceLastDownloadedPair>[] updates = null;

            var updater = locator.GetUpdater<BinanceLastDownloadedPair>();
            updates = new UpdateDefinition<BinanceLastDownloadedPair>[]
                {
                    updater.Set(a => a.Pair, lastDownloadedPair.Pair),
                    updater.Set(a => a.Timestamp, lastDownloadedPair.Timestamp),
                };

            return updates;
        }

     
        private ILogger<BinanceLastDownloadedPairUnit> _logger;
        private IMapper _mapper;
    }
}
