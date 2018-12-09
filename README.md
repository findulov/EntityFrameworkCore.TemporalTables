# EntityFrameworkCore.TemporalTables
Extension library for Entity Framework Core which allows developers who use SQL Server to easily use temporal tables.

How to use it?

<h2>Service Provider configuration</h2>

1. Use ``UseInternalServiceProvider()`` on ``DbContextOptionsBuilder`` when registering your DbContext to replace Entity Framework's internal service provider with yours. For example:

```
services.AddDbContextPool<MyDbContext>((provider, options) =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    options.UseInternalServiceProvider(provider);
});
```

2. Then register all Entity Framework services into your service provider by using:
``services.AddEntityFrameworkSqlServer();``

3. And finally call:
``services.RegisterTemporalTablesForDatabase<MyDbContext>();``

Use the first two settings only when you want your temporal tables to be executed by default when using ``Update-Database`` command from Package Manager Console or ``DbContext.Database.Migrate() / DbContext.Database.MigrateAsync()``.

If you want to handle the temporal tables SQL execution yourself, you can do it manually by using:
```
var temporalTableSqlExecutor = serviceProvider.GetService<ITemporalTableSqlExecutor<MyDbContext>>();
temporalTableSqlExecutor.Execute();
```

<br />

<h2>DbContext configuration</h2>

In ``OnModelCreating(ModelBuilder modelBuilder)`` method you have the following options:

* ``modelBuilder.UseTemporalTables()`` - create temporal table for all of your entities by default.
* ``modelBuilder.PreventTemporalTables()`` - do not create temporal table for none of your entities by default.

* ``modelBuilder.Entity<TEntity>(b => b.UseTemporalTable());`` - create a temporal table for the specified entity ``TEntity``. This method is compatible with ``UseTemporalTables()`` and ``PreventTemporalTables()`` methods - e.g. you can call ``PreventTemporalTables()`` and then only register the entities you want.
* ``modelBuilder.Entity<TEntity>(b => b.PreventTemporalTable());`` - do not create a temporal table for the specified entity ``TEntity``. This method is also compatible with ``UseTemporalTables()`` and ``PreventTemporalTables()`` methods.

<br />
You can refer to the <a href="https://github.com/findulov/EntityFrameworkCore.TemporalTables/tree/master/EntityFrameworkCore.TemporalTables.TestApp">sample application</a> for more configuration information.

You can install the NuGet package from here: https://www.nuget.org/packages/EntityFrameworkCore.TemporalTables/
