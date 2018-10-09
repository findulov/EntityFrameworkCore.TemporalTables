using EntityFrameworkCore.TemporalTables.Extensions;
using EntityFrameworkCore.TemporalTables.TestApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.TemporalTables.TestApp
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseTemporalTables();

            modelBuilder.Entity<User>(b =>
            {
                b.UseTemporalTable();

                b.HasData(new User
                {
                    Id = 1,
                    UserName = "testUser",
                    Password = "testPassword",
                    IsDeleted = false
                });
            });

            modelBuilder.Entity<Role>(b => b.PreventTemporalTable());
        }
    }
}
