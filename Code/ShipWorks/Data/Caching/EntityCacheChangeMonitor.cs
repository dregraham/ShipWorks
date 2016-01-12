using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Interapptive.Shared;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Users;
using ThreadTimer = System.Threading.Timer;

namespace ShipWorks.Data.Caching
{
    /// <summary>
    /// Wraps an entity cache and monitors it for changes
    /// </summary>
    public sealed class EntityCacheChangeMonitor : IDisposable
    {
        EntityCache entityCache;
        EntityRelationCache relationCache;

        EntityChangeTrackingMonitor changeMonitor;

        ThreadTimer changeMonitorTimer;
        TimeSpan changeMonitorFrequency = TimeSpan.FromSeconds(10);

        private ExecutionMode executionMode;

        bool disposed = false;
        object disposedLock = new object();

        /// <summary>
        /// Raised when entities are detected as being dirty and are removed from cache.  Can be called from any thread.
        /// </summary>
        public event EntityCacheChangeMonitoredChangedEventHandler CacheChanged;

        /// <summary>
        /// Construct a new monitor that will use the given cache as its backing store
        /// </summary>
        public EntityCacheChangeMonitor(EntityCache entityCache, EntityRelationCache relationCache = null)
            : this(entityCache, relationCache, Program.ExecutionMode)
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCacheChangeMonitor"/> class. This is version of the 
        /// constructor is primarily for integration testing as it allows consumers to provide a specific execution mode
        /// rather than having the expectation of Program.ExecutionMode being assigned.
        /// </summary>
        public EntityCacheChangeMonitor(EntityCache entityCache, EntityRelationCache relationCache, ExecutionMode executionMode)
        {
            if (entityCache == null)
            {
                throw new ArgumentNullException("entityCache");
            }

            this.entityCache = entityCache;
            this.relationCache = relationCache;

            // Fall back to Program.ExecutionMode if none is provided
            this.executionMode = executionMode ?? Program.ExecutionMode;

            // If they were both specified, they have to support the same types
            if (relationCache != null)
            {
                if (entityCache.EntityTypes.Count() != relationCache.EntityTypes.Count() ||
                    entityCache.EntityTypes.Intersect(relationCache.EntityTypes).Count() != entityCache.EntityTypes.Count())
                {
                    throw new InvalidOperationException("When specifying a relationCache, it must monitor the same EntityType set as the EntityCache.");
                }
            }

            changeMonitor = new EntityChangeTrackingMonitor();
            changeMonitor.Initialize(entityCache.EntityTypes);

            // Create the timer.  This will execute the timer one time.
            changeMonitorTimer = new ThreadTimer(OnChangeMonitorTimer, null, changeMonitorFrequency, TimeSpan.FromMilliseconds(-1));
        }

        /// <summary>
        /// The change tracking sync version the database was at on the last sync
        /// </summary>
        public long LastSyncVersion
        {
            get { return changeMonitor.LastSyncVersion; }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Debug.Assert(!executionMode.IsUISupported || !Program.MainForm.InvokeRequired);

            lock (disposedLock)
            {
                disposed = true;

                // We don't own the cache - but we do own and have to destroy the timer
                if (changeMonitorTimer != null)
                {
                    changeMonitorTimer.Dispose();
                    changeMonitorTimer = null;
                }
            }
        }

        /// <summary>
        /// Callback for the timer that monitors for entity changes
        /// </summary>
        private void OnChangeMonitorTimer(object state)
        {
            ApplicationBusyToken operationToken = GetCheckForChangesOperationToken();

            // If we couldn't get the token, nothing to do until next time
            if (operationToken == null)
            {
                // Kick off another interval if we aren't disposed
                KickoffNextTimerInterval();
            }
            else
            {
                ThreadPool.QueueUserWorkItem(
                    ExceptionMonitor.WrapWorkItem(AsyncMonitorChanges),
                    operationToken);
            }
        }

        /// <summary>
        /// Kickoff the next timer interval, as long as we haven't been disposed
        /// </summary>
        private void KickoffNextTimerInterval()
        {
            lock (disposedLock)
            {
                if (!disposed)
                {
                    changeMonitorTimer.Change(changeMonitorFrequency, TimeSpan.FromMilliseconds(-1));
                }
            }
        }

        /// <summary>
        /// Get the token to use while we check for changes
        /// </summary>
        private ApplicationBusyToken GetCheckForChangesOperationToken()
        {
            if (ConnectionSensitiveScope.IsActive ||
                !UserSession.IsLoggedOn ||
                ConnectionMonitor.Status != ConnectionMonitorStatus.Normal)
            {
                return null;
            }

            ApplicationBusyToken operationToken;
            if (!ApplicationBusyManager.TryOperationStarting("synchronizing data", out operationToken))
            {
                return null;
            }

            // Recheck login after we've gotten the token
            if (!UserSession.IsLoggedOn)
            {
                ApplicationBusyManager.OperationComplete(operationToken);
                return null;
            }

            return operationToken;
        }

        /// <summary>
        /// Called on background thread to monitor for changes to data
        /// </summary>
        [NDependIgnoreLongMethod]
        private void AsyncMonitorChanges(object state)
        {
            ApplicationBusyToken busyToken = (ApplicationBusyToken) state;

            var changeSets = changeMonitor.CheckForChanges();

            // Tracks various changes to EntityTypes
            List<EntityType> updatedTypes = new List<EntityType>();
            List<EntityType> insertedTypes = new List<EntityType>();
            List<EntityType> deletedTypes = new List<EntityType>();

            // Go through each changeset (one per entity we are monitoring)
            foreach (EntityChangeTrackingChangeset changeset in changeSets)
            {
                if (!changeset.IsValid || changeset.Inserts.Count > 0) insertedTypes.Add(changeset.EntityType);
                if (!changeset.IsValid || changeset.Updates.Count > 0) updatedTypes.Add(changeset.EntityType);
                if (!changeset.IsValid || changeset.Deletes.Count > 0) deletedTypes.Add(changeset.EntityType);

                if (changeset.IsValid)
                {
                    // Remove all the inserts and deletes from the cache
                    foreach (long key in changeset.Inserts.Concat(changeset.Deletes))
                    {
                        entityCache.Remove(key);
                    }

                    // Also removes all the updates from the cache... and also remove any relations that were "RelatedFrom" (on the Many side) of the given
                    // key from the relation cache.  B\c the updated entity could have changed its parent key.  Unlikely, but we support listening for it.
                    foreach (long key in changeset.Updates)
                    {
                        entityCache.Remove(key);

                        if (relationCache != null)
                        {
                            relationCache.ClearRelatedFrom(key);
                        }
                    }
                }
                else
                {
                    // Have to clear everything of this type - since its not valid, we can't be sure what's old.  So we just have to
                    // start over.
                    entityCache.Clear(changeset.EntityType);
                }
            }

            if (relationCache != null)
            {
                // Any time there is an insert\delete of an EntityType, we need to make sure we wipe any cached relations where we are storing them on the target side of a OneToMany,
                // because the insert\delete could have changed that result set.  Not looking at Updates here does however mean we could end up leaving stale sorts in cache, but whatever,
                // I think we are good with that.
                foreach (EntityType entityType in insertedTypes.Concat(deletedTypes).Distinct())
                {
                    relationCache.ClearRelatedTo(entityType);
                }
            }

            // If we are disposed, no need to callback, or to restart the timer
            lock (disposedLock)
            {
                if (disposed)
                {
                    busyToken.Dispose();

                    return;
                }
            }

            if (insertedTypes.Count + updatedTypes.Count + deletedTypes.Count > 0)
            {
                EntityCacheChangeMonitoredChangedEventHandler handler = CacheChanged;
                if (handler != null)
                {
                    handler(null, new EntityCacheChangeMonitoredChangedEventArgs(insertedTypes, updatedTypes, deletedTypes));
                }
            }

            busyToken.Dispose();

            if (executionMode.IsUIDisplayed)
            {
                Program.MainForm.BeginInvoke((System.Windows.Forms.MethodInvoker) delegate
                    {
                        // Kick off another interval if we aren't disposed
                        KickoffNextTimerInterval();
                    });
            }
            else
            {
                // Kick off another interval if we aren't disposed
                KickoffNextTimerInterval();
            }
        }
    }
}
