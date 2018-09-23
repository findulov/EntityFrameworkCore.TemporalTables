using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.TemporalTables.Sql
{
    /// <summary>
    /// Database SQL query execution wrapper around <see cref="DbContext" />.
    /// </summary>
    public interface ISqlQueryExecutor<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Executes a raw SQL query.
        /// </summary>
        void ExecuteSqlQuery(string sql, bool useTransaction = false);
    }
}
