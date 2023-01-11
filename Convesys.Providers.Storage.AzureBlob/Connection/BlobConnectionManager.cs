using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Pirina.Kernel.Storage;
using Pirina.Providers.Storage.AzureBlob.Exceptions;
using System;
using System.Threading.Tasks;

namespace Pirina.Providers.Storage.AzureBlob.Connection
{
    public class BlobConnectionManager : IStorageConnectionManager<CloudBlobClient>
    {
        private readonly IStorageConfiguration _storageConfiguration;
        private CloudBlobClient _cloudBlobClient;

        public BlobConnectionManager(Func<IStorageConfiguration> storageConfigurationFactory)
        {
            if (storageConfigurationFactory == null) throw new ArgumentNullException(nameof(storageConfigurationFactory));
            this._storageConfiguration = storageConfigurationFactory() ?? throw new ArgumentNullException(nameof(storageConfigurationFactory));
        }

        public async Task<CloudBlobClient> GetStorageClient()
        {
            if (this._cloudBlobClient == null)
                await Connect();
            return this._cloudBlobClient;
        }

        private Task Connect()
        {
            var connectionString = this._storageConfiguration.ConnectionString;
            if (!CloudStorageAccount.TryParse(connectionString, out var account))
                throw new BlobStorageConnectionError($"Was unable to parse the connection string {connectionString}");

            this._cloudBlobClient = account.CreateCloudBlobClient();
            return Task.CompletedTask;
        }
    }
}