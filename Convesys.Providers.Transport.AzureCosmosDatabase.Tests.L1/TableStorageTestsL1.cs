using Microsoft.Azure.Cosmos.Table;
using NUnit.Framework;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Pirina.Providers.Storage.Table.Tests.L1
{
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity()
        {
        }

        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Upsert()
        {
            var storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=convesystorages;AccountKey=Mvb/W8vBdgTrqRVaHJa9VdIcd9B4gG1baMnWCVqCbET/SjQxXcF/GRvc9z/Lf9LYBSNqBVvKikFe6gHhhTGDCg==;EndpointSuffix=core.windows.net";


            // Retrieve storage account information from connection string.
            Microsoft.Azure.Cosmos.Table.CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            Console.WriteLine("Create a Table for the demo");
            var tableName = "FileMetadata";
            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Created Table named: {0}", tableName);
            }
            else
            {
                Console.WriteLine("Table {0} already exists", tableName);
            }
            var query = table.CreateQuery<CustomerEntity>();
            var queryResult = query.ToList();
            var entity = new CustomerEntity("last name", "first name");
            // Create the InsertOrReplace table operation
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

            // Execute the operation.
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            CustomerEntity insertedCustomer = result.Result as CustomerEntity;

            // Get the request units consumed by the current operation. RequestCharge of a TableResult is only applied to Azure Cosmos DB
            if (result.RequestCharge.HasValue)
            {
                Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result.RequestCharge);
            }

            //return insertedCustomer;
            Console.WriteLine();
            //return table;
        }

        [Test]
        public async Task Read()
        {
            var storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=convesystorages;AccountKey=Mvb/W8vBdgTrqRVaHJa9VdIcd9B4gG1baMnWCVqCbET/SjQxXcF/GRvc9z/Lf9LYBSNqBVvKikFe6gHhhTGDCg==;EndpointSuffix=core.windows.net";


            // Retrieve storage account information from connection string.
            Microsoft.Azure.Cosmos.Table.CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            Console.WriteLine("Create a Table for the demo");
            var tableName = "FileMetadata";
            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            //if (await table.CreateIfNotExistsAsync())
            //{
            //    Console.WriteLine("Created Table named: {0}", tableName);
            //}
            //else
            //{
            //    Console.WriteLine("Table {0} already exists", tableName);
            //}
            var query = table.CreateQuery<CustomerEntity>();
            var queryResult = query.ToImmutableList();
            //var entity = new CustomerEntity("last name", "first name");
            // Create the InsertOrReplace table operation
            //TableOperation insertOrMergeOperation = TableOperation.(entity);

            // Execute the operation.
            //TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            //CustomerEntity insertedCustomer = result.Result as CustomerEntity;

            // Get the request units consumed by the current operation. RequestCharge of a TableResult is only applied to Azure Cosmos DB
            //if (result.RequestCharge.HasValue)
            //{
              //  Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result.RequestCharge);
            //}

            //return insertedCustomer;
            //Console.WriteLine();
            //return table;
        }

        public static Microsoft.Azure.Cosmos.Table.CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            Microsoft.Azure.Cosmos.Table.CloudStorageAccount storageAccount;
            try
            {
                storageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
        public class CustomerEntity : TableEntity
        {
            public CustomerEntity()
            {
            }

            public CustomerEntity(string lastName, string firstName)
            {
                PartitionKey = lastName;
                RowKey = firstName;
            }

            public string Email { get; set; }
            public string PhoneNumber { get; set; }
        }
    }
}