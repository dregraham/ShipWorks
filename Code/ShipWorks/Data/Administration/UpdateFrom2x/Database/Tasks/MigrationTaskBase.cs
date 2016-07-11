using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Interapptive.Shared.Data;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks
{
    /// <summary>
    /// Serves as the base class for both a discrete unit of work for the
    /// db upgrade process.
    /// </summary>
    public abstract class MigrationTaskBase
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(MigrationTaskBase));

        // this instance can be executed
        bool canRun = false;

        // unique identify this task
        string identifier = "";

        // database to execute against
        string database = "";

        // whether or not database is an archive db
        bool isArchiveDatabase = false;

        // the execution plan this task is participating in
        MigrationExecutionPlan executionPlan;

        // how this task prototype creates other instances
        MigrationTaskInstancing instancing;

        // declares how this task is to be executed, ex. once or repeated
        MigrationTaskRunPattern runPattern;

        // estimated number of work units
        int estimatedWorkCount = 0;

        // current work progress
        int actualWorkCount = 0;

        // Script loader instance used by this task
        MigrationSqlScriptLoader scriptLoader;

        // bookkeeping
        DateTime createTime;
        DateTime startTime;
        DateTime lastUpdateTime;

        // done or not
        bool completed;

        /// <summary>
        /// Returns the running progress item
        /// </summary>
        protected IProgressReporter Progress
        {
            get
            {
                if (MigrationContext.Current == null)
                {
                    // just create a new one so callers don't fail or need to check
                    return new ProgressItem("");
                }

                return MigrationContext.Current.ProgressItem;
            }
        }

        #region General Properties

        /// <summary>
        /// The script loaded used by this task
        /// </summary>
        public MigrationSqlScriptLoader ScriptLoader
        {
            get { return scriptLoader; }
            set { scriptLoader = value; }
        }

        /// <summary>
        /// Declares how the task is to be instantiated in the execution plan
        /// </summary>
        public MigrationTaskInstancing Instancing
        {
            get { return instancing; }
            set { instancing = value; }
        }

        /// <summary>
        /// Controls how  task is executed - one time or until no results returned
        /// </summary>
        public MigrationTaskRunPattern RunPattern
        {
            get { return runPattern; }
            set { runPattern = value; }
        }

        /// <summary>
        /// Identifies the task type
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        /// <summary>
        /// Gets and sets whether or not this instance is complete and runnable
        /// </summary>
        public bool CanRun
        {
            get { return canRun; }
            set { canRun = value; }
        }

        /// <summary>
        /// Gets the database this script is targeted toward
        /// </summary>
        public string Database
        {
            get { return database; }
            set { database = value; }
        }


        /// <summary>
        /// Gets and sets the task creation time
        /// </summary>
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        /// <summary>
        /// Execution Start Time
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        /// <summary>
        /// Execution end time
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return lastUpdateTime; }
            set { lastUpdateTime = value; }
        }

        /// <summary>
        /// Gets and sets the estimated number of work units to do
        /// </summary>
        public int EstimatedWorkCount
        {
            get { return estimatedWorkCount; }
            set { estimatedWorkCount = value; }
        }

        /// <summary>
        /// Gets and sets the actual progress in number of work units completed
        /// </summary>
        public int ActualWorkCount
        {
            get { return actualWorkCount; }
            set { actualWorkCount = value; }
        }

        /// <summary>
        /// Completed execution
        /// </summary>
        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }

        /// <summary>
        /// Whether or not this task is working on an archive database
        /// </summary>
        public bool IsArchiveDatabase
        {
            get { return isArchiveDatabase; }
            set { isArchiveDatabase = value; }
        }

        /// <summary>
        /// Uniquely identifies the exuection plan instance
        /// </summary>
        public string ExecutionPlanIdentifier
        {
            get
            {
                if (string.IsNullOrEmpty(database))
                {
                    return identifier;
                }
                else
                {
                    return string.Format("{0}.{1}", identifier, database);
                }
            }
        }

        /// <summary>
        /// Identifying type code
        /// </summary>
        public abstract MigrationTaskTypeCode TaskTypeCode { get; }
        #endregion

        #region Instantiation

        /// <summary>
        /// Constructor
        /// </summary>
        protected MigrationTaskBase(string identifier)
            : this(identifier, MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce)
        {
        }

        /// <summary>
        /// Constructor for overriding the instancing and run pattern.
        /// </summary>
        protected MigrationTaskBase(string identifier, MigrationTaskInstancing instancing, MigrationTaskRunPattern runPattern)
        {
            this.identifier = identifier;
            this.instancing = instancing;
            this.runPattern = runPattern;
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        protected MigrationTaskBase(MigrationTaskBase toCopy)
        {
            // id
            identifier = toCopy.identifier;
            database = toCopy.identifier;

            // progress
            estimatedWorkCount = toCopy.estimatedWorkCount;
            actualWorkCount = 0;

            // behavior
            runPattern = toCopy.runPattern;
            instancing = toCopy.instancing;

            // bookkeeping
            completed = toCopy.completed;
            startTime = toCopy.startTime;
            createTime = toCopy.CreateTime;
            lastUpdateTime = toCopy.lastUpdateTime;

            // utility
            scriptLoader = toCopy.scriptLoader;
        }

        /// <summary>
        /// Method for creating a "runtime" instance based on this task
        /// </summary>
        public MigrationTaskBase CreateRuntimeInstance(MigrationExecutionPlan executionPlan, string databaseName)
        {
            MigrationTaskBase runtimeInstance = Clone();
            runtimeInstance.database = databaseName;
            runtimeInstance.CanRun = true;
            runtimeInstance.executionPlan = executionPlan;

            return runtimeInstance;
        }

        /// <summary>
        /// Clone the element
        /// </summary>
        public abstract MigrationTaskBase Clone();

        #endregion

        #region Progress

        /// <summary>
        /// Directs the execution plan to update it's progress indicators
        /// </summary>
        private void UpdateProgress()
        {
            // propogate this back up to the engine since it must be aware of the overall progress to
            // accurately set ProgressPercent.
            if (executionPlan != null)
            {
                executionPlan.UpdateProgress();
            }
        }

        /// <summary>
        /// Calculate the amount of work this task has to do
        /// </summary>
        public void CalculateEstimate()
        {
            // Run the Estimate phase
            estimatedWorkCount = RunTask(MigrationTaskExecutionPhase.Estimate);
        }

        /// <summary>
        /// Record the start of this task.
        ///
        /// Not using Run's connection because that is potentially for an archive DB
        /// </summary>
        private void RecordStart()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE v2m_MigrationPlan SET " +
                                          "StartTime = @startTime, " +
                                          "LastUpdated = @lastUpdated " +
                                          "WHERE TaskIdentifier = @taskIdentifier AND DatabaseName = @databaseName";

                        StartTime = DateTime.UtcNow;
                        cmd.Parameters.AddWithValue("@startTime", StartTime);
                        cmd.Parameters.AddWithValue("@lastUpdated", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@taskIdentifier", Identifier);
                        cmd.Parameters.AddWithValue("@databaseName", Database);

                        // also include progress once estimation is in

                        if (SqlCommandProvider.ExecuteNonQuery(cmd) > 1)
                        {
                            // Sanity check, this shouldn't happen
                            throw new InvalidOperationException("Multiple MigrationPlan rows updated, corrupt upgrade.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Record the start of this task.
        ///
        /// Not using Run's connection because that is potentially for an archive DB
        /// </summary>
        private void RecordProgress()
        {
            // record the current time
            LastUpdateTime = DateTime.UtcNow;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandText = "UPDATE v2m_MigrationPlan SET " +
                                          "LastUpdated = @lastUpdated, " +
                                          "Completed = @completed, " +
                                          "Progress = @progress " +
                                          "WHERE TaskIdentifier = @taskIdentifier AND DatabaseName = @databaseName";
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@lastUpdated", LastUpdateTime);
                        cmd.Parameters.AddWithValue("@completed", Completed);
                        cmd.Parameters.AddWithValue("@taskIdentifier", Identifier);
                        cmd.Parameters.AddWithValue("@databaseName", Database);
                        cmd.Parameters.AddWithValue("@progress", ActualWorkCount);

                        // also include progress once estimation is in

                        if (SqlCommandProvider.ExecuteNonQuery(cmd) > 1)
                        {
                            // Sanity check, this shouldn't happen
                            throw new InvalidOperationException("Multiple MigrationPlan rows updated, corrupt upgrade.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Allows a derived class to report a single unit of work done
        /// </summary>
        protected void ReportWorkProgress()
        {
            ReportWorkProgress(1);
        }

        /// <summary>
        /// Allow derived classes to report work done which
        /// incrases
        /// </summary>
        protected void ReportWorkProgress(int unitsCompleted)
        {
            // increment the amount of work done
            actualWorkCount += unitsCompleted;

            // The total progress needs to be updated
            UpdateProgress();
        }

        #endregion

        /// <summary>
        /// Perform any pre-estimate and pre-execution initialization
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Run a single unit of work.
        ///
        ///     Return the number of work units complete.
        ///     Return 0 if CompleteWorkUnit() is being used instead to report progress
        ///
        /// If the task RunPattern is Repeat, the task will be called repeatedly until non-zero is returned
        /// </summary>
        protected abstract int Run();

        /// <summary>
        /// Perform calculations to get the estimated number of work units to be performed.
        ///
        /// Ruturn the number of work units.
        /// </summary>
        protected abstract int RunEstimate(SqlConnection con);

        /// <summary>
        /// Run the task.  The connection provided is connected to the Database specified on the task.
        /// </summary>
        public int RunTask(MigrationTaskExecutionPhase phase)
        {
            // safeguard to protect against deserialized or otherwise not-completetely configured tasks from running
            if (!canRun)
            {
                throw new MigrationException(String.Format("The MigrationTaskBase instance with id '{0}' is not runnable.", ExecutionPlanIdentifier));
            }

            // track the number of work units performed or estimated
            int count = 0;

            if (phase == MigrationTaskExecutionPhase.Estimate)
            {
                using (SqlConnection con = OpenConnectionForTask(this))
                {
                    log.InfoFormat("Running task {0} for Estimate.", ExecutionPlanIdentifier);
                    count = RunEstimate(con);

                }

                // set the estimated value
                estimatedWorkCount = count;

                // write the estimate to the database
                RecordProgress();
            }
            else
            {
                // Record that we've started running for real
                RecordStart();

                // Run a single time
                if (runPattern == MigrationTaskRunPattern.RunOnce)
                {
                    log.InfoFormat("Running task {0} once.", ExecutionPlanIdentifier);

                    // run the task
                    count = Run();

                    // done
                    Completed = true;

                    // track progress
                    ReportWorkProgress(count);

                    // write back to the MigrationPlan
                    RecordProgress();
                }
                else
                {
                    // repeat running until no results are returned
                    int i = 0;
                    do
                    {
                        // log activity
                        log.InfoFormat("Running task {0} until 0 returned.", ExecutionPlanIdentifier);

                        i = Run();

                        // log activity
                        log.InfoFormat("Task {0} returned {1} units completed", ExecutionPlanIdentifier, i);

                        // 0 indicates we are completely done
                        if (i == 0)
                        {
                            Completed = true;
                        }

                        // increment and record the returned progress
                        ReportWorkProgress(i);
                        RecordProgress();

                        // check for canceling, get out if necessary
                        if (Progress.IsCancelRequested)
                        {
                            return count;
                        }

                        // keep track of total work
                        count += i;

                    } while (i > 0);
                }
            }

            // return the rows impacted
            return count;
        }

        /// <summary>
        /// Gets a database connection for the task provided.  The connection
        /// will be opened to the database the task is configured to access.
        /// </summary>
        public static SqlConnection OpenConnectionForTask(MigrationTaskBase task)
        {
            // the task needs the main database connection
            if (!task.IsArchiveDatabase)
            {
                return SqlSession.Current.OpenConnection();
            }

            // open a connection to the main database and change databases to the archive
            // note: may be better to just construct a new connection instead of doing it this way
            // due to (connection pooling issues)
            SqlConnection con = SqlSession.Current.OpenConnection();
            con.ChangeDatabase(task.Database);

            return con;
        }
    }
}