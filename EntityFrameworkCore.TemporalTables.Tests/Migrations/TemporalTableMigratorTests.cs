using EntityFrameworkCore.TemporalTables.Cache;
using EntityFrameworkCore.TemporalTables.Extensions;
using EntityFrameworkCore.TemporalTables.Sql;
using EntityFrameworkCore.TemporalTables.Sql.Factory;
using EntityFrameworkCore.TemporalTables.Sql.Table;
using EntityFrameworkCore.TemporalTables.Tests.Mocks;
using EntityFrameworkCore.TemporalTables.Tests.Mocks.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace EntityFrameworkCore.TemporalTables.Tests.Migrations
{
    [TestClass]
    public class TemporalTableMigratorTests
    {
        [TestInitialize]
        public void Setup()
        {
            TemporalEntitiesCache.Clear();
        }

        [TestMethod]
        public void TestIfCreateTemporalTableSqlWillBeGeneratedForUserIfTableConfigurationIsNotTemporal()
        {
            string temporalTableSql = GetTemporalTableSqlDependingOnIfTableIsTemporalOrNot<User>(
                new FakeTableHelperNonTemporal(),
                typeof(User));

            // Temporal table doesn't exists so create it
            Assert.IsTrue(temporalTableSql.StartsWith("IF NOT EXISTS"));
        }

        [TestMethod]
        public void TestIfEmptySqlWillBeGeneratedForUserIfTableConfigurationIsTemporal()
        {
            string temporalTableSql = GetTemporalTableSqlDependingOnIfTableIsTemporalOrNot<User>(
                new FakeTableHelperTemporal(),
                typeof(User));

            // Temporal table exists so no SQL code will be generated.
            Assert.IsTrue(string.IsNullOrWhiteSpace(temporalTableSql));
        }

        [TestMethod]
        public void TestIfDropTemporalTableSqlWillBeGeneratedForUserIfTableConfigurationIsNotTemporal()
        {
            string temporalTableSql = GetTemporalTableSqlDependingOnIfTableIsTemporalOrNot<User>(
                new FakeTableHelperTemporal());

            // Temporal table exists but we don't want it, so drop it.
            Assert.IsTrue(temporalTableSql.StartsWith("IF EXISTS"));
        }

        [TestMethod]
        public void TestIfEmptySqlWillBeGeneratedForUserIfTableConfigurationIsNotTemporal()
        {
            string temporalTableSql = GetTemporalTableSqlDependingOnIfTableIsTemporalOrNot<User>(
                new FakeTableHelperNonTemporal());

            // Temporal table doesn't exist and we don't want it, so empty SQL should be generated.
            Assert.IsTrue(string.IsNullOrWhiteSpace(temporalTableSql));
        }

        private string GetTemporalTableSqlDependingOnIfTableIsTemporalOrNot<TEntity>(
            ITableHelper<FakeDataContext> tableHelper,
            params Type[] entityTypesToCreateTemporalTablesFor) where TEntity : class
        {
            var options = DbContextOptionsSetup.Setup<FakeDataContext>();

            using (var context = new FakeDataContext(options))
            {
                // Register entity types to support temporal tables.
                entityTypesToCreateTemporalTablesFor
                    ?.ToList()
                    ?.ForEach(e => TemporalEntitiesCache.Add(context.Model.FindEntityType(e)));

                var entityType = context.Model.FindEntityType(typeof(TEntity));

                var temporalTableSqlBuilder = new TemporalTableSqlBuilder<FakeDataContext>(
                    context,
                    new TemporalTableSqlGeneratorFactory(),
                    tableHelper);

                string sql = temporalTableSqlBuilder.BuildTemporalTablesSqlForEntityTypes(new[] { entityType });

                return sql;
            }
        }
    }
}
