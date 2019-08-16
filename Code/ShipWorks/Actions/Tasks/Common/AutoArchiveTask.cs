using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Archive;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// ActionTask that auto archives the database.
    /// </summary>
    [ActionTask("Auto archive database", "AutoArchiveTask", ActionTaskCategory.Administration, true)]
    [Component]
    public class AutoArchiveTask : ActionTask, ISingleDispatchAction
    {
        private readonly IOrderArchiver orderArchiver;
        private readonly ILog log;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoArchiveTask" /> class.
        /// </summary>
        public AutoArchiveTask(IDateTimeProvider dateTimeProvider, 
            IOrderArchiver orderArchiver, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
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
            return triggerType == ActionTriggerType.Scheduled ||
                   (InterapptiveOnly.MagicKeysDown && triggerType == ActionTriggerType.UserInitiated);
        }

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new ActionTaskEditor();
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

                // Cache the result from TangoWebClient.GetTangoCustomerId() now so that if the db connection
                // is lost, we can still track the customer id.
                string tangoCustomerId = TangoWebClient.GetTangoCustomerId();
                Telemetry.GetCustomerID = () => tangoCustomerId;

                ScheduledTrigger scheduledTrigger = new ScheduledTrigger(context.Step.TaskSettings);
                TimeSpan startTimeOfDay = scheduledTrigger.Schedule.StartDateTimeInUtc.TimeOfDay;
                DateTime scheduledStart = dateTimeProvider.UtcNow.Date + startTimeOfDay;
                DateTime scheduledEndTimeInUtc = scheduledStart + TimeSpan.FromMinutes(TimeoutInMinutes);

                if (dateTimeProvider.UtcNow < scheduledEndTimeInUtc)
                {
                    using (new LoggedStopwatch(log, "Auto archive Total Time."))
                    {
                        IResult result = await orderArchiver.Archive(cutoffDate, false).ConfigureAwait(false);

                        if (result.Failure)
                        {
                            throw result.Exception;
                        }
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
