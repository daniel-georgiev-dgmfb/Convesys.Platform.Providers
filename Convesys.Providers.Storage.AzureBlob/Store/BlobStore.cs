using Microsoft.Azure.Storage.Blob;
using Pirina.Kernel.Compression;
using Pirina.Kernel.Extensions;
using Pirina.Kernel.Serialisation;
using Pirina.Kernel.Storage;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pirina.Providers.Storage.AzureBlob.Store
{
    //TODO: Add Second (endpoint) and retry logic
    public class BlobStore : IStorage<Guid>
    {
        private readonly IStorageConnection<CloudBlobContainer, Guid> _storageConnection;
        private readonly ISerialiser _serialiser;
        private readonly IBlobSizeCalculator _blobSizeCalculator;
        private readonly ICompressor _compressor;

        public BlobStore(IStorageConnection<CloudBlobContainer, Guid> storageConnection, ISerialiser serialiser, ICompressor compressor, IBlobSizeCalculator blobSizeCalculator)
        {
            _storageConnection = storageConnection ?? throw new ArgumentNullException(nameof(storageConnection));
            _serialiser = serialiser ?? throw new ArgumentNullException(nameof(serialiser));
            _blobSizeCalculator = blobSizeCalculator ?? throw new ArgumentNullException(nameof(blobSizeCalculator));
            _compressor = compressor ?? throw new ArgumentNullException(nameof(compressor));
        }

        public async Task AddAsync<TData>(TData data, Guid id, string key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (id == Guid.Empty) throw new ArgumentException($"{nameof(id)} cannot be Empty");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException($"{nameof(key)} cannot be Null or Empty");

            using (var stream = await Serialise(data))
            {
                stream.Position = 0;
                var block = await GetBlockAsync(id, key,
                    await _blobSizeCalculator.GetBlockSize(stream.Length));
                await block.UploadFromStreamAsync(stream);
            }
        }

        public async Task AddAsync<TData>(TData data, Guid id, string key, string objectName)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (id == Guid.Empty) throw new ArgumentException($"{nameof(id)} cannot be Empty");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException($"{nameof(key)} cannot be Null or Empty");
            if (string.IsNullOrEmpty(objectName)) throw new ArgumentException($"{nameof(objectName)} cannot be Null or Empty");

            var path = Path.Combine(objectName, key);
            await AddAsync(data, id, path);
        }

        public async Task<TData> GetAsync<TData>(Guid id, string key)
        {
            if (id == Guid.Empty) throw new ArgumentException($"{nameof(id)} cannot be Empty");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException($"{nameof(key)} cannot be Null or Empty");

            TData data;
            var block = await GetBlockAsync(id, key);
            using (var ms = new MemoryStream())
            {
                await block.DownloadToStreamAsync(ms);
                data = await ConvertStreamToData<TData>(ms);
            }
            return data;
        }

        public async Task<TData> GetAsync<TData>(Guid id, string key, string objectName)
        {
            if (id == Guid.Empty) throw new ArgumentException($"{nameof(id)} cannot be Empty");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException($"{nameof(key)} cannot be Null or Empty");
            if (string.IsNullOrEmpty(objectName)) throw new ArgumentException($"{nameof(objectName)} cannot be Null or Empty");

            var path = Path.Combine(objectName, key);
            return await GetAsync<TData>(id, path);
        }

        public async Task RemoveAsync(Guid id, string key)
        {
            if (id == Guid.Empty) throw new ArgumentException($"{nameof(id)} cannot be Empty");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException($"{nameof(key)} cannot be Null or Empty");

            var block = await GetBlockAsync(id, key);
            await block.DeleteIfExistsAsync();
        }

        public async Task RemoveAsync(Guid id, string key, string objectName)
        {
            if (id == Guid.Empty) throw new ArgumentException($"{nameof(id)} cannot be Empty");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException($"{nameof(key)} cannot be Null or Empty");
            if (string.IsNullOrEmpty(objectName)) throw new ArgumentException($"{nameof(objectName)} cannot be Null or Empty");

            var path = Path.Combine(objectName, key);
            await RemoveAsync(id, path);
        }

        public async Task RemoveAllAsync(Guid transactionId)
        {
            if (transactionId == Guid.Empty) throw new ArgumentException($"{nameof(transactionId)} cannot be Empty");
            await _storageConnection.RemoveObjectAsync(transactionId);
        }

        private async Task<CloudBlockBlob> GetBlockAsync(Guid transactionId, string fileName)
        {
            var container = await _storageConnection.GetObjectAsync(transactionId);
            return container.GetBlockBlobReference(fileName);
        }

        private async Task<CloudBlockBlob> GetBlockAsync(Guid transactionId, string fileName, int size)
        {
            var block = await GetBlockAsync(transactionId, fileName);
            block.StreamWriteSizeInBytes = size;
            return block;
        }

        private async Task<Stream> Serialise(object data)
        {
            var serialised = await _serialiser.Serialise(data);
            serialised.Position = 0;
            var compressed = await this._compressor.Compress(serialised);
            return compressed;
        }

        private async Task<TData> ConvertStreamToData<TData>(Stream stream)
        {
            stream.Position = 0;
            var decompressed = await this._compressor.Decompress(stream);
            decompressed.Position = 0;
            var obj = await _serialiser.Deserialise<TData>(decompressed);
            return obj;
        }
    }
}