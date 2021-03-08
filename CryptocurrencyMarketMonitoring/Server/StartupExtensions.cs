using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
