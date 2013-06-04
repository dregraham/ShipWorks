using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks
{
    /// <summary>
    /// The execution modes/phases tasks can be in.
    /// </summary>
    public enum MigrationTaskExecutionPhase
    {
        None = 0,

        // calculating estimated units of work
        Estimate = 1,

        // actually executing
        Execute = 2
    }
}
