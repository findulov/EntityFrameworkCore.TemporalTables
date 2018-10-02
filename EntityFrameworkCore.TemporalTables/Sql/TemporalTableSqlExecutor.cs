using EntityFrameworkCore.TemporalTables.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.TemporalTables.Sql
{
    /// <inheritdoc />
    public class TemporalTableSqlExecutor<TContext> : ITemporalTableSqlExecutor<TContext>
        where TContext : DbContext
    {
        private readonly ISqlQueryExecutor<TContext> sqlQueryExecutor;
        private readonly ITemporalTableSqlBuilder<TContext> temporalTableSqlBuilder;

        public TemporalTableSqlExecutor(
            ISqlQueryExecutor<TContext> sqlQueryExecutor,
            ITemporalTableSqlBuilder<TContext> temporalTableSqlBuilder)
        {
            this.sqlQueryExecutor = sqlQueryExecutor;
            this.temporalTableSqlBuilder = temporalTableSqlBuilder;
        }

        /// <inheritdoc />
        public void Execute()
        {
            string sql = temporalTableSqlBuilder.BuildTemporalTablesSql();

            if (!string.IsNullOrWhiteSpace(sql))
            {
                sqlQueryExecutor.ExecuteSqlQuery(sql, useTransaction: true);
            }
        }
    }
}
