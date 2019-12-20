namespace EntityFrameworkCore.TemporalTables.Extensions
{
    using System;
    using System.Linq;
    using EntityFrameworkCore.TemporalTables.Cache;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata;

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
            ValidateDbSet(dbSet);

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
            ValidateDbSet(dbSet);

            var sql = FormattableString.Invariant($"SELECT * FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME BETWEEN {{0}} AND {{1}}");
            return dbSet.FromSqlRaw(sql, startDate, endDate).AsNoTracking();
        }

        /// <summary>
        /// Validates that temporal tables are enabled on the provided data set.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        private static void ValidateDbSet<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class
        {
            var entityType = GetEntityType(dbSet);
            if (!TemporalEntitiesCache.IsEntityConfigurationTemporal(entityType))
            {
                throw new ArgumentException(FormattableString.Invariant($"The entity '{entityType.DisplayName()}' is not using temporal tables."), nameof(dbSet));
            }
        }

        /// <summary>
        /// Gets the name of the table for the provided DbSet.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <returns>The name of the table.</returns>
        private static string GetTableName<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class
        {
            var entityType = GetEntityType(dbSet);
            var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
            var tableName = tableNameAnnotation.Value.ToString();
            return tableName;
        }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <returns>The entity type.</returns>
        private static IEntityType GetEntityType<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class
        {
            var dbContext = GetDbContext(dbSet);

            var model = dbContext.Model;
            var entityTypes = model.GetEntityTypes();
            return entityTypes.First(t => t.ClrType == typeof(TEntity));
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <returns>The DbContext for the provided DbSet.</returns>
        private static DbContext GetDbContext<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
            return currentDbContext.Context;
        }
    }
}
