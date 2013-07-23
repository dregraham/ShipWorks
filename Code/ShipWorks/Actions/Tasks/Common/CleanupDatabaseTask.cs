using System.Collections.Generic;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;

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
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            StopAfterHours = 30;
            CleanupAfterDays = 90;
            return new CleanupDatabaseTaskEditor(this);
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
            log.Info("Starting database cleanup...");
            base.Run();
        }
    }
}
