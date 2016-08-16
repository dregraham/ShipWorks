using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// IEntityProvider implementation that used an EntityChangeTrackingMonitor to determine when to entities need ejected from the cache and to raise
    /// events to help consumers know then the grid needs refreshed.
    /// </summary>
    public sealed class EntityCacheEntityProvider : IEntityProvider, IDisposable
    {
        EntityType entityType;

        // Local cache of entities
        EntityCache entityCache;

        // The monitor used to detect changes to the cache and notify owners that a refresh is needed
        EntityCacheChangeMonitor cacheChangeMonitor;

        /// <summary>
        /// Raised when entities are detected as being dirty and are removed from cache.  Can be called from any thread.
        /// </summary>
        public event EntityCacheChangeMonitoredChangedEventHandler EntityChangesDetected;

        /// <summary>
        /// Create an EntityProvider that updates itself based on change tracking monitored changes.  Prefetch should really only
        /// be used for child data that is not change monitored, otherwise the standard cache approach should be used.
        /// </summary>
        public EntityCacheEntityProvider(EntityType entityType, PrefetchPath2 prefetch = null, bool monitorForChanges = true)
        {
            List<PrefetchPath2> prefetchList = null;
            if (prefetch != null)
            {
                prefetchList = new List<PrefetchPath2> { prefetch };
            }

            this.entityType = entityType;
            this.entityCache = new EntityCache(new EntityType[] { entityType }, prefetchList);

            if (monitorForChanges)
            {
                this.cacheChangeMonitor = new EntityCacheChangeMonitor(entityCache);
                this.cacheChangeMonitor.CacheChanged += new EntityCacheChangeMonitoredChangedEventHandler(OnChangeMonitorChangedCache);
            }
        }

        /// <summary>
        /// Dispose of resources held by the change tracker
        /// </summary>
        public void Dispose()
        {
            if (cacheChangeMonitor != null)
            {
                cacheChangeMonitor.CacheChanged -= new EntityCacheChangeMonitoredChangedEventHandler(OnChangeMonitorChangedCache);
                cacheChangeMonitor.Dispose();
                cacheChangeMonitor = null;
            }

            if (entityCache != null)
            {
                entityCache.Dispose();
                entityCache = null;
            }
        }

        /// <summary>
        /// The type of entity this provider returns entities for
        /// </summary>
        public EntityType EntityType
        {
            get { return entityType; }
        }

        /// <summary>
        /// Fetch the entity with the given ID.  If it is not in cache, and fetchIfMissing is true, it will be retrieved from the database.
        /// </summary>
        public EntityBase2 GetEntity(long entityID, bool fetchIfMissing = true)
        {
            return entityCache.GetEntity(entityID, fetchIfMissing);
        }

        /// <summary>
        /// Gets all the entities represented by the give keys.  Each key must be of the same EntityType
        /// </summary>
        public List<EntityBase2> GetEntities(List<long> keyList)
        {
            return entityCache.GetEntities(keyList);
        }

        /// <summary>
        /// Called back when our change monitoring cache wrapper reports that changes have been detected and put in place
        /// </summary>
        private void OnChangeMonitorChangedCache(object sender, EntityCacheChangeMonitoredChangedEventArgs e)
        {
            Program.MainForm.BeginInvoke((System.Windows.Forms.MethodInvoker) delegate
                {
                    // Only call it back if we're not disposed
                    if (entityCache != null)
                    {
                        EntityCacheChangeMonitoredChangedEventHandler handler = EntityChangesDetected;
                        if (handler != null)
                        {
                            handler(this, e);
                        }
                    }
                });
        }
    }
}
