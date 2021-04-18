using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions.Services
{
    public interface IUserService
    {
        Task<UserDto> LoginAsync(LoginDto login);
        Task CreateAsync(UserDto userDto);
        Task<UserDto> GetAsync(string id);
    }
}
