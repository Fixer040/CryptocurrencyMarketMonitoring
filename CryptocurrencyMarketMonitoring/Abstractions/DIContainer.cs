using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CryptocurrencyMarketMonitoring.Abstractions
{
    public static class DIContainer
    {
        public static string ContentRootPath { get; set; }
        public static ILifetimeScope Container { private get; set; }
        public static IConfiguration Configuration { get; set; }
        public static bool IsRegistered<TService>()
        {
            return Container.IsRegistered<TService>();
        }
        public static bool IsRegisteredWithName<TService>(string name)
        {
            return Container.IsRegisteredWithName<TService>(name);
        }
        public static bool IsRegisteredWithKey<TService>(object key)
        {
            return Container.IsRegisteredWithKey<TService>(key);
        }
        public static IEnumerable<TService> GetAll<TService>()
        {
            IEnumerable<TService> retval = default;
            if (!Container.IsRegistered<IEnumerable<TService>>()) return retval;
            return Container.Resolve<IEnumerable<TService>>();
        }
        public static IEnumerable<TService> GetAll<TService>(params Autofac.Core.Parameter[] parameters)
        {
            IEnumerable<TService> retval = default;
            if (!Container.IsRegistered<IEnumerable<TService>>()) return retval;
            return Container.Resolve<IEnumerable<TService>>(parameters);
        }
        public static IEnumerable<TService> GetAllNamed<TService>(string name)
        {
            IEnumerable<TService> retval = default;
            if (!Container.IsRegisteredWithName<IEnumerable<TService>>(name)) return retval;
            return Container.ResolveNamed<IEnumerable<TService>>(name);
        }
        public static IEnumerable<TService> GetAllNamed<TService>(string name, params Autofac.Core.Parameter[] parameters)
        {
            IEnumerable<TService> retval = default;
            if (!Container.IsRegisteredWithName<IEnumerable<TService>>(name)) return retval;
            return Container.ResolveKeyed<IEnumerable<TService>>(name, parameters);
        }

        public static void Shutdown()
        {
            if (Container != null)
            {
                Container.Dispose();

                Container = null;
            }
        }

        public static IEnumerable<TService> GetAllKeyed<TService>(object key)
        {
            IEnumerable<TService> retval = default;
            if (!Container.IsRegisteredWithKey<IEnumerable<TService>>(key)) return retval;
            return Container.ResolveKeyed<IEnumerable<TService>>(key);
        }
        public static IEnumerable<TService> GetAllKeyed<TService>(object key, params Autofac.Core.Parameter[] parameters)
        {
            IEnumerable<TService> retval = default;
            if (!Container.IsRegisteredWithKey<IEnumerable<TService>>(key)) return retval;
            return Container.ResolveKeyed<IEnumerable<TService>>(key, parameters);
        }
        public static ILifetimeScope BeginScope()
        {
            return Container.BeginLifetimeScope();
        }
        public static TService GetScoped<TService>(ILifetimeScope scope) where TService : class
        {
            TService retval = default;
            scope.TryResolve<TService>(out retval);
            return retval;
        }
        public static TService GetScopedNamed<TService>(ILifetimeScope scope, string name)
        {
            TService retval = default;
            if (!scope.IsRegisteredWithName<TService>(name)) return retval;
            return scope.ResolveNamed<TService>(name);
        }
        public static TService GetScopedNamed<TService>(ILifetimeScope scope, string name, params Autofac.Core.Parameter[] parameters)
        {
            TService retval = default;
            if (!scope.IsRegisteredWithName<TService>(name)) return retval;
            return scope.ResolveNamed<TService>(name, parameters);
        }
        public static TService GetScopedKeyed<TService>(ILifetimeScope scope, object key)
        {
            TService retval = default;
            if (!scope.IsRegisteredWithKey<TService>(key)) return retval;
            return scope.ResolveKeyed<TService>(key);
        }
        public static TService GetScopedKeyed<TService>(ILifetimeScope scope, object key, params Autofac.Core.Parameter[] parameters)
        {
            TService retval = default;
            if (!scope.IsRegisteredWithKey<TService>(key)) return retval;
            return scope.ResolveKeyed<TService>(key, parameters);
        }
        public static TService BeginScopeService<TService>() where TService : class
        {
            TService retval = default;
            var scope = Container.BeginLifetimeScope();
            scope.TryResolve<TService>(out retval);
            return retval;
        }

        public static TService BeginScopeService<TService>(params Autofac.Core.Parameter[] parameters)
        {
            TService retval = default;
            var scope = Container.BeginLifetimeScope();
            if (!scope.IsRegistered<TService>()) return retval;
            scope.Resolve<TService>(parameters);
            return retval;
        }

        public static TService BeginScopeServiceNamed<TService>(string name)
        {
            TService retval = default;
            var scope = Container.BeginLifetimeScope();
            if (!scope.IsRegisteredWithName<TService>(name)) return retval;
            return scope.ResolveNamed<TService>(name);
        }
        public static TService BeginScopeServiceNamed<TService>(string name, params Autofac.Core.Parameter[] parameters)
        {
            TService retval = default;
            var scope = Container.BeginLifetimeScope();
            if (!scope.IsRegisteredWithName<TService>(name)) return retval;
            return scope.ResolveNamed<TService>(name, parameters);
        }
        public static TService BeginScopeServiceKeyed<TService>(object key)
        {
            TService retval = default;
            var scope = Container.BeginLifetimeScope();
            if (!scope.IsRegisteredWithKey<TService>(key)) return retval;
            return scope.ResolveKeyed<TService>(key);
        }
        public static TService BeginScopeServiceKeyed<TService>(object key, params Autofac.Core.Parameter[] parameters)
        {
            TService retval = default;
            var scope = Container.BeginLifetimeScope();
            if (!scope.IsRegisteredWithKey<TService>(key)) return retval;
            return scope.ResolveKeyed<TService>(key, parameters);
        }
        public static TService Get<TService>() where TService : class
        {
            TService retval = default;
            Container.TryResolve<TService>(out retval);
            return retval;
        }
        public static TService Get<TService>(params Autofac.Core.Parameter[] parameters)
        {
            TService retval = default;
            if (!Container.IsRegistered<TService>()) return retval;
            return Container.Resolve<TService>(parameters);
        }
        public static TService GetNamed<TService>(string name)
        {
            TService retval = default;
            if (!Container.IsRegisteredWithName<TService>(name)) return retval;
            return Container.ResolveNamed<TService>(name);
        }
        public static TService GetNamed<TService>(string name, params Autofac.Core.Parameter[] parameters)
        {
            TService retval = default;
            if (!Container.IsRegisteredWithName<TService>(name)) return retval;
            return Container.ResolveNamed<TService>(name, parameters);
        }
        public static TService GetKeyd<TService>(object key)
        {
            TService retval = default;
            if (!Container.IsRegisteredWithKey<TService>(key)) return retval;
            return Container.ResolveKeyed<TService>(key);
        }
        public static TService GetKeyd<TService>(object key, params Autofac.Core.Parameter[] parameters)
        {
            TService retval = default;
            if (!Container.IsRegisteredWithKey<TService>(key)) return retval;
            return Container.ResolveKeyed<TService>(key, parameters);
        }
        public static object Get(Type serviceType)
        {
            object retval = null;
            if (!Container.IsRegistered(serviceType)) return retval;
            return Container.Resolve(serviceType);
        }
        public static object Get(Type serviceType, params Autofac.Core.Parameter[] parameters)
        {
            object retval = null;
            if (!Container.IsRegistered(serviceType)) return retval;
            return Container.Resolve(serviceType, parameters);
        }
        public static object GetNamed(Type serviceType, string name)
        {
            object retval = null;
            if (!Container.IsRegisteredWithName(name, serviceType)) return retval;
            return Container.ResolveNamed(name, serviceType);
        }
        public static object GetNamed(Type serviceType, string name, params Autofac.Core.Parameter[] parameters)
        {
            object retval = null;
            if (!Container.IsRegisteredWithName(name, serviceType)) return retval;
            return Container.ResolveNamed(name, serviceType, parameters);
        }
        public static object GetKeyd(Type serviceType, object key)
        {
            object retval = null;
            if (!Container.IsRegisteredWithKey(key, serviceType)) return retval;
            return Container.ResolveKeyed(key, serviceType);
        }
        public static object GetKeyd(Type serviceType, object key, params Autofac.Core.Parameter[] parameters)
        {
            object retval = null;
            if (!Container.IsRegisteredWithKey(key, serviceType)) return retval;
            return Container.ResolveKeyed(key, serviceType, parameters);
        }
        public static void Release()
        {
            Container.Dispose();
        }
        public static IServiceProvider CreateServiceProvider()
        {
            return new Autofac.Extensions.DependencyInjection.AutofacServiceProvider(Container);
        }
    }
}
