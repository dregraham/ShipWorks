using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Filters.Content.Editors
{
    /// <summary>
    /// Delegate used for the ConditionChanged event
    /// </summary>
    public delegate void ConditionTypeChangedEventHandler(object sender, ConditionTypeChangedEventArgs e);

    /// <summary>
    /// EventArgs for the ConditionChanged event
    /// </summary>
    public class ConditionTypeChangedEventArgs : EventArgs
    {
        Condition oldCondition;
        Condition newCondition;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionTypeChangedEventArgs(Condition oldCondition, Condition newCondition)
        {
            this.oldCondition = oldCondition;
            this.newCondition = newCondition;
        }

        /// <summary>
        /// The condition type that is being changed from
        /// </summary>
        public Condition OldCondition
        {
            get
            {
                return oldCondition;
            }
        }

        /// <summary>
        /// The condition type that is changed to
        /// </summary>
        public Condition NewCondition
        {
            get
            {
                return newCondition;
            }
        }
    }
}
