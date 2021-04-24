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
    public class BinanceChartDataUnit : UnitOfWorkMongoBase, IBinanceChartDataUnit<BinanceChartData>
    {
        public BinanceChartDataUnit(ILoggerFactory loggerFactory, IMongoRepositoryLocator locator, IMapper mapper) : base(loggerFactory, locator)
        {
            _logger = loggerFactory.CreateLogger<BinanceChartDataUnit>();
            _mapper = mapper;
        }


        public async Task<UserDto> GetAsync(Expression<Func<User, bool>> filter)
        {
            return await ExecuteCommandAsync<User, UserDto>(async locator =>
            {
                var user = await locator.FirstAsync(filter);

                var userDto = _mapper.Map<UserDto>(user);

                return userDto;
            });
        }

        public async Task CreateManyAsync(UserDto userDto)
        {
            if (userDto == null) return;

            await ExecuteCommandAsync<User>(async locator =>
            {
                var user = _mapper.Map<User>(userDto);

                await locator.InsertAsync(user);

            });
        }

     
        private ILogger<BinanceChartDataUnit> _logger;
        private IMapper _mapper;
    }
}
