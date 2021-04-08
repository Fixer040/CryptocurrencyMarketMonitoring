using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Client.Services
{
    public interface IAuthenticationService
    {
        UserDto User { get; }
        Task Initialize();
        Task Login(string username, string password);
        Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private IHttpService _httpService;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;

        public UserDto User { get; private set; }

        public AuthenticationService(
            IHttpService httpService,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService
        )
        {
            _httpService = httpService;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<UserDto>("user");
        }

        public async Task Login(string username, string password)
        {
            User = await _httpService.Post<UserDto>("/User/login", new { username, password });
            await _localStorageService.SetItem("user", User);
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem("user");
            _navigationManager.NavigateTo("");
        }
    }
}