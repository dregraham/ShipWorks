using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions
{
    /// <summary>
    /// The overral status of running a queue
    /// </summary>
    public enum ActionRunnerResult
    {
        /// <summary>
        /// The action ran.  The actual result of the run could be anything
        /// </summary>
        Ran,

        /// <summary>
        /// The action did not run because it was locked by another ShipWorks instance
        /// </summary>
        Locked,

        /// <summary>
        /// The action did not run because it appears to have been deleted
        /// </summary>
        Missing,

        /// <summary>
        /// The action did not run because it has no tasks
        /// </summary>
        NoTasks,

        /// <summary>
        /// The action did not run because this is not the correct computer
        /// </summary>
        WrongComputer,

        /// <summary>
        /// The action did not run because it is postponed on this or another computer
        /// </summary>
        Postponed
    }
}
