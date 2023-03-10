//  ----------------------------------------------------------------------- 
//   <copyright file="AddTenantedEntityTests.cs" company="Glasswall Solutions Ltd.">
//       Glasswall Solutions Ltd.
//   </copyright>
//  ----------------------------------------------------------------------- 

namespace Pirina.Providers.EntityFramework.Tests.L1.GlasswallDbContextTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Kernel.Data.ORM;
    using Kernel.Data.Tenancy;
    using Moq;
    using NUnit.Framework;
    using TestArtifacts;

    [TestFixture]
    public class AddTenantedEntityTests : DbTestBase
    {
        private IList<TenantEntity> _list;
        private readonly Guid _objTenantId = Guid.NewGuid();
        private readonly TenantEntity _tenantEntity = new TenantEntity();

        protected override Guid GetGuid()
        {
            return _objTenantId;
        }
        protected override void SetUpTenantManagerMock()
        {
            base.SetUpTenantManagerMock();

            TenantManagerMock.Setup(a => a.AssignTenantId(It.IsAny<BaseTenantModel>())).Callback(() => SetTenantId(_tenantEntity, _objTenantId));
        }

        [OneTimeSetUp]
        public void TestSetUp()
        {
            var configurationMockObject = ConfigurationMock.Object;

            using (var context = new PirinaDbContext(DbContextOptions, configurationMockObject) as IDbContext)
            {
                context.Add(_tenantEntity);
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new PirinaDbContext(DbContextOptions, configurationMockObject) as IDbContext)
            {
                _list = context.Set<TenantEntity>().ToList();
            }
        }

        [Test]
        [Category(L1.EntityFramework)]
        public void One_entity_added()
        {
            Assert.That(_list.Count, Is.EqualTo(1));
        }

        [Test]
        [Category(L1.EntityFramework)]
        public void Retrieved_entity_is_the_inserted_entity()
        {
            Assert.That(_list.First().Id, Is.EqualTo(_tenantEntity.Id));
        }

        [Test]
        [Category(L1.EntityFramework)]
        public void Tenant_manager_is_used_to_set_the_tenant_id()
        {
            TenantManagerMock.Verify(a => a.AssignTenantId(It.IsAny<BaseTenantModel>()), Times.Once);
        }
    }
}