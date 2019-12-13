using EntityFrameworkCore.TemporalTables.SqlTemplates.Helpers;
using System;

namespace EntityFrameworkCore.TemporalTables.Sql.Generation
{
    /// <inheritdoc />
    public abstract class BaseTemporalTableSqlGenerator : ITemporalTableSqlGenerator
    {
        protected string schemaName;

        protected string tableName;

        protected string TableWithSchema { get; }

        protected string HistoryTableName { get; }

        protected string SysTimeConstraint { get; }

        public BaseTemporalTableSqlGenerator(string tableName, string schemaName = "dbo")
        {
            this.tableName = tableName;
            this.schemaName = schemaName;

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("Table name not initialized");
            }

            TableWithSchema = $"[{schemaName}].[{tableName}]";
            HistoryTableName = $"[{schemaName}].[{tableName}History]";
            SysTimeConstraint = schemaName == "dbo"
                ? tableName
                : $"{schemaName}_{tableName}";
        }

        public abstract string Generate();
    }
}
