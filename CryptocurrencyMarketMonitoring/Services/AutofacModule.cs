using Autofac;
using AutoMapper;
using Binance.Net;
using Binance.Net.Interfaces;
using CoinGecko.Clients;
using CoinGecko.Entities.Response.Coins;
using CoinGecko.Interfaces;
using CryptocurrencyMarketMonitoring.Abstractions;
using CryptocurrencyMarketMonitoring.Abstractions.Managers;
using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Model.Documents;
using CryptocurrencyMarketMonitoring.Shared;
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





            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<IBinanceKline, BinanceChartData>();
                cfg.CreateMap<BinanceChartData, ChartDataDto>()
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.OpenTime))
                    .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.BaseVolume));
                cfg.CreateMap<CoinMarkets, OverviewDto>()
                    .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol.ToUpperInvariant()))
                    .ForMember(dest => dest.PriceChangePercentage24HInCurrency, opt => opt.MapFrom(src => src.PriceChangePercentage24HInCurrency / 100))
                    .ForMember(dest => dest.PriceChangePercentage7DInCurrency, opt => opt.MapFrom(src => src.PriceChangePercentage7DInCurrency / 100));



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
