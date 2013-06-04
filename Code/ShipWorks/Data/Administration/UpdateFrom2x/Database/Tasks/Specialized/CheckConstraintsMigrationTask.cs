using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using log4net;
using Interapptive.Shared.Data;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    public class CheckConstraintsMigrationTask : MigrationTaskBase
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(CheckConstraintsMigrationTask));

        class ConstraintViolation
        {
            public string Table { get; set; }
            public string Constraint { get; set; }
            public string Where { get; set; }
        }


        /// <summary>
        /// Copy constructor
        /// </summary>
        public CheckConstraintsMigrationTask(CheckConstraintsMigrationTask copy)
            : base(copy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CheckConstraintsMigrationTask()
            : base(WellKnownMigrationTaskIds.CheckConstraints, MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce)
        {
            

        }

        /// <summary>
        /// Gets the task type code
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.CheckConstraintsTask; }
        }

        /// <summary>
        /// Cloning
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new CheckConstraintsMigrationTask(this);
        }

        /// <summary>
        /// Execute the task
        /// </summary>
        protected override int Run()
        {
            // not transacted, these are ALTER DATABASE commands
            using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
            {
                Progress.Detail = "Checking database integrity..";

                // Pre-schema creation database changes
                string script = ScriptLoader["Check2xConstraints.sql"].Content;

                List<ConstraintViolation> violations = new List<ConstraintViolation>();
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    // give it 5 minutes to check consistency
                    cmd.CommandTimeout = 300; 
                    cmd.CommandText = script;
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                violations.Add(new ConstraintViolation
                                                {
                                                    Table = (string)reader["Table"],
                                                    Constraint = (string)reader["Constraint"],
                                                    Where = (string)reader["Where"]
                                                }
                                );
                            }
                        }
                        while (reader.NextResult());
                    }
                }

                if (violations.Count > 0)
                {
                    log.ErrorFormat("Constraint Violations in database {0}", Database);
                    violations.ForEach(v => log.ErrorFormat("{0}.{1} where {2}", v.Table, v.Constraint, v.Where));

                    throw new MigrationException(String.Format("The database {0} has {1} constraint violations that must be fixed before being upgraded to Version 3.  See log for details.", Database, violations.Count));
                }

                ReportWorkProgress();
                return 1;
            }
        }

        /// <summary>
        /// Provide a work estimate
        /// </summary>
        protected override int RunEstimate(SqlConnection con)
        {
            return 1;
        }
    }
}
