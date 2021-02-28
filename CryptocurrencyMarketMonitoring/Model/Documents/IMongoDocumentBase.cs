using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptocurrencyMarketMonitoring.Model.Documents
{
    public interface IMongoDocumentBase
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
