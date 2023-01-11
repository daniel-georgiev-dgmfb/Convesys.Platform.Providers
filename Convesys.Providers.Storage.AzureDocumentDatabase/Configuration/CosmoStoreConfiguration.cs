using Pirina.Kernel.Data.Connection;
using Pirina.Providers.Databases.AzureCosmosDatabase.Resolver;

namespace Pirina.Providers.Databases.AzureCosmosDatabase.Configuration
{
    public class CosmoStoreConfiguration : IDocumentDbConfiguration
    {
        public string DatabaseId { get; private set; }
        public string EndPointUri { get; private set; }
        public string PrimaryKey { get; private set;  }
        public string AuthKey { get; private set; }

        public CosmoStoreConfiguration(IConnectionStringProvider<IDocumentDbConfiguration> cosmosConnectionResolver)
        {
            var connectionSettings = cosmosConnectionResolver.GetConnectionString();

            DatabaseId = connectionSettings.DatabaseId;
            EndPointUri = connectionSettings.EndPointUri;
            PrimaryKey = connectionSettings.PrimaryKey;
            AuthKey = connectionSettings.AuthKey;
        }
    }
}
