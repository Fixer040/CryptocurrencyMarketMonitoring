using CryptocurrencyMarketMonitoring.Model.Documents.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Model.Documents
{
    [CollectionName("users")]
    [ConnectionName("NoSql")]
    public class User : MongoDocumentBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string TOTP { get; set; }
    }
}
