using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Gateway to provide lists of action queus to be processed
    /// </summary>
    public abstract class ActionQueueGateway
    {
        /// <summary>
        /// Get an ordered set of actions in the queue that need to be executed, starting after the given queue ID
        /// </summary>
        public abstract List<long> GetNextQueuePage(long lastQueueID);

        /// <summary>
        /// Indicates if it's possible for new queue's to be added to the gateway source while the gateway is being processed
        /// </summary>
        public abstract bool CanNewQueuesArrive { get; }
    }
}
