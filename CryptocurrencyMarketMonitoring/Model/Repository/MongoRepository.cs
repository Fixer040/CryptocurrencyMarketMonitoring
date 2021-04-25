using CryptocurrencyMarketMonitoring.Model.Documents;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Model.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IMongoDocumentBase
    {
        #region MongoSpecific 

        static MongoRepository()
        {
            ConventionPack pack = new ConventionPack();
            pack.Add(new IgnoreIfNullConvention(true));

            ConventionRegistry.Register("Ignore null properties of data", pack, t => true);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="connectionString">connection string</param>
        public MongoRepository(string connectionString, params string[] collectionNameParams)
        {
            Collection = DatabaseHelper<TDocument>.GetCollectionFromConnectionString(connectionString, collectionNameParams);
        }

        /// <summary>
        /// mongo collection
        /// </summary>
        public IMongoCollection<TDocument> Collection
        {
            get; private set;
        }

        /// <summary>
        /// filter for collection
        /// </summary>
        public FilterDefinitionBuilder<TDocument> Filter
        {
            get
            {
                return Builders<TDocument>.Filter;
            }
        }

        public IndexKeysDefinitionBuilder<TDocument> Index
        {
            get
            {
                return Builders<TDocument>.IndexKeys;
            }
        }

        /// <summary>
        /// projector for collection
        /// </summary>
        public ProjectionDefinitionBuilder<TDocument> Project
        {
            get
            {
                return Builders<TDocument>.Projection;
            }
        }

        /// <summary>
        /// updater for collection
        /// </summary>
        public UpdateDefinitionBuilder<TDocument> Updater
        {
            get
            {
                return Builders<TDocument>.Update;
            }
        }

        private IFindFluent<TDocument, TDocument> Query(Expression<Func<TDocument, bool>> filter)
        {
            if (filter == null)
            {
                return Query();
            }
            else
            {
                return Collection.Find(filter);
            }
        }

        private IFindFluent<TDocument, TDocument> Query()
        {
            return Collection.Find(Filter.Empty);
        }
        public long GetCount()
        {
            return Collection.CountDocuments(Filter.Empty);
        }
        public long GetCount(Expression<Func<TDocument, bool>> filter)
        {
            return Collection.CountDocuments(filter);
        }
        public async Task<long> GetCountAsync()
        {
            return await Collection.CountDocumentsAsync(Filter.Empty);
        }
        public async Task<long> GetCountAsync(Expression<Func<TDocument, bool>> filter)
        {
            return await Collection.CountDocumentsAsync(filter);
        }
        #endregion MongoSpecific

        #region CRUD

        #region Delete

        /// <summary>
        /// delete entity
        /// </summary>
        /// <param name="entity">entity</param>
        public void Delete(TDocument entity)
        {
            Delete(entity.Id);
        }
        public async Task DeleteAsync(TDocument entity)
        {
            await DeleteAsync(entity.Id);
        }

        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="id">id</param>
        public virtual void Delete(ObjectId id)
        {
            Retry(() =>
            {
                return Collection.DeleteOne(i => i.Id.Equals(id));
            });
        }
        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="id">id</param>
        public virtual async Task DeleteAsync(ObjectId id)
        {
            await RetryAsync(async () =>
            {
                return await Collection.DeleteOneAsync(i => i.Id.Equals(id));
            });
        }

        /// <summary>
        /// delete items with filter
        /// </summary>
        /// <param name="filter">expression filter</param>
        public void Delete(Expression<Func<TDocument, bool>> filter)
        {
            Retry(() =>
            {
                return Collection.DeleteMany(filter);
            });
        }

        public async Task DeleteAsync(Expression<Func<TDocument, bool>> filter)
        {
            await RetryAsync(async () =>
            {
                return await Collection.DeleteManyAsync(filter);
            });
        }

        #endregion Delete

        #region Find
        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter)
        {
            return Query(filter).ToList();
        }
        public virtual async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter)
        {
            return await Query(filter).ToListAsync();
        }
        public IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, int? pageIndex, int? size)
        {
            return Find(filter, i => i.Id, pageIndex, size);
        }
        public async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, int? pageIndex, int? size)
        {
            return await FindAsync(filter, i => i.Id, pageIndex, size);
        }
        public IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size)
        {
            return Find(filter, order, pageIndex, size, true);
        }
        public async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size)
        {
            return await FindAsync(filter, order, pageIndex, size, true);
        }
        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending)
        {
            return Retry(() =>
            {
                var query = Query(filter).Skip(pageIndex * size).Limit(size);
                return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToList();
            });
        }
        public IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TDocument>> projection)
        {
            return Retry(() =>
            {
                var finalProjection = Builders<TDocument>.Projection.Expression(projection);
                var query = Query(filter).Project<TDocument>(finalProjection);

                return query.ToList();
            });
        }
        public virtual async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending)
        {
            return await RetryAsync(async () =>
            {
                var query = Query(filter).Skip(pageIndex * size).Limit(size);
                var orderedFindFluent = (isDescending ? query.SortByDescending(order) : query.SortBy(order));
                return await orderedFindFluent.ToListAsync();
            });
        }

        public virtual async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending, Expression<Func<TDocument, TDocument>> projection)
        {
            return await RetryAsync(async () =>
            {
                var finalProjection = Builders<TDocument>.Projection.Expression(projection);

                var query = Query(filter).Skip(pageIndex * size).Limit(size);
                var orderedFindFluent = (isDescending ? query.SortByDescending(order) : query.SortBy(order));
                return await orderedFindFluent.Project<TDocument>(finalProjection).ToListAsync();
            });
        }


        public virtual async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TDocument>> projection)
        {
            return await RetryAsync(async () =>
            {
                var finalProjection = Builders<TDocument>.Projection.Expression(projection);
                var query = Query(filter).Project<TDocument>(finalProjection);
                return await query.ToListAsync();
            });
        }

        #endregion Find

        #region FindAll
        public IEnumerable<TDocument> FindAll()
        {
            return Retry(() =>
            {
                return Query().ToList();
            });
        }
        public IEnumerable<TDocument> FindAll(Expression<Func<TDocument, TDocument>> projection)
        {
            return Retry(() =>
            {
                var finalProjection = Builders<TDocument>.Projection.Expression(projection);
                return Query().Project(finalProjection).ToList();
            });
        }
        public async Task<IEnumerable<TDocument>> FindAllAsync()
        {
            return await RetryAsync(async () =>
            {
                return await Query().ToListAsync();
            });
        }
        public async Task<IEnumerable<TDocument>> FindAllAsync(Expression<Func<TDocument, TDocument>> projection)
        {
            return await RetryAsync(async () =>
            {
                var finalProjection = Builders<TDocument>.Projection.Expression(projection);
                return await Query().Project(finalProjection).ToListAsync();
            });
        }
        public IEnumerable<TDocument> FindAll(int? pageIndex, int? size)
        {
            return FindAll(i => i.Id, pageIndex, size);
        }
        public async Task<IEnumerable<TDocument>> FindAllAsync(int? pageIndex, int? size)
        {
            return await FindAllAsync(i => i.Id, pageIndex, size);
        }
        public IEnumerable<TDocument> FindAll(Expression<Func<TDocument, object>> order, int? pageIndex, int? size)
        {
            return FindAll(order, pageIndex, size, true);
        }
        public async Task<IEnumerable<TDocument>> FindAllAsync(Expression<Func<TDocument, object>> order, int? pageIndex, int? size)
        {
            return await FindAllAsync(order, pageIndex, size, true);
        }
        public IEnumerable<TDocument> FindAll(Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending)
        {
            return Retry(() =>
            {
                var query = Query().Skip(pageIndex * size).Limit(size);
                return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToList();
            });
        }
        public async Task<IEnumerable<TDocument>> FindAllAsync(Expression<Func<TDocument, object>> order, int? pageIndex, int? size, bool isDescending)
        {
            return await RetryAsync(async () =>
            {
                var query = Query().Skip(pageIndex * size).Limit(size);
                return await (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToListAsync();
            });
        }
        #endregion FindAll

        #region First
        public TDocument First()
        {
            return FindAll(i => i.Id, 0, 1, false).FirstOrDefault();
        }
        public TDocument First(Expression<Func<TDocument, TDocument>> projection)
        {
            return FindAll(projection).OrderBy(x => x.Id).FirstOrDefault();
        }
        public async Task<TDocument> FirstAsync()
        {
            return await RetryAsync(async () =>
            {
                var query = Query().Skip(0).Limit(1);
                var orderedFindFluent = (query.SortBy(i => i.Id));
                return await orderedFindFluent.FirstOrDefaultAsync();
            });
        }
        public async Task<TDocument> FirstAsync(Expression<Func<TDocument, TDocument>> projection)
        {
            return await RetryAsync(async () =>
            {
                var finalProjection = Builders<TDocument>.Projection.Expression(projection);
                var query = Query().Project(finalProjection).Skip(0).Limit(1);
                var orderedFindFluent = (query.SortBy(i => i.Id));
                return await orderedFindFluent.FirstOrDefaultAsync();
            });
        }
        public TDocument First(Expression<Func<TDocument, bool>> filter)
        {
            return First(filter, i => i.Id);
        }
        public async Task<TDocument> FirstAsync(Expression<Func<TDocument, bool>> filter)
        {
            return await FirstAsync(filter, i => i.Id);
        }
        public TDocument First(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order)
        {
            return First(filter, order, false);
        }
        public async Task<TDocument> FirstAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order)
        {
            return await FirstAsync(filter, order, false);
        }
        public TDocument First(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending)
        {
            return Find(filter, order, 0, 1, isDescending).SingleOrDefault();
        }
        public async Task<TDocument> FirstAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending)
        {
            return await RetryAsync(async () =>
            {
                var query = Query(filter).Skip(0).Limit(1);
                var orderedFindFluent = (isDescending ? query.SortByDescending(order) : query.SortBy(order));
                return await orderedFindFluent.FirstOrDefaultAsync();
            });
        }
        #endregion First

        #region Get

        /// <summary>
        /// get by id
        /// </summary>
        /// <param name="id">id value</param>
        /// <returns>entity of <typeparamref name="TDocument"/></returns>
        public virtual TDocument Get(ObjectId id)
        {
            return Retry(() =>
            {
                return Find(i => i.Id.Equals(id)).FirstOrDefault();
            });
        }

        public virtual TDocument Get(ObjectId id, Expression<Func<TDocument, TDocument>> projection)
        {
            return Retry(() =>
            {
                return Find(i => i.Id.Equals(id), projection).FirstOrDefault();
            });
        }

        public virtual async Task<TDocument> GetAsync(ObjectId id)
        {
            return await RetryAsync(async () =>
            {
                var query = Query(f => f.Id.Equals(id)).Skip(0).Limit(1);
                var orderedFindFluent = (query.SortBy(o => o.Id));
                return await orderedFindFluent.FirstOrDefaultAsync();
            });
        }

        public virtual async Task<TDocument> GetAsync(ObjectId id, Expression<Func<TDocument, TDocument>> projection)
        {
            return await RetryAsync(async () =>
            {
                var finalProjection = Builders<TDocument>.Projection.Expression(projection);
                var query = Query(f => f.Id.Equals(id)).Project(finalProjection).Skip(0).Limit(1);
                var orderedFindFluent = (query.SortBy(o => o.Id));
                return await orderedFindFluent.FirstOrDefaultAsync();
            });
        }

        #endregion Get

        #region Insert

        /// <summary>
        /// insert entity
        /// </summary>
        /// <param name="entity">entity</param>
        public virtual void Insert(TDocument entity)
        {
            Retry(() =>
            {
                Collection.InsertOne(entity);
                return true;
            });
        }

        public virtual async Task InsertAsync(TDocument entity)
        {
            await RetryAsync(async () =>
            {
                await Collection.InsertOneAsync(entity);
                return true;
            });
        }

        /// <summary>
        /// insert entity collection
        /// </summary>
        /// <param name="entities">collection of entities</param>
        public virtual void Insert(IEnumerable<TDocument> entities)
        {
            Retry(() =>
            {
                Collection.InsertMany(entities, new InsertManyOptions() { IsOrdered = false });
                return true;
            });
        }

        public virtual async Task InsertAsync(IEnumerable<TDocument> entities)
        {
            await RetryAsync(async () =>
            {
                await Collection.InsertManyAsync(entities, new InsertManyOptions() { IsOrdered = false });
                return true;
            });
        }

        #endregion Insert

        #region Last
        /// <summary>
        /// get last item in collection
        /// </summary>
        /// <returns>entity of <typeparamref name="TDocument"/></returns>
        public TDocument Last()
        {
            return FindAll(i => i.Id, 0, 1, false).LastOrDefault();
        }
        public TDocument Last(Expression<Func<TDocument, TDocument>> projection)
        {
            return FindAll(projection).OrderBy(x => x.Id).LastOrDefault();
        }
        public async Task<TDocument> LastAsync()
        {
            return await RetryAsync(async () =>
            {
                var query = Query().Skip(0).Limit(1);
                var orderedFindFluent = (query.SortByDescending(i => i.Id));
                return await orderedFindFluent.FirstOrDefaultAsync();
            });
        }
        public async Task<TDocument> LastAsync(Expression<Func<TDocument, TDocument>> projection)
        {
            return await RetryAsync(async () =>
            {
                var finalProjection = Builders<TDocument>.Projection.Expression(projection);
                var query = Query().Project(finalProjection).Skip(0).Limit(1);
                var orderedFindFluent = (query.SortByDescending(i => i.Id));
                return await orderedFindFluent.FirstOrDefaultAsync();
            });
        }
        /// <summary>
        /// get last item in query
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>entity of <typeparamref name="TDocument"/></returns>
        public TDocument Last(Expression<Func<TDocument, bool>> filter)
        {
            return Last(filter, i => i.Id);
        }
        public async Task<TDocument> LastAsync(Expression<Func<TDocument, bool>> filter)
        {
            return await LastAsync(filter, i => i.Id);
        }
        /// <summary>
        /// get last item in query with order
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <returns>entity of <typeparamref name="TDocument"/></returns>
        public TDocument Last(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order)
        {
            return Last(filter, order, false);
        }
        public async Task<TDocument> LastAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order)
        {
            return await LastAsync(filter, order, false);
        }
        /// <summary>
        /// get last item in query with order and direction
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">ordering parameters</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>entity of <typeparamref name="TDocument"/></returns>
        public TDocument Last(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending)
        {
            return First(filter, order, !isDescending);
        }
        public async Task<TDocument> LastAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending)
        {
            return await FirstAsync(filter, order, !isDescending);
        }
        #endregion Last

        #region Replace

        /// <summary>
        /// replace an existing entity
        /// </summary>
        /// <param name="entity">entity</param>
        public virtual void Replace(TDocument entity)
        {
            Retry(() =>
            {
                return Collection.ReplaceOne(i => i.Id.Equals(entity.Id), entity);
            });
        }

        public virtual async Task ReplaceAsync(TDocument entity)
        {
            await RetryAsync(async () =>
            {
                return await Collection.ReplaceOneAsync(i => i.Id.Equals(entity.Id), entity);
            });
        }

        /// <summary>
        /// replace collection of entities
        /// </summary>
        /// <param name="entities">collection of entities</param>
        public void Replace(IEnumerable<TDocument> entities)
        {
            foreach (TDocument entity in entities)
            {
                Replace(entity);
            }
        }


        public async Task ReplaceAsync(IEnumerable<TDocument> entities)
        {
            foreach (TDocument entity in entities)
            {
                await ReplaceAsync(entity);
            }
        }

        #endregion Replace

        #region Update

        /// <summary>
        /// update a property field in an entity
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="field">field</param>
        /// <param name="value">new value</param>
        /// <returns>true if successful, otherwise false</returns>
        public bool Update<TField>(TDocument entity, Expression<Func<TDocument, TField>> field, TField value)
        {
            return Update(entity, Updater.Set(field, value));
        }

        public async Task<bool> UpdateAsync<TField>(TDocument entity, Expression<Func<TDocument, TField>> field, TField value)
        {
            return await UpdateAsync(entity, Updater.Set(field, value));
        }

        /// <summary>
        /// update an entity with updated fields
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="update">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public virtual bool Update(ObjectId id, params UpdateDefinition<TDocument>[] updates)
        {
            return Update(Filter.Eq(i => i.Id, id), updates);
        }
        public virtual async Task<bool> UpdateAsync(ObjectId id, params UpdateDefinition<TDocument>[] updates)
        {
            return await UpdateAsync(Filter.Eq(i => i.Id, id), updates);
        }

        /// <summary>
        /// update an entity with updated fields
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="update">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public virtual bool Update(TDocument entity, params UpdateDefinition<TDocument>[] updates)
        {
            return Update(entity.Id, updates);
        }
        public virtual async Task<bool> UpdateAsync(TDocument entity, params UpdateDefinition<TDocument>[] updates)
        {
            return await UpdateAsync(entity.Id, updates);
        }

        /// <summary>
        /// update a property field in entities
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="filter">filter</param>
        /// <param name="field">field</param>
        /// <param name="value">new value</param>
        /// <returns>true if successful, otherwise false</returns>
        public bool Update<TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value)
        {
            return Update(filter, Updater.Set(field, value));
        }

        public async Task<bool> UpdateAsync<TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value)
        {
            return await UpdateAsync(filter, Updater.Set(field, value));
        }

        /// <summary>
        /// update found entities by filter with updated fields
        /// </summary>
        /// <param name="filter">collection filter</param>
        /// <param name="update">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public bool Update(FilterDefinition<TDocument> filter, params UpdateDefinition<TDocument>[] updates)
        {
            return Retry(() =>
            {
                var update = Updater.Combine(updates);
                return Collection.UpdateMany(filter, update).IsAcknowledged;
            });
        }
        public async Task<bool> UpdateAsync(FilterDefinition<TDocument> filter, params UpdateDefinition<TDocument>[] updates)
        {
            return await RetryAsync(async () =>
            {
                var update = Updater.Combine(updates);
                var result = await Collection.UpdateManyAsync(filter, update);
                return result.IsAcknowledged;
            });
        }

        /// <summary>
        /// update found entities by filter with updated fields
        /// </summary>
        /// <param name="filter">collection filter</param>
        /// <param name="update">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        public bool Update(Expression<Func<TDocument, bool>> filter, params UpdateDefinition<TDocument>[] updates)
        {
            return Retry(() =>
            {
                //var update = Updater.Combine(updates).Set("_m", DateTime.UtcNow.ToEpochTime());
                var update = Updater.Combine(updates).Set("_m", DateTime.UtcNow);
                return Collection.UpdateMany(filter, update).IsAcknowledged;
            });
        }

        public async Task<bool> UpdateAsync(Expression<Func<TDocument, bool>> filter, params UpdateDefinition<TDocument>[] updates)
        {
            return await RetryAsync(async () =>
            {
                var update = Updater.Combine(updates).Set("_m", DateTime.UtcNow);
                var retval = await Collection.UpdateManyAsync(filter, update);
                return retval.IsAcknowledged;
            });
        }

        #endregion Update

        #endregion CRUD

        #region Simplicity

        /// <summary>
        /// validate if filter result exists
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>true if exists, otherwise false</returns>
        public bool Any(Expression<Func<TDocument, bool>> filter)
        {
            return Retry(() =>
            {
                return Collection.AsQueryable<TDocument>().Any(filter);
            });
        }

        /// <summary>
        /// validate if filter result exists
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>true if exists, otherwise false</returns>
        public async Task<bool> AnyAsync()
        {
            return await RetryAsync(async () =>
            {
                return await Collection.AsQueryable<TDocument>().AnyAsync();
            });
        }

        #endregion Simplicity

        #region RetryPolicy
        /// <summary>
        /// retry operation for three times if IOException occurs
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="action">action</param>
        /// <returns>action result</returns>
        /// <example>
        /// return Retry(() => 
        /// { 
        ///     do_something;
        ///     return something;
        /// });
        /// </example>
        protected virtual TResult Retry<TResult>(Func<TResult> action)
        {
            return RetryPolicy
                .Handle<MongoConnectionException>(i => i.InnerException.GetType() == typeof(IOException))
                .Retry(3)
                .Execute(action);
        }


        protected virtual async Task<TResult> RetryAsync<TResult>(Func<Task<TResult>> action)
        {
            return await RetryPolicy
                .Handle<MongoConnectionException>(i => i.InnerException.GetType() == typeof(IOException))
                .RetryAsync(3)
                .ExecuteAsync(action);
        }
        #endregion


        #region index


        public string CreateIndex(Expression<Func<TDocument, object>> field, bool desc)
        {
            IndexKeysDefinition<TDocument> keys = default(IndexKeysDefinition<TDocument>);

            if (desc)
                keys = this.Index.Descending(field);
            else
                keys = this.Index.Ascending(field);

            CreateIndexModel<TDocument> model = new CreateIndexModel<TDocument>(keys);

            return Retry(() =>
            {
                return Collection.Indexes.CreateOne(model);
            });
        }
        public async Task<string> CreateIndexAsync(Expression<Func<TDocument, object>> field, bool desc)
        {
            IndexKeysDefinition<TDocument> keys = default(IndexKeysDefinition<TDocument>);

            if (desc)
                keys = this.Index.Descending(field);
            else
                keys = this.Index.Ascending(field);

            CreateIndexModel<TDocument> model = new CreateIndexModel<TDocument>(keys);

            return await RetryAsync(async () =>
            {
                return await Collection.Indexes.CreateOneAsync(model);
            });
        }

        public async Task<string> CreateIndexAsync(IndexKeysDefinition<TDocument> keys, CreateIndexOptions options)
        {
            CreateIndexModel<TDocument> model = new CreateIndexModel<TDocument>(keys, options);

            return await RetryAsync(async () =>
            {
                return await Collection.Indexes.CreateOneAsync(model);
            });
        }

        #endregion
    }
}
