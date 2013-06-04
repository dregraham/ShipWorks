using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions
{
    /// <summary>
    /// The postponent activity type that took place during the execution of an action step
    /// </summary>
    public enum ActionStepPostponementActivity
    {
        /// <summary>
        /// The step did not get postponed, nor consume previously postponed steps
        /// </summary>
        None,

        /// <summary>
        /// The step postponed
        /// </summary>
        Postponed,

        /// <summary>
        /// The step consumed previously postponed other steps
        /// </summary>
        Consumed
    }
}
