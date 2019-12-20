using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.TemporalTables.Extensions
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// Filters the DbSet with entities at a given date and time.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <param name="date">The date.</param>
        /// <returns>The entities represented at the specified time.</returns>
        public static IQueryable<TEntity> AsOf<TEntity>(this DbSet<TEntity> dbSet, DateTimeOffset date)
            where TEntity : class
        {
            var sql = FormattableString.Invariant($"SELECT * FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME AS OF {{0}}");
            return dbSet.FromSqlRaw(sql, date).AsNoTracking();
        }

        /// <summary>
        /// Filters the DbSet with entities between the provided dates.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The entities found between the provided dates.</returns>
        /// <remarks>The same entity might be returned more than once if it was modified
        /// during that time frame.</remarks>
        public static IQueryable<TEntity> Between<TEntity>(this DbSet<TEntity> dbSet, DateTimeOffset startDate, DateTimeOffset endDate)
            where TEntity : class
        {
            var sql = FormattableString.Invariant($"SELECT * FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME BETWEEN {{0}} AND {{1}}");
            return dbSet.FromSqlRaw(sql, startDate, endDate).AsNoTracking();
        }

        /// <summary>
        /// Gets the name of the table for the provided DbSet.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <returns>The name of the table.</returns>
        private static string GetTableName<TEntity>(DbSet<TEntity> dbSet) where TEntity : class
        {
            var dbContext = GetDbContext(dbSet);

            var model = dbContext.Model;
            var entityTypes = model.GetEntityTypes();
            var entityType = entityTypes.First(t => t.ClrType == typeof(TEntity));
            var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
            var tableName = tableNameAnnotation.Value.ToString();
            return tableName;
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <returns>The DbContext for the provided DbSet.</returns>
        private static DbContext GetDbContext<TEntity>(DbSet<TEntity> dbSet) where TEntity : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
            return currentDbContext.Context;
        }
    }
}
