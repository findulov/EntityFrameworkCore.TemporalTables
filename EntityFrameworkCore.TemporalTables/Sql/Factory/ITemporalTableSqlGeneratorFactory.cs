using EntityFrameworkCore.TemporalTables.Sql.Generation;

namespace EntityFrameworkCore.TemporalTables.Sql.Factory
{
    /// <summary>
    /// Factory used to create an innstance of <see cref="ITemporalTableSqlGenerator" />.
    /// </summary>
    public interface ITemporalTableSqlGeneratorFactory
    {
        /// <summary>
        /// Creates an innstance of <see cref="ITemporalTableSqlGenerator" />.
        /// </summary>
        /// <param name="isEntityConfigurationTemporal">Specifies if the entity is configured to be temporal.</param>
        /// <param name="isEntityTemporalInDatabase">Specifies if the entity table is already temporal in the database.</param>
        /// <param name="tableName">The table name.</param>
        /// <param name="schemaName">The schema name.</param>
        /// <returns></returns>
        ITemporalTableSqlGenerator CreateInstance(
            bool isEntityConfigurationTemporal,
            bool isEntityTemporalInDatabase,
            string tableName,
            string schemaName = "dbo");
    }
}
