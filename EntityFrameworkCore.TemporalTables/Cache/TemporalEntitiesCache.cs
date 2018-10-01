using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EntityFrameworkCore.TemporalTables.Tests")]
namespace EntityFrameworkCore.TemporalTables.Cache
{
    internal static class TemporalEntitiesCache
    {
        private static readonly ConcurrentDictionary<string, IEntityType> addedTemporalEntities = new ConcurrentDictionary<string, IEntityType>();
        private static readonly ConcurrentDictionary<string, IEntityType> removedTemporalEntities = new ConcurrentDictionary<string, IEntityType>();
        
        internal static void Add(IEntityType entityType)
        {
            if (addedTemporalEntities.TryAdd(entityType.Name, entityType))
            {
                if (removedTemporalEntities.TryGetValue(entityType.Name, out IEntityType removedEntityType))
                {
                    removedTemporalEntities.TryRemove(entityType.Name, out removedEntityType);
                }
            }
        }

        internal static void Remove(IEntityType entityType)
        {
            if (removedTemporalEntities.TryAdd(entityType.Name, entityType))
            {
                if (addedTemporalEntities.TryGetValue(entityType.Name, out IEntityType addedEntityType))
                {
                    addedTemporalEntities.TryRemove(entityType.Name, out addedEntityType);
                }
            }
        }

        internal static bool IsEntityConfigurationTemporal(IEntityType entityType)
        {
            return addedTemporalEntities.ContainsKey(entityType.Name) && !removedTemporalEntities.ContainsKey(entityType.Name);
        }

        internal static IReadOnlyList<IEntityType> GetAll()
        {
            return addedTemporalEntities
                .Concat(removedTemporalEntities)
                .Select(e => e.Value)
                .ToList();
        }

        internal static void Clear()
        {
            addedTemporalEntities.Clear();
            removedTemporalEntities.Clear();
        }
    }
}
