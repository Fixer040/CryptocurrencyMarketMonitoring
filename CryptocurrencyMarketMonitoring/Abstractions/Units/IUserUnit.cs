using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions.Units
{
    public interface IUserUnit<TDocument> : IDisposable
    {
        Task<UserDto> GetAsync(Expression<Func<TDocument, bool>> filter);
        Task<UserDto> GetAsync(string id);
        Task CreateAsync(UserDto user);
        Task UpdateAsync(string id, UserDto user);
    }
}
