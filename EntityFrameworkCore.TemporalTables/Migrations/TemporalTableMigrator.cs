using EntityFrameworkCore.TemporalTables.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Storage;

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
            IRelationalConnection connection,
            ISqlGenerationHelper sqlGenerationHelper,
            ICurrentDbContext currentDbContext,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger,
            IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger,
            IDatabaseProvider databaseProvider,
            ITemporalTableSqlExecutor<TContext> temporalTableSqlExecutor)
            : base(migrationsAssembly, historyRepository, databaseCreator, migrationsSqlGenerator, rawSqlCommandBuilder, migrationCommandExecutor, connection, sqlGenerationHelper, currentDbContext, logger, commandLogger, databaseProvider)
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
