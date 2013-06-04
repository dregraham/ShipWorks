using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions
{
    /// <summary>
    /// The status of an entry in the ActionQueue
    /// </summary>
    public enum ActionQueueStatus
    {
        /// <summary>
        /// Initial state, the action was just dispatched
        /// </summary>
        Dispatched = 0,

        /// <summary>
        /// The action is currently being processed.  The workflow may be done (every step processed), but some
        /// steps may be postponed.
        /// </summary>
        Incomplete = 1,

        /// <summary>
        /// The action was ran, and is complete with success
        /// </summary>
        Success = 2,

        /// <summary>
        /// The action was ran, and there was a failure
        /// </summary>
        Error = 3,

        /// <summary>
        /// The queue is suspeneded while waiting for a Postopned step
        /// </summary>
        Postponed = 4,

        /// <summary>
        /// The queue has had its postponed step consumed, and is ready to keep going the next time its processed
        /// </summary>
        ResumeFromPostponed = 5
    }
}
