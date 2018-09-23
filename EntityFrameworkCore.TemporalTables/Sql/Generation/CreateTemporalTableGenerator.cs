using EntityFrameworkCore.TemporalTables.SqlTemplates;
using EntityFrameworkCore.TemporalTables.SqlTemplates.Helpers;
using System.Collections.Generic;

namespace EntityFrameworkCore.TemporalTables.Sql.Generation
{
    /// <summary>
    /// Generates a SQL code used to create a temporal table.
    /// </summary>
    public class CreateTemporalTableGenerator : BaseTemporalTableSqlGenerator
    {
        public CreateTemporalTableGenerator(string schemaName, string tableName)
            : base(schemaName, tableName)
        {
        }

        /// <inheritdoc />
        public override string Generate()
        {
            string sql = SqlTemplateProcessor.Process(SqlTemplates.SqlTemplates.CreateTemporalTable,
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
