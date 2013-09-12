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
                ResultsetFields resultFields = new ResultsetFields(3);
                resultFields.DefineField(ActionQueueFields.ActionID, 0, "ActionID", "");
                resultFields.DefineField(ActionQueueFields.ActionQueueID, 1, "ActionQueueID", "");
                resultFields.DefineField(ActionQueueFields.InternalComputerLimitedList, 2, "InternalComputerLimitedList", "");

                // Only process the scheduled actions based on the action manager configuration
                Predicate isProcessableQueueType = ActionQueueFields.ActionQueueType == (int) ActionManager.ActionQueueType;

                if(!UserInterfaceExecutionMode.IsProcessRunning)
                {
                    // Additionally process UI actions if the UI is not running
                    isProcessableQueueType |= ActionQueueFields.ActionQueueType == (int)ActionQueueType.UserInterface;
                }

                RelationPredicateBucket bucket = new RelationPredicateBucket(
                    isProcessableQueueType &
                    (ActionQueueFields.ActionQueueID > lastQueueID) &
                    (ActionQueueFields.Status == (int) ActionQueueStatus.Dispatched |
                     ActionQueueFields.Status == (int) ActionQueueStatus.Incomplete |
                     ActionQueueFields.Status == (int) ActionQueueStatus.ResumeFromPostponed));

                // Process the actions as they came into the queue regardless of the action queue type
                SortExpression sortExpression = new SortExpression();
                sortExpression.Add(ActionQueueFields.ActionQueueID | SortOperator.Ascending);

                using (SqlDataReader reader = (SqlDataReader) adapter.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, 100, sortExpression, true))
                {
                    List<long> keys = new List<long>();

                    while (reader.Read())
                    {
                        string internalComputerLimitedList = reader.GetString(2);

                        ComputerActionPolicy computerActionPolicy = new ComputerActionPolicy(internalComputerLimitedList);
                        if (computerActionPolicy.IsComputerAllowed(UserSession.Computer))
                        {
                            // Add the ActionQueueID
                            keys.Add(reader.GetInt64(1));
                        }
                    }

                    return keys;
                }
            }
        }
    }
}