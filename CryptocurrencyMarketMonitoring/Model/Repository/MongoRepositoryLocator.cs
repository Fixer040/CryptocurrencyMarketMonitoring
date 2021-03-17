using CryptocurrencyMarketMonitoring.Model.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Model.Repository
{
    public class MongoRepositoryLocator : IMongoRepositoryLocator
    {

        public bool Any<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Any(filter);
        }

        public void Delete<TEntity>(ObjectId id) where TEntity : IMongoDocumentBase
        {
            GetRepository<TEntity>().Delete(id);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase
        {
            GetRepository<TEntity>().Delete(entity);
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            GetRepository<TEntity>().Delete(filter);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Find(filter, projection);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Find(filter);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter, int pageIndex, int size) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Find(filter, pageIndex, size);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Find(filter, order, pageIndex, size);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Find(filter, order, pageIndex, size, isDescending);
        }

        public IEnumerable<TEntity> FindAll<TEntity>() where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().FindAll();
        }

        public IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().FindAll(projection);
        }

        public IEnumerable<TEntity> FindAll<TEntity>(int pageIndex, int size) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().FindAll(pageIndex, size);
        }

        public IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, object>> order, int pageIndex, int size) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().FindAll(order, pageIndex, size);
        }

        public IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().FindAll(order, pageIndex, size, isDescending);
        }

        public TEntity First<TEntity>() where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().First();
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().First(projection);
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().First(filter);
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().First(filter, order);
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().First(filter, order, isDescending);
        }

        public TEntity Get<TEntity>(ObjectId id) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Get(id);
        }

        public TEntity Get<TEntity>(ObjectId id, Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Get(id);
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase
        {
            GetRepository<TEntity>().Insert(entity);
        }

        public void Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : IMongoDocumentBase
        {
            GetRepository<TEntity>().Insert(entities);
        }

        public TEntity Last<TEntity>() where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Last();
        }

        public TEntity Last<TEntity>(Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Last(projection);
        }

        public TEntity Last<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Last(filter);
        }

        public TEntity Last<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Last(filter, order);
        }

        public TEntity Last<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Last(filter, order, isDescending);
        }

        public void Replace<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase
        {
            GetRepository<TEntity>().Replace(entity);
        }

        public void Replace<TEntity>(IEnumerable<TEntity> entities) where TEntity : IMongoDocumentBase
        {
            GetRepository<TEntity>().Replace(entities);
        }

        public bool Update<TEntity>(ObjectId id, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Update(id, updates);
        }

        public bool Update<TEntity>(TEntity entity, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Update(entity, updates);
        }

        public bool Update<TEntity>(FilterDefinition<TEntity> filter, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Update(filter, updates);
        }

        public bool Update<TEntity>(Expression<Func<TEntity, bool>> filter, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Update(filter, updates);
        }

        public bool Update<TEntity, TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Update(entity, field, value);
        }

        public bool Update<TEntity, TField>(FilterDefinition<TEntity> filter, Expression<Func<TEntity, TField>> field, TField value) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Update(filter, field, value);
        }

        public string CreateIndex<TEntity>(Expression<Func<TEntity, object>> field, bool desc) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().CreateIndex(field, desc);
        }


        public IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Collection;
        }

        public FilterDefinitionBuilder<TEntity> GetFilter<TEntity>() where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Filter;
        }

        public IndexKeysDefinitionBuilder<TEntity> GetIndex<TEntity>() where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Index;
        }

        public ProjectionDefinitionBuilder<TEntity> GetProject<TEntity>() where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Project;
        }

        public UpdateDefinitionBuilder<TEntity> GetUpdater<TEntity>() where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().Updater;
        }
        protected Dictionary<Type, object> RepositoryMap = new Dictionary<Type, object>();

        public IMongoRepository<TEntity> GetRepository<TEntity>() where TEntity : IMongoDocumentBase
        {
            var type = typeof(TEntity);
            if (RepositoryMap.Keys.Contains(type)) return RepositoryMap[type] as IMongoRepository<TEntity>;
            var repository = CreateRepository<TEntity>();
            RepositoryMap.Add(type, repository);
            return repository;
        }

        /// <summary>
        /// ulož typ do repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        public async Task<bool> AnyAsync<TEntity>() where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().AnyAsync();
        }

        public async Task<string> CreateIndexAsync<TEntity>(Expression<Func<TEntity, object>> field, bool desc) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().CreateIndexAsync(field, desc);
        }

        public async Task<string> CreateIndexAsync<TEntity>(IndexKeysDefinition<TEntity> keys, CreateIndexOptions options) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().CreateIndexAsync(keys, options);
        }

        public async Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            await GetRepository<TEntity>().DeleteAsync(filter);
        }

        public async Task DeleteAsync<TEntity>(ObjectId id) where TEntity : IMongoDocumentBase
        {
            await GetRepository<TEntity>().DeleteAsync(id);
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase
        {
            await GetRepository<TEntity>().DeleteAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TEntity>() where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAllAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAllAsync(projection);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, object>> order, int? pageIndex, int? size) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAllAsync(order, pageIndex, size);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, object>> order, int? pageIndex, int? size, bool isDescending) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAllAsync(order, pageIndex, size, isDescending);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TEntity>(int pageIndex, int size) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAllAsync(pageIndex, size);
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAsync(filter);
        }
        public async Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAsync(filter, projection);
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int? pageIndex, int? size) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAsync(filter, order, pageIndex, size);
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int? pageIndex, int? size, bool isDescending) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAsync(filter, order, pageIndex, size, isDescending);
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int? pageIndex, int? size, bool isDescending, Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAsync(filter, order, pageIndex, size, isDescending, projection);
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, int? pageIndex, int? size) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FindAsync(filter, pageIndex, size);
        }

        public async Task<TEntity> FirstAsync<TEntity>() where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FirstAsync();
        }

        public async Task<TEntity> FirstAsync<TEntity>(Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FirstAsync(projection);
        }

        public async Task<TEntity> FirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FirstAsync(filter);
        }

        public async Task<TEntity> FirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FirstAsync(filter, order);
        }

        public async Task<TEntity> FirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().FirstAsync(filter, order, isDescending);
        }

        public async Task<TEntity> GetAsync<TEntity>(ObjectId id) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().GetAsync(id);
        }

        public async Task<TEntity> GetAsync<TEntity>(ObjectId id, Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().GetAsync(id, projection);
        }

        public async Task InsertAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : IMongoDocumentBase
        {
            await GetRepository<TEntity>().InsertAsync(entities);
        }

        public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase
        {
            await GetRepository<TEntity>().InsertAsync(entity);
        }

        public async Task<TEntity> LastAsync<TEntity>() where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().LastAsync();
        }

        public async Task<TEntity> LastAsync<TEntity>(Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().LastAsync(projection);
        }

        public async Task<TEntity> LastAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().LastAsync(filter);
        }

        public async Task<TEntity> LastAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().LastAsync(filter, order);
        }

        public async Task<TEntity> LastAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().LastAsync(filter, order, isDescending);
        }

        public async Task ReplaceAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : IMongoDocumentBase
        {
            await GetRepository<TEntity>().ReplaceAsync(entities);
        }

        public async Task ReplaceAsync<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase
        {
            await GetRepository<TEntity>().ReplaceAsync(entity);
        }

        public async Task<bool> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> filter, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().UpdateAsync(filter, updates);
        }

        public async Task<bool> UpdateAsync<TEntity>(FilterDefinition<TEntity> filter, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().UpdateAsync(filter, updates);
        }

        public async Task<bool> UpdateAsync<TEntity>(TEntity entity, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().UpdateAsync(entity, updates);
        }

        public async Task<bool> UpdateAsync<TEntity>(ObjectId id, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().UpdateAsync(id, updates);
        }

        public async Task<bool> UpdateAsync<TEntity, TField>(FilterDefinition<TEntity> filter, Expression<Func<TEntity, TField>> field, TField value) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().UpdateAsync(filter, field, value);
        }

        public async Task<bool> UpdateAsync<TEntity, TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().UpdateAsync(entity, field, value);
        }
        public long GetCount<TEntity>() where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().GetCount();
        }
        public long GetCount<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            return GetRepository<TEntity>().GetCount(filter);
        }
        public async Task<long> GetCountAsync<TEntity>() where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().GetCountAsync();
        }
        public async Task<long> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase
        {
            return await GetRepository<TEntity>().GetCountAsync(filter);
        }

        private IMongoRepository<TEntity> CreateRepository<TEntity>() where TEntity : IMongoDocumentBase
        {
            string connectionString = DatabaseHelper<TEntity>.GetDefaultConnectionString();
            return new MongoRepository<TEntity>(connectionString);

        }
    }
}
