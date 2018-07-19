using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// ActionTask that manages database table indexes state (enabled/disabled)..
    /// </summary>
    [ActionTask("Manage Index State", "ManageIndexState", ActionTaskCategory.Administration, true)]
    public class ManageIndexStateTask : ActionTask
    {
        private const int DefaultDaysBack = 14;
        private const int DefaultMinIndexAdvantage = 100;
        private readonly ILog log;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ISqlSession sqlSession;
        private const int timeoutHours = 3;
        private readonly int timeoutSeconds = (int) TimeSpan.FromHours(timeoutHours).TotalSeconds;
        private readonly IManageDisabledIndexesRepo mangeDisabledIndexesRepo;
        private readonly IMissingIndexResolver missingIndexResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageIndexStateTask" /> class.
        /// </summary>
        public ManageIndexStateTask(IDateTimeProvider dateTimeProvider, Func<Type, ILog> logFactory, ISqlSession sqlSession,
            IManageDisabledIndexesRepo mangeDisabledIndexesRepo, IMissingIndexResolver missingIndexResolver)
        {
            this.dateTimeProvider = dateTimeProvider;
            log = logFactory(typeof(ManageIndexStateTask));
            this.sqlSession = sqlSession;
            this.mangeDisabledIndexesRepo = mangeDisabledIndexesRepo;
            this.missingIndexResolver = missingIndexResolver;

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
        /// <exception cref="System.InvalidOperationException">There is not an editor associated with the task.</exception>
        public override ActionTaskEditor CreateEditor()
        {
            return new ActionTaskEditor();
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
                XElement minIndexUsageElement = doc.Descendants("MinIndexUsage").FirstOrDefault();
                int daysBack = Functional.ParseInt(daysBackElement?.Value).Match(x => x, _ => DefaultDaysBack);
                decimal minIndexUsage = Functional.ParseDecimal(minIndexUsageElement?.Value).Match(x => x, _ => DefaultMinIndexAdvantage);

                if (dateTimeProvider.UtcNow < scheduledEndTimeInUtc)
                {
                    using (new LoggedStopwatch(log, $"Manage Index State Total Time. Days back: {daysBack}"))
                    {
                        DisableUnusedIndexes(daysBack);
                        EnableUnusedIndexes(minIndexUsage);
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
        private void DisableUnusedIndexes(int daysBack)
        {
            using (new LoggedStopwatch(log, "Finished disabling unused indexes."))
            {
                using (DbConnection sqlConnection = sqlSession.OpenConnection(timeoutSeconds))
                {
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

        /// <summary>
        /// Enable missing ShipWorks indexes.
        /// </summary>
        private void EnableUnusedIndexes(decimal minIndexAdvantage)
        {
            IEnumerable<DisabledIndex> diabledIndexView;
            IEnumerable<MissingIndex> missingIndexView;
            using (ISqlAdapter adapter = new SqlAdapter(sqlSession.OpenConnection(timeoutHours)))
            {
                missingIndexView = mangeDisabledIndexesRepo.GetMissingIndexRequestsView(adapter, minIndexAdvantage);
                diabledIndexView = mangeDisabledIndexesRepo.GetShipWorksDisabledDefaultIndexesView(adapter);
            }

            var indexesToEnable = missingIndexResolver.GetIndexesToEnable(missingIndexView, diabledIndexView);

            foreach (DisabledIndex indexToEnable in indexesToEnable)
            {
                string enableIndexSql = indexToEnable.EnableIndexSql;
                string indexDescriptor = $"[{indexToEnable.TableName}].[{indexToEnable.IndexName}]";

                using (new LoggedStopwatch(log, $"Enabling index {indexDescriptor}."))
                {
                    try
                    {
                        using (DbConnection sqlConnection = sqlSession.OpenConnection(timeoutSeconds))
                        {
                            using (DbCommand sqlCommand = sqlConnection.CreateCommand())
                            {
                                sqlCommand.CommandTimeout = timeoutSeconds;
                                sqlCommand.CommandText = enableIndexSql;
                                sqlCommand.CommandType = CommandType.Text;
                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"An error occurred while attempting to re-enable index {indexDescriptor}", ex);
                        throw;
                    }
                }
            }
        }
    }
}
