using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions
{
    /// <summary>
    /// The status of a step in an action queue
    /// </summary>
    public enum ActionQueueStepStatus
    {
        // The step has not been ran
        Pending = 0,

        /// <summary>
        /// The step completed successfully
        /// </summary>
        Success = 1,

        /// <summary>
        /// The step completed with failure
        /// </summary>
        Error = 2,

        /// <summary>
        /// The step was skipped due to a filter condition
        /// </summary>
        Skipped = 3,

        /// <summary>
        /// The step has been postponed to wait for additional steps to complete with.  Such as labels trying to fill sheets.
        /// </summary>
        Postponed = 4
    }
}
