using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace EntityFrameworkCore.TemporalTables.Sql
{
    public interface ITemporalTableSqlBuilder<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Builds SQL code for temporal tables for the specified entity types based on their configuration.
        /// </summary>
        /// <param name="entityTypes">A collection of entity types to generate temporal table SQL code for.</param>
        /// <param name="appendSeparator">Whether to use a dashes separator indicator in the SQL code between the entity types. (---------)</param>
        /// <returns></returns>
        string BuildTemporalTableSqlForEntityTypes(IEnumerable<IEntityType> entityTypes, bool appendSeparator = true);
    }
}
