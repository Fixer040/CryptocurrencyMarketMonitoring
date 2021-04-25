using CryptocurrencyMarketMonitoring.Abstractions;
using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Abstractions.Units;
using CryptocurrencyMarketMonitoring.Model.Documents;
using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Services
{

    public class UserService : IUserService
    {

        public UserService(IPasswordHasherService passwordHasherService, IOptions<JwtOptions> jwtOptions)
        {
            _passwordHasherService = passwordHasherService;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<UserDto> LoginAsync(LoginDto login)
        {
            using (var unit = DIContainer.BeginScopeService<IUserUnit<User>>())
            {
                var user = await unit.GetAsync(x => x.Email == login.Username);

                if (user == null)
                    throw new Exception("User not found.");

                var passwordCheck = _passwordHasherService.Check(user.PasswordHash, login.Password);

                if (!passwordCheck.Verified)
                    throw new Exception("Invalid password.");


                var token = GenerateJwtToken(user);
                user.Token = token;

                return user;
            }
        }

        public async Task CreateAsync(UserDto userDto)
        {
            using (var unit = DIContainer.BeginScopeService<IUserUnit<User>>())
            {
                var user = await unit.GetAsync(x => x.Email == userDto.Email);

                if (user != null)
                    throw new Exception("User with this email already exists!");

                userDto.PasswordHash = _passwordHasherService.Hash(userDto.Password);

                await unit.CreateAsync(userDto);
            }
        }


        public async Task<UserDto> GetAsync(string id)
        {
            using (var unit = DIContainer.BeginScopeService<IUserUnit<User>>())
            {
                return await unit.GetAsync(id);
            }
        }


        private string GenerateJwtToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(
                source: Convert.FromBase64String(_jwtOptions.Private),
                bytesRead: out int _
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        private readonly IPasswordHasherService _passwordHasherService;
        private readonly JwtOptions _jwtOptions;
    }
}