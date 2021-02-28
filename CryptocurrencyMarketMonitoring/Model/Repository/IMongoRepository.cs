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
    public interface IMongoRepository<TDocument> where TDocument : IMongoDocumentBase
    {
        IMongoCollection<TDocument> Collection { get; }
        FilterDefinitionBuilder<TDocument> Filter { get; }
        IndexKeysDefinitionBuilder<TDocument> Index { get; }
        ProjectionDefinitionBuilder<TDocument> Project { get; }
        UpdateDefinitionBuilder<TDocument> Updater { get; }

        bool Any(Expression<Func<TDocument, bool>> filter);
        Task<bool> AnyAsync();
        string CreateIndex(Expression<Func<TDocument, object>> field, bool desc);
        Task<string> CreateIndexAsync(Expression<Func<TDocument, object>> field, bool desc);
        Task<string> CreateIndexAsync(IndexKeysDefinition<TDocument> keys, CreateIndexOptions options);
        void Delete(Expression<Func<TDocument, bool>> filter);
        void Delete(TDocument entity);
        void Delete(ObjectId id);
        Task DeleteAsync(Expression<Func<TDocument, bool>> filter);
        Task DeleteAsync(ObjectId id);
        Task DeleteAsync(TDocument entity);
        IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter);
        IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TDocument>> projection);
        IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size);
        IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending);
        IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, int? pageIndex, int? size);
        IEnumerable<TDocument> FindAll();
        IEnumerable<TDocument> FindAll(Expression<Func<TDocument, TDocument>> projection);
        IEnumerable<TDocument> FindAll(Expression<Func<TDocument, object>> order, int? pageIndex, int? size);
        IEnumerable<TDocument> FindAll(Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending);
        IEnumerable<TDocument> FindAll(int? pageIndex, int? size);
        Task<IEnumerable<TDocument>> FindAllAsync();
        Task<IEnumerable<TDocument>> FindAllAsync(Expression<Func<TDocument, TDocument>> projection);
        Task<IEnumerable<TDocument>> FindAllAsync(Expression<Func<TDocument, object>> order, int? pageIndex, int? size);
        Task<IEnumerable<TDocument>> FindAllAsync(Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending);
        Task<IEnumerable<TDocument>> FindAllAsync(int? pageIndex, int? size);
        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter);
        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TDocument>> projection);
        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size);
        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending);
        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending, Expression<Func<TDocument, TDocument>> projection);

        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, int? pageIndex, int? size);

        TDocument First();
        TDocument First(Expression<Func<TDocument, TDocument>> projection);
        TDocument First(Expression<Func<TDocument, bool>> filter);
        TDocument First(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order);
        TDocument First(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending);
        Task<TDocument> FirstAsync();
        Task<TDocument> FirstAsync(Expression<Func<TDocument, TDocument>> projection);
        Task<TDocument> FirstAsync(Expression<Func<TDocument, bool>> filter);
        Task<TDocument> FirstAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order);
        Task<TDocument> FirstAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending);
        TDocument Get(ObjectId id);
        TDocument Get(ObjectId id, Expression<Func<TDocument, TDocument>> projection);
        Task<TDocument> GetAsync(ObjectId id);
        Task<TDocument> GetAsync(ObjectId id, Expression<Func<TDocument, TDocument>> projection);
        void Insert(IEnumerable<TDocument> entities);
        void Insert(TDocument entity);
        Task InsertAsync(IEnumerable<TDocument> entities);
        Task InsertAsync(TDocument entity);
        TDocument Last();
        TDocument Last(Expression<Func<TDocument, TDocument>> projection);
        TDocument Last(Expression<Func<TDocument, bool>> filter);
        TDocument Last(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order);
        TDocument Last(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending);
        Task<TDocument> LastAsync();
        Task<TDocument> LastAsync(Expression<Func<TDocument, TDocument>> projection);
        Task<TDocument> LastAsync(Expression<Func<TDocument, bool>> filter);
        Task<TDocument> LastAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order);
        Task<TDocument> LastAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending);
        void Replace(IEnumerable<TDocument> entities);
        void Replace(TDocument entity);
        Task ReplaceAsync(IEnumerable<TDocument> entities);
        Task ReplaceAsync(TDocument entity);
        bool Update(Expression<Func<TDocument, bool>> filter, params UpdateDefinition<TDocument>[] updates);
        bool Update(FilterDefinition<TDocument> filter, params UpdateDefinition<TDocument>[] updates);
        bool Update(TDocument entity, params UpdateDefinition<TDocument>[] updates);
        bool Update(ObjectId id, params UpdateDefinition<TDocument>[] updates);
        bool Update<TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value);
        bool Update<TField>(TDocument entity, Expression<Func<TDocument, TField>> field, TField value);
        Task<bool> UpdateAsync(Expression<Func<TDocument, bool>> filter, params UpdateDefinition<TDocument>[] updates);
        Task<bool> UpdateAsync(FilterDefinition<TDocument> filter, params UpdateDefinition<TDocument>[] updates);
        Task<bool> UpdateAsync(TDocument entity, params UpdateDefinition<TDocument>[] updates);
        Task<bool> UpdateAsync(ObjectId id, params UpdateDefinition<TDocument>[] updates);
        Task<bool> UpdateAsync<TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value);
        Task<bool> UpdateAsync<TField>(TDocument entity, Expression<Func<TDocument, TField>> field, TField value);

        long GetCount();
        long GetCount(Expression<Func<TDocument, bool>> filter);
        Task<long> GetCountAsync();
        Task<long> GetCountAsync(Expression<Func<TDocument, bool>> filter);

    }
}
