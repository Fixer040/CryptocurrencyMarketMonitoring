using Autofac;
using Binance.Net;
using Binance.Net.Interfaces;
using CoinGecko.Clients;
using CoinGecko.Interfaces;
using CryptocurrencyMarketMonitoring.Abstractions.Managers;
using CryptocurrencyMarketMonitoring.Abstractions.Services;
using Microsoft.Extensions.Hosting;
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
            builder.RegisterType<CryptocurrencyOverviewManager>().As<ICryptocurrencyOverviewManager>().As<IHostedService>().SingleInstance();
            builder.RegisterType<CoinGeckoClient>().As<ICoinGeckoClient>();
            builder.RegisterType<BinanceClient>().As<IBinanceClient>().SingleInstance();
        }
    }
}
