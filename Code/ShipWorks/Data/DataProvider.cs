using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using System.Threading;
using ShipWorks.Filters;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using System.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using System.Diagnostics;
using WindowsTimer = System.Windows.Forms.Timer;
using ShipWorks.Users;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using log4net;
using System.Runtime.Caching;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Utility;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks.Data
{
    /// <summary>
    /// Central location for obtaining singleton entity cache objects
    /// </summary>
    public static class DataProvider
    {
        static readonly ILog log = LogManager.GetLogger(typeof(DataProvider));

        // Cache of individual entities, keyed by their ID
        static EntityCache entityCache;
        static EntityRelationCache relationCache;

        // Provider of headers
        static OrderHeaderProvider orderHeaderProvider;

        // The wrapper that monitors the cache for changes
        static EntityCacheChangeMonitor cacheChangeMonitor;

        private static ExecutionMode executionMode;

        // The entity types we support caching of and monitor for changes
        static EntityType[] changeMonitoredEntityTypes = new EntityType[]
                {
                    EntityType.CustomerEntity,
                    EntityType.OrderEntity,
                    EntityType.OrderItemEntity,
                    EntityType.OrderItemAttributeEntity,
                    EntityType.OrderChargeEntity,
                    EntityType.OrderPaymentDetailEntity,
                    EntityType.ShipmentEntity,
                    EntityType.ShipmentCustomsItemEntity,
                    EntityType.PrintResultEntity,
                    EntityType.NoteEntity,
                    EntityType.EmailOutboundEntity,
                    EntityType.StoreEntity,
                    EntityType.ServiceStatusEntity
                };

        // Maintains version information of each entity and when update, insert, and deletes are detected for it
        static Dictionary<EntityType, EntityTypeChangeVersion> entityTypeChangeVersions;

        /// <summary>
        /// Raised when entities are detected as being dirty and are removed from cache.  Can be called from any thread.
        /// </summary>
        public static event EventHandler EntityChangeDetected;

        /// <summary>
        /// Raised when order entities are detected as being dirty and are removed from cache.  Can be called from any thread.
        /// </summary>
        public static event EventHandler OrderEntityChangeDetected;

        /// <summary>
        /// Raised when shipment entities are detected as being dirty and are removed from cache. Can be called from any thread.
        /// </summary>
        public static event EventHandler ShipmentEntityChangeDetected;

        /// <summary>
        /// Do one-time application level initialization
        /// </summary>
        public static void InitializeForApplication()
        {
            InitializeForApplication(Program.ExecutionMode);
        }

        /// <summary>
        /// Do one-time application level initialization
        /// </summary>
        public static void InitializeForApplication(ExecutionMode mode)
        {
            executionMode = mode;

            entityTypeChangeVersions = changeMonitoredEntityTypes.ToDictionary(e => e, e => new EntityTypeChangeVersion(e));

            entityCache = new EntityCache(changeMonitoredEntityTypes);
            relationCache = new EntityRelationCache(entityCache, executionMode);

            orderHeaderProvider = new OrderHeaderProvider(entityCache, executionMode);

            // Listen for local entity saves so we can clear those out of our cache right away
            CommonEntityBase.EntityPersisted += new EntityPersistedEventHandler(OnLocalEntityPersisted);
        }

        /// <summary>
        /// Clears any existing cache entries in preparation for a newly connected database
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            InitializeForCurrentDatabase(Program.ExecutionMode);
        }

        /// <summary>
        /// Clears any existing cache entries in preparation for a newly connected database. This
        /// overloaded version of InitializeForCurrentDatabase is intended to be used for 
        /// integration testing purposes.
        /// </summary>
        public static void InitializeForCurrentDatabase(ExecutionMode mode)
        {
            Reset();

            executionMode = mode;

            // Properly dispose the old one if there is one
            if (cacheChangeMonitor != null)
            {
                cacheChangeMonitor.CacheChanged -= new EntityCacheChangeMonitoredChangedEventHandler(OnChangeMonitorChangedCache);
                cacheChangeMonitor.Dispose();
                cacheChangeMonitor = null;
            }

            cacheChangeMonitor = new EntityCacheChangeMonitor(entityCache, relationCache, executionMode);
            cacheChangeMonitor.CacheChanged += new EntityCacheChangeMonitoredChangedEventHandler(OnChangeMonitorChangedCache);
        }

        /// <summary>
        /// Clear the cache of just the entities, the relations and other things remain
        /// </summary>
        public static void ClearEntityCache()
        {
            entityCache.Clear();

            foreach (EntityType entityType in changeMonitoredEntityTypes)
            {
                entityTypeChangeVersions[entityType].Increment();
            }
        }

        /// <summary>
        /// Clear all contents of the cache
        /// </summary>
        private static void Reset()
        {
            entityCache.Clear();
            relationCache.Clear();

            orderHeaderProvider.Clear();

            foreach (EntityType entityType in changeMonitoredEntityTypes)
            {
                entityTypeChangeVersions[entityType].Increment();
            }
        }

        /// <summary>
        /// Remove the entity with the given ID from the cache.  If it doesn't exist, no change occurs.
        /// </summary>
        public static void RemoveEntity(long entityID)
        {
            entityCache.Remove(entityID);

            if (EntityUtility.HasEntitySeedInfo(entityID) && changeMonitoredEntityTypes.Contains(EntityUtility.GetEntityType(entityID)))
            {
                IncrementChangeVersion(EntityUtility.GetEntityType(entityID), EntityPersistedAction.Delete);
            }
        }

        /// <summary>
        /// Get the current change versioning for the given entity
        /// </summary>
        public static EntityTypeChangeVersion GetEntityTypeChangeVersion(EntityType entityType)
        {
            return entityTypeChangeVersions[entityType];
        }

        /// <summary>
        /// Get the last sync version used to sync the cache with changes from the database
        /// </summary>
        public static long GetLastSqlSyncVersion()
        {
            return cacheChangeMonitor.LastSyncVersion;
        }

        /// <summary>
        /// Gets the entity with the given ID from cache.  If it does not exist, it is loaded.
        /// </summary>
        public static EntityBase2 GetEntity(long entityID, bool fetchIfMissing = true)
        {
            return entityCache.GetEntity(entityID, fetchIfMissing);
        }

        /// <summary>
        /// Gets all the entities represented by the give keys.  Each key must be of the same EntityType
        /// </summary>
        public static List<EntityBase2> GetEntities(List<long> keyList)
        {
            return entityCache.GetEntities(keyList);
        }

        /// <summary>
        /// Get all entities of the given type that are related to the specified entityID
        /// </summary>
        public static List<EntityBase2> GetRelatedEntities(long entityID, EntityType entityType)
        {
            List<long> keyList = GetRelatedKeys(entityID, entityType);

            return GetEntities(keyList);
        }

        /// <summary>
        /// Translate the given list ID into all the keys they relate to of the given target type
        /// </summary>
        public static List<long> GetRelatedKeys(long id, EntityType relateToType)
        {
            return GetRelatedKeys(new List<long>(new long[] { id }), relateToType);
        }

        /// <summary>
        /// Translate the given list of ID's into all the keys they relate to of the given target type.  If fetchIsMissing is false, and ANY keys in the list are not cached, then null is returned.
        /// </summary>
        public static List<long> GetRelatedKeys(List<long> idList, EntityType relateToType, bool fetchIfMissing = true, SortDefinition sort = null)
        {
            return relationCache.GetRelatedKeys(idList, relateToType, fetchIfMissing, sort);
        }

        /// <summary>
        /// Get header information (the StoreID, and IsManual) setting for the given order
        /// </summary>
        public static OrderHeader GetOrderHeader(long orderID)
        {
            // 1st try what's in the cache right now
            OrderHeader header = orderHeaderProvider.GetHeader(orderID);

            if (header != null)
            {
                return header;
            }
            // If it's not there... Order must have been deleted before we ever saw it - not sure what else we can do here
            else
            {
                return new OrderHeader(orderID, -1005, true);
            }
        }

        /// <summary>
        /// An entity has just been saved to the database
        /// </summary>
        private static void OnLocalEntityPersisted(object sender, EntityPersistedEventArgs e)
        {
            EntityBase2 entity = (EntityBase2) sender;

            if (entity.Fields.PrimaryKeyFields.Count == 0 || !(entity.Fields.PrimaryKeyFields[0].CurrentValue is long))
            {
                return;
            }

            long key = (long) entity.Fields.PrimaryKeyFields[0].CurrentValue;

            if (!EntityUtility.HasEntitySeedInfo(key) || !changeMonitoredEntityTypes.Contains(EntityUtility.GetEntityType(key)))
            {
                return;
            }

            EntityType entityType = EntityUtility.GetEntityType(key);

            // First it has to have been full refetched after an Insert or Update
            bool shouldCacheEntity = e.Action != EntityPersistedAction.Delete && entity.Fields.State == EntityState.Fetched;

            // Second - special case - since the "child" shipment rows share a ShipmentID - but we only want to cache the true ShipmentEntity type,
            // we have to make sure we are caching for instance a FedExShipmentEntity under the ShipmentID where the primary ShipmentEntity is 
            // expected to be.
            if (shouldCacheEntity && entityType == EntityType.ShipmentEntity && !(entity is ShipmentEntity))
            {
                shouldCacheEntity = false;
            }

            if (shouldCacheEntity)
            {
                entityCache.SetCache(key, entity);
            }
            else
            {
                entityCache.Remove(key);
            }

            if (e.Action == EntityPersistedAction.Insert || e.Action == EntityPersistedAction.Delete)
            {
                // We know some entity of this type (say a shipment) got inserted or deleted.  So we need
                // to clear the cache of anyone with related shipment keys - since there could be more or less
                // of them now.
                relationCache.ClearRelatedTo(entityType);
            }
            else
            {
                relationCache.ClearRelatedFrom(key);
            }

            IncrementChangeVersion(entityType, e.Action);

            // NOTE: We don't raise an event here.  This method could be getting called lots of time rapidly - like when updating a huge selection in bulk.
            // It's up to the code that does the editing to trigger a grid refresh \ whatever when its done.
        }

        /// <summary>
        /// Called back when our change monitoring cache wrapper reports that changes have been detected and put in place
        /// </summary>
        private static void OnChangeMonitorChangedCache(object sender, EntityCacheChangeMonitoredChangedEventArgs e)
        {
            // Record changes to all affected types
            e.Inserted.ForEach(entityType => IncrementChangeVersion(entityType, EntityPersistedAction.Insert));
            e.Updated.ForEach(entityType => IncrementChangeVersion(entityType, EntityPersistedAction.Update));
            e.Deleted.ForEach(entityType => IncrementChangeVersion(entityType, EntityPersistedAction.Delete));

            // If we know orders have been added, go ahead and kickoff the loading of latest headers
            if (e.Inserted.Contains(EntityType.OrderEntity))
            {
                orderHeaderProvider.InitiateHeaderLoading();
            }

            if (executionMode.IsUIDisplayed)
            {
                Program.MainForm.BeginInvoke((System.Windows.Forms.MethodInvoker) delegate
                    {
                        RaiseEntityChangeDetected(e);
                    });
            }
            else
            {
                RaiseEntityChangeDetected(e);
            }
        }

        /// <summary>
        /// Handle caching and change versioning stuff for the given entityType and action
        /// </summary>
        private static void IncrementChangeVersion(EntityType entityType, EntityPersistedAction action)
        {
            // Increment the change version for this action
            entityTypeChangeVersions[entityType].Increment(action);
        }

        /// <summary>
        /// Raise the EntityChangeDetected event
        /// </summary>
        private static void RaiseEntityChangeDetected(EntityCacheChangeMonitoredChangedEventArgs e)
        {
            EventHandler handler = EntityChangeDetected;
            if (handler != null)
            {
                handler(null, EventArgs.Empty);
            }
            
            // Only raise the order specific entity change event if it's subscribed to 
            // and if any of the changes are for order entities
            EventHandler orderHandler = OrderEntityChangeDetected;
            if (orderHandler != null)
            {
                if (DidEntityTypeChanged(EntityType.OrderEntity, e))
                {
                    orderHandler(null, EventArgs.Empty);
                }
            }

            // Only raise the shipment specific entity change event if it's subscribed to 
            // and if any of the cahnges are for shipment entities
            EventHandler shipmentHandler = ShipmentEntityChangeDetected;
            if (shipmentHandler != null && DidEntityTypeChanged(EntityType.ShipmentEntity, e))
            {
                shipmentHandler(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Detect if the entity type specified changed.
        /// </summary>
        private static bool DidEntityTypeChanged(EntityType entityType, EntityCacheChangeMonitoredChangedEventArgs args)
        {
            return args.Inserted.Any(x => x == entityType) ||
                   args.Updated.Any(x => x == entityType) ||
                   args.Deleted.Any(x => x == entityType);
        }

    }
}
