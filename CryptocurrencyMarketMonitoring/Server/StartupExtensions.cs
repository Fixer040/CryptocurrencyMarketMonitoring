using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using Binance.Net;
using CryptoExchange.Net.RateLimiter;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Server
{
    public static class StartupExtensions
    {
        public static void AddControllerModules(this IServiceCollection services, IConfigurationRoot configuration, string contentRootPath)
        {
            configuration.GetSection("Modules:WebApiAssembly").GetChildren().ToList().ForEach(section =>
            {
                var path = Path.Combine(contentRootPath, $"{section.Value}");
                if (File.Exists(path))
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
                    services.AddControllers().AddApplicationPart(assembly).AddControllersAsServices();
                }
            });
        }

        public static void RegisterAutofacModules(this ContainerBuilder builder, IConfigurationRoot configuration, string contentRootPath)
        {
            var assemblies = new List<Assembly>();

            var entryAssembly = Assembly.GetEntryAssembly();
            assemblies.Add(entryAssembly);
            configuration.GetSection("Modules:Assembly").GetChildren().ToList().ForEach(section =>
            {
                var path = Path.Combine(contentRootPath, $"{section.Value}");
                if (File.Exists(path))
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
                    assemblies.Add(assembly);
                }
            });
            builder.RegisterAssemblyModules(assemblies.ToArray());
        }


        public static void ConfigureBinanceClientSettings(this IServiceCollection services)
        {
            BinanceClient.SetDefaultOptions(new Binance.Net.Objects.Spot.BinanceClientOptions()
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("sDKjPTMjAtgW6jTD9cB4IiJG3B4qyprUEsBxL2HtErGNxoFRFzn51yOAxCCcGwQG", "PAUmjPzTSkx1R57ycXgI1vLcSrVpJQYzgjrL7Koch1TvORXJaO0zEwgmDGB8ovmQ"),
                RateLimitingBehaviour = CryptoExchange.Net.Objects.RateLimitingBehaviour.Wait,
                RateLimiters = new List<CryptoExchange.Net.Interfaces.IRateLimiter>() { new RateLimiterTotal(500, new TimeSpan(0, 1, 0)) }
            }); ;
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterSignalRHubs(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            return builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(Hub).IsAssignableFrom(t))
                .ExternallyOwned();
        }
    }
}
