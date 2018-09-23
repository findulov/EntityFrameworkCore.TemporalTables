using EntityFrameworkCore.TemporalTables.Cache;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.TemporalTables.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Configure temporal table for the specified entity type configuration.
        /// </summary>
        public static void UseTemporalTable<TEntity>(this EntityTypeBuilder<TEntity> entity) where TEntity : class
        {
            TemporalEntitiesCache.Add(entity.Metadata);
        }

        /// <summary>
        /// Configure not using a temporal table for the specified entity type configuration.
        /// </summary>
        public static void PreventTemporalTable<TEntity>(this EntityTypeBuilder<TEntity> entity) where TEntity : class
        {
            TemporalEntitiesCache.Remove(entity.Metadata);
        }
    }
}
