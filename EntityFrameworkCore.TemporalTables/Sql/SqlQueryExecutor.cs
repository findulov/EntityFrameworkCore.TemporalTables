using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EntityFrameworkCore.TemporalTables.Sql
{
    public class SqlQueryExecutor<TContext> : ISqlQueryExecutor<TContext>
        where TContext : DbContext
    {
        private readonly TContext context;

        public SqlQueryExecutor(TContext context)
        {
            this.context = context;
        }
        
        public void ExecuteSqlQuery(string sql, bool useTransaction = false)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            if (useTransaction)
            {
                sqlBuilder.AppendLine("SET XACT_ABORT ON;");
                sqlBuilder.AppendLine("BEGIN TRANSACTION;");
                sqlBuilder.AppendLine();

                sqlBuilder.AppendLine(sql);

                sqlBuilder.AppendLine("COMMIT;");
            }
            else
            {
                sqlBuilder.AppendLine(sql);
            }

            context.Database.ExecuteSqlCommand(sqlBuilder.ToString());
        }
    }
}
