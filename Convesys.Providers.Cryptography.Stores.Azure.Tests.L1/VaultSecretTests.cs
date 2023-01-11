using NUnit.Framework;
using Pirina.Common.Configuration;
using Pirina.Providers.Logging.Microsoft;
using System.Threading.Tasks;

namespace Pirina.Providers.Cryptography.Stores.Azure.Tests.L1
{
    public class VaultSecretTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetSecret()
        {
            //ARRANGE

            var cache = new MemoryCacheRuntimeImplementor();
            var logger = new Logger<AzureSecretVault>(new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory());
            var configuration = new LocalStoreConfiguration();
            configuration["ASPNETCORE_VAULTDNS"] = "https://convesys-vault.vault.azure.net/";
            configuration["ASPNETCORE_VAULT_CLIENTID"] = "35764cec-6bfc-40c0-a238-674f2ab8ab75";
            configuration["ASPNETCORE_VAULT_SECRET"] = "-6A.87=AUwGOdCfD7BFKxMr]lgA22dg@";
            var vault = new AzureSecretVault(configuration, cache, logger);
            var secretContext = new SecretContext("dbAuthKey");
            //ACT

            var secret = await vault.GetSecret(secretContext);
            Assert.AreEqual("Password1", secret);
            
            //ASSERT
        }
    }
}