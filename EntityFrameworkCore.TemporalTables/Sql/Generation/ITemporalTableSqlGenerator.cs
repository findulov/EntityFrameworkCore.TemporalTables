namespace EntityFrameworkCore.TemporalTables.Sql.Generation
{
    /// <summary>
    /// Abstraction layer used to provide SQL generation for temporal tables.
    /// </summary>
    public interface ITemporalTableSqlGenerator
    {
        /// <summary>
        /// Generates the appropriate SQL code for temporal table.
        /// </summary>
        string Generate();
    }
}
