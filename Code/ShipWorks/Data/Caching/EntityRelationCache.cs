using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Web.Caching;
using System.ComponentModel;
using System.Diagnostics;
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
using ShipWorks.Data.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using System.Data;
using System.Data.SqlClient;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks.Data.Caching
{
    /// <summary>
    /// Provides functionality for caching keys related to a parent entity
    /// </summary>
    public sealed class EntityRelationCache : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EntityRelationCache));

        // When getting related keys, how many to send to the db for translation at once
        const int maxRelatedKeysChunkSize = 50;

        // Single cache instance
        MemoryCache cache = new MemoryCache("EntityRelationCache");

        // The entity cache we can use as a backup plan instead of hitting the db in some cases
        EntityCache entityCache;

        // The supported entity types
        EntityType[] entityTypes;

        // Notifiers of changes
        Dictionary<EntityType, EntityTypeChangeNotifier> changeNotifiers;

        private readonly ExecutionMode executionMode;
        
        // What we actually store in the cache
        class CacheEntry
        {
            public long FromKey { get; set; }
            public long[] RelatedKeys { get; set; }
            public bool OneToMany { get; set; }
            public string SortDescription { get; set; }
        }

        // System wide management of simultaneous data fetching
        static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityRelationCache(EntityCache entityCache, ExecutionMode executionMode)
        {
            this.entityCache = entityCache;
            this.entityTypes = entityCache.EntityTypes.ToArray();
            this.executionMode = executionMode;

            changeNotifiers = entityTypes.ToDictionary(e => e, e => new EntityTypeChangeNotifier(e));
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
        /// Translate the given list of ID's into all the keys they relate to of the given target type.  If fetchIsMissing is false, and ANY keys in the list are not cached, then null is returned.
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public List<long> GetRelatedKeys(List<long> idList, EntityType relateToType, bool fetchIfMissing, SortDefinition sort)
        {
            if (idList == null)
            {
                throw new ArgumentNullException("idList");
            }

            if (idList.Count == 0)
            {
                return new List<long>();
            }

            // The should all be of the same type
            EntityType entityType = EntityUtility.GetEntityType(idList[0]);

            // See if its already what we need
            if (entityType == relateToType)
            {
                return idList.ToList();
            }

            long[][] resultKeys = new long[idList.Count][];

            // The key in the dictionary is the ID that needs children fetched (that we don't have cached).  The value is the index
            // it was originally passed in as, and the index in the 'resultKeys' array to which the results will be assigned.
            Dictionary<long, int> needsFetched = DetermineMissingKeys(idList, entityType, relateToType, sort, resultKeys);

            // Get out if we need to fetch stuff and we don't want to fetch if missing
            if (needsFetched.Count > 0 && !fetchIfMissing)
            {
                return null;
            }

            // If we still need to fetch
            if (needsFetched.Count > 0)
            {
                // If we are on the UI thread, don't wait if we can't get the lock. On the background thread, always wait.
                bool tookLock = semaphore.Wait((executionMode.IsUIDisplayed && Program.MainForm.InvokeRequired) ? -1 : 0);

                try
                {
                    // Now that we have the lock, recheck to see what entities are available - more could have come in while we were waiting
                    needsFetched = DetermineMissingKeys(idList, entityType, relateToType, sort, resultKeys);

                    if (needsFetched.Count > 0)
                    {
                        EntityField2 entityPKField = EntityUtility.GetPrimaryKeyField(entityType);
                        EntityField2 relateToPKField = EntityUtility.GetPrimaryKeyField(relateToType);

                        // Get the relations we need for the SQL to connect from the entity to the context
                        RelationCollection relations = EntityUtility.FindRelationChain(entityType, relateToType);
                        if (relations == null)
                        {
                            throw new InvalidOperationException(string.Format("A relation chain was not found from {0} to {1}.", entityType, relateToType));
                        }

                        // We already checked that this entitytype is not the same as the related to type - so we should have at least one relation
                        Debug.Assert(relations.Count > 0);

                        // Only cache the results for simple parent->child or child->parent relations.  Otherwise it would be too hard to track when things were
                        // dirty.  Most situations in ShipWokrs use single relations like this, with the exception probably of just template processing
                        // and determining context keys.
                        bool cacheResults = relations.Count == 1;

                        // If we've decided to cache results, we've got some more checking...
                        if (cacheResults)
                        {
                            // The Many side is always the one we need to be monitoring - since it's the one the ParentID on it.
                            EntityType monitoredType = relations[0].StartElementIsPkSide ? relateToType : entityType;

                            // Can't cache if its not monitored - b\c we wouldn't know when it was invalidated
                            cacheResults = entityTypes.Contains(monitoredType);
                        }

                        // We will need the source parent ID if there is more than once source entityID
                        bool needFetchEntityID = needsFetched.Count > 1;

                        // We need to try to pull the ids of the context type
                        ResultsetFields resultFields = new ResultsetFields(needFetchEntityID ? 2 : 1);
                        resultFields.DefineField(relateToPKField, 0, "RelatedID", "");

                        // Are we grabbing the parent key?
                        if (needFetchEntityID)
                        {
                            resultFields.DefineField(entityPKField, 1, "EntityID", "");
                        }

                        List<Tuple<long, long>> resultRows = new List<Tuple<long, long>>();

                        // Fetch a DataRow for each related key.  We only do up to "chunkSize" parent keys at a time.
                        int chunkPosition = 0;
                        while (chunkPosition < needsFetched.Count)
                        {
                            long[] chunk = needsFetched.Keys.Skip(chunkPosition).Take(maxRelatedKeysChunkSize).ToArray();

                            // Advance for next time
                            chunkPosition += chunk.Length;

                            // Using the specified entity id as the filter
                            RelationPredicateBucket bucket = new RelationPredicateBucket(entityPKField == chunk);
                            bucket.Relations.AddRange(relations);

                            // Add in relations required to sort, if any
                            if (sort != null && sort.Relations != null)
                            {
                                bucket.Relations.AddRange(sort.Relations);
                            }

                            using (SqlAdapter adapter = new SqlAdapter())
                            {
                                using (SqlDataReader reader = (SqlDataReader) adapter.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, 0, (sort != null ? sort.SortExpression : null), false))
                                {
                                    while (reader.Read())
                                    {
                                        resultRows.Add(Tuple.Create(reader.GetInt64(0), needFetchEntityID ? reader.GetInt64(1) : 0));
                                    }
                                }
                            }
                        }

                        // If we were needing the EntityID
                        if (needFetchEntityID)
                        {
                            var groupings = resultRows.GroupBy(r => r.Item2, r => r.Item1);

                            foreach (var group in groupings)
                            {
                                // Get the array of keys for the group
                                long[] relatedKeys = group.ToArray();

                                // And add them to our final result set to be returned
                                resultKeys[needsFetched[group.Key]] = relatedKeys;

                                // Cache if necessary
                                if (cacheResults)
                                {
                                    SetCache(group.Key, relateToType, relations[0], sort, relatedKeys);
                                }
                            }
                        }
                        else
                        {
                            Debug.Assert(needsFetched.Count == 1);

                            // Get the array of keys for the entity
                            long[] relatedKeys = resultRows.Select(r => r.Item1).ToArray();

                            // And add them to our final result set to be returned
                            resultKeys[0] = relatedKeys;

                            // Cache if necssary
                            if (cacheResults)
                            {
                                SetCache(needsFetched.Keys.First(), relateToType, relations[0], sort, relatedKeys);
                            }
                        }
                    }
                }
                finally
                {
                    if (tookLock)
                    {
                        semaphore.Release();
                    }
                }
            }

            return resultKeys.Where(keys => keys != null).SelectMany(keys => keys).Distinct().ToList();
        }

        /// <summary>
        /// Determine which entities in the given idlist needs fetched.  The one's that dont are put in the resultKeys array, indexed based on the original requested
        /// key order
        /// </summary>
        private Dictionary<long, int> DetermineMissingKeys(List<long> idList, EntityType entityType, EntityType relateToType, SortDefinition sort, long[][] resultKeys)
        {
            Dictionary<long, int> needsFetched = new Dictionary<long, int>();

            // Determine which ones we need
            for (int i = 0; i < idList.Count; i++)
            {
                // If we havnt already check for and cached this one...
                if (resultKeys[i] == null)
                {
                    long key = idList[i];

                    long[] keys = CheckCache(key, relateToType, sort);

                    if (keys == null)
                    {
                        // Special case for orders and looking for specific parent relationships
                        if (entityType == EntityType.OrderEntity)
                        {
                            if (relateToType == EntityType.StoreEntity)
                            {
                                keys = new long[] { DataProvider.GetOrderHeader(key).StoreID };
                            }

                            if (relateToType == EntityType.CustomerEntity)
                            {
                                OrderEntity order = (OrderEntity) entityCache.GetEntity(key, false);
                                if (order != null)
                                {
                                    keys = new long[] { order.CustomerID };
                                }
                            }
                        }
                    }

                    if (keys == null)
                    {
                        if (!needsFetched.ContainsKey(key))
                        {
                            needsFetched.Add(key, i);
                        }
                    }
                    else
                    {
                        resultKeys[i] = keys;
                    }
                }
            }

            return needsFetched;
        }

        /// <summary>
        /// Check the cache for the given set of keys in the specified sort
        /// </summary>
        private long[] CheckCache(long entityID, EntityType relateToType, SortDefinition sort)
        {
            CacheEntry entry =  (CacheEntry) cache[GetCacheKey(entityID, relateToType)];
            if (entry == null)
            {
                return null;
            }

            string requestedSortDescription = sort != null ? sort.GetDescription() : string.Empty;
            if (entry.SortDescription == requestedSortDescription)
            {
                return entry.RelatedKeys;
            }
            else
            {
                if (requestedSortDescription.Length == 0)
                {
                    return entry.RelatedKeys.OrderBy(k => k).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Add the given data to the cache
        /// </summary>
        private void SetCache(long entityID, EntityType relateToType, IRelation relation, SortDefinition sort, long[] relatedKeys)
        {
            bool oneToMany = relation.StartElementIsPkSide;

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = TimeSpan.FromMinutes(30);

            // For one to many, we invalidate if anything on the many side changes
            if (oneToMany)
            {
                if (!entityTypes.Contains(relateToType))
                {
                    throw new InvalidOperationException(string.Format("Trying to cache entityType {0} which is not monitored for changes.", relateToType.ToString()));
                }

                policy.ChangeMonitors.Add(new EntityTypeChangeMonitor(changeNotifiers[relateToType]));
            }
            else
            {
                EntityType entityType = EntityUtility.GetEntityType(entityID);

                if (!entityTypes.Contains(entityType))
                {
                    throw new InvalidOperationException(string.Format("Trying to cache entityType {0} which is not monitored for changes.", entityType.ToString()));
                }
            }

            #if DEBUG
                policy.RemovedCallback = new CacheEntryRemovedCallback(OnCacheItemRemoved);
            #endif

            string sortDescription = sort != null ? sort.GetDescription() : string.Empty;

            CacheEntry entry = new CacheEntry();
            entry.FromKey = entityID;
            entry.SortDescription = sortDescription;

            // Ensure if there is no sort, that we store them ordered by key
            entry.RelatedKeys = (sortDescription.Length == 0) ? relatedKeys.OrderBy(k => k).ToArray() : relatedKeys;

            // Need to know if its OneToMany or not to know when to eject certain entries
            entry.OneToMany = oneToMany;

            cache.Set(
                GetCacheKey(entityID, relateToType),
                entry,
                policy);
        }

        /// <summary>
        /// Completely clear the cache
        /// </summary>
        public void Clear()
        {
            MemoryCache previous = cache;
            cache = new MemoryCache("EntityRelationCache");

            previous.Dispose();
        }

        /// <summary>
        /// Clear the cache of all entries where the "relatedToType" is of the given type, and the relatedToType was on the Many side of the relation.  So for the OrderItem entity type
        /// Order->OrderItem relations would get cleared, but not OrderItem->OrderItemAttribute
        /// </summary>
        public void ClearRelatedTo(EntityType entityType)
        {
            if (changeNotifiers.ContainsKey(entityType))
            {
                changeNotifiers[entityType].NotifyChanged();
            }
        }

        /// <summary>
        /// Clear the cache of all entries where the start side of the relation is the given key, and its the start side of a ManyToOne relation.  So for an OrderItem key,
        /// its OrderItem->Order relation would get cleared, but not OrderItem->OrderItemAttribute.
        /// </summary>
        public void ClearRelatedFrom(long key)
        {
            // The Enumerator implementation of the cache does a thread-safe snapshot of the data, so it's 
            // safe to do this loop.
            foreach (var pair in cache)
            {
                CacheEntry entry = (CacheEntry) pair.Value;

                if (entry.FromKey == key && !entry.OneToMany)
                {
                    cache.Remove(pair.Key);
                }
            }
        }

        /// <summary>
        /// Get the cache key based on the given entityID
        /// </summary>
        private static string GetCacheKey(long entityID, EntityType relateToType)
        {
            return string.Format("{0}_{1}", entityID, relateToType);
        }

        /// <summary>
        /// Called when an item is removed from the cache
        /// </summary>
        private static void OnCacheItemRemoved(CacheEntryRemovedArguments args)
        {
            log.DebugFormat("EntityRelatedKeys {0} removed from cache due to {1}", args.CacheItem.Key, args.RemovedReason);
        }
    }
}
