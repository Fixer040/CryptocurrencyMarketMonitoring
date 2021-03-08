using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Abstractions
{
    public abstract class DIContainerConfigurator : IDisposable
    {
        public DIContainerConfigurator(IHostEnvironment env)
        {
            ContentRootPath = env.ContentRootPath;
            EnvironmentName = env.EnvironmentName;
        }
        public ILifetimeScope Container { get { return _container; } set { _container = value; DIContainer.Container = _container; DIContainer.ContentRootPath = ContentRootPath;} }
        public IServiceProvider Provider { get; protected set; }
        public IServiceCollection Services { get; protected set; }
        public IConfigurationRoot Configuration { get { return _configuration; } set { _configuration = value; DIContainer.Configuration = _configuration; } }
        public List<Type> CommandTypes { get; set; } = new List<Type>();
        public void SetContentRootPath(string path)
        {
            ContentRootPath = path;
        }
        public string ContentRootPath { get; private set; }
        public string EnvironmentName { get; private set; }
        public string[] CommandLineArguments { get; private set; }
        public abstract void ConfigureServices(IServiceCollection services);
        public abstract void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory);
        #region IDisposable interface implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_container != null)
                    {
                        _container.Dispose();
                    }
                }
            }

            _isDisposed = true;
        }
        ~DIContainerConfigurator()
        {
            Dispose(false);
        }
        #endregion

        ILifetimeScope _container;
        bool _isDisposed;
        IConfigurationRoot _configuration;

    }
}
