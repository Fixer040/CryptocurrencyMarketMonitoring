using Autofac;
using AutoMapper;
using Binance.Net.Interfaces;
using CoinGecko.Entities.Response.Coins;
using CryptocurrencyMarketMonitoring.Abstractions.Units;
using CryptocurrencyMarketMonitoring.Model.Documents;
using CryptocurrencyMarketMonitoring.Model.Repository;
using CryptocurrencyMarketMonitoring.Model.Units;
using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Model
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserUnit>().As<IUserUnit<User>>().InstancePerLifetimeScope();
            builder.RegisterType<BinanceChartDataUnit>().As<IBinanceChartDataUnit<BinanceChartData>>().InstancePerLifetimeScope();
            builder.RegisterType<BinanceLastDownloadedPairUnit>().As<IBinanceLastDownloadedPairUnit<BinanceLastDownloadedPair>>().InstancePerLifetimeScope();

            builder.RegisterType<MongoRepositoryLocator>().As<IMongoRepositoryLocator>();


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
