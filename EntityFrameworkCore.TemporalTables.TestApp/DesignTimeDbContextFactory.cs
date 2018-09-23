using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using EntityFrameworkCore.TemporalTables.Extensions;

namespace EntityFrameworkCore.TemporalTables.TestApp
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<DataContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseInternalServiceProvider(provider);
            });

            services.AddEntityFrameworkSqlServer();
            services.RegisterTemporalTablesForDatabase<DataContext>();

            var serviceProvider = services.BuildServiceProvider();

            builder.UseSqlServer(connectionString);
            builder.UseInternalServiceProvider(serviceProvider);

            return serviceProvider.GetService<DataContext>();
        }
    }
}
