using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.TemporalTables.Tests.Mocks
{
    public class DbContextOptionsSetup
    {
        public static DbContextOptions<TContext> Setup<TContext>() where TContext : DbContext
        {
            return new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(databaseName: "TemporalTablesDatabase")
                .Options;
        }
    }
}
