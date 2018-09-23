using EntityFrameworkCore.TemporalTables.Extensions;
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
                        
            dbContext.Database.Migrate();
        }
    }
}
