using Catalog.API.Configurations;
using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IOptions<DatabaseSettings> databaseOptions)
        {
            var client = new MongoClient(databaseOptions.Value.ConnectionString);
            var database = client.GetDatabase(databaseOptions.Value.CollectionName);

            Products = database.GetCollection<Product>(databaseOptions.Value.CollectionName);
        }

        public IMongoCollection<Product> Products { get; }
    }
}