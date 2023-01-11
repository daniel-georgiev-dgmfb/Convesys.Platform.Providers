using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using NUnit.Framework;
using Pirina.Common.Compression.Deflation;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pirina.Providers.Storage.FileShare.Test.L1
{
    public class Tests
    {
        //private Guid _id;

        [SetUp]
        public void Setup()
        {
            //this._id = Guid.Parse("3ed90667-0e97-4d25-8f7a-024f58c09854");
        }

        [Test]
        public async Task FileStorageTests()
        {
            try
            { 
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=convesystorages;AccountKey=Mvb/W8vBdgTrqRVaHJa9VdIcd9B4gG1baMnWCVqCbET/SjQxXcF/GRvc9z/Lf9LYBSNqBVvKikFe6gHhhTGDCg==;EndpointSuffix=core.windows.net";
            var account = CloudStorageAccount.Parse(connectionString);
            account.CreateCloudFileClient();
            var client = account.CreateCloudFileClient();
            var share = client.GetShareReference("share1");
            share.CreateIfNotExists();

                // Ensure that the share exists.
                if (share.Exists())
                {
                    string policyName = "SharePolicy" + DateTime.UtcNow.Ticks;

                    // Create a new stored access policy and define its constraints.
                    SharedAccessFilePolicy sharedPolicy = new SharedAccessFilePolicy()
                    {
                        SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                        Permissions = SharedAccessFilePermissions.Read | SharedAccessFilePermissions.Write
                    };

                    // Get existing permissions for the share.
                    FileSharePermissions permissions = share.GetPermissions();

                    // Add the stored access policy to the share's policies. Note that each policy must have a unique name.
                    permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
                    share.SetPermissions(permissions);

                    // Generate a SAS for a file in the share and associate this access policy with it.
                    CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                    var res = await rootDir.CreateIfNotExistsAsync();
                    CloudFileDirectory sampleDir = rootDir.GetDirectoryReference("CustomFiles");
                    await sampleDir.CreateIfNotExistsAsync();
                    CloudFile file = sampleDir.GetFileReference("DMS_specification 200405.pdf");
                    string sasToken = file.GetSharedAccessSignature(null, policyName);
                    Uri fileSasUri = new Uri(file.StorageUri.PrimaryUri.ToString() + sasToken);

                    // Create a new CloudFile object from the SAS, and write some text to the file.
                    CloudFile fileSas = new CloudFile(fileSasUri);
                    var fileAsStream = new FileStream("D:\\Software\\DMS specification 200405.pdf", FileMode.Open, FileAccess.Read);
                    var compressor = new DeflationCompressor();
                    var compressed = await compressor.Compress(fileAsStream);
                    
                    await fileSas.UploadFromStreamAsync(compressed);
                    var content = new MemoryStream();
                    await fileSas.DownloadToStreamAsync(content);
                    content.Position = 0;
                    var decompressed = await compressor.Decompress(content);
                    decompressed.Position = 0;
                    var fileContent = ((MemoryStream)decompressed).ToArray();
                    await File.WriteAllBytesAsync(String.Format("D:\\Temp\\{0}", fileSas.Name), fileContent);
                    
                    Assert.Pass();
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}