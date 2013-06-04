using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks
{
    /// <summary>
    /// The ways a task can be executed.  
    /// </summary>
    public enum MigrationTaskRunPattern
    {
        /// <summary>
        /// A task is to be executed a single time
        /// </summary>
        RunOnce,

        /// <summary>
        /// a task is to be executed over and over until 0 is returned from Run()
        /// </summary>
        Repeated
    }
}
