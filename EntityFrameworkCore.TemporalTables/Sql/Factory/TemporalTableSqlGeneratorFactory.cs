using EntityFrameworkCore.TemporalTables.Sql.Generation;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.TemporalTables.Sql.Factory
{
    /// <inheritdoc />
    public class TemporalTableSqlGeneratorFactory : ITemporalTableSqlGeneratorFactory
    {
        /// <inheritdoc />
        public ITemporalTableSqlGenerator CreateInstance(
            bool isEntityConfigurationTemporal,
            bool isEntityTemporalInDatabase,
            string tableName,
            string schemaName)
        {
            ITemporalTableSqlGenerator temporalTableSqlGenerator = null;

            if (isEntityConfigurationTemporal && !isEntityTemporalInDatabase)
            {
                temporalTableSqlGenerator = new CreateTemporalTableGenerator(tableName, schemaName);
            }
            else if (!isEntityConfigurationTemporal && isEntityTemporalInDatabase)
            {
                temporalTableSqlGenerator = new DropTemporalTableGenerator(tableName, schemaName);
            }
            else
            {
                temporalTableSqlGenerator = new NoSqlTemporalTableGenerator(tableName, schemaName);
            }

            return temporalTableSqlGenerator;
        }
    }
}
