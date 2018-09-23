using EntityFrameworkCore.TemporalTables.Cache;
using EntityFrameworkCore.TemporalTables.Tests.Mocks;
using EntityFrameworkCore.TemporalTables.Tests.Mocks.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace EntityFrameworkCore.TemporalTables.Tests.Cache
{
    [TestClass]
    public class TemporalEntitiesCacheTests
    {
        [TestMethod]
        public void AddTemporalTableTest()
        {
            var options = DbContextOptionsSetup.Setup<FakeDataContext>();

            using (var context = new FakeDataContext(options))
            {
                var entityType = context.Model.FindEntityType(typeof(User));

                // Add entity type.
                TemporalEntitiesCache.Add(entityType);

                // Test if the entity type exists in the cache.
                Assert.IsTrue(TemporalEntitiesCache.IsEntityConfigurationTemporal(entityType));
            }
        }

        [TestMethod]
        public void RemoveTemporalTableTest()
        {
            var options = DbContextOptionsSetup.Setup<FakeDataContext>();

            using (var context = new FakeDataContext(options))
            {
                var entityType = context.Model.FindEntityType(typeof(User));

                // Add entity type.
                TemporalEntitiesCache.Add(entityType);

                // Remove entity type.
                TemporalEntitiesCache.Remove(entityType);
                
                // Test if the entity type is actually removed.
                Assert.IsTrue(!TemporalEntitiesCache.IsEntityConfigurationTemporal(entityType));
            }
        }
    }
}
