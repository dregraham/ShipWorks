using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Caching
{
    /// <summary>
    /// Delegate prototype for the CacheChanged event
    /// </summary>
    public delegate void EntityCacheChangeMonitoredChangedEventHandler(object sender, EntityCacheChangeMonitoredChangedEventArgs e);

    /// <summary>
    /// EventArgs for the CacheChanged event
    /// </summary>
    public class EntityCacheChangeMonitoredChangedEventArgs : EventArgs
    {
        List<EntityType> inserted;
        List<EntityType> updated;
        List<EntityType> deleted;

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityCacheChangeMonitoredChangedEventArgs(List<EntityType> inserted, List<EntityType> updated, List<EntityType> deleted)
        {
            this.inserted = inserted;
            this.updated = updated;
            this.deleted = deleted;
        }

        /// <summary>
        /// The list of EntityType's that had inserted since last time
        /// </summary>
        public List<EntityType> Inserted
        {
            get { return inserted; }
        }

        /// <summary>
        /// The list of EntityType's that had updates since last time
        /// </summary>
        public List<EntityType> Updated
        {
            get { return updated; }
        }

        /// <summary>
        /// The list of EntityType's that had delets since last time
        /// </summary>
        public List<EntityType> Deleted
        {
            get { return deleted; }
        }
    }
}
