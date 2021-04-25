using Autofac;
using AutoMapper;
using Binance.Net;
using Binance.Net.Interfaces;
using CoinGecko.Clients;
using CoinGecko.Interfaces;
using CryptocurrencyMarketMonitoring.Abstractions;
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
            builder.RegisterType<OverviewService>().As<IOverviewService>();
            builder.RegisterType<OverviewManager>().As<IOverviewManager>().As<IHostedService>().SingleInstance();
            builder.RegisterType<BinanceChartDownloadManager>().As<IBinanceChartDownloadManager>().As<IHostedService>().SingleInstance();
            builder.RegisterType<CoinGeckoClient>().As<ICoinGeckoClient>();
            builder.RegisterType<ChartDataService>().As<IChartDataService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<BinanceClient>().As<IBinanceClient>().SingleInstance();
            builder.RegisterType<PasswordHasherService>().As<IPasswordHasherService>();
            builder.RegisterType<OverviewSubscriptionManager>().As<IOverviewSubscriptionManager>().SingleInstance();
        }
    }
}
