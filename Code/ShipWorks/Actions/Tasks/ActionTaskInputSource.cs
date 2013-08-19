using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Controls where the data that is fed into the task comes from
    /// </summary>
    public enum ActionTaskInputSource
    {
        /// <summary>
        /// No data should be fed into the task
        /// </summary>
        Nothing = -1,

        /// <summary>
        /// The data is the record\row that triggered the action
        /// </summary>
        TriggeringRecord = 0,

        /// <summary>
        /// The data is the entire contents of a selected filter
        /// </summary>
        FilterContents = 1
    }
}
