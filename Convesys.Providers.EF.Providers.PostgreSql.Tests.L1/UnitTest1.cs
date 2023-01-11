using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pirina.Providers.EF.Providers.PostgreSql.Tests.L1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            try
            {
                var configuration = new LocalStoreConfiguration();
                configuration.SetValue("PsgrConnectionString", "User ID=postgres;Password=Password1;Host=localhost;Port=5432;Database=testdb");
                var connectionStringProvider = new PostgreConnectionStringBuilder(configuration);
                var configuration1 = new CustomConfiguration(connectionStringProvider);
                var options = new DbContextOptionsBuilderMock();
                var context = new PirinaDbContext(options, configuration1);
                
                var testModel = new TestModel();
                var result = context.Add(testModel);
                await ((IDbContext)context).SaveChangesAsync();
                Assert.Pass();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        [Test]
        public async Task Test2()
        {
            try
            {
                var configuration = new LocalStoreConfiguration();
                configuration.SetValue("PsgrConnectionString", "User ID=postgres;Password=Password1;Host=localhost;Port=5432;Database=testdb");
                //configuration.SetValue();
                var connectionStringProvider = new PostgreConnectionStringBuilder(configuration);
                var configuration1 = new CustomConfiguration(connectionStringProvider);
                var options = new DbContextOptionsBuilderMock();
                var context = new PirinaDbContext(options, configuration1);

                var testModel = new TestModel();
                var result = ((IDbContext)context).Set<TestModel>().ToList();
                Assert.Pass();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}