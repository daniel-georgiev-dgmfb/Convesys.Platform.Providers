using Moq;
using NUnit.Framework;
using Pirina.Kernel.Data.Connection;
using Pirina.Providers.Databases.AzureCosmosDatabase.Configuration;
using Pirina.Providers.Databases.AzureCosmosDatabase.Resolver;

namespace Pirina.Providers.Transport.AzureCosmosDatabase.Tests.L0.Configuration
{
    [TestFixture]
    public class PirinaMessageStoreConfigurationTests
    {
        [SetUp]
        public void Initialise()
        {
            _mockConnectionResolver = new Mock<IConnectionStringProvider<CosmosConnectionSettings>>();

            _mockConnectionResolver.Setup(x => x.GetConnectionString()).Returns(
                new CosmosConnectionSettings
                {
                    DatabaseId = DatabaseId,
                    EndPointUri = EndPointUriValue,
                    PrimaryKey = PrimaryKeyValue
                });
        }

        private const string DatabaseId = "FileTrust.MessageStore";
        private const string EndPointUriValue = "EndPointUriValue";
        private const string PrimaryKeyValue = "PrimaryKeyValue";

        private Mock<IConnectionStringProvider<CosmosConnectionSettings>> _mockConnectionResolver;

        [Test]
        public void DatabaseId_Can_Be_Retrieved()
        {
            // Arrange
            var expectedDatabaseId = DatabaseId;

            // Act
            var result = new CosmoStoreConfiguration(_mockConnectionResolver.Object);

            // Assert
            Assert.That(result.DatabaseId, Is.EqualTo(expectedDatabaseId));
        }

        [Test]
        public void EndPointUri_Can_Be_Retrieved()
        {
            // Arrange
            var expectedEndPointUri = EndPointUriValue;

            // Act
            var result = new CosmoStoreConfiguration(_mockConnectionResolver.Object);

            // Assert
            Assert.That(result.EndPointUri, Is.EqualTo(expectedEndPointUri));
        }

        [Test]
        public void PrimaryKey_Can_Be_Retrieved()
        {
            // Arrange
            var expectedPrimaryKey = PrimaryKeyValue;

            // Act
            var result = new CosmoStoreConfiguration(_mockConnectionResolver.Object);

            // Assert
            Assert.That(result.PrimaryKey, Is.EqualTo(expectedPrimaryKey));
        }
    }
}