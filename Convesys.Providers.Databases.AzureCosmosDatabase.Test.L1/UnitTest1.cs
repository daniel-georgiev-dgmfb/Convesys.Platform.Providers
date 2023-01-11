using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Storage;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Pirina.Kernel.Data;
using SharpCompress.IO;
using System;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Pirina.Providers.Databases.AzureCosmosDatabase.Test.L1
{
    //internal class CosmosDBConnectionString
    //{
    //    public CosmosDBConnectionString(string connectionString)
    //    {
    //        // Use this generic builder to parse the connection string
    //        DbConnectionStringBuilder builder = new DbConnectionStringBuilder
    //        {
    //            ConnectionString = connectionString
    //        };

    //        if (builder.TryGetValue("AccountKey", out object key))
    //        {
    //            AuthKey = key.ToString();
    //        }

    //        if (builder.TryGetValue("AccountEndpoint", out object uri))
    //        {
    //            ServiceEndpoint = new Uri(uri.ToString());
    //        }
    //    }

    //    public Uri ServiceEndpoint { get; set; }

    //    public string AuthKey { get; set; }
    //}

    public class Tests
    {
        protected class TestModel : BaseTransactionModel
        {
            public TestModel()
            {
                base.Id = Guid.NewGuid();
            }
            public Guid id { get { return base.Id; } }
            public string surname { get; set; }

            public string foreName { get; set; }
        }

        protected class MockConfiguration : ICosmosDbConfiguration
        {
            public string DatabaseId => "metadata";

            public string EndPointUri => "https://convensysdb.documents.azure.com:443/";

            public string PrimaryKey => "UserName";

            public string AuthKey => "q0upOxCvTtUOWcwXgZ53DNYcP48jZYsponWfXAupnihp07n3uzRupqAMn4QpdQiVXN1N1eAtomQgNbNkug3W8Q==";
                                      //R7e5UPmPwdGKTYwZevT77ttp70R0pXMuWoa55xDW1VFmlwA0KAhvdjXvH7XsduW7XhEkhQ37HgzUWiHuxFOs4w==
        }
        [SetUp]
        public void Setup()
        {
        }

        //[Test]
        //public async Task Test1_a()
        //{
        //    try
        //    {
        //        var authKey = "q0upOxCvTtUOWcwXgZ53DNYcP48jZYsponWfXAupnihp07n3uzRupqAMn4QpdQiVXN1N1eAtomQgNbNkug3W8Q==";
        //        var option = new CosmosClientOptions { ConnectionMode = Microsoft.Azure.Cosmos.ConnectionMode.Gateway };
        //        var cosmosClient = new CosmosClient("https://convensysdb.documents.azure.com:443/", authKey, option);
        //        var metadataDb = await cosmosClient.CreateDatabaseIfNotExistsAsync("IdentityAccessManagement");
        //        var containerProperties = new ContainerProperties { Id = "users", PartitionKeyPath = "/surname" };
        //        var containerResponse = await metadataDb.Database.CreateContainerIfNotExistsAsync(containerProperties);
        //        var container = metadataDb.Database.GetContainer("users");
                
        //        var item = await container.CreateItemAsync(new TestModel { surname = "doe" }, new PartitionKey("doe"));
                

        //        Assert.Pass();
        //    }

        //    catch (Exception e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}

        

        [Test]
        public async Task Test3()
        {
            try
            {
                var tenant = new Guid("2504e3b1-1ff9-4d0e-929a-585c41d50d96");
                var configuration = new MockConfiguration();
                var id = Guid.NewGuid();
                var context = new CosmosDbContext(configuration);
                await context.InitDb();
                var response = await context.Add(new TestModel { Id = id, surname = "doe", TenantId = tenant, foreName = "john", TransactionId = Guid.NewGuid(), Created = DateTimeOffset.Now });

            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}