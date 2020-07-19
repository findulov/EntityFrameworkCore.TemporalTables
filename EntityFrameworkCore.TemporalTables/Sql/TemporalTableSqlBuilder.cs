using EntityFrameworkCore.TemporalTables.Cache;
using EntityFrameworkCore.TemporalTables.Sql.Factory;
using EntityFrameworkCore.TemporalTables.Sql.Generation;
using EntityFrameworkCore.TemporalTables.Sql.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.TemporalTables.Sql
{
    public class TemporalTableSqlBuilder<TContext> : ITemporalTableSqlBuilder<TContext>
        where TContext : DbContext
    {
        private readonly TContext context;
        private readonly ITemporalTableSqlGeneratorFactory temporalTableSqlGeneratorFactory;
        private readonly ITableHelper<TContext> tableHelper;

        public TemporalTableSqlBuilder(
            TContext context,
            ITemporalTableSqlGeneratorFactory temporalTableSqlGeneratorFactory,
            ITableHelper<TContext> tableHelper)
        {
            this.context = context;
            this.temporalTableSqlGeneratorFactory = temporalTableSqlGeneratorFactory;
            this.tableHelper = tableHelper;
        }

        /// <inheritdoc />
        public string BuildTemporalTablesSql(bool appendSeparator = true)
        {
            var entityTypes = EnsureTemporalEntitiesCacheIsInitialized(() => TemporalEntitiesCache.GetAll());

            return BuildTemporalTablesSqlForEntityTypes(entityTypes);
        }

        /// <inheritdoc />
        public string BuildTemporalTablesSqlForEntityTypes(IEnumerable<IEntityType> entityTypes, bool appendSeparator = true)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            foreach (var entityType in entityTypes)
            {
                string sql = BuildTemporalTableSqlFromEntityTypeConfiguration(entityType, appendSeparator);

                if (!string.IsNullOrWhiteSpace(sql))
                {
                    sqlBuilder.Append(sql);
                }
            }

            return sqlBuilder.ToString();
        }
        
        private string BuildTemporalTableSqlFromEntityTypeConfiguration(IEntityType entityType, bool appendSeparator)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            var relationalEntityType = context.Model.FindEntityType(entityType.Name);
            if (relationalEntityType is IEntityType)
            {

              string tableName = relationalEntityType.GetTableName();
              string schema = relationalEntityType.GetSchema() ?? "dbo";

              bool isEntityConfigurationTemporal = TemporalEntitiesCache.IsEntityConfigurationTemporal(entityType);
              bool isEntityTemporalInDatabase = tableHelper.IsTableTemporal(tableName, schema);

              ITemporalTableSqlGenerator temporalTableSqlGenerator = temporalTableSqlGeneratorFactory
                  .CreateInstance(isEntityConfigurationTemporal, isEntityTemporalInDatabase, tableName, schema);

              string temporalTableSql = temporalTableSqlGenerator.Generate();

              if (!string.IsNullOrWhiteSpace(temporalTableSql))
              {
                sqlBuilder.AppendLine(temporalTableSql);

                if (appendSeparator)
                {
                  sqlBuilder.AppendLine(new string('-', 100));
                }
              }
            }
            return sqlBuilder.ToString();
        }

        private IReadOnlyList<IEntityType> EnsureTemporalEntitiesCacheIsInitialized(Func<IReadOnlyList<IEntityType>> getEntityTypesFromCache)
        {
            // If OnModelCreating() method in the DbContext is not called yet, TemporalEntitiesCache will not be called
            // to cache the configuration for entity types and temporal tables SQL code will not be generated.
            // In such scenario, we just need to open a database connection manually to trigger OnModelCreating() and then close it.

            var entityTypes = getEntityTypesFromCache();

            if (entityTypes.Count == 0)
            {
                context.Database.OpenConnection();
                context.Database.CloseConnection();

                entityTypes = getEntityTypesFromCache();
            }

            return entityTypes;
        }
    }
}
