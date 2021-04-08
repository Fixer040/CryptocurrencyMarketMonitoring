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
        UserDto Login(LoginDto login);
        UserDto GetById(string id);
    }
}
