using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Transactions;
using Interapptive.Shared.Data;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database
{
    /// <summary>
    /// Class representing the dynamically generated upgrade
    /// execution plan.  A single MigrationTaskBase in the MigrationEngine
    /// can result in multiple instances of the same type in the generated executin
    /// plan.
    ///
    /// Once the execution plan is generated and execution has started, it is resumable.
    /// </summary>
    public class MigrationExecutionPlan
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(MigrationExecutionPlan));

        // Task prototypes to base the execution plan off of
        IList<MigrationTaskBase> taskPrototypes;

        // The runtime task instances that make up the execution plan
        List<MigrationTaskBase> taskInstances;

        // The currently executing task
        int executingTaskIndex = -1;

        // name of the current database
        string mainDatabaseName = "";

        // reference to the owning engine
        MigrationController controller;

        // whether or not an existing plan is to be resumed
        bool resumable = false;

        /// <summary>
        /// Gets if the execution plan is in a resumable state.
        /// </summary>
        public bool Resumable
        {
            get { return resumable; }
        }

        /// <summary>
        /// Gets the engine this plan is a part of
        /// </summary>
        public MigrationController MigrationController
        {
            get { return controller; }
        }

        /// <summary>
        /// Convenience property for getting the currently executing task.
        ///
        /// Null if it isn't on a task.
        /// </summary>
        public MigrationTaskBase CurrentTask
        {
            get
            {
                if (executingTaskIndex >= 0 && executingTaskIndex < taskInstances.Count)
                {
                    return taskInstances[executingTaskIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MigrationExecutionPlan(MigrationController controller, List<MigrationTaskBase> taskPrototypes)
        {
            this.controller = controller;
            this.taskPrototypes = taskPrototypes.AsReadOnly();
            this.mainDatabaseName = SqlSession.Current.Configuration.DatabaseName;
        }

        /// <summary>
        /// Initializes the execution plan.
        /// </summary>
        public void Initialize()
        {
            log.Info("Initializing MigrationExecutionPlan");

            // open a database connection
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // Create the execution plan in-memory
                GenerateExecutionPlan();

                List<MigrationTaskBase> savedTaskInstances = DeserializePlanFromDb(con);

                // if a previous plan is in progress, validate that it agrees with the one just generated
                if (savedTaskInstances.Count > 0)
                {
                    // an upgrade wasn't already in process
                    ReconcileExecutionPlan(savedTaskInstances);

                    // plan validated, we are in progress
                    resumable = true;
                }
            }

            // initialize every task
            foreach (MigrationTaskBase task in taskInstances)
            {
                task.Initialize();
            }
        }

        #region Plan Generation/Serialization/Deserialization

        /// <summary>
        /// Compares the execution plan that was saved to the database with that one
        /// which was just generated.  Any discrepency means the upgrade is corrupt and
        /// must exit.
        /// </summary>
        private void ReconcileExecutionPlan(List<MigrationTaskBase> existingTasks)
        {
            if (taskInstances.Count != existingTasks.Count)
            {
                log.ErrorFormat("Execution plan mismatch.  Generated plan contains {0} tasks, in-progress plan contains {1} tasks.", taskInstances.Count, existingTasks.Count);
                throw new MigrationException("Execution plan mismatch, terminating upgrade.");
            }

            // compare each
            for (int i = 0; i < taskInstances.Count; i++)
            {
                // match the type and the identifier
                if (taskInstances[i].GetType() != existingTasks[i].GetType() ||
                    taskInstances[i].ExecutionPlanIdentifier != existingTasks[i].ExecutionPlanIdentifier)
                {
                    // failure
                    log.ErrorFormat("Execution plan mismatch @ index {0}  \n" +
                                    "     Generated (identifier, type): {1}, {2}\n" +
                                    "     Existing (identifier, type): {3}, {4}",
                                    i,
                                    taskInstances[i].ExecutionPlanIdentifier, taskInstances[i].GetType(),
                                    existingTasks[i].ExecutionPlanIdentifier, existingTasks[i].GetType());


                    throw new MigrationException("This database migration failed and is corrupt.  Please restore your Version 2 database and re-run the Upgrade Wizard.");
                }
            }

            // now go through and carry over completedness and other necessary values
            for (int i = 0; i < taskInstances.Count; i++)
            {
                taskInstances[i].Completed = existingTasks[i].Completed;
                taskInstances[i].CreateTime = existingTasks[i].CreateTime;
                taskInstances[i].StartTime = existingTasks[i].StartTime;
                taskInstances[i].LastUpdateTime = existingTasks[i].LastUpdateTime;
                taskInstances[i].ActualWorkCount = existingTasks[i].ActualWorkCount;
            }
        }

        /// <summary>
        /// Saves the execution plan to the database for progress tracking and resumability
        /// </summary>
        private void SerializePlanToDb(SqlConnection con)
        {
            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = @"INSERT INTO v2m_MigrationPlan (
                                        TaskTypeCode,
                                        TaskIdentifier,
                                        DatabaseName,
                                        IsArchiveDB,
                                        CreateTime,
                                        StartTime,
                                        LastUpdated,
                                        Completed,
                                        Estimatedwork,
                                        Progress
                                        )
                                    Values (
                                        @TaskTypeCode,
                                        @TaskIdentifier,
                                        @DatabaseName,
                                        @IsArchiveDB,
                                        @CreateTime,
                                        @StartTime,
                                        @LastUpdated,
                                        @Completed,
                                        @EstimatedWork,
                                        @Progress
                                    )";

                    cmd.Parameters.Add("@TaskTypeCode", SqlDbType.Int);
                    cmd.Parameters.Add("@TaskIdentifier", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@DatabaseName", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@IsArchiveDB", SqlDbType.Bit);
                    cmd.Parameters.Add("@CreateTime", SqlDbType.DateTime);
                    cmd.Parameters.Add("@StartTime", SqlDbType.DateTime);
                    cmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime);
                    cmd.Parameters.Add("@Completed", SqlDbType.Bit);
                    cmd.Parameters.Add("@EstimatedWork", SqlDbType.Int);
                    cmd.Parameters.Add("@Progress", SqlDbType.Int);

                    foreach (MigrationTaskBase task in taskInstances)
                    {
                        cmd.Parameters["@TaskTypeCode"].Value = (int) task.TaskTypeCode;
                        cmd.Parameters["@TaskIdentifier"].Value = task.Identifier;
                        cmd.Parameters["@DatabaseName"].Value = task.Database;
                        cmd.Parameters["@IsArchiveDB"].Value = task.IsArchiveDatabase;
                        cmd.Parameters["@CreateTime"].Value = DateTime.UtcNow;
                        cmd.Parameters["@StartTime"].Value = SqlDateTime.MinValue.Value;
                        cmd.Parameters["@LastUpdated"].Value = DateTime.UtcNow;
                        cmd.Parameters["@Completed"].Value = task.Completed;
                        cmd.Parameters["@EstimatedWork"].Value = task.EstimatedWorkCount;
                        cmd.Parameters["@Progress"].Value = task.ActualWorkCount;

                        SqlCommandProvider.ExecuteNonQuery(cmd);
                    }

                    trans.Complete();
                }
            }
        }

        /// <summary>
        /// Deserializes the saved execution plan froma  prior/failed execution if it exists
        /// </summary>
        private List<MigrationTaskBase> DeserializePlanFromDb(SqlConnection con)
        {
            // the tasks being deserialized from the database
            List<MigrationTaskBase> savedTasks = new List<MigrationTaskBase>();

            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = "SELECT * FROM dbo.v2m_MigrationPlan Order by MigrationPlanId";
                cmd.CommandType = CommandType.Text;

                // query away
                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        MigrationTaskTypeCode typeCode = (MigrationTaskTypeCode) ((int) reader["TaskTypeCode"]);

                        // create a new instance based on the typecode
                        MigrationTaskBase task = MigrationController.CreateTaskInstance(typeCode);

                        // populate from serialized data
                        task.Identifier = (string) reader["TaskIdentifier"];
                        task.Database = (string) reader["DatabaseName"];
                        task.IsArchiveDatabase = (bool) reader["IsArchiveDB"];
                        task.CreateTime = (DateTime) reader["CreateTime"];
                        task.StartTime = (DateTime) reader["StartTime"];
                        task.LastUpdateTime = (DateTime) reader["LastUpdated"];
                        task.Completed = (bool) reader["Completed"];
                        task.EstimatedWorkCount = (int) reader["EstimatedWork"];
                        task.ActualWorkCount = (int) reader["Progress"];

                        // add it to the list
                        savedTasks.Add(task);
                    }
                }
            }

            return savedTasks;
        }

        /// <summary>
        /// Creates the execution plan in memory based on the
        /// existing archive databases and task prototypes.
        /// </summary>
        private void GenerateExecutionPlan()
        {
            log.Info("Generating the migration execution plan");
            taskInstances = new List<MigrationTaskBase>();
            foreach (MigrationTaskBase task in taskPrototypes)
            {
                MigrationTaskBase instance = null;

                // create the task instance for the main database
                if (task.Instancing == MigrationTaskInstancing.MainDatabaseOnly ||
                    task.Instancing == MigrationTaskInstancing.MainDatabaseAndArchives)
                {
                    // main database only
                    instance = task.CreateRuntimeInstance(this, mainDatabaseName);

                    taskInstances.Add(instance);
                }

                // create the task for archive databases too
                if (task.Instancing == MigrationTaskInstancing.ArchiveDatabasesOnly ||
                    task.Instancing == MigrationTaskInstancing.MainDatabaseAndArchives)
                {
                    foreach (ArchiveSet archiveSet in controller.ArchiveSets)
                    {
                        instance = task.CreateRuntimeInstance(this, archiveSet.DBName);
                        instance.IsArchiveDatabase = true;

                        taskInstances.Add(instance);
                    }
                }
            }

            log.InfoFormat("{0} task prototypes resulted in {1} runtime instances.", taskPrototypes.Count, taskInstances.Count);
        }

        #endregion

        #region Progress

        /// <summary>
        /// Recalculates the current progress item's PercentComplete
        /// based on all of the tasks in the plan
        /// </summary>
        public void UpdateProgress()
        {
            // get the current progressitem out of the migration context
            IProgressReporter progressItem = MigrationContext.Current.ProgressItem;

            // if we aren't in a reportable operation, just exit
            if (progressItem == null)
            {
                return;
            }

            if (executingTaskIndex < 0)
            {
                progressItem.PercentComplete = 0;
            }
            else if (executingTaskIndex >= taskInstances.Count)
            {
                progressItem.PercentComplete = 100;
            }
            else
            {
                // current progress = (completed task count / total task count) + ( 1 / total task count) * ( current task actual / current task estimated)
                //                  = ( executing Index / total task count) ....
                double percent = ((double) executingTaskIndex / taskInstances.Count) + (1.0 / taskInstances.Count) * Math.Min(1.0, ((double) CurrentTask.ActualWorkCount / Math.Max(1, CurrentTask.EstimatedWorkCount)));
                percent = Math.Round(percent, 2);
                progressItem.PercentComplete = Math.Min(100, (int) (100 * percent));
            }
        }

        /// <summary>
        /// Sets the current progress item's detail with the text
        /// provided.
        /// </summary>
        public void SetProgressDetail(string detail)
        {
            // get the current progressitem out of the migration context
            IProgressReporter progressItem = MigrationContext.Current.ProgressItem;

            // if we aren't in a reportable operation, just exit
            if (progressItem == null)
            {
                return;
            }

            progressItem.Detail = detail;
        }

        #endregion

        #region Execution

        /// <summary>
        /// Gets the step to start on
        /// </summary>
        /// <returns></returns>
        private bool GetNextTask()
        {
            while (executingTaskIndex < taskInstances.Count - 1)
            {
                // move to the next
                executingTaskIndex++;

                // just a safeguard
                if (executingTaskIndex < 0)
                {
                    continue;
                }

                // go until we find the first incomplete one
                if (!taskInstances[executingTaskIndex].Completed)
                {
                    return true;
                }
            }

            // no more to execute
            return false;
        }

        /// <summary>
        /// Perform estimate calculation on each task instance
        /// </summary>
        public void CalculateEstimates()
        {
            // progress reporting
            IProgressReporter progress = MigrationContext.Current.ProgressItem;

            // reset our position
            executingTaskIndex = -1;

            progress.Starting();
            progress.Detail = "Calculating...";

            // estimates are not recalculated during resume scenarios.   It is just too difficult to create
            // estimates when the state of the database is completely unknown.
            if (!resumable)
            {
                // cycle through the tasks in the execution plan
                while (GetNextTask())
                {
                    if (progress.IsCancelRequested)
                    {
                        // reset our position
                        MigrationContext.Current.CurrentTask = null;
                        executingTaskIndex = -1;

                        return;
                    }

                    // push the current task to the context
                    MigrationContext.Current.CurrentTask = CurrentTask;

                    CurrentTask.CalculateEstimate();

                    // update progress
                    double percent = Math.Round(Math.Min(100, (executingTaskIndex + 1.0) / taskInstances.Count), 2);
                    progress.PercentComplete = (int) (100 * percent);
                }

                MigrationContext.Current.CurrentTask = null;

                // reset our position
                executingTaskIndex = -1;
            }

            // complete progress
            progress.PercentComplete = 100;
            progress.Detail = "Done";
            progress.Completed();
        }

        /// <summary>
        /// Run or resume the execution plan
        /// </summary>
        public void Execute()
        {
            // progress reporting
            IProgressReporter progress = MigrationContext.Current.ProgressItem;
            progress.Starting();

            // open a database connection
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // if we aren't resuming, we must save the exeuction plan
                if (!resumable)
                {
                    SerializePlanToDb(con);
                }
            }

            // determine where to start
            while (GetNextTask())
            {
                if (progress.IsCancelRequested)
                {
                    // cleanup and exit
                    MigrationContext.Current.CurrentTask = null;
                    return;
                }

                // push the current task to the context
                MigrationContext.Current.CurrentTask = CurrentTask;

                // update progress and detail
                UpdateProgress();

                // run it
                CurrentTask.RunTask(MigrationTaskExecutionPhase.Execute);

                // update progress and detail
                UpdateProgress();
            }

            MigrationContext.Current.CurrentTask = null;

            //completed
            progress.PercentComplete = 100;
            progress.Completed();
            progress.Detail = "Done";
        }



        #endregion
    }
}
