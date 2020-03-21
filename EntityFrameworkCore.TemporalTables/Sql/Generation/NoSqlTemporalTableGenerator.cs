using System;

namespace EntityFrameworkCore.TemporalTables.Sql.Generation
{
    /// <summary>
    /// No real SQL code generation for temporal tables. Refer to the Null object pattern for more details.
    /// </summary>
    public class NoSqlTemporalTableGenerator : BaseTemporalTableSqlGenerator
    {
        public NoSqlTemporalTableGenerator(string tableName, string schemaName)
            : base(tableName, schemaName)
        {
        }

        public override string Generate()
        {
            return string.Empty;
        }
    }
}
