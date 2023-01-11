using System.Threading.Tasks;

namespace Pirina.Providers.Storage.AzureBlob.Store
{
    public interface IBlobSizeCalculator
    {
        Task<int> GetBlockSize(long streamLength);
    }
}