using Autofac;
using AutoMapper;
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
            builder.RegisterType<ChartDataService>().As<IChartDataService>();
            builder.RegisterType<UserService>().As<IUserService>();

            builder.RegisterType<BinanceClient>().As<IBinanceClient>().SingleInstance();


            builder.Register(context => new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<MyModel, MyDto>;
                //etc...
            })).AsSelf().SingleInstance();


            builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();
        }
    }
}
