using Microsoft.Azure.Cosmos;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Pirina.Providers.Databases.AzureCosmosDatabase.Test.L1
{
    public class Test2
    {
        private const string EndpointUrl = "https://convensys-dbs.documents.azure.com:443/";
        private const string AuthorizationKey = "XRbnLGqgvM4ZI4ND9YSR3UXHIx5SUIBxgLpvcFZ2edFPafZ6lEaUUbBuSiu2dTbG8nWYT271lcjfoOqmZtGhNA==";
        private const string DatabaseId = "IdentityAccessManagement";
        private const string ContainerId = "users";
        
        [Test]
        public async Task Test()
        {
            try
            {

                var option = new CosmosClientOptions { ConnectionMode = Microsoft.Azure.Cosmos.ConnectionMode.Gateway };
                CosmosClient cosmosClient = new CosmosClient(EndpointUrl, AuthorizationKey, option);
                await Test2.CreateDatabaseAsync(cosmosClient);
                await Test2.CreateContainerAsync(cosmosClient);
                await Test2.AddItemsToContainerAsync(cosmosClient);
                //await T1.QueryItemsAsync(cosmosClient);
                //await T1.ReplaceFamilyItemAsync(cosmosClient);
                //await T1.DeleteFamilyItemAsync(cosmosClient);
                //await T1.DeleteDatabaseAndCleanupAsync(cosmosClient);
            }
            catch(Exception e)
            {

            }
        }

        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        private static async Task CreateDatabaseAsync(CosmosClient cosmosClient)
        {
            // Create a new database
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(Test2.DatabaseId);
            //Console.WriteLine("Created Database: {0}\n", database.Database.Id);
        }

        /// <summary>
        /// Create the container if it does not exist. 
        /// Specify "/LastName" as the partition key since we're storing family information, to ensure good distribution of requests and storage.
        /// </summary>
        /// <returns></returns>
        private static async Task CreateContainerAsync(CosmosClient cosmosClient)
        {
            // Create a new container
            var container = await cosmosClient.GetDatabase(Test2.DatabaseId).CreateContainerIfNotExistsAsync(Test2.ContainerId, "/LastName");
            //Console.WriteLine("Created Container: {0}\n", container.Container.Id);
        }

        /// <summary>
        /// Add Family items to the container
        /// </summary>
        private static async Task AddItemsToContainerAsync(CosmosClient cosmosClient)
        {
            #region
            // Create a family object for the Andersen family
            Family andersenFamily = new Family
            {
                Id = "Andersen.1",
                LastName = "Andersen",
                Parents = new Parent[]
                {
            new Parent { FirstName = "Thomas" },
            new Parent { FirstName = "Mary Kay" }
                },
                Children = new Child[]
                {
            new Child
            {
                FirstName = "Henriette Thaulow",
                Gender = "female",
                Grade = 5,
                Pets = new Pet[]
                {
                    new Pet { GivenName = "Fluffy" }
                }
            }
                },
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = false
            };
            #endregion

            var container = cosmosClient.GetContainer(Test2.DatabaseId, Test2.ContainerId);
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Family> andersenFamilyResponse = await container.CreateItemAsync<Family>(andersenFamily, new PartitionKey(andersenFamily.LastName));
                //ItemResponse<Family> andersenFamilyResponse = await container.ReadItemAsync<Family>(andersenFamily.Id, new PartitionKey(andersenFamily.LastName));
                //Console.WriteLine("Item in database with id: {0} already exists\n", andersenFamilyResponse.Value.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<Family> andersenFamilyResponse = await container.CreateItemAsync<Family>(andersenFamily, new PartitionKey(andersenFamily.LastName));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
                //Console.WriteLine("Created item in database with id: {0}\n", andersenFamilyResponse.Value.Id);
            }

            // Create a family object for the Wakefield family
            Family wakefieldFamily = new Family
            {
                Id = "Wakefield.7",
                LastName = "Wakefield",
                Parents = new Parent[]
                {
            new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
            new Parent { FamilyName = "Miller", FirstName = "Ben" }
                },
                Children = new Child[]
                {
            new Child
            {
                FamilyName = "Merriam",
                FirstName = "Jesse",
                Gender = "female",
                Grade = 8,
                Pets = new Pet[]
                {
                    new Pet { GivenName = "Goofy" },
                    new Pet { GivenName = "Shadow" }
                }
            },
            new Child
            {
                FamilyName = "Miller",
                FirstName = "Lisa",
                Gender = "female",
                Grade = 1
            }
                },
                Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
                IsRegistered = true
            };

            // Create an item in the container representing the Wakefield family. Note we provide the value of the partition key for this item, which is "Wakefield"
            ItemResponse<Family> wakefieldFamilyResponse = await container.UpsertItemAsync<Family>(wakefieldFamily, new PartitionKey(wakefieldFamily.LastName));

            // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
            //Console.WriteLine("Created item in database with id: {0}\n", wakefieldFamilyResponse..Id);
        }
    }
}