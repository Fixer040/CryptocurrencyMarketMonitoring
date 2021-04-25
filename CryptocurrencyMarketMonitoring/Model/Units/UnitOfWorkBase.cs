using CryptocurrencyMarketMonitoring.Model.Documents;
using CryptocurrencyMarketMonitoring.Model.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Model.Units
{
    public abstract class UnitOfWorkMongoBase : IDisposable
    {
        public UnitOfWorkMongoBase(ILoggerFactory loggerFactory, IMongoRepositoryLocator locator)
        {
            _logger = loggerFactory.CreateLogger<UnitOfWorkMongoBase>();
            Locator = locator;
        }
        protected IMongoRepositoryLocator Locator { get; private set; }
        protected TResult TryExecuteCommand<TEntity, TResult>(Func<IMongoRepositoryLocator, TResult> command) where TResult : class
                                                                                                              where TEntity : IMongoDocumentBase
        {
            try
            {
                return command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }
        }
        protected async Task<TResult> TryExecuteCommandAsync<TEntity, TResult>(Func<IMongoRepositoryLocator, Task<TResult>> command) where TResult : class
                                                                                                                                     where TEntity : IMongoDocumentBase
        {
            try
            {
                return await command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }

        }
        protected bool TryExecuteCommand<TEntity>(Func<IMongoRepositoryLocator, bool> command) where TEntity : IMongoDocumentBase
        {
            try
            {
                return command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }
        }

        protected async Task<bool> TryExecuteCommandAsync<TEntity>(Func<IMongoRepositoryLocator, Task<bool>> command) where TEntity : IMongoDocumentBase
        {
            try
            {
                return await command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }
        }

        protected void ExecuteCommand<TEntity>(Action<IMongoRepositoryLocator> command) where TEntity : IMongoDocumentBase
        {
            try
            {
                command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }

        }
        protected async Task ExecuteCommandAsync<TEntity>(Func<IMongoRepositoryLocator, Task> command) where TEntity : class, IMongoDocumentBase
        {
            try
            {
                await command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }
        }

        protected TEntity ExecuteCommand<TEntity>(Func<IMongoRepositoryLocator, TEntity> command) where TEntity : class, IMongoDocumentBase
        {
            try
            {
                return command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }
        }

        protected async Task<TResult> ExecuteCommandAsync<TEntity, TResult>(Func<IMongoRepositoryLocator, Task<TResult>> command) where TEntity : class, IMongoDocumentBase
        {
            try
            {
                return await command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }
        }

        protected IEnumerable<TEntity> ExecuteCommand<TEntity>(Func<IMongoRepositoryLocator, IEnumerable<TEntity>> command) where TEntity : class, IMongoDocumentBase
        {
            try
            {
                return command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }
        }

        protected async Task<IEnumerable<TResult>> ExecuteCommandAsync<TEntity, TResult>(Func<IMongoRepositoryLocator, Task<IEnumerable<TResult>>> command) where TEntity : class, IMongoDocumentBase
        {
            try
            {
                return await command.Invoke(Locator);
            }
            catch (MongoDB.Bson.BsonSerializationException bsonException)
            {
                _logger.LogError(bsonException.Message, GetExceptionParams(bsonException));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, GetExceptionParams(exception));
                throw;
            }

        }

        #region disposing
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
                }
            }
            Locator = null;
            _isDisposed = true;
        }

        object[] GetExceptionParams(Exception exception)
        {
            object[] exceptionParams = null;
            if (exception.InnerException != null)
            {
                exceptionParams = new object[] { exception.StackTrace, exception.InnerException, exception.InnerException.StackTrace };
            }
            else
            {
                exceptionParams = new object[] { exception.StackTrace };
            }

            return exceptionParams;
        }

        ~UnitOfWorkMongoBase()
        {
            Dispose(false);
        }
        #endregion disposing
        bool _isDisposed;

        ILogger<UnitOfWorkMongoBase> _logger;
    }
}
