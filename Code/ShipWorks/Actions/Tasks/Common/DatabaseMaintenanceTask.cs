using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ShipWorks.Common;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using System.Reactive.Disposables;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// ActionTask that rebuilds database table indexes and other maintenance tasks.
    /// </summary>
    [ActionTask("Database Maintenance", "DatabaseMaintenance", ActionTaskCategory.Administration, true)]
    public class DatabaseMaintenanceTask : ActionTask
    {
        private ILog log;
        private IDateTimeProvider dateTimeProvider;
        private const int timeoutHours = 3;
        private readonly int timeoutSeconds = (int) TimeSpan.FromHours(timeoutHours).TotalSeconds;
        private string sqlConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseMaintenanceTask" /> class.
        /// </summary>
        /// <param name="dateTimeProvider">The date time provider.</param>
        /// <param name="log">The log.</param>
        public DatabaseMaintenanceTask(IDateTimeProvider dateTimeProvider, Func<Type, ILog> logFactory)
        {
            this.dateTimeProvider = dateTimeProvider;
            log = logFactory(typeof(DatabaseMaintenanceTask));

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
                    using (new LoggedStopwatch(log, "Database Maintenance Total Time."))
                    {
                        RebuildIndex();
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"An error occurred while running the database maintenance task. {ex.Message}";
                log.Error(message, ex);

                throw;
            }
        }

        /// <summary>
        /// Rebuilds the given table index.
        /// </summary>
        private void RebuildIndex()
        {
            using (new LoggedStopwatch(log, "Finished database maintenance."))
            {
                using (DbConnection sqlConnection = DataAccessAdapter.CreateConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    string sql = $@"
                        EXECUTE dbo.IndexOptimize
                            @Databases = '{sqlConnection.Database}',
                            @FragmentationLow = NULL,
                            @FragmentationMedium = 'INDEX_REBUILD_OFFLINE',
                            @FragmentationHigh = 'INDEX_REBUILD_OFFLINE',
                            @UpdateStatistics = 'ALL',
                            @OnlyModifiedStatistics = 'Y',
                            @Execute='Y',
                            @TimeLimit = {timeoutSeconds}
                        ";

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
                        sqlCommand.CommandText = sql;
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a connection string, based on SqlAdapter.Default.ConnectionString, and modifies it to have a new
        /// number of minutes for the timeout.
        /// </summary>
        private string ConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(sqlConnectionString))
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(SqlAdapter.Default.ConnectionString);
                    sqlConnectionStringBuilder.ConnectTimeout = timeoutSeconds;
                    sqlConnectionString = sqlConnectionStringBuilder.ConnectionString;
                }

                return sqlConnectionString;
            }
        }
    }
}
