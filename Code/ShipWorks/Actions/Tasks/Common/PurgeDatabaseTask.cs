using Interapptive.Shared.Utility;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using System.Data.SqlClient;
using System.Data;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for deleting/purging old data.
    /// </summary>
    [ActionTask("Delete old data", "PurgeDatabase", ActionTaskCategory.Administration)]
    public class PurgeDatabaseTask : ActionTask
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PurgeDatabaseTask));
        readonly ISqlPurgeScriptRunner scriptRunner;
        readonly IDateTimeProvider dateProvider;

        readonly List<PurgeDatabaseType> purgeOrder = new List<PurgeDatabaseType>
            {
                PurgeDatabaseType.Labels,
                PurgeDatabaseType.PrintJobs,
                PurgeDatabaseType.Email,
                PurgeDatabaseType.Orders,
                PurgeDatabaseType.Audit
            };

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
        public override ActionTaskInputRequirement InputRequirement
        {
            get { return ActionTaskInputRequirement.None; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the purge task can timeout.
        /// </summary>
        /// <value>
        /// <c>true</c> if the purge task can timeout; otherwise, <c>false</c>.
        /// </value>
        public bool CanTimeout { get; set; }

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
        /// Gets or sets whether the database should be shrunk after the purge is complete.
        /// </summary>
        public bool ReclaimDiskSpace { get; set; }

        /// <summary>
        /// Creates the editor that is used to edit the task.
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new PurgeDatabaseTaskEditor(this);
        }

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
        /// Runs the purge scripts.
        /// </summary>
        protected override void Run()
        {
            DateTime sqlDate = scriptRunner.SqlUtcDateTime;
            DateTime localStopExecutionAfter = dateProvider.UtcNow.AddHours(TimeoutInHours);
            DateTime runUntil = sqlDate.AddHours(TimeoutInHours);
            DateTime olderThan = sqlDate.AddDays(-RetentionPeriodInDays);

            // Purge the data and shrink the database
            Dictionary<PurgeDatabaseType, Exception> exceptions = PurgeData(localStopExecutionAfter, olderThan, runUntil);
            ShrinkDatabase(localStopExecutionAfter);

            // Fail the task if any exceptions were encountered
            if (exceptions.Any())
            {
                string exceptionMessage = string.Format("An error occurred while deleting the following items: {0}",
                    exceptions.Keys
                        .Select(x => EnumHelper.GetDescription(x))
                        .Aggregate((x, y) => x + ", " + y));

                throw new ActionTaskRunException(exceptionMessage, new ExceptionCollection(new ArrayList(exceptions.Values)));
            }
        }

        /// <summary>
        /// Purges the data from the database.
        /// </summary>
        /// <param name="localStopExecutionAfter">The local stop execution after.</param>
        /// <param name="olderThan">The older than.</param>
        /// <param name="runUntil">The run until.</param>
        /// <returns>A Dictionary containing any exceptions that were encountered.</returns>
        private Dictionary<PurgeDatabaseType, Exception> PurgeData(DateTime localStopExecutionAfter, DateTime olderThan, DateTime runUntil)
        {
            Dictionary<PurgeDatabaseType, Exception> exceptions = new Dictionary<PurgeDatabaseType, Exception>();

            List<PurgeDatabaseType> purges = purgeOrder.Intersect(Purges).ToList();

            // If we try to do any purges, add the deletion of abandoned resources last.
            if (purges.Any())
            {
                purges.Insert(purges.Count, PurgeDatabaseType.AbandonedResources);
            }

            foreach (PurgeDatabaseType purge in purges)
            {
                // Stop executing if we've been running longer than the time the user has allowed.
                if (CanTimeout && localStopExecutionAfter < dateProvider.UtcNow)
                {
                    log.Info("Stopping purge because it has exceeded the maximum allowed time.");
                    break;
                }

                // For orders, there is no script - we do it using C#
                if (purge == PurgeDatabaseType.Orders)
                {
                    PurgeOrders(olderThan, localStopExecutionAfter);
                }
                else
                {
                    string scriptName = EnumHelper.GetApiValue(purge);

                    log.InfoFormat("Running {0}, deleting data older than {1}...", scriptName, olderThan);
                    try
                    {
                        scriptRunner.RunScript(scriptName, olderThan, CanTimeout ? runUntil : (DateTime?) null, 5);
                        log.InfoFormat("Finished {0} successfully.", scriptName);
                    }
                    catch (DbException ex)
                    {
                        // Catch a DbException instead of a SqlException so this catch can be tested.
                        // SqlException is sealed and can't be easily constructed.
                        exceptions.Add(purge, ex);
                        log.InfoFormat("Error running purge: {0}.", ex.Message);
                    }
                }
            }

            return exceptions;
        }

        /// <summary>
        /// Conditionally shrinks the database.
        /// </summary>
        /// <param name="localStopExecutionAfter">The local stop execution after.</param>
        /// <exception cref="ActionTaskRunException"></exception>
        private void ShrinkDatabase(DateTime localStopExecutionAfter)
        {
            try
            {
                if (ReclaimDiskSpace && (!CanTimeout || localStopExecutionAfter < dateProvider.UtcNow))
                {
                    scriptRunner.ShrinkDatabase();
                }
            }
            catch (DbException ex)
            {
                // Catch a DbException instead of a SqlException so this catch can be tested.
                // SqlException is sealed and can't be easily constructed.
                log.InfoFormat("Error shrinking database: {0}.", ex.Message);
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Purge old orders
        /// </summary>
        private void PurgeOrders(DateTime olderThan, DateTime stopAfter)
        {
            // First we need to query orders that are older than the time frame configured by the task
            ResultsetFields resultFields = new ResultsetFields(2);
            resultFields.DefineField(OrderFields.OrderID, 0, "OrderID", "");
            resultFields.DefineField(OrderFields.CustomerID, 1, "CustomerID", "");

            // Delete the oldest orders first
            SortExpression sort = new SortExpression(OrderFields.OrderDate | SortOperator.Ascending);

            int pageSize = 100;

            while (true)
            {
                // <orderID, customerID>
                List<Tuple<long, long>> toDelete = new List<Tuple<long, long>>();

                RelationPredicateBucket bucket = new RelationPredicateBucket(OrderFields.OrderDate <= olderThan);
                using (SqlDataReader reader = (SqlDataReader) SqlAdapter.Default.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, pageSize, sort, true))
                {
                    while (reader.Read())
                    {
                        toDelete.Add(Tuple.Create(reader.GetInt64(0), reader.GetInt64(1)));
                    }
                }

                // Now go through each one and delete it
                foreach (var tuple in toDelete)
                {
                    long orderID = tuple.Item1;
                    long customerID = tuple.Item2;

                    // First delete the order
                    DeletionService.DeleteOrder(orderID);

                    // See if the customer has any orders left
                    object result = SqlAdapter.Default.GetScalar(CustomerFields.RollupOrderCount, null, AggregateFunction.None, CustomerFields.CustomerID == customerID);

                    if (!(result is DBNull))
                    {
                        int ordersLeft = Convert.ToInt32(result);

                        if (ordersLeft == 0)
                        {
                            DeletionService.DeleteCustomer(customerID);
                        }
                    }

                    // Stop executing if we've been running longer than the time the user has allowed.
                    if (CanTimeout && stopAfter < dateProvider.UtcNow)
                    {
                        log.Info("Stopping purge because it has exceeded the maximum allowed time.");
                        return;
                    }
                }

                // If there aren't any more, or if it was less than our page size, then we must be done
                if (toDelete.Count < pageSize)
                {
                    log.InfoFormat("There are no more orders left to purge older than the retention period.");
                    return;
                }
            }
        }

        /// <summary>
        /// Is the task allowed to be run using the specified trigger type?
        /// </summary>
        /// <param name="triggerType">Type of trigger that should be tested</param>
        /// <returns></returns>
        public override bool IsAllowedForTrigger(ActionTriggerType triggerType)
        {
            // So I could kick these off with custom buttons
            if (InterapptiveOnly.MagicKeysDown)
            {
                return true;
            }

            return triggerType == ActionTriggerType.Scheduled;
        }
    }
}
