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
        private int cleanupAfterDays = 90;
        private int stopAfterMinutes = 30;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            return new CleanupDatabaseTaskEditor(this);
        }

        /// <summary>
        /// Should cleanups be stopped if they go to long?
        /// </summary>
        public bool StopLongCleanups { get; set; }

        /// <summary>
        /// Defines the maximum minutes a cleanup can run
        /// </summary>
        public int StopAfterMinutes
        {
            get { return stopAfterMinutes; }
            set { stopAfterMinutes = value; }
        }

        /// <summary>
        /// How many days worth of data should be kept?  All older will be cleaned.
        /// </summary>
        public int CleanupAfterDays
        {
            get { return cleanupAfterDays; }
            set { cleanupAfterDays = value; }
        }

        /// <summary>
        /// This task does not require any input to run.
        /// </summary>
        public override bool RequiresInput
        {
            get { return false; }
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
