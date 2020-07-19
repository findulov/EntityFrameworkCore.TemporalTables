using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkCore.TemporalTables.Migrations
{
    internal class TemporalTableMigratorResolver : IMigrator
    {
        private IMigrator CurrentTemporalTableMigrator { get; set; }

        private static Type ReflectiveTemporalTableManager(ICurrentDbContext currentDbContext) => typeof(TemporalTableMigrator<>).MakeGenericType(currentDbContext.Context.GetType());

        public TemporalTableMigratorResolver(IEnumerable<ITemporalTableMigrator> temporalTableMigrators, ICurrentDbContext currentDbContext)
        {
            CurrentTemporalTableMigrator = temporalTableMigrators.LastOrDefault(m => ReflectiveTemporalTableManager(currentDbContext).IsAssignableFrom(m.GetType()));
        }

        public string GenerateScript(string fromMigration = null, string toMigration = null, bool idempotent = false)
        {
            return CurrentTemporalTableMigrator.GenerateScript(fromMigration, toMigration, idempotent);
        }

        public void Migrate(string targetMigration = null)
        {
            CurrentTemporalTableMigrator.Migrate(targetMigration);
        }

        public async Task MigrateAsync(string targetMigration = null, CancellationToken cancellationToken = default)
        {
            await CurrentTemporalTableMigrator.MigrateAsync(targetMigration, cancellationToken);
        }
    }
}
