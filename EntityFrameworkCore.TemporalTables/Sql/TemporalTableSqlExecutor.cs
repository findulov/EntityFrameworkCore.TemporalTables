using EntityFrameworkCore.TemporalTables.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.TemporalTables.Sql
{
    /// <inheritdoc />
    public class TemporalTableSqlExecutor<TContext> : ITemporalTableSqlExecutor<TContext>
        where TContext : DbContext
    {
        private readonly TContext context;
        private readonly ISqlQueryExecutor<TContext> sqlQueryExecutor;
        private readonly ITemporalTableSqlBuilder<TContext> temporalTableSqlBuilder;

        public TemporalTableSqlExecutor(
            TContext context,
            ISqlQueryExecutor<TContext> sqlQueryExecutor,
            ITemporalTableSqlBuilder<TContext> temporalTableSqlBuilder)
        {
            this.context = context;
            this.sqlQueryExecutor = sqlQueryExecutor;
            this.temporalTableSqlBuilder = temporalTableSqlBuilder;
        }

        /// <inheritdoc />
        public void Execute()
        {
            var entityTypes = EnsureTemporalEntitiesCacheIsInitialized(() => TemporalEntitiesCache.GetAll());

            string sql = temporalTableSqlBuilder.BuildTemporalTableSqlForEntityTypes(entityTypes);

            if (!string.IsNullOrWhiteSpace(sql))
            {
                sqlQueryExecutor.ExecuteSqlQuery(sql, useTransaction: true);
            }
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
