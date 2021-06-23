using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCore.TemporalTables.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public static IQueryable<TEntity> AsOf<TEntity>(this DbSet<TEntity> dbSet, DateTime date)
            where TEntity : class
        {
            ValidateDbSet(dbSet);
            var PropertyNames = dbSet.GetMappedPropertiesWithPeriodProperties();
            var propertyNamesClause = string.Join(", ", PropertyNames);

            var sql = FormattableString.Invariant($"SELECT {propertyNamesClause} FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME AS OF {{0}}");
            return dbSet.FromSqlRaw(sql, date).AsNoTracking();
        }

        /// <summary>
        /// Filters the DbSet with entities at a given date and time.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <param name="dateTimeOffset">The date time offset.</param>
        /// <returns>The entities represented at the specified time.</returns>
        public static IQueryable<TEntity> AsOf<TEntity>(this DbSet<TEntity> dbSet, DateTimeOffset dateTimeOffset)
            where TEntity : class
        {
            ValidateDbSet(dbSet);
            var PropertyNames = dbSet.GetMappedPropertiesWithPeriodProperties();
            var propertyNamesClause = string.Join(", ", PropertyNames);

            var sql = FormattableString.Invariant($"SELECT {propertyNamesClause} FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME AS OF {{0}}");
            return dbSet.FromSqlRaw(sql, dateTimeOffset).AsNoTracking();
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
        public static IQueryable<TEntity> Between<TEntity>(this DbSet<TEntity> dbSet, DateTime startDate, DateTime endDate)
            where TEntity : class
        {
            ValidateDbSet(dbSet);
            var PropertyNames = dbSet.GetMappedPropertiesWithPeriodProperties();
            var propertyNamesClause = string.Join(", ", PropertyNames);

            var sql = FormattableString.Invariant($"SELECT {propertyNamesClause} FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME BETWEEN {{0}} AND {{1}}");
            return dbSet.FromSqlRaw(sql, startDate, endDate).AsNoTracking();
        }

        /// <summary>
        /// Filters the DbSet with entities between the provided dates.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <param name="startDateOffset">The start date time offset.</param>
        /// <param name="endDateOffset">The end date time offset.</param>
        /// <returns>The entities found between the provided dates.</returns>
        /// <remarks>The same entity might be returned more than once if it was modified
        /// during that time frame.</remarks>
        public static IQueryable<TEntity> Between<TEntity>(this DbSet<TEntity> dbSet, DateTimeOffset startDateOffset, DateTimeOffset endDateOffset)
            where TEntity : class
        {
            ValidateDbSet(dbSet);
            var PropertyNames = dbSet.GetMappedPropertiesWithPeriodProperties();
            var propertyNamesClause = string.Join(", ", PropertyNames);

            var sql = FormattableString.Invariant($"SELECT {propertyNamesClause} FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME BETWEEN {{0}} AND {{1}}");
            return dbSet.FromSqlRaw(sql, startDateOffset, endDateOffset).AsNoTracking();
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
        public static IQueryable<TEntity> ContainedIn<TEntity>(this DbSet<TEntity> dbSet, DateTime startDate, DateTime endDate)
            where TEntity : class
        {
            ValidateDbSet(dbSet);
            var PropertyNames = dbSet.GetMappedPropertiesWithPeriodProperties();
            var propertyNamesClause = string.Join(", ", PropertyNames);

            var sql = FormattableString.Invariant($"SELECT {propertyNamesClause} FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME CONTAINED IN ({{0}}, {{1}})");
            return dbSet.FromSqlRaw(sql, startDate, endDate).AsNoTracking();
        }

        /// <summary>
        /// Filters the DbSet with entities between the provided dates.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <param name="startDateOffset">The start date time offset.</param>
        /// <param name="endDateOffset">The end date time offset.</param>
        /// <returns>The entities found between the provided dates.</returns>
        /// <remarks>The same entity might be returned more than once if it was modified
        /// during that time frame.</remarks>
        public static IQueryable<TEntity> ContainedIn<TEntity>(this DbSet<TEntity> dbSet, DateTimeOffset startDateOffset, DateTimeOffset endDateOffset)
            where TEntity : class
        {
            ValidateDbSet(dbSet);
            var PropertyNames = dbSet.GetMappedPropertiesWithPeriodProperties();
            var propertyNamesClause = string.Join(", ", PropertyNames);

            var sql = FormattableString.Invariant($"SELECT {propertyNamesClause} FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME CONTAINED IN ({{0}}, {{1}})");
            return dbSet.FromSqlRaw(sql, startDateOffset, endDateOffset).AsNoTracking();
        }

        /// <summary>
        /// Selects the entities for all time periods
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <returns>All of the for all time-periods</returns>
        public static IQueryable<TEntity> ForAll<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            ValidateDbSet(dbSet);
            var PropertyNames = dbSet.GetMappedPropertiesWithPeriodProperties();
            var propertyNamesClause = string.Join(", ", PropertyNames);

            var sql = FormattableString.Invariant($"SELECT {propertyNamesClause} FROM [{GetTableName(dbSet)}] FOR SYSTEM_TIME ALL");
            return dbSet.FromSqlRaw(sql).AsNoTracking();
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

        private static IEnumerable<string> GetMappedPropertiesWithPeriodProperties<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            //TODO: these columns should be customizable, and should only be added if the period columns are not already mapped from the existing EF configuration
            string SysStartTime = "[SysStartTime]";
            string SysEndTime = "[SysEndTime]";

            IEnumerable<string> propertyNames = null;

            var entityType = dbSet
                .GetService<ICurrentDbContext>()
                .Context.Model.FindEntityType(typeof(TEntity));

            //TODO: I'm not sure how this will work with Owned entities or TPT/TPH
            StoreObjectIdentifier? so = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);

            if (so.HasValue)
            {
                propertyNames = entityType
                    .GetProperties()
                    .Where(s => !string.IsNullOrWhiteSpace(s.GetColumnName(so.Value)))
                    .Select(p => $"[{p.GetColumnName(so.Value)}]");

                var isOwned = dbSet
                    .GetService<ICurrentDbContext>()
                    .Context.Model.FindEntityType(typeof(TEntity))
                    .IsOwned();

                //Don't add columns if 
                if (!isOwned)
                {
                    propertyNames = propertyNames.Union(new string[] { SysStartTime, SysEndTime }).Distinct();
                }
            }

            return propertyNames;
        }
    }
}
