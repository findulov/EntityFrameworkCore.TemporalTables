using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.TemporalTables.Sql.Table
{
    public interface ITableHelper<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Checks if the relational table is temporal.
        /// </summary>
        bool IsTableTemporal(string tableName, string schema = "dbo");
    }
}
