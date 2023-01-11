using Pirina.Kernel.DependencyResolver;
using Pirina.Providers.Databases.AzureCosmosDatabase.Configuration;
using System.Threading.Tasks;

namespace Pirina.Providers.Databases.AzureCosmosDatabase.Initialisation
{
    public static class AzureCosmosDatabaseInitialiser
    {
        public static Task InitialiseInternal(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<CosmoStoreConfiguration>(Lifetime.PerThread);
            dependencyResolver.RegisterType<DocumentDbContext>(Lifetime.PerThread);

            return Task.CompletedTask;
        }
    }
}