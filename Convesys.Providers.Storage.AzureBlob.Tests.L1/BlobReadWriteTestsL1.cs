using Microsoft.Azure.Storage.Blob;
using NUnit.Framework;
using Pirina.Common.Compression.Deflation;
using Pirina.Common.Serialisation.Binary.Extensions;
using Pirina.Kernel.Compression;
using Pirina.Kernel.DependencyResolver;
using Pirina.Kernel.Serialisation;
using Pirina.Kernel.Storage;
using Pirina.Providers.Microsoft.DependencyInjection;
using Pirina.Providers.Storage.AzureBlob.Configuration;
using Pirina.Providers.Storage.AzureBlob.Connection;
using Pirina.Providers.Storage.AzureBlob.Initialisation;
using Pirina.Providers.Storage.AzureBlob.Store;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pirina.Providers.Storage.AzureBlob.Tests.L1
{
    public class BlobReadWriteTestsL1
    {
        private Guid _id;

        [SetUp]
        public void Setup()
        {
            this._id = Guid.Parse("2bca1e16-ff90-489f-9dfe-bb0bfad59fa1");
        }

        [Test]
        public async Task AddFileToBlobStorage()
        {
            //ARRANGE
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=convesystorages;AccountKey=Mvb/W8vBdgTrqRVaHJa9VdIcd9B4gG1baMnWCVqCbET/SjQxXcF/GRvc9z/Lf9LYBSNqBVvKikFe6gHhhTGDCg==;EndpointSuffix=core.windows.net";
            var resolver = new MicrosoftDependencyInjection();
            await BlobStoreInitialiser.Initialise(resolver);
            resolver.RegisterType<IStorage<Guid>, BlobStore>(Kernel.DependencyResolver.Lifetime.Transient);
            resolver.RegisterInstance<Func<IStorageConfiguration>>(() => new BlobConfiguration(connectionString), Lifetime.Singleton);
            ICompressor compressor = new DeflationCompressor();
            BinarySerialisationInitialiser.AddBinarySerialisation(resolver);
            resolver.RegisterInstance<Func<IStorageConfiguration>>(() => new BlobConfiguration(connectionString), Lifetime.Singleton);
            await resolver.Initialise();
            var blobConfiguration = new BlobConfiguration(connectionString);
            var storeManager = resolver.Resolve<IStorageConnectionManager<CloudBlobClient>>();
            var fileContent = File.ReadAllBytes(@"D:\Software\Document-management-system\Pirina-Infrastructure-Providers\Pirina.Providers.Storage.AzureBlob.Tests.L1\DMS specification 200405.pdf");
            var blobConnection = new BlobConnection(storeManager);
            var serialiser = resolver.Resolve<ISerialiser>();
            var blobStorage = new BlobStore(blobConnection, serialiser, compressor, new BlobSizeCalculator());
            var id = this._id;
            //ACT
            await blobStorage.AddAsync(fileContent, id, "DMS specification 200405.pdf");
            //ASSERT
            Assert.Pass();
        }

        [Test]
        public async Task ReadFileToBlobStorage()
        {
            //ARRANGE
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=convesystorages;AccountKey=Mvb/W8vBdgTrqRVaHJa9VdIcd9B4gG1baMnWCVqCbET/SjQxXcF/GRvc9z/Lf9LYBSNqBVvKikFe6gHhhTGDCg==;EndpointSuffix=core.windows.net";
            var resolver = new MicrosoftDependencyInjection();
            await BlobStoreInitialiser.Initialise(resolver);
            resolver.RegisterType<IStorage<Guid>, BlobStore>(Lifetime.Transient);
            resolver.RegisterType<ICompressor, DeflationCompressor>(Lifetime.Transient);
            resolver.RegisterInstance<Func<IStorageConfiguration>>(() => new BlobConfiguration(connectionString), Lifetime.Singleton);
            BinarySerialisationInitialiser.AddBinarySerialisation(resolver);
            await resolver.Initialise();
            var blobConfiguration = new BlobConfiguration(connectionString);
            var storeManager = resolver.Resolve<IStorageConnectionManager<CloudBlobClient>>();
            var blobConnection = new BlobConnection(storeManager);
            var serialiser = resolver.Resolve<ISerialiser>();
            var compressor = resolver.Resolve<ICompressor>();
            var blobStorage = new BlobStore(blobConnection, serialiser, compressor, new BlobSizeCalculator());
            var id = this._id;
            var key = "README.md";
            //ACT
            var content = await blobStorage.GetAsync<byte[]>(id, key);
            File.Delete(String.Format("D:\\Temp\\Downloaded\\{0}", key));
            await File.WriteAllBytesAsync(String.Format("D:\\Temp\\Downloaded\\{0}", key), content);
            var exists = File.Exists(String.Format("D:\\Temp\\Downloaded\\{0}", key));
            //ASSERT
            Assert.IsTrue(exists);
        }
    }
}