using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.TemporalTables.Sql
{
    /// <summary>
    /// Abstraction layer responsible for executing the SQL code for temporal tables.
    /// </summary>
    public interface ITemporalTableSqlExecutor<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Executes the SQL code for temporal tables.
        /// </summary>
        void Execute();
    }
}
