using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.SqlServer.General;
using ShipWorks.Users;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Gateway to provide lists of action queus to be processed
    /// </summary>
    public abstract class ActionQueueGateway
    {
        protected ILog log;
        protected const int pageSize = 100;
        protected Predicate isProcessableQueueType;

        /// <summary>
        /// Checks the queue to see if there's any work to do.
        /// </summary>
        public virtual bool AnyWorkToDo(DbConnection sqlConnection)
        {
            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(ActionQueueFields.ActionQueueID, 0, "ActionQueueID", "");

            RelationPredicateBucket bucket = new RelationPredicateBucket(isProcessableQueueType);

            List<ActionQueueItem> queueItems = new List<ActionQueueItem>(pageSize);

            using (SqlAdapter adapter = new SqlAdapter(sqlConnection))
            {
                using (IDataReader reader = adapter.FetchDataReader(resultFields, bucket, CommandBehavior.Default, 1, null, true))
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt64(0) > 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Indicates if new queues can be added to the gateway source while the gateway is being processed
        /// </summary>
        public virtual bool CanNewQueuesArrive => true;

        /// <summary>
        /// Type of gateway this is
        /// </summary>
        public abstract ActionQueueGatewayType GatewayType { get; }

        /// <summary>
        /// Get an ordered set of actions in the queue that need to be executed, starting after the given queue ID
        /// </summary>
        public virtual List<long> GetNextQueuePage(long lastQueueID)
        {
            ResultsetFields resultFields = new ResultsetFields(2);
            resultFields.DefineField(ActionQueueFields.ActionQueueID, 0, "ActionQueueID", "");
            resultFields.DefineField(ActionQueueFields.InternalComputerLimitedList, 1, "InternalComputerLimitedList", "");

            // We need to keep going until there are no more, or until we find some that we can process.  If we didn't do this, if the first 100 (or whatever our page size is) were
            // not allowed to run on this computer, then we'd return an empty list as the result set, and ignore any that WERE for us that were after the first 100.  In other words, 
            // instead of just looking through the first 100 for something meant for this computer, we need to keep going all the way until the end until we find some.
            while (true)
            {
                try
                {
                    List<ActionQueueItem> queueItems = GetNextQueuePageFromDatabase(resultFields, isProcessableQueueType, lastQueueID);

                    List<long> keys = queueItems.Where(x => x.IsComputerAllowed())
                        .Select(x => x.ActionQueueID)
                        .ToList();

                    // Locally this is now our next highest known actionQueueID, and where we will start if we end up having to loop back around due to this page not having any for us to process
                    if (queueItems.Any())
                    {
                        lastQueueID = queueItems.Max(x => x.ActionQueueID);   
                    }

                    // If we found some keys, or if there just aren't any more, then we are ready to return the result
                    if (keys.Count > 0 || queueItems.Count < pageSize)
                    {
                        return keys;
                    }
                }
                catch (Exception ex)
                {
                    // We have to catch the generic exception because there are quite a few exceptions that wrap a SqlException
                    if (UtilityFunctions.IsDeadlockException(ex.GetBaseException() as SqlException))
                    {
                        // Just log a deadlock exception and try again
                        log.Warn("Deadlock detected while trying to get actions", ex);
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Does the actual retrieval of queue items from the database
        /// </summary>
        /// <returns>There were a lot of deadlocks happening when retrieving this data, so it's been moved into its own function
        /// and the time the reader is open has been made as small as possible.</returns>
        private static List<ActionQueueItem> GetNextQueuePageFromDatabase(ResultsetFields resultFields, Predicate isProcessableQueueType, long lastQueueID)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket(
                        isProcessableQueueType &
                        (ActionQueueFields.ActionQueueID > lastQueueID) &
                        (ActionQueueFields.Status == (int)ActionQueueStatus.Dispatched |
                         ActionQueueFields.Status == (int)ActionQueueStatus.Incomplete |
                         ActionQueueFields.Status == (int)ActionQueueStatus.ResumeFromPostponed));

            // Process the actions as they came into the queue regardless of the action queue type
            SortExpression sortExpression = new SortExpression();
            sortExpression.Add(ActionQueueFields.ActionQueueID | SortOperator.Ascending);

            List<ActionQueueItem> queueItems = new List<ActionQueueItem>(pageSize);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                using (IDataReader reader = adapter.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, pageSize, sortExpression, true))
                {
                    while (reader.Read())
                    {
                        queueItems.Add(new ActionQueueItem(reader.GetInt64(0), reader.GetString(1)));
                    }
                }
            }

            return queueItems;
        }

        /// <summary>
        /// DTO to hold the action queue item details retrieved from the database
        /// </summary>
        /// <remarks>We could have used a Tuple, but it's harder to get meaing from Item1 and Item2 vs. ActionQueueID and ComputerList</remarks>
        protected class ActionQueueItem
        {
            private readonly string computerList;

            /// <summary>
            /// Constructor
            /// </summary>
            public ActionQueueItem(long actionQueueID, string computerList)
            {
                this.computerList = computerList;
                ActionQueueID = actionQueueID;
            }

            /// <summary>
            /// Id of the action queue item
            /// </summary>
            public long ActionQueueID { get; private set; }

            /// <summary>
            /// Gets whether the current computer is allowed for this action queue item
            /// </summary>
            public bool IsComputerAllowed()
            {
                return new ComputerActionPolicy(computerList).IsComputerAllowed(UserSession.Computer);
            }
        }
    }
}