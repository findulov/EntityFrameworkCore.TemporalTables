using EntityFrameworkCore.TemporalTables.Migrations;
using EntityFrameworkCore.TemporalTables.Sql;
using EntityFrameworkCore.TemporalTables.Sql.Factory;
using EntityFrameworkCore.TemporalTables.Sql.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.TemporalTables.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Register temporal table services for the specified <see cref="DbContext"/>.
        /// </summary>
        public static IServiceCollection RegisterTemporalTablesForDatabase<TContext>(
            this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<ISqlQueryExecutor<TContext>, SqlQueryExecutor<TContext>>();
            services.AddScoped<ITableHelper<TContext>, TableHelper<TContext>>();
            services.AddScoped<ITemporalTableSqlBuilder<TContext>, TemporalTableSqlBuilder<TContext>>();
            services.AddScoped<ITemporalTableSqlExecutor<TContext>, TemporalTableSqlExecutor<TContext>>();
            services.AddScoped<IMigrator, TemporalTableMigrator<TContext>>();

            services.AddSingleton<ITemporalTableSqlGeneratorFactory, TemporalTableSqlGeneratorFactory>();

            return services;
        }
    }
}
