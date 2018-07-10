using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using System.Reactive.Disposables;
using System.Xml.Linq;
using Interapptive.Shared.Extensions;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// ActionTask that manages database table indexes state (enabled/disabled)..
    /// </summary>
    [ActionTask("Manage Index State", "ManageIndexState", ActionTaskCategory.Administration, true)]
    public class ManageIndexStateTask : ActionTask
    {
        private readonly ILog log;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ISqlSession sqlSession;
        private const int timeoutHours = 3;
        private readonly int timeoutSeconds = (int) TimeSpan.FromHours(timeoutHours).TotalSeconds;
        private string sqlConnectionString;
        private int daysBack = 14;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageIndexStateTask" /> class.
        /// </summary>
        public ManageIndexStateTask(IDateTimeProvider dateTimeProvider, Func<Type, ILog> logFactory, ISqlSession sqlSession)
        {
            this.dateTimeProvider = dateTimeProvider;
            log = logFactory(typeof(ManageIndexStateTask));
            this.sqlSession = sqlSession;

            TimeoutInMinutes = 180;
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
        /// <exception cref="System.InvalidOperationException">There is not an editor associated with the task.</exception>
        public override ActionTaskEditor CreateEditor()
        {
            // This task should not appear in the UI
            throw new InvalidOperationException("There is not an editor associated with the task for managing database index state.");
        }

        /// <summary>
        /// Run the index state management script.
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            try
            {
                ScheduledTrigger scheduledTrigger = new ScheduledTrigger(context.Step.TaskSettings);
                TimeSpan startTimeOfDay = scheduledTrigger.Schedule.StartDateTimeInUtc.TimeOfDay;
                DateTime scheduledStart = dateTimeProvider.UtcNow.Date + startTimeOfDay;
                DateTime scheduledEndTimeInUtc = scheduledStart + TimeSpan.FromMinutes(TimeoutInMinutes);

                XDocument doc = XDocument.Parse(context.Step.TaskSettings);
                XElement daysBackElement = doc.Descendants("DaysBack").FirstOrDefault();
                if (daysBackElement != null)
                {
                    int.TryParse(daysBackElement.Value, out daysBack);
                }

                if (dateTimeProvider.UtcNow < scheduledEndTimeInUtc)
                {
                    using (new LoggedStopwatch(log, $"Manage Index State Total Time. Days back: {daysBack}"))
                    {
                        DisableUnusedIndexes();
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"An error occurred while running the Manage Index State task. {ex.Message}";
                log.Error(message, ex);

                throw;
            }
        }

        /// <summary>
        /// Disable unused indexes.
        /// </summary>
        private void DisableUnusedIndexes()
        {
            using (new LoggedStopwatch(log, "Finished disabling unused indexes."))
            {
                using (DbConnection sqlConnection = sqlSession.OpenConnection(timeoutSeconds))
                {
                    if (sqlConnection is SqlConnection connection)
                    {
                        void infoHandler(object sender, SqlInfoMessageEventArgs e) => log.Info(e.Message);

                        connection.FireInfoMessageEventOnUserErrors = true;
                        connection.InfoMessage += infoHandler;

                        Disposable.Create(() => connection.InfoMessage -= infoHandler);
                    }

                    using (DbCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandTimeout = timeoutSeconds;
                        sqlCommand.CommandText = $"EXECUTE DisableUnusedIndexes @daysBack = {daysBack}";
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
