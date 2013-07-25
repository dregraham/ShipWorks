using System.Collections.Generic;
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
        /// List of scripts that need to be run for the cleanup
        /// </summary>
        private IEnumerable<string> ScriptsToRun
        {
            get
            {
                return Purges.Select(x => EnumHelper.GetApiValue(x));
            }
        }

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new PurgeDatabaseTaskEditor(this);
        }

        /// <summary>
        /// Load the object data from the given XPath. Overriden to populate purges.
        /// </summary>
        /// <param name="xpath"></param>
        public override void DeserializeXml(XPathNavigator xpath)
        {
            base.DeserializeXml(xpath);

            Purges = new List<PurgeDatabaseType>();
            XPathNodeIterator purges = xpath.Select("/Settings/Purges/*[@value]");

            foreach (object purge in purges)
            {
                var purgeNavigator = ((XPathNavigator) purge);
                
                purgeNavigator.MoveToAttribute("value", "");
                Purges.Add((PurgeDatabaseType)(purgeNavigator).ValueAsInt);
            }
        }
        
        /// <summary>
        /// Runs the purge scripts.
        /// </summary>
        protected override void Run()
        {
            DateTime sqlDate = scriptRunner.SqlUtcDateTime;
            DateTime localStopExecutionAfter = dateProvider.UtcNow.AddHours(TimeoutInHours);
            DateTime sqlStopExecutionAfter = sqlDate.AddHours(TimeoutInHours);
            // DateTime deleteOlderThan = sqlDate.AddDays(-RetentionPeriodInDays);

            log.Info("Starting database purge...");

            foreach (string scriptName in ScriptsToRun)
            {
                // Stop executing if we've been running longer than the time the user has allowed.
                if (StopLongPurges && localStopExecutionAfter < dateProvider.UtcNow)
                {
                    log.Info("Stopping purge because it has exceeded the maximum allowed time.");
                    break;
                }

                log.InfoFormat("Running {0}...", scriptName);
                scriptRunner.RunScript(scriptName, RetentionPeriodInDays, StopLongPurges ? sqlStopExecutionAfter : sqlDate.AddYears(10));
                log.InfoFormat("Finished {0}.", scriptName);
            }
        }     
    }
}
