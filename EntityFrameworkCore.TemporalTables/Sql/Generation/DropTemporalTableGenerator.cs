using EntityFrameworkCore.TemporalTables.SqlTemplates;
using EntityFrameworkCore.TemporalTables.SqlTemplates.Helpers;
using System.Collections.Generic;

namespace EntityFrameworkCore.TemporalTables.Sql.Generation
{
    /// <summary>
    /// Generates a SQL code used to drop a temporal table.
    /// </summary>
    public class DropTemporalTableGenerator : BaseTemporalTableSqlGenerator
    {
        public DropTemporalTableGenerator(string tableName, string schemaName) 
            : base(tableName, schemaName)
        {
        }

        public override string Generate()
        {
            string sql = SqlTemplateProcessor.Process(SqlTemplates.SqlTemplates.DropTemporalTable,
                new Dictionary<string, string>
                {
                    { Constants.TableWithSchemaPlaceholder, TableWithSchema },
                    { Constants.TableNamePlaceholder, tableName },
                    { Constants.HistoryTableNamePlaceholder, HistoryTableName },
                    { Constants.SystemTimeConstraintPlaceholder, SysTimeConstraint }
                });

            return sql;
        }
    }
}
