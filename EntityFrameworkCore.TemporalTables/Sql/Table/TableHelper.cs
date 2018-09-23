using EntityFrameworkCore.TemporalTables.SqlTemplates;
using EntityFrameworkCore.TemporalTables.SqlTemplates.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace EntityFrameworkCore.TemporalTables.Sql.Table
{
    public class TableHelper<TContext> : ITableHelper<TContext>
        where TContext : DbContext
    {
        private readonly TContext context;

        public TableHelper(TContext context)
        {
            this.context = context;
        }

        public bool IsTableTemporal(string tableName, string schema = "dbo")
        {
            string sqlTemplateContent = SqlTemplateProcessor.Process(SqlTemplates.SqlTemplates.IsDatabaseTableTemporal,
                new Dictionary<string, string> { { Constants.TableWithSchemaPlaceholder, $"{schema}.{tableName}" } });

            int numberOfRowsAffected = -1;

            // Querying SELECT statements using the classic ADO.NET way until Entity Framework Core team provide a better way to query non-entity records.
            var connection = context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            var command = connection.CreateCommand();
            command.CommandText = sqlTemplateContent;

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                numberOfRowsAffected = reader.GetInt32(0);
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            return numberOfRowsAffected > 0;
        }
    }
}
