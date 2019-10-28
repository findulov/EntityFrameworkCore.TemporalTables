using EntityFrameworkCore.TemporalTables.Extensions;
using EntityFrameworkCore.TemporalTables.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace EntityFrameworkCore.TemporalTables.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            IServiceCollection services = new ServiceCollection();

            services.AddDbContextPool<DataContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseInternalServiceProvider(provider);
            });
            
            services.AddEntityFrameworkSqlServer();

            services.RegisterTemporalTablesForDatabase<DataContext>();

            var serviceProvider = services
                .BuildServiceProvider();

            var dbContext = serviceProvider.GetService<DataContext>();

            // Update temporal tables automatically by calling Migrate() / MigrateAsync() or Update-Database from Package Manager Console.
            dbContext.Database.Migrate();

            // Just generate the temporal tables SQL without executing it against the database.
            var temporalTableSqlBuilder = serviceProvider.GetService<ITemporalTableSqlBuilder<DataContext>>();
            string sql = temporalTableSqlBuilder.BuildTemporalTablesSql();

            // Execute the temporal tables SQL code against the database (SQL code is generated internally without exposing to outside).
            var temporalTableSqlExecutor = serviceProvider.GetService<ITemporalTableSqlExecutor<DataContext>>();
            temporalTableSqlExecutor.Execute();
        }
    }
}
