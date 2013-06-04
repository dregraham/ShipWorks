using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Caching
{
    /// <summary>
    /// Provides event to notify listeners that an EntityType has experiences "changes"
    /// </summary>
    public class EntityTypeChangeNotifier
    {
        EntityType entityType;

        /// <summary>
        /// Raised when the EntityType "changes"
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityTypeChangeNotifier(EntityType entityType)
        {
            this.entityType = entityType;
        }

        /// <summary>
        /// The EntityType represented by the notifier
        /// </summary>
        public EntityType EntityType
        {
            get { return entityType; }
        }

        /// <summary>
        /// Notifies any listeners that a "change" has occurred
        /// </summary>
        public void NotifyChanged()
        {
            EventHandler handler = Changed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
