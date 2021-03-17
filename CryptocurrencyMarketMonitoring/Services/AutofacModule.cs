using Autofac;
using CryptocurrencyMarketMonitoring.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Services
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CryptocurrencyOverviewService>().As<ICryptocurrencyOverviewService>();
        }
    }
}
