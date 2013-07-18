using System;
using ShipWorks.Actions.Triggers;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Flags for for determining if tasks can be run as scheduled and/or non-scheduled
    /// </summary>
    /// <remarks>Classification was used instead of Type to avoid confusion with the
    /// <see cref="ActionTriggerType"/> enum</remarks>
    [Flags]
    public enum ActionTriggerClassifications
    {
        /// <summary>
        /// Used for checking which trigger classifications are allowed
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Task is allowed to be run from scheduled trigger
        /// </summary>
        Scheduled = 0x00000001,

        /// <summary>
        /// Task is allowed to be run from non-scheduled trigger
        /// </summary>
        NonScheduled = 0x00000002
    }
}
