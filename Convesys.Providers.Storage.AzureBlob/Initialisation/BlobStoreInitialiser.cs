using Microsoft.Azure.Storage.Blob;
using Pirina.Kernel.DependencyResolver;
using Pirina.Kernel.Storage;
using Pirina.Providers.Storage.AzureBlob.Connection;
using Pirina.Providers.Storage.AzureBlob.Factories;
using Pirina.Providers.Storage.AzureBlob.Store;
using System;
using System.Threading.Tasks;

namespace Pirina.Providers.Storage.AzureBlob.Initialisation
{

    public static class BlobStoreInitialiser
    {
        public static Task Initialise(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType(typeof(IStorageConnectionManager<CloudBlobClient>), typeof(BlobConnectionManager), Lifetime.Transient);
            dependencyResolver.RegisterType(typeof(IStorageConnection<CloudBlobContainer, Guid>), typeof(BlobConnection), Lifetime.Transient);
            dependencyResolver.RegisterType<BlobSizeCalculator>(Lifetime.Transient);
            //dependencyResolver.RegisterType<BlobStore>(Lifetime.Transient);
            dependencyResolver.RegisterType(typeof(IStorageFactory<Guid>), typeof(BlobStore),Lifetime.Singleton);
            dependencyResolver.RegisterType(typeof(IStorageConnectionManager<CloudBlobClient>), typeof(BlobConnectionManager), Lifetime.Singleton);
            return Task.CompletedTask;
        }
    }
}