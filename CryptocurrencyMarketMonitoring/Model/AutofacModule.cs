using Autofac;
using CryptocurrencyMarketMonitoring.Model.Repository;
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
            builder.RegisterType<IMongoRepositoryLocator>().As<IMongoRepositoryLocator>();
        }
    }
}
