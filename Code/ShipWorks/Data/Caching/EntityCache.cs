using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Web.Caching;
using System.ComponentModel;
using System.Diagnostics;
using ShipWorks.ApplicationCore.ExecutionMode;
using log4net;
using System.Web;
using System.Threading;
using Interapptive.Shared.Utility;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Interapptive.Shared.Collections;
using System.Runtime.Caching;
using ShipWorks.Data.Model;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Caching
{
    /// <summary>
    /// Provides functionality for caching entities.  When entities are returned, a deep clone is returned, not the original entity.
    /// </summary>
    public sealed class EntityCache : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EntityCache));

        // Single cache instance
        MemoryCache cache = new MemoryCache("EntityCache");

        // The supported entity types
        EntityType[] entityTypes;

        // Notifiers of changes
        Dictionary<EntityType, EntityTypeChangeNotifier> changeNotifiers;

        Dictionary<EntityType, PrefetchPath2> prefetchPaths = new Dictionary<EntityType, PrefetchPath2>();

        /// <summary>
        /// Creates a cache that can hold the given entity types
        /// </summary>
        public EntityCache(IEnumerable<EntityType> entityTypes)
            : this(entityTypes, null)
        {

        }

        /// <summary>
        /// Creates a cache that can hold the given entity types.  If any prefetch paths are specified, they will be used
        /// each time the associated entity type is checked. Prefetch should really only be used for child data that is not change monitored, otherwise
        /// the standard cache approach should be used.
        /// </summary>
        public EntityCache(IEnumerable<EntityType> entityTypes, IEnumerable<PrefetchPath2> prefetchPaths)
        {
            this.entityTypes = entityTypes.ToArray();

            changeNotifiers = entityTypes.ToDictionary(e => e, e => new EntityTypeChangeNotifier(e));

            if (prefetchPaths != null && prefetchPaths.Any())
            {
                this.prefetchPaths = prefetchPaths.ToDictionary(p => (EntityType) p.RootEntityType);
            }
        }

        /// <summary>
        /// Dipose of the cache completely
        /// </summary>
        public void Dispose()
        {
            if (cache != null)
            {
                cache.Dispose();
                cache = null;
            }
        }

        /// <summary>
        /// The EntityType's that this cache supports caching
        /// </summary>
        public IEnumerable<EntityType> EntityTypes
        {
            get { return entityTypes; }
        }

        /// <summary>
        /// Gets the entity with the given ID from cache.  If it does not exist, it is loaded if fetchIfMissing is true
        /// </summary>
        public EntityBase2 GetEntity(long entityID, bool fetchIfMissing)
        {
            return GetEntity(entityID, fetchIfMissing, SqlAdapter.Default);
        }

        /// <summary>
        /// Gets the entity with the given ID from cache.  If it does not exist, it is loaded if fetchIfMissing is true
        /// </summary>
        public EntityBase2 GetEntity(long entityID, bool fetchIfMissing, SqlAdapter adapter)
        {
            EntityBase2 entity = EntityUtility.CloneEntity((EntityBase2) cache[GetCacheKey(entityID)]);

            if (entity == null && fetchIfMissing)
            {
                EntityType entityType = EntityUtility.GetEntityType(entityID);
                IEntityField2 pkField = EntityUtility.GetPrimaryKeyField(entityType);

                Stopwatch sw = Stopwatch.StartNew();

                entity = (EntityBase2) adapter.FetchNewEntity(
                    GeneralEntityFactory.Create(entityType).GetEntityFactory(),
                    new RelationPredicateBucket(new FieldCompareValuePredicate(pkField, null, ComparisonOperator.Equal, entityID)),
                    GetPrefetch(entityType));

                log.DebugFormat("*** EntityCache.GetEntity (Fetch), {0}", sw.Elapsed.TotalSeconds);

                if (entity.Fields.State == EntityState.Fetched)
                {
                    SetCache(entityID, entity);
                }
                else
                {
                    entity = null;
                }
            }

            return entity;
        }

        /// <summary>
        /// Gets all the entities represented by the give keys.  Each key must be of the same EntityType
        /// </summary>
        public List<EntityBase2> GetEntities(List<long> keyList)
        {
            return GetEntities(keyList, SqlAdapter.Default);
        }

        /// <summary>
        /// Gets all the entities represented by the give keys.  Each key must be of the same EntityType
        /// </summary>
        public List<EntityBase2> GetEntities(List<long> keyList, SqlAdapter adapter)
        {
            if (keyList == null)
            {
                throw new ArgumentNullException("keyList");
            }

            List<EntityBase2> entities = new List<EntityBase2>(keyList.Count);

            if (keyList.Count == 0)
            {
                return entities;
            }

            List<long> needsFetched = DetermineMissingEntities(keyList, entities);

            if (needsFetched.Count > 0)
            {
                EntityType entityType = EntityUtility.GetEntityType(keyList[0]);
                EntityField2 pkField = EntityUtility.GetPrimaryKeyField(entityType);

                // Fetch each one that was missing or old
                EntityCollection collection = new EntityCollection(GeneralEntityFactory.Create(entityType).GetEntityFactory());
                adapter.FetchEntityCollection(collection, new RelationPredicateBucket(pkField == needsFetched), GetPrefetch(entityType));

                foreach (EntityBase2 entity in collection)
                {
                    SetCache((long) entity.Fields.PrimaryKeyFields[0].CurrentValue, entity);
                    entities.Add(entity);
                }
            }

            return entities;
        }

        /// <summary>
        /// Determine which entities are missing and need fetched based on the given key collection.  Any entities that are found are added to the given collection
        /// </summary>
        private List<long> DetermineMissingEntities(List<long> keyList, List<EntityBase2> entities)
        {
            List<long> needsFetched = new List<long>();

            // Look through each key in the list
            foreach (long key in keyList)
            {
                // See if we have the entity
                EntityBase2 entity = GetEntity(key, false);
                if (entity != null)
                {
                    entities.Add(entity);
                }
                else
                {
                    needsFetched.Add(key);
                }
            }

            return needsFetched;
        }

        /// <summary>
        /// Cache the given entity with the given entityID
        /// </summary>
        public void SetCache(long entityID, EntityBase2 entity)
        {
            EntityType entityType = EntityUtility.GetEntityType(entityID);

            if (!entityTypes.Contains(entityType))
            {
                throw new InvalidOperationException(string.Format("Trying to cache entityType {0} which is not monitored for changes.", entityType.ToString()));
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = TimeSpan.FromMinutes(30);
            policy.ChangeMonitors.Add(new EntityTypeChangeMonitor(changeNotifiers[entityType]));

            #if DEBUG
                policy.RemovedCallback = new CacheEntryRemovedCallback(OnCacheItemRemoved);
            #endif

            cache.Set(
                GetCacheKey(entityID),
                EntityUtility.CloneEntity(entity, GetPrefetch(entityType) != null),
                policy);
        }

        /// <summary>
        /// Remove the given entity from the cache
        /// </summary>
        public void Remove(long entityID)
        {
            try
            {
                cache.Remove(GetCacheKey(entityID));
            }
            catch (NullReferenceException)
            {
                // Just log the NullReferenceException because if cache is null, we wouldn't need
                // to worry about removing the entity anyway. This is in response to FogBugz #253969
                log.Warn("An entity was requested to be removed, but the cache was null");
            }
        }

        /// <summary>
        /// Completely clear the cache
        /// </summary>
        public void Clear()
        {
            MemoryCache previous = cache;
            cache = new MemoryCache("MemoryCache");

            previous.Dispose();
        }

        /// <summary>
        /// Clear the cache of all entries of the given entityType
        /// </summary>
        public void Clear(EntityType entityType)
        {
            changeNotifiers[entityType].NotifyChanged();
        }

        /// <summary>
        /// Get the cache key based on the given entityID
        /// </summary>
        private static string GetCacheKey(long entityID)
        {
            return entityID.ToString();
        }

        /// <summary>
        /// Get the prefetch to use for the given entity, or null if none has been configured
        /// </summary>
        private PrefetchPath2 GetPrefetch(EntityType entityType)
        {
            PrefetchPath2 prefetch = null;
            if (prefetchPaths != null)
            {
                prefetchPaths.TryGetValue(entityType, out prefetch);
            }

            return prefetch;
        }

        /// <summary>
        /// Called when an item is removed from the cache
        /// </summary>
        private static void OnCacheItemRemoved(CacheEntryRemovedArguments args)
        {
            log.DebugFormat("Entity {0} removed from cache due to {1}", args.CacheItem.Key, args.RemovedReason);
        }
    }
}
