using EntityFrameworkCore.TemporalTables.Tests.Mocks.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace EntityFrameworkCore.TemporalTables.Tests.Mocks
{
    public class FakeDataContext : DbContext
    {
        public FakeDataContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
