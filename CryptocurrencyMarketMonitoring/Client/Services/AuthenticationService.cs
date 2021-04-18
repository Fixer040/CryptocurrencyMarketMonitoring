using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Client.Services
{
    public interface IAuthenticationService
    {
        UserDto User { get; }
        Task Initialize();
        Task Login(LoginDto login);
        Task Logout();
        public event Action OnChange;

    }

    public class AuthenticationService : IAuthenticationService
    {
        public event Action OnChange;

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

        public async Task Login(LoginDto login)
        {
            User = await _httpService.Post<UserDto>("/User/login", login);
            await _localStorageService.SetItem("user", User);
            NotifyStateChanged();

        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem("user");
            _navigationManager.NavigateTo("");
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

    }
}