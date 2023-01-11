using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using MongoDB.Driver;
using Pirina.Kernel.Data;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Pirina.Providers.Databases.AzureCosmosDatabase
{
    public class DocumentDbContext : ITransitDbContext
    {
        private readonly IDocumentDbConfiguration _cosmosDbConfiguration;
        private  DocumentClient _client;
        private readonly Uri _databaseUri;
        private Microsoft.Azure.Documents.Database _database;

        public DocumentDbContext(IDocumentDbConfiguration cosmosDbConfiguration)
        {
            //var authKey = "R7e5UPmPwdGKTYwZevT77ttp70R0pXMuWoa55xDW1VFmlwA0KAhvdjXvH7XsduW7XhEkhQ37HgzUWiHuxFOs4w==";
            this._cosmosDbConfiguration = cosmosDbConfiguration;
            //var builder = new DocumentClient(cosmosDbConfiguration.EndPointUri, authKey);
            this._client = new DocumentClient(new Uri(cosmosDbConfiguration.EndPointUri), cosmosDbConfiguration.AuthKey);
        }

        public async Task InitDb()
        {
            var options = new CosmosClientOptions() { ConnectionMode = Microsoft.Azure.Cosmos.ConnectionMode.Gateway };
            //_client = new CosmosClient(_cosmosDbConfiguration.EndPointUri, options);
            this._database = new Microsoft.Azure.Documents.Database { Id = _cosmosDbConfiguration.DatabaseId };
            var respond = await _client.CreateDatabaseIfNotExistsAsync(_database);
            this._database = respond.Resource;
        }
        public async Task<T> Add<T>(T item) where T : BaseTransactionModel
        {
            var collectionName = GetCollectionName<T>();
            var db = this._database;
            var container = await _client.CreateDocumentAsync(db.SelfLink, item);
            //var response = await container.Container.UpsertItemAsync<T>(item);
            //_client.CreateDocumentCollectionIfNotExistsAsync(_databaseUri, new DocumentCollection { Id = collectionName }).Wait();

            //if (IsDocumentInInCollection<T>(item))
            //{
            //    _client.ReplaceDocumentAsync(
            //        UriFactory.CreateDocumentUri(_cosmosDbConfiguration.DatabaseId, collectionName, item.TransactionId.ToString()),
            //        item);
            //}
            //else
            //{
            //    _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_cosmosDbConfiguration.DatabaseId, collectionName), item).Wait();
            //}

            return item;
        }

        public bool Remove<T>(T item) where T : BaseTransactionModel
        {
            throw new NotImplementedException();
            //var collectionName = GetCollectionName<T>();

            //if (IsDocumentInInCollection<T>(item))
            //{
            //    _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_cosmosDbConfiguration.DatabaseId, collectionName, item.TransactionId.ToString()));
            //}

            //return true;
        }

        public IQueryable<T> Set<T>() where T : BaseTransactionModel
        {
            throw new NotImplementedException();
            //var collectionName = GetCollectionName<T>();

            //return _client.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(_cosmosDbConfiguration.DatabaseId, collectionName));
        }

        private static string GetCollectionName<T>() where T : BaseTransactionModel
        {
            var t = typeof(T);
            var collectionName = t.FullName;

            return collectionName;
        }

        private bool IsDocumentInInCollection<T>(T item) where T : BaseTransactionModel
        {
            var result = Set<T>().Where(txm => txm.TransactionId == item.TransactionId);
            var resultInList = result.ToList();
            return resultInList.Any();
        }

        T ITransitDbContext.Add<T>(T item)
        {
            throw new NotImplementedException();
        }
    }
}