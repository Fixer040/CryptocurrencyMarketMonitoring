using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDA0MTUwQDMxMzgyZTM0MmUzMEpKVDI5bDh6aTFYbEdGZi9NelZ1MnFKa2V0WmFGZE5PUmR1bFJ2M0pDNzg9");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
