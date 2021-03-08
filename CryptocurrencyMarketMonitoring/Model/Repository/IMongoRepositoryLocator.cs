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
    public interface IMongoRepositoryLocator
    {

        #region CRUD

        #region Delete

        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="id">id</param>
        void Delete<TEntity>(ObjectId id) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// delete entity
        /// </summary>
        /// <param name="entity">entity</param>
        void Delete<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// delete items with filter
        /// </summary>
        /// <param name="filter">expression filter</param>
        void Delete<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;

        #endregion Delete

        #region Find

        /// <summary>
        /// find entities
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>collection of entity</returns>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// find entities with paging
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter, int pageIndex, int size) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// find entities with paging and ordering
        /// default ordering is descending
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// find entities with paging and ordering in direction
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>collection of entity</returns>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending) where TEntity : IMongoDocumentBase;

        #endregion Find

        #region FindAll

        /// <summary>
        /// fetch all items in collection
        /// </summary>
        /// <returns>collection of entity</returns>
        IEnumerable<TEntity> FindAll<TEntity>() where TEntity : IMongoDocumentBase;

        /// <summary>
        /// fetch all items in collection with paging
        /// </summary>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<TEntity> FindAll<TEntity>(int pageIndex, int size) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// fetch all items in collection with paging and ordering
        /// default ordering is descending
        /// </summary>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, object>> order, int pageIndex, int size) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// fetch all items in collection with paging and ordering in direction
        /// </summary>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>collection of entity</returns>
        IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, object>> order, int pageIndex, int size, bool isDescending) where TEntity : IMongoDocumentBase;

        #endregion FindAll

        #region First

        /// <summary>
        /// get first item in collection
        /// </summary>
        /// <returns>entity of <typeparamref name="T"/></returns>
        TEntity First<TEntity>() where TEntity : IMongoDocumentBase;

        /// <summary>
        /// get first item in query
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// get first item in query with order
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// get first item in query with order and direction
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending) where TEntity : IMongoDocumentBase;

        #endregion First

        #region Get

        /// <summary>
        /// get by id
        /// </summary>
        /// <param name="id">id value</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        TEntity Get<TEntity>(ObjectId id) where TEntity : IMongoDocumentBase;

        #endregion Get

        #region Insert

        /// <summary>
        /// insert entity
        /// </summary>
        /// <param name="entity">entity</param>
        void Insert<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// insert entity collection
        /// </summary>
        /// <param name="entities">collection of entities</param>
        void Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : IMongoDocumentBase;

        #endregion Insert

        #region Last

        /// <summary>
        /// get last item in collection
        /// </summary>
        /// <returns>entity of <typeparamref name="T"/></returns>
        TEntity Last<TEntity>() where TEntity : IMongoDocumentBase;

        /// <summary>
        /// get last item in query
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        TEntity Last<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// get last item in query with order
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        TEntity Last<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// get last item in query with order and direction
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        TEntity Last<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending) where TEntity : IMongoDocumentBase;

        #endregion Last

        #region Replace

        /// <summary>
        /// replace an existing entity
        /// </summary>
        /// <param name="entity">entity</param>
        void Replace<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// replace collection of entities
        /// </summary>
        /// <param name="entities">collection of entities</param>
        void Replace<TEntity>(IEnumerable<TEntity> entities) where TEntity : IMongoDocumentBase;

        #endregion Replace

        #region Update

        /// <summary>
        /// update an entity with updated fields
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="update">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update<TEntity>(ObjectId id, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// update an entity with updated fields
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="update">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update<TEntity>(TEntity entity, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// update found entities by filter with updated fields
        /// </summary>
        /// <param name="filter">collection filter</param>
        /// <param name="update">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update<TEntity>(FilterDefinition<TEntity> filter, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// update found entities by filter with updated fields
        /// </summary>
        /// <param name="filter">collection filter</param>
        /// <param name="update">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update<TEntity>(Expression<Func<TEntity, bool>> filter, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// update a property field in an entity
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="field">field</param>
        /// <param name="value">new value</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update<TEntity, TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value) where TEntity : IMongoDocumentBase;

        /// <summary>
        /// update a property field in entities
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="filter">filter</param>
        /// <param name="field">field</param>
        /// <param name="value">new value</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update<TEntity, TField>(FilterDefinition<TEntity> filter, Expression<Func<TEntity, TField>> field, TField value) where TEntity : IMongoDocumentBase;

        #endregion Update

        #endregion CRUD

        #region Simplicity

        /// <summary>
        /// validate if filter result exists
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>true if exists, otherwise false</returns>
        bool Any<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;
        Task<bool> AnyAsync<TEntity>() where TEntity : IMongoDocumentBase;

        #endregion Simplicity

        #region index
        string CreateIndex<TEntity>(Expression<Func<TEntity, object>> field, bool desc) where TEntity : IMongoDocumentBase;
        #endregion index


        #region MongoSpecific

        /// <summary>
        /// mongo collection
        /// </summary>
        IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : IMongoDocumentBase;

        /// <summary>
        /// filter for collection
        /// </summary>
        FilterDefinitionBuilder<TEntity> GetFilter<TEntity>() where TEntity : IMongoDocumentBase;

        /// <summary>
        /// projector for collection
        /// </summary>
        ProjectionDefinitionBuilder<TEntity> GetProject<TEntity>() where TEntity : IMongoDocumentBase;

        /// <summary>
        /// updater for collection
        /// </summary>
        UpdateDefinitionBuilder<TEntity> GetUpdater<TEntity>() where TEntity : IMongoDocumentBase;



        IndexKeysDefinitionBuilder<TEntity> GetIndex<TEntity>() where TEntity : IMongoDocumentBase;

        #endregion MongoSpecific





        Task<string> CreateIndexAsync<TEntity>(Expression<Func<TEntity, object>> field, bool desc) where TEntity : IMongoDocumentBase;
        Task<string> CreateIndexAsync<TEntity>(IndexKeysDefinition<TEntity> keys, CreateIndexOptions options) where TEntity : IMongoDocumentBase;
        Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;
        Task DeleteAsync<TEntity>(ObjectId id) where TEntity : IMongoDocumentBase;
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAllAsync<TEntity>() where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, object>> order, int? pageIndex, int? size) where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, object>> order, int? pageIndex, int? size, bool isDescending) where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAllAsync<TEntity>(int pageIndex, int size) where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int? pageIndex, int? size) where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int? pageIndex, int? size, bool isDescending) where TEntity : IMongoDocumentBase;
        Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, int? pageIndex, int? size, bool isDescending, Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase;

        Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, int? pageIndex, int? size) where TEntity : IMongoDocumentBase;

        Task<TEntity> FirstAsync<TEntity>() where TEntity : IMongoDocumentBase;
        Task<TEntity> FirstAsync<TEntity>(Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase;
        Task<TEntity> FirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;
        Task<TEntity> FirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order) where TEntity : IMongoDocumentBase;
        Task<TEntity> FirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending) where TEntity : IMongoDocumentBase;
        Task<TEntity> GetAsync<TEntity>(ObjectId id) where TEntity : IMongoDocumentBase;
        Task<TEntity> GetAsync<TEntity>(ObjectId id, Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase;
        Task InsertAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : IMongoDocumentBase;
        Task InsertAsync<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase;
        Task<TEntity> LastAsync<TEntity>() where TEntity : IMongoDocumentBase;
        Task<TEntity> LastAsync<TEntity>(Expression<Func<TEntity, TEntity>> projection) where TEntity : IMongoDocumentBase;
        Task<TEntity> LastAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;
        Task<TEntity> LastAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order) where TEntity : IMongoDocumentBase;
        Task<TEntity> LastAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> order, bool isDescending) where TEntity : IMongoDocumentBase;
        Task ReplaceAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : IMongoDocumentBase;
        Task ReplaceAsync<TEntity>(TEntity entity) where TEntity : IMongoDocumentBase;
        Task<bool> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> filter, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase;
        Task<bool> UpdateAsync<TEntity>(FilterDefinition<TEntity> filter, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase;
        Task<bool> UpdateAsync<TEntity>(TEntity entity, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase;
        Task<bool> UpdateAsync<TEntity>(ObjectId id, params UpdateDefinition<TEntity>[] updates) where TEntity : IMongoDocumentBase;
        Task<bool> UpdateAsync<TEntity, TField>(FilterDefinition<TEntity> filter, Expression<Func<TEntity, TField>> field, TField value) where TEntity : IMongoDocumentBase;
        Task<bool> UpdateAsync<TEntity, TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value) where TEntity : IMongoDocumentBase;
        long GetCount<TEntity>() where TEntity : IMongoDocumentBase;
        long GetCount<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;
        Task<long> GetCountAsync<TEntity>() where TEntity : IMongoDocumentBase;
        Task<long> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IMongoDocumentBase;
    }
}
