using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database
{
    /// <summary>
    /// The possible states of a ShipWorks 2 -> 3 Data Migration 
    /// </summary>
    public enum MigrationState
    {
        // Undetermined
        Unknown = 0,
        
        // No migration has been started
        NotStarted = 1,

        // A previous migration has run/failed.
        ResumeRequired = 2,

        // Currently Executing
        Running = 3,

        // All migrationt tasks have run, but post-tasks have not
        MainExecutionComplete = 4
    }
}