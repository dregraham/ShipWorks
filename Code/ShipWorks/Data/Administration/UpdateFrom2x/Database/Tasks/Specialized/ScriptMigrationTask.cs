using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using System.Text.RegularExpressions;
using System.IO;
using log4net;
using System.Transactions;
using System.Diagnostics;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    /// <summary>
    /// Class representing a script that needs to be executed on
    /// as part of hte upgrade process
    /// </summary>
    public class ScriptMigrationTask : MigrationTaskBase
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(ScriptMigrationTask));
						  
        // path to script (relative to the script loader) to be executed
        string scriptResource = "";

        // text for progress detail
        string progressDetail = "";

        /// <summary>
        /// Get the type code for this migration task
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.ScriptMigrationTask; }
        }

        /// <summary>
        /// Constructor used during deserialization
        /// </summary>
        public ScriptMigrationTask() : base("")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScriptMigrationTask(string identifier, string scriptResource, MigrationTaskInstancing instancing, MigrationTaskRunPattern runPattern, string progressDetail) 
            : base (identifier, instancing, runPattern)
        {
            this.scriptResource = scriptResource;
            this.progressDetail = progressDetail;
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public ScriptMigrationTask(ScriptMigrationTask toCopy) 
            : base(toCopy)
        {
            this.scriptResource = toCopy.scriptResource;
            this.progressDetail = toCopy.progressDetail;
        }

        /// <summary>
        /// Make a clone of this instance
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new ScriptMigrationTask(this);
        }

        /// <summary>
        /// Run the work estimation
        /// </summary>
        protected override int RunEstimate(SqlConnection con)
        {
            string estimateResource = scriptResource;

            // when running the Estimate, we use the resource scriptResource.estimate.sql
            if (estimateResource.EndsWith(".sql"))
            {
                estimateResource = estimateResource.Substring(0, estimateResource.Length - 4);
            }

            estimateResource = estimateResource + ".estimate";

            // possibly contain in a transaction that gets rolled back to prevent any damage
            SqlScript script = ScriptLoader.LoadScript(estimateResource);
            if (script == null)
            {
                // there was no paired .estimate script.  Assuming 1 work unit.
                return 1;
            }

            if (script.Batches.Count > 1)
            {
                return script.Batches.Count;
            }
            else
            {
                // single batch, so we'll try to execute it and pull out the RowCount
                try
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandText = script.Content;

                        object result = SqlCommandProvider.ExecuteScalar(cmd);
                        if (result != null && result.GetType() == typeof(int))
                        {
                            return (int)result;
                        }
                        else
                        {
                            // consider the entire one unit of work
                            return 1;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new MigrationException(string.Format("Failure executing script '{0}", script.Name), ex);
                }
            }
        }

        /// <summary>
        /// Execute the task
        /// </summary>
        protected override int Run()
        {
            // possibly contain in a transaction that gets rolled back to prevent any damage

            SqlScript script = ScriptLoader.LoadScript(scriptResource);
            Debug.Assert(script != null, "Unable to locate upgrade script " + scriptResource);

            Progress.Detail = progressDetail;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(20)))
            {
                using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
                {
                    try
                    {
                        if (script.Batches.Count > 1)
                        {
                            // treat each batch as a unit of work, don't need to actually run the script
                            script.BatchCompleted += (o, e) => ReportWorkProgress();

                            // execute the script using the regular batch-capable methods
                            script.Execute(con);

                            // done with the transaction
                            scope.Complete();

                            return ActualWorkCount;
                        }
                        else
                        {
                            using (SqlCommand cmd = SqlCommandProvider.Create(con))
                            {
                                cmd.CommandText = "SET ARITHABORT, XACT_ABORT ON";
                                SqlCommandProvider.ExecuteNonQuery(cmd);

                                cmd.CommandText = script.Content;

                                log.InfoFormat("Running script {0} on {1}", script.Name, Database);

                                object result = SqlCommandProvider.ExecuteScalar(cmd);

                                // done with the transaction
                                scope.Complete();

                                if (result != null && result.GetType() == typeof(int))
                                {
                                    return (int)result;
                                }
                                else
                                {
                                    // so the script didn't return a rowcount.  
                                    // if the RunPattern is once, we'll just return that a single unit of work was done.
                                    if (RunPattern == MigrationTaskRunPattern.RunOnce)
                                    {
                                        return 1;
                                    }
                                    else
                                    {
                                        // otherwise we'll return 0 so it won't get called again
                                        return 0;
                                    }
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        log.ErrorFormat("Failure executing migration script '{0}:{1}' for task instance {2}: {3}", script.Name, ex.LineNumber, ExecutionPlanIdentifier, ex.Message);
                        throw new MigrationException(string.Format("Failure executing script '{0}':\n\n{1}: {2}", script.Name, ex.LineNumber, ex.Message), ex);
                    }
                }
            }
        }
    }
}