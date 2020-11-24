using EntityFrameworkCore.TemporalTables.Cache;
using EntityFrameworkCore.TemporalTables.Sql.Factory;
using EntityFrameworkCore.TemporalTables.Sql.Generation;
using EntityFrameworkCore.TemporalTables.Sql.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EntityFrameworkCore.TemporalTables.Migrations
{
    public class TemporalTablesMigrationsSqlGenerator<TContext> : SqlServerMigrationsSqlGenerator
        where TContext : DbContext
    {
        private readonly ITemporalTableSqlGeneratorFactory temporalTableSqlGeneratorFactory;
        private readonly ITableHelper<TContext> tableHelper;

        public TemporalTablesMigrationsSqlGenerator(
            MigrationsSqlGeneratorDependencies dependencies,
            IRelationalAnnotationProvider migrationsAnnotations, 
            ITemporalTableSqlGeneratorFactory temporalTableSqlGeneratorFactory,
            ITableHelper<TContext> tableHelper)
            : base(dependencies, migrationsAnnotations)
        {
            this.temporalTableSqlGeneratorFactory = temporalTableSqlGeneratorFactory;
            this.tableHelper = tableHelper;
        }

        protected override void Generate(DropTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
        {
            string tableName = operation.Name;
            string schema = operation.Schema ?? "dbo";

            bool isEntityTemporalInDatabase = tableHelper.IsTableTemporal(tableName, schema);

            if (isEntityTemporalInDatabase)
            {
                ITemporalTableSqlGenerator temporalTableSqlGenerator = new DropTemporalTableGenerator(tableName, schema);
                string temporalTableSql = temporalTableSqlGenerator.Generate();

                builder.AppendLine(temporalTableSql);
            }

            base.Generate(operation, model, builder, terminate);
        }
    }
}
