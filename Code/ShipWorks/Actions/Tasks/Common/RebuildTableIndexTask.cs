using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Administration.Indexing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ShipWorks.Common;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// ActionTask that rebuilds database table indexes as needed based on index performance history.
    /// </summary>
    [ActionTask("Rebuild database indexes", "RebuildTableIndex", ActionTaskCategory.Administration, true)]
    public class RebuildTableIndexTask : ActionTask
    {
        private ILog log;
        private IIndexMonitor indexMonitor;
        private IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RebuildTableIndexTask"/> class.
        /// </summary>
        public RebuildTableIndexTask()
            : this(new IndexMonitor(), new DateTimeProvider(), LogManager.GetLogger(typeof(RebuildTableIndexTask)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebuildTableIndexTask" /> class.
        /// </summary>
        /// <param name="indexMonitor">The index monitor.</param>
        /// <param name="dateTimeProvider">The date time provider.</param>
        /// <param name="log">The log.</param>
        public RebuildTableIndexTask(IIndexMonitor indexMonitor, IDateTimeProvider dateTimeProvider, ILog log)
            : base()
        {
            this.log = log;
            this.indexMonitor = indexMonitor;
            this.dateTimeProvider = dateTimeProvider;

            TimeoutInMinutes = 120;
        }

        /// <summary>
        /// Indicates if the task requires input to function.  Such as the contents of a filter, or the item that caused the action.
        /// </summary>
        public override ActionTaskInputRequirement InputRequirement
        {
            get { return ActionTaskInputRequirement.None; }
        }

        /// <summary>
        /// Defines the maximum number of minutes the task can be running.
        /// </summary>
        public int TimeoutInMinutes { get; set; }

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
        /// Is the task allowed to be run using the specified trigger type?
        /// </summary>
        /// <param name="triggerType">Type of trigger that should be tested</param>
        /// <returns><c>true</c> when the task can be run with the given trigger; otherwise <c>false</c>.</returns>
        public override bool IsAllowedForTrigger(ActionTriggerType triggerType)
        {
            return triggerType == ActionTriggerType.Scheduled;
        }

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">There is not an editor associated with the task for rebuilding database indexes.</exception>
        public override ActionTaskEditor CreateEditor()
        {
            // This task should not appear in the UI
            throw new InvalidOperationException("There is not an editor associated with the task for rebuilding database indexes.");
        }

        /// <summary>
        /// Rebuilds the table indexes.
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            try
            {
                ScheduledTrigger scheduledTrigger = new ScheduledTrigger(context.Step.TaskSettings);
                TimeSpan startTimeOfDay = scheduledTrigger.Schedule.StartDateTimeInUtc.TimeOfDay;
                DateTime scheduledStart = dateTimeProvider.UtcNow.Date + startTimeOfDay;
                DateTime scheduledEndTimeInUtc = scheduledStart + TimeSpan.FromMinutes(TimeoutInMinutes);

                if (dateTimeProvider.UtcNow < scheduledEndTimeInUtc)
                {
                    // The time window has not expired so fetch the indexes that need to be rebuilt
                    Queue<TableIndex> indexesToRebuild = new Queue<TableIndex>(indexMonitor.GetIndexesToRebuild());
                    LogIndexesNeedingRebuilt(indexesToRebuild.ToList());

                    using (new LoggedStopwatch(log, "Rebuilding Indexes Total Time."))
                    {
                        while (indexesToRebuild.Any() && dateTimeProvider.UtcNow < scheduledEndTimeInUtc)
                        {
                            // Time window still hasn't expired, so we can rebuild the next index
                            RebuildIndex(indexesToRebuild.Dequeue());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("An error occurred while running the task to rebuild indexes. {0}", ex.Message);
                log.Error(message, ex);

                throw;
            }
        }

        /// <summary>
        /// Logs the indexes needing rebuilt.
        /// </summary>
        /// <param name="indexes">The indexes.</param>
        private void LogIndexesNeedingRebuilt(List<TableIndex> indexes)
        {
            StringBuilder message = new StringBuilder();

            if (indexes.Any())
            {
                message.AppendFormat("The following indexes need to be rebuilt: {0}", 
                    string.Join<TableIndex>(", ", indexes));
            }
            else
            {
                message.Append("No indexes need to be rebuilt at this time.");
            }

            log.Info(message.ToString());
        }

        /// <summary>
        /// Rebuilds the index while logging the amount of time it took to complete.
        /// </summary>
        /// <param name="index">The index being rebuilt.</param>
        private void RebuildIndex(TableIndex index)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                log.InfoFormat("Rebuilding index {0}", index);
                indexMonitor.RebuildIndex(index);

                stopwatch.Stop();
                log.InfoFormat("Finished rebuilding index {0} ({1} ms)", index, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                // Just log the exception, so any other indexes that need to get addressed can be rebuilt
                string message = string.Format("An error occurred while rebuilding index {0}.", index);
                log.Error(message, ex);
            }
        }
    }
}
