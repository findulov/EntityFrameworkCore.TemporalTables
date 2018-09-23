using EntityFrameworkCore.TemporalTables.Cache;
using EntityFrameworkCore.TemporalTables.Sql.Factory;
using EntityFrameworkCore.TemporalTables.Sql.Generation;
using EntityFrameworkCore.TemporalTables.Sql.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.TemporalTables.Sql
{
    public class TemporalTableSqlBuilder<TContext> : ITemporalTableSqlBuilder<TContext>
        where TContext : DbContext
    {
        private readonly ITemporalTableSqlGeneratorFactory temporalTableSqlGeneratorFactory;
        private readonly ITableHelper<TContext> tableHelper;

        public TemporalTableSqlBuilder(
            ITemporalTableSqlGeneratorFactory temporalTableSqlGeneratorFactory,
            ITableHelper<TContext> tableHelper)
        {
            this.temporalTableSqlGeneratorFactory = temporalTableSqlGeneratorFactory;
            this.tableHelper = tableHelper;
        }

        /// <inheritdoc />
        public string BuildTemporalTableSqlForEntityTypes(IEnumerable<IEntityType> entityTypes, bool appendSeparator = true)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            foreach (var entityType in entityTypes)
            {
                string sql = BuildTemporalTableSqlFromEntityTypeConfiguration(entityType, appendSeparator: true);

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

            var relationalEntityType = entityType.Relational();

            string tableName = relationalEntityType.TableName;
            string schema = relationalEntityType.Schema ?? "dbo";

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

            return sqlBuilder.ToString();
        }
    }
}
