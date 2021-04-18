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
    public class UserUnit : UnitOfWorkMongoBase, IUserUnit<User>
    {
        public UserUnit(ILoggerFactory loggerFactory, IMongoRepositoryLocator locator, IMapper mapper) : base(loggerFactory, locator)
        {
            _logger = loggerFactory.CreateLogger<UserUnit>();
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

        public async Task<UserDto> GetAsync(string id)
        {
            return await ExecuteCommandAsync<User, UserDto>(async locator =>
            {           
                var user = await locator.GetAsync<User>(ObjectId.Parse(id));

                var userDto = _mapper.Map<UserDto>(user);

                return userDto;
            });
        }


        public async Task CreateAsync(UserDto userDto)
        {
            if (userDto == null) return;

            await ExecuteCommandAsync<User>(async locator =>
            {
                var user = _mapper.Map<User>(userDto);

                await locator.InsertAsync(user);

            });
        }

        public async Task UpdateAsync(string id, UserDto userDto)
        {
            if (userDto == null) return;

            await ExecuteCommandAsync<User>(async locator =>
            {
                var user = _mapper.Map<User>(userDto);

                var u = GetUpdaterIdentityUserDto(locator, user);

                if (await locator.UpdateAsync<User>(id, u))
                {
                }
            });

       }

        UpdateDefinition<User>[] GetUpdaterIdentityUserDto(IMongoRepositoryLocator locator, User user)
        {
            UpdateDefinition<User>[] updates = null;

            var updater = locator.GetUpdater<User>();
            updates = new UpdateDefinition<User>[]
                {
                    updater.Set(a => a.Username, user.Username),
                    updater.Set(a => a.PasswordHash, user.PasswordHash),
                    updater.Set(a => a.FirstName, user.FirstName),
                    updater.Set(a => a.LastName, user.LastName),
                    updater.Set(a => a.Email, user.Email)
                };

            return updates;
        }

     
        private ILogger<UserUnit> _logger;
        private IMapper _mapper;
    }
}
