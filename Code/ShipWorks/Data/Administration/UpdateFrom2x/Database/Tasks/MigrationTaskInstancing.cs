using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks
{
    /// <summary>
    /// The ways a single task definition can be instantiated in the 
    /// final execution plan for database upgrades.
    /// </summary>
    public enum MigrationTaskInstancing
    {
        // The task is to be inserted once into the resulting
        // execution plan
        MainDatabaseOnly,

        // The task is to result in multiple task instances, one for each
        // archive database and main database.
        MainDatabaseAndArchives,

        // The task is to result in multiple task instances, one for each archive
        // database.
        ArchiveDatabasesOnly
    }
}
