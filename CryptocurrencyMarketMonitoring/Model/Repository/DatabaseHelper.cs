using CryptocurrencyMarketMonitoring.Model.Attributes;
using CryptocurrencyMarketMonitoring.Model.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Model.Repository
{
    internal static class DatabaseHelper<TDocument> where TDocument : IMongoDocumentBase
    {
        internal static IMongoCollection<TDocument> GetCollection()
        {
            return GetCollectionFromConnectionString(GetDefaultConnectionString());
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and connectionstring.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="connectionString">The connectionstring to use to get the collection from.</param>
        /// <returns>Returns a MongoCollection from the specified type and connectionstring.</returns>
        internal static IMongoCollection<TDocument> GetCollectionFromConnectionString(string connectionString)
        {
            return GetCollectionFromConnectionString(connectionString, GetCollectionName()[0]);
        }

        internal static IMongoCollection<TDocument> GetCollectionTempFromConnectionString(string connectionString)
        {
            return GetCollectionFromConnectionString(connectionString, GetCollectionName()[1]);
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and connectionstring.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="connectionString">The connectionstring to use to get the collection from.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>Returns a MongoCollection from the specified type and connectionstring.</returns>
        internal static IMongoCollection<TDocument> GetCollectionFromConnectionString(string connectionString, string collectionName)
        {
            return GetCollectionFromUrl(new MongoUrl(connectionString), collectionName);
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and url.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="url">The url to use to get the collection from.</param>
        /// <returns>Returns a MongoCollection from the specified type and url.</returns>
        internal static IMongoCollection<TDocument> GetCollectionFromUrl(MongoUrl url)
        {
            return GetCollectionFromUrl(url, GetCollectionName()[0]);
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and url.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="url">The url to use to get the collection from.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>Returns a MongoCollection from the specified type and url.</returns>
        internal static IMongoCollection<TDocument> GetCollectionFromUrl(MongoUrl url, string collectionName)
        {
            return GetDatabaseFromUrl(url).GetCollection<TDocument>(collectionName);
        }

        /// <summary>
        /// Creates and returns a MongoDatabase from the specified url.
        /// </summary>
        /// <param name="url">The url to use to get the database from.</param>
        /// <returns>Returns a MongoDatabase from the specified url.</returns>
        private static IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var client = new MongoClient(url);
            return client.GetDatabase(url.DatabaseName); // WriteConcern defaulted to Acknowledged
        }

        #region Collection Name

        /// <summary>
        /// Determines the collection name for T and assures it is not empty
        /// </summary>
        /// <typeparam name="T">The type to determine the collection name for.</typeparam>
        /// <returns>Returns the collection name for T.</returns>
        private static string[] GetCollectionName()
        {
            string[] collections = typeof(TDocument).GetTypeInfo().BaseType.Equals(typeof(object)) ?
                                      GetCollectionNameFromInterface() :
                                      GetCollectionNameFromType();

            return collections.Select(c => c?.ToLowerInvariant()).ToArray();
        }

        /// <summary>
        /// Determines the collection name from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get the collection name from.</typeparam>
        /// <returns>Returns the collection name from the specified type.</returns>
        private static string[] GetCollectionNameFromInterface()
        {
            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = typeof(TDocument).GetTypeInfo().GetCustomAttribute(typeof(CollectionNameAttribute));
            return att != null
                ? new string[] { ((CollectionNameAttribute)att).Name, ((CollectionNameAttribute)att).NameTemp }
                : new string[] { typeof(TDocument).Name, string.Empty };
        }

        /// <summary>
        /// Determines the collectionname from the specified type.
        /// </summary>
        /// <param name="entitytype">The type of the entity to get the collectionname from.</param>
        /// <returns>Returns the collectionname from the specified type.</returns>
        private static string[] GetCollectionNameFromType()
        {
            Type entitytype = typeof(TDocument);
            string collectionName;
            string collectionNameTemp;
            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = entitytype.GetTypeInfo().GetCustomAttribute(typeof(CollectionNameAttribute));
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionName = ((CollectionNameAttribute)att).Name;
                collectionNameTemp = ((CollectionNameAttribute)att).NameTemp;
            }
            else
            {
                //if (typeof(Entity).IsAssignableFrom(entitytype))
                //{
                //    // No attribute found, get the basetype
                //    while (!entitytype.BaseType.Equals(typeof(Entity)))
                //    {
                //        entitytype = entitytype.BaseType;
                //    }
                //}
                collectionName = entitytype.Name;
                collectionNameTemp = string.Empty;
            }

            return new string[] { collectionName, collectionNameTemp };
        }



        #endregion Collection Name

        #region Connection Name

        /// <summary>
        /// Determines the connection name for T and assures it is not empty
        /// </summary>
        /// <typeparam name="T">The type to determine the connection name for.</typeparam>
        /// <returns>Returns the connection name for T.</returns>
        private static string GetConnectionName()
        {
            string connectionName;
            connectionName = typeof(TDocument).GetTypeInfo().BaseType.Equals(typeof(object)) ?
                                      GetConnectionNameFromInterface() :
                                      GetConnectionNameFromType();

            if (string.IsNullOrEmpty(connectionName))
            {
                connectionName = typeof(TDocument).Name;
            }
            return connectionName.ToLowerInvariant();
        }

        /// <summary>
        /// Determines the connection name from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get the connection name from.</typeparam>
        /// <returns>Returns the connection name from the specified type.</returns>
        private static string GetConnectionNameFromInterface()
        {
            // Check to see if the object (inherited from Entity) has a ConnectionName attribute
            var att = typeof(TDocument).GetTypeInfo().GetCustomAttribute(typeof(ConnectionNameAttribute));
            return (att != null) ? ((ConnectionNameAttribute)att).Name : typeof(TDocument).Name;
        }

        /// <summary>
        /// Determines the connection name from the specified type.
        /// </summary>
        /// <param name="entitytype">The type of the entity to get the connection name from.</param>
        /// <returns>Returns the connection name from the specified type.</returns>
        private static string GetConnectionNameFromType()
        {
            Type entitytype = typeof(TDocument);
            string connectionname;

            // Check to see if the object (inherited from Entity) has a ConnectionName attribute
            var att = entitytype.GetTypeInfo().GetCustomAttribute(typeof(ConnectionNameAttribute));
            if (att != null)
            {
                // It does! Return the value specified by the ConnectionName attribute
                connectionname = ((ConnectionNameAttribute)att).Name;
            }
            else
            {
                if (typeof(MongoDocumentBase).GetTypeInfo().IsAssignableFrom(entitytype))
                {
                    // No attribute found, get the basetype
                    while (!entitytype.GetTypeInfo().BaseType.Equals(typeof(MongoDocumentBase)))
                    {
                        entitytype = entitytype.GetTypeInfo().BaseType;
                    }
                }
                connectionname = entitytype.Name;
            }

            return connectionname;
        }

        #endregion Connection Name

        #region Connection String

        /// <summary>
        /// Retrieves the default connectionstring from the appsettings.json file.
        /// </summary>
        /// <returns>Returns the default connectionstring from the appsettings.json file.</returns>
        internal static string GetDefaultConnectionString()
        {
            return string.Empty;
            //return DIContainer.Configuration.GetConnectionString(GetConnectionName());
        }

        internal static string GetSessionName()
        {
            return string.Empty;
            //var retval = DIContainer.Configuration.GetSessionName();
            //retval = string.IsNullOrEmpty(retval) ? "Default" : retval;
            //return retval;
        }

        #endregion Connection String
    }
}
