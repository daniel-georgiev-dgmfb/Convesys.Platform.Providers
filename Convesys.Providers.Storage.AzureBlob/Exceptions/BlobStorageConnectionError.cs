using System;

namespace Pirina.Providers.Storage.AzureBlob.Exceptions
{
    [Serializable]
    public class BlobStorageConnectionError : Exception
    {
        public BlobStorageConnectionError(string message) : base(message)
        {
        }
    }
}
