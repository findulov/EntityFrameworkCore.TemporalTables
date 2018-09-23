using EntityFrameworkCore.TemporalTables.Cache;
using EntityFrameworkCore.TemporalTables.Sql;
using EntityFrameworkCore.TemporalTables.Sql.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace EntityFrameworkCore.TemporalTables.Migrations
{
    public class TemporalTableMigrator<TContext> : Migrator 
        where TContext : DbContext
    {
        private readonly ITemporalTableSqlExecutor<TContext> temporalTableSqlExecutor;

        public TemporalTableMigrator(
            IMigrationsAssembly migrationsAssembly,
            IHistoryRepository historyRepository,
            IDatabaseCreator databaseCreator,
            IMigrationsSqlGenerator migrationsSqlGenerator,
            IRawSqlCommandBuilder rawSqlCommandBuilder,
            IMigrationCommandExecutor migrationCommandExecutor,
            IRelationalConnection connection, ISqlGenerationHelper sqlGenerationHelper,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger,
            IDatabaseProvider databaseProvider,
            ITemporalTableSqlExecutor<TContext> temporalTableSqlExecutor)
            : base(migrationsAssembly, historyRepository, databaseCreator, migrationsSqlGenerator, rawSqlCommandBuilder, migrationCommandExecutor, connection, sqlGenerationHelper, logger, databaseProvider)
        {
            this.temporalTableSqlExecutor = temporalTableSqlExecutor;
        }

        public override void Migrate(string targetMigration = null)
        {
            base.Migrate(targetMigration);
            
            temporalTableSqlExecutor.Execute();
        }
    }
}
