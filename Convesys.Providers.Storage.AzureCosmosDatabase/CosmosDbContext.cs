using Microsoft.Azure.Cosmos;
using Pirina.Kernel.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pirina.Providers.Databases.AzureCosmosDatabase
{
    public class CosmosDbContext : ITransitDbContext
    {
        private readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        private  CosmosClient _client;
        private Microsoft.Azure.Cosmos.Database _database;
        private CosmosClientOptions _cosmosClientOptions;
        public CosmosDbContext(ICosmosDbConfiguration cosmosDbConfiguration)
        {
            this._cosmosDbConfiguration = cosmosDbConfiguration;
            this._cosmosClientOptions = new CosmosClientOptions() { ConnectionMode = Microsoft.Azure.Cosmos.ConnectionMode.Gateway };
            this._client = new CosmosClient(cosmosDbConfiguration.EndPointUri, cosmosDbConfiguration.AuthKey, this._cosmosClientOptions);
        }

        public async Task InitDb()
        {
            var respond = await _client.CreateDatabaseIfNotExistsAsync(_cosmosDbConfiguration.DatabaseId);
            this._database = respond.Database;
        }
        public async Task<T> Add<T>(T item) where T : BaseTransactionModel
        {
            var collectionName = GetCollectionName<T>();
            var collectionNameLastPart = collectionName.Split(new[] { '.', '+' }).Last();
            var db = _client.GetDatabase(_cosmosDbConfiguration.DatabaseId);
            var container = await db.CreateContainerIfNotExistsAsync(collectionNameLastPart, "/tenantid");
            var response = await container.Container.CreateItemAsync<T>(item);
            
            return item;
        }

        public bool Remove<T>(T item) where T : BaseTransactionModel
        {
            throw new NotImplementedException();
        }

        IQueryable<T> ITransitDbContext.Set<T>(Expression<Func<T, bool>> predicate)
        {
            var collectionName = GetCollectionName<T>();
            var result = this._database.GetContainer(collectionName).GetItemLinqQueryable<T>().Where(predicate);
            return result;
        }

        public IQueryable<T> Set<T>() where T : BaseTransactionModel
        {
            return ((ITransitDbContext)this).Set<T>(_ => true);
        }
        
        private static string GetCollectionName<T>() where T : BaseTransactionModel
        {
            var t = typeof(T);
            var collectionName = t.FullName;

            return collectionName;
        }
    }
}