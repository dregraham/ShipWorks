using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Orders.Archive;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// ActionTask that auto archives the database.
    /// </summary>
    [ActionTask("Auto archive database", "AutoArchiveTask", ActionTaskCategory.Administration, true)]
    [Component]
    public class AutoArchiveTask : ActionTask
    {
        private readonly IOrderArchiver orderArchiver;
        private readonly ILog log;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoArchiveTask" /> class.
        /// </summary>
        public AutoArchiveTask(IDateTimeProvider dateTimeProvider, IOrderArchiver orderArchiver)
        {
            this.orderArchiver = orderArchiver;
            log = LogManager.GetLogger(typeof(AutoArchiveTask));
            this.dateTimeProvider = dateTimeProvider;

            TimeoutInMinutes = 2 * 60;  // If the action wasn't started within two hours, don't start it.
            ExecuteOnDayOfWeek = DayOfWeek.Sunday;
            NumberOfDaysToKeep = 90;
        }

        /// <summary>
        /// Number of days worth of orders to keep
        /// </summary>
        public int NumberOfDaysToKeep { get; set; }

        /// <summary>
        /// Day of the week to create the archive
        /// </summary>
        public DayOfWeek ExecuteOnDayOfWeek { get; set; }

        /// <summary>
        /// Indicates if the task requires input to function.  Such as the contents of a filter, or the item that caused the action.
        /// </summary>
        public override ActionTaskInputRequirement InputRequirement => ActionTaskInputRequirement.None;

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
        /// <exception cref="System.InvalidOperationException">There is not an editor associated with the task for auto archiving databases.</exception>
        public override ActionTaskEditor CreateEditor()
        {
            // This task should not appear in the UI
            throw new InvalidOperationException("There is not an editor associated with the task for auto archiving databases.");
        }

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Auto archive the database.
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            try
            {
                DateTime cutoffDate = dateTimeProvider.UtcNow.AddDays(-1 * NumberOfDaysToKeep);

                ScheduledTrigger scheduledTrigger = new ScheduledTrigger(context.Step.TaskSettings);
                TimeSpan startTimeOfDay = scheduledTrigger.Schedule.StartDateTimeInUtc.TimeOfDay;
                DateTime scheduledStart = dateTimeProvider.UtcNow.Date + startTimeOfDay;
                DateTime scheduledEndTimeInUtc = scheduledStart + TimeSpan.FromMinutes(TimeoutInMinutes);

                if (dateTimeProvider.UtcNow < scheduledEndTimeInUtc)
                {
                    using (new LoggedStopwatch(log, "Auto archive Total Time."))
                    {
                        await orderArchiver.Archive(cutoffDate).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"An error occurred while running the task to auto archive. {ex.Message}";
                log.Error(message, ex);

                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
