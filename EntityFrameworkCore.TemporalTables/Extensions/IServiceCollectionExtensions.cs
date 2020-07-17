using EntityFrameworkCore.TemporalTables.Migrations;
using EntityFrameworkCore.TemporalTables.Sql;
using EntityFrameworkCore.TemporalTables.Sql.Factory;
using EntityFrameworkCore.TemporalTables.Sql.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            //services.AddScoped<IMigrator, TemporalTableMigrator<TContext>>(); //<-- with 2 temporal-DbContexts, an IMigrator gets registered twice, the second migrator takes precedence, so it is called by EF, even when the other Context is passed as the arg to the CLI tool
            services.AddScoped<ITemporalTableMigrator, TemporalTableMigrator<TContext>>();
            services.AddScoped<IMigrator, TemporalTableMigratorResolver>();
            services.AddScoped<IMigrationsSqlGenerator, TemporalTablesMigrationsSqlGenerator<TContext>>();

            services.AddSingleton<ITemporalTableSqlGeneratorFactory, TemporalTableSqlGeneratorFactory>();

            return services;
        }
    }
}
