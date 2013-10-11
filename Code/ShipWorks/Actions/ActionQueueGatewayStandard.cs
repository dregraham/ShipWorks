using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks.Actions
{
    /// <summary>
    /// The standard action queue gateway that retrieves all Incomplete\Dispatched queues from the database
    /// </summary>
    public class ActionQueueGatewayStandard : ActionQueueGateway
    {
        const int pageSize = 100;

        /// <summary>
        /// Indicates if new queues can be added to the gateway source while the gateway is being processed
        /// </summary>
        public override bool CanNewQueuesArrive
        {
            get
            {
                // New queues can arrive in the database at any time
                return true;
            }
        }

        /// <summary>
        /// Get an ordered set of actions in the queue that need to be executed, starting after the given queue ID
        /// </summary>
        public override List<long> GetNextQueuePage(long lastQueueID)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                ResultsetFields resultFields = new ResultsetFields(2);
                resultFields.DefineField(ActionQueueFields.ActionQueueID, 0, "ActionQueueID", "");
                resultFields.DefineField(ActionQueueFields.InternalComputerLimitedList, 1, "InternalComputerLimitedList", "");

                // By default only process the queue type that matches this process
                Predicate isProcessableQueueType = ActionQueueFields.ActionQueueType == (int) ActionManager.ExecutionModeActionQueueType;

                // If the UI isn't running somehwere, and we are the background process, go ahead and do UI actions too since it's not open
                if (!Program.ExecutionMode.IsUISupported && !UserInterfaceExecutionMode.IsProcessRunning)
                {
                    // Additionally process UI actions if the UI is not running
                    isProcessableQueueType |= ActionQueueFields.ActionQueueType == (int) ActionQueueType.UserInterface;
                }

                // We need to keep going until there are no more, or until we find some that we can process.  If we didn't do this, if the first 100 (or whatever our page size is) were
                // not allowed to run on this computer, then we'd return an empty list as the result set, and ignore any that WERE for us that were after the first 100.  In other words, 
                // instead of just looking through the first 100 for something meant for this computer, we need to keep going all the way until the end until we find some.
                while (true)
                {
                    RelationPredicateBucket bucket = new RelationPredicateBucket(
                        isProcessableQueueType &
                        (ActionQueueFields.ActionQueueID > lastQueueID) &
                        (ActionQueueFields.Status == (int) ActionQueueStatus.Dispatched |
                         ActionQueueFields.Status == (int) ActionQueueStatus.Incomplete |
                         ActionQueueFields.Status == (int) ActionQueueStatus.ResumeFromPostponed));

                    // Process the actions as they came into the queue regardless of the action queue type
                    SortExpression sortExpression = new SortExpression();
                    sortExpression.Add(ActionQueueFields.ActionQueueID | SortOperator.Ascending);

                    using (SqlDataReader reader = (SqlDataReader) adapter.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, pageSize, sortExpression, true))
                    {
                        List<long> keys = new List<long>();
                        int results = 0;

                        while (reader.Read())
                        {
                            long actionQueueID = reader.GetInt64(0);
                            string computerList = reader.GetString(1);

                            // Locally this is now our next highest known actionQueueID, and where we will start if we end up having to loop back around due to this page not having any for us to process
                            lastQueueID = actionQueueID;
                            results++;

                            // If it matches the policy add, add it
                            ComputerActionPolicy computerActionPolicy = new ComputerActionPolicy(computerList);
                            if (computerActionPolicy.IsComputerAllowed(UserSession.Computer))
                            {
                                keys.Add(actionQueueID);
                            }
                        }
                        
                        // If we found some keys, or if there just aren't any more, then we are ready to return the result
                        if (keys.Count > 0 || results < pageSize)
                        {
                            return keys;
                        }
                    }
                }
            }
        }
    }
}