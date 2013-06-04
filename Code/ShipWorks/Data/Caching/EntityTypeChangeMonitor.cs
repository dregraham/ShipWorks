using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Caching
{
    /// <summary>
    /// A custom cache ChangeMonitor for monitoring for notifications of changes to a specific EntityType
    /// </summary>
    public class EntityTypeChangeMonitor : ChangeMonitor
    {
        EntityTypeChangeNotifier notifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityTypeChangeMonitor(EntityTypeChangeNotifier notifier)
        {
            if (notifier == null)
            {
                throw new ArgumentNullException("notifier");
            }

            this.notifier = notifier;
            this.notifier.Changed += OnNotifierNotification;

            InitializationComplete();
        }

        /// <summary>
        /// UniqueID based on cache monitored data
        /// </summary>
        public override string UniqueId
        {
            get { return notifier.EntityType.ToString(); }
        }

        /// <summary>
        /// Disposal
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            notifier.Changed -= OnNotifierNotification;
        }

        /// <summary>
        /// Called back to notify us that there has been a change
        /// </summary>
        private void OnNotifierNotification(object sender, EventArgs e)
        {
            OnChanged(notifier.EntityType);
        }
    }
}
