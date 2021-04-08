using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CryptocurrencyMarketMonitoring.Client.Services;
using Syncfusion.Blazor;




namespace CryptocurrencyMarketMonitoring.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.Services.AddSyncfusionBlazor();
			
			
		
            builder.RootComponents.Add<App>("#app");

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDI2MDAyQDMxMzgyZTM0MmUzMGRVdUIyTVhyNm9wbUVPRmtDMGsvL3hsTm9KNXdkVDJYMVROTmwzTy9LVlU9");

            builder.Services
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IHttpService, HttpService>()
                .AddScoped<ILocalStorageService, LocalStorageService>();


            builder.Services.AddScoped(x => {
                return new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            });

            var host = builder.Build();

            var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
            await authenticationService.Initialize();

            await host.RunAsync();
        }
    }
}
