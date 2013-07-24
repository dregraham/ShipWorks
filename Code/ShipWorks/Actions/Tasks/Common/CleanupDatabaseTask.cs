using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using System;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Connection;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for cleaning old data
    /// </summary>
    [ActionTask("Clean up database", "CleanupDatabase", ActionTriggerClassifications.Scheduled)]
    public class CleanupDatabaseTask : ActionTask
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(CleanupDatabaseTask));
        private readonly ISqlPurgeScriptRunner scriptRunner;
        private readonly IDateTimeProvider dateProvider; 

        /// <summary>
        /// Cleanups the type of the database.
        /// </summary>
        public CleanupDatabaseTask() : this(new SqlPurgeScriptRunner(), new DateTimeProvider())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scriptRunner">Specifies which sql purge runner gets used for unit testing</param>
        /// <param name="dateProvider">Specifies how current dates will be retrieved so tests can use expected times</param>
        public CleanupDatabaseTask(ISqlPurgeScriptRunner scriptRunner, IDateTimeProvider dateProvider) : base()
        {
            StopAfterHours = 1;
            CleanupAfterDays = 30;
            Purges = new List<CleanupDatabaseType>();
            this.scriptRunner = scriptRunner;
            this.dateProvider = dateProvider;
        }


        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new CleanupDatabaseTaskEditor(this);
        }

        /// <summary>
        /// Load the object data from the given XPath. Overriden to populate purges.
        /// </summary>
        /// <param name="xpath"></param>
        public override void DeserializeXml(XPathNavigator xpath)
        {
            base.DeserializeXml(xpath);

            Purges = new List<CleanupDatabaseType>();

            var purges = xpath.Select("/Settings/Purges/*[@value]");

            foreach (object purge in purges)
            {
                var purgeNavigator = ((XPathNavigator) purge);
                
                purgeNavigator.MoveToAttribute("value", "");
                Purges.Add((CleanupDatabaseType)(purgeNavigator).ValueAsInt);
            }
        }

        /// <summary>
        /// Should cleanups be stopped if they go to long?
        /// </summary>
        public bool StopLongCleanups { get; set; }

        /// <summary>
        /// Defines the maximum hours a cleanup can run
        /// </summary>
        public int StopAfterHours
        {
            get;
            set;
        }

        /// <summary>
        /// How many days worth of data should be kept?  All older will be cleaned.
        /// </summary>
        public int CleanupAfterDays
        {
            get;
            set;
        }

        /// <summary>
        /// This task does not require any input to run.
        /// </summary>
        public override bool RequiresInput
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the purges requested for this task.
        /// </summary>
        public List<CleanupDatabaseType> Purges
        {
            get;
            set;
        }

        /// <summary>
        /// Runs the cleanup scripts.
        /// </summary>
        protected override void Run()
        {
            DateTime sqlDate = scriptRunner.SqlUtcDateTime;
            DateTime localStopExecutionAfter = dateProvider.UtcNow.AddHours(StopAfterHours);
            DateTime sqlStopExecutionAfter = sqlDate.AddHours(StopAfterHours);
            //DateTime deleteOlderThan = sqlDate.AddDays(-CleanupAfterDays);

            log.Info("Starting database cleanup...");

            foreach (string scriptName in ScriptsToRun)
            {
                // Stop executing if we've been running longer than the time the user has allowed.
                if (StopLongCleanups && localStopExecutionAfter < dateProvider.UtcNow)
                {
                    log.Info("Stopping cleanup because it has exceeded the maximum allowed time.");
                    break;
                }

                log.InfoFormat("Running {0}...", scriptName);
                scriptRunner.RunScript(scriptName, CleanupAfterDays, StopLongCleanups ? sqlStopExecutionAfter : sqlDate.AddYears(10));
                log.InfoFormat("Finished {0}.", scriptName);
            }
        }

        /// <summary>
        /// List of scripts that need to be run for the cleanup
        /// </summary>
        private IEnumerable<string> ScriptsToRun
        {
            get
            {
                return Purges.Select(x => EnumHelper.GetApiValue(x));
            }
        }
    }
}
