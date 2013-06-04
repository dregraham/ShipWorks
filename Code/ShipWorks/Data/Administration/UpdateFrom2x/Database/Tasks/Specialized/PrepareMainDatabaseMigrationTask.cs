using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;
using log4net;
using System.Data;
using System.Diagnostics;
using System.Transactions;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    /// <summary>
    /// Task for getting a Version 2 database ready to be converted to version 3.
    /// </summary>
    public class PrepareMainDatabaseMigrationTask : MigrationTaskBase
    {
        // this task does 4 tasks
        const int workCount = 4;

        /// <summary>
        /// Get the type code for this task
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.PrepareDatabaseTask; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrepareMainDatabaseMigrationTask()
            : base(WellKnownMigrationTaskIds.PrepareDatabase, MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce)
        {

        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        protected PrepareMainDatabaseMigrationTask(PrepareMainDatabaseMigrationTask toCopy) 
            : base(toCopy)
        {

        }

        /// <summary>
        /// Clone the task
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new PrepareMainDatabaseMigrationTask(this);
        }

        /// <summary>
        /// Run the work estimation
        /// </summary>
        protected override int RunEstimate(SqlConnection con)
        {
            return workCount;
        }

        /// <summary>
        /// Execute the task
        /// </summary>
        protected override int Run()
        {
            // not transacted, these are ALTER DATABASE commands
            using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
            {
                SqlUtility.SetSql2008CompatibilityLevel(con);

                // Pre-schema creation database changes
                ScriptLoader["Prepare2xDatabase"].Execute(con);
                ReportWorkProgress();

                // Clear out the pool so that connection holding onto SINGLE_USER gets released
                SqlConnection.ClearAllPools();
                ReportWorkProgress();

                // return value is meaningless here
                return 0;
            }
        }
    }
}