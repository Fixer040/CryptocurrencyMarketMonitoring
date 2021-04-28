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

        }
    }
}
