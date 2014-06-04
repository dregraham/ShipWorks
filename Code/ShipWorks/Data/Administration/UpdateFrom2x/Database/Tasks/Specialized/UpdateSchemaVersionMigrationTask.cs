using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    /// <summary>
    /// Should be the final migration task to run that updates the schema version to what the v2 migrator updates to
    /// </summary>
    public class UpdateSchemaVersionMigrationTask : MigrationTaskBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateSchemaVersionMigrationTask()
            : base(WellKnownMigrationTaskIds.UpdateSchemaVersion, MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce)
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public UpdateSchemaVersionMigrationTask(UpdateSchemaVersionMigrationTask toCopy)
            : base(toCopy)
        {

        }

        /// <summary>
        /// Unique TaskCode for later factory instantiation of the task
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.UpdateSchemaVersionTask; }
        }

        /// <summary>
        /// Create an exact clone of the task
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new UpdateSchemaVersionMigrationTask(this);
        }

        /// <summary>
        /// Run the task
        /// </summary>
        protected override int Run()
        {
            Progress.Detail = "Updating schema version...";

            using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
            {
                SqlSchemaUpdater.UpdateSchemaVersionStoredProcedure(con, new Version("3.0.0.6"));
            }

            return 0;
        }

        /// <summary>
        /// Estimate
        /// </summary>
        protected override int RunEstimate(SqlConnection con)
        {
            return 1;
        }
    }
}
