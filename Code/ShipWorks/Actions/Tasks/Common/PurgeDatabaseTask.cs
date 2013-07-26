using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Xml.XPath;
using System;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for deleting/purging old data.
    /// </summary>
    [ActionTask("Delete old data", "PurgeDatabase", ActionTriggerClassifications.Scheduled)]
    public class PurgeDatabaseTask : ActionTask
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PurgeDatabaseTask));
        private readonly ISqlPurgeScriptRunner scriptRunner;
        private readonly IDateTimeProvider dateProvider;
        private readonly List<PurgeDatabaseType> purgeOrder = new List<PurgeDatabaseType>
        {
            PurgeDatabaseType.Labels,
            PurgeDatabaseType.PrintJobs,
            PurgeDatabaseType.Email,
            PurgeDatabaseType.Audit,
            PurgeDatabaseType.Downloads
        };
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeDatabaseTask"/> class.
        /// </summary>
        public PurgeDatabaseTask() 
            : this(new SqlPurgeScriptRunner(), new DateTimeProvider())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeDatabaseTask"/> class.
        /// </summary>
        /// <param name="scriptRunner">Specifies which sql purge runner gets used for unit testing</param>
        /// <param name="dateProvider">Specifies how current dates will be retrieved so tests can use expected times</param>
        public PurgeDatabaseTask(ISqlPurgeScriptRunner scriptRunner, IDateTimeProvider dateProvider)
        {
            TimeoutInHours = 1;
            RetentionPeriodInDays = 30;
            
            Purges = new List<PurgeDatabaseType>();

            this.scriptRunner = scriptRunner;
            this.dateProvider = dateProvider;
        }

        /// <summary>
        /// This task does not require any input to run.
        /// </summary>
        public override bool RequiresInput
        {
            get { return false; }
        }

        /// <summary>
        /// Should cleanups be stopped if they go to long?
        /// </summary>
        public bool StopLongPurges { get; set; }

        /// <summary>
        /// Defines the maximum number of hours a purge can be running.
        /// </summary>
        public int TimeoutInHours { get; set; }

        /// <summary>
        /// How many days worth of data should be kept? Any records older than the
        /// retention period will be deleted.
        /// </summary>
        public int RetentionPeriodInDays { get; set; }

        /// <summary>
        /// Gets or sets the purges requested for this task.
        /// </summary>
        public List<PurgeDatabaseType> Purges { get; set; }

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new PurgeDatabaseTaskEditor(this);
        }

        /// <summary>
        /// Gets a type from the specified type string
        /// </summary>
        /// <param name="value">Name and namespace of the type to get.</param>
        /// <returns>The actual type for the string, or null if it can't be found.</returns>
        /// <remarks>This is overridden so that the ShipWorks assembly is searched.</remarks>
        protected override Type GetTypeByFullName(string value)
        {
            return Type.GetType(value);
        }

        /// <summary>
        /// Runs the purge scripts.
        /// </summary>
        protected override void Run()
        {
            DateTime sqlDate = scriptRunner.SqlUtcDateTime;
            DateTime localStopExecutionAfter = dateProvider.UtcNow.AddHours(TimeoutInHours);
            DateTime sqlStopExecutionAfter = sqlDate.AddHours(TimeoutInHours);
            DateTime earliestRetentionDateInUtc = sqlDate.AddDays(-RetentionPeriodInDays);

            Dictionary<PurgeDatabaseType, Exception> exceptions = new Dictionary<PurgeDatabaseType, Exception>();

            foreach (PurgeDatabaseType purge in purgeOrder.Intersect(Purges))
            {
                // Stop executing if we've been running longer than the time the user has allowed.
                if (StopLongPurges && localStopExecutionAfter < dateProvider.UtcNow)
                {
                    log.Info("Stopping purge because it has exceeded the maximum allowed time.");
                    break;
                }

                string scriptName = EnumHelper.GetApiValue(purge);

                log.InfoFormat("Running {0}, deleting data older than {1}...", scriptName, earliestRetentionDateInUtc);
                try
                {
                    scriptRunner.RunScript(scriptName, earliestRetentionDateInUtc, StopLongPurges ? sqlStopExecutionAfter : SqlDateTime.MaxValue.Value);
                    log.InfoFormat("Finished {0} successfully.", scriptName);
                }
                catch (DbException ex)
                {
                    // Catch a DbException instead of a SqlException so this catch can be tested.
                    // SqlException is sealed and can't be easily constructed.
                    exceptions.Add(purge, ex);
                    log.InfoFormat("Error running purge: {0}.", ex.Message);
                }
            }

            // If any exceptions were saved, fail the task
            if (exceptions.Any())
            {
                string exceptionMessage = string.Format("An error occurred while deleting the following items: {0}",
                    exceptions.Keys
                        .Select(x => EnumHelper.GetDescription(x))
                        .Aggregate((x, y) => x + ", " + y));
                throw new ActionTaskRunException(exceptionMessage, 
                    new ExceptionCollection(new ArrayList(exceptions.Values)));
            }
        }
    }
}
