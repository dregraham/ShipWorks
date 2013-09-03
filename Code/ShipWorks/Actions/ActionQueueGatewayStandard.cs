using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;

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

                // Always grab the UI actions and only grab the scheduled actions based on the ActionManager (i.e. an action 
                // manager capable of processing scheduled actions is also capable of processing UI based actions but an UI
                // action manager is only capable of processing UI based actions
                RelationPredicateBucket bucket = new RelationPredicateBucket(
                    (ActionQueueFields.ActionQueueType == (int) ActionQueueType.UserInterface | ActionQueueFields.ActionQueueType == (int) ActionManager.ActionQueueType) &
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
                        long actionID = reader.GetInt64(0);
                        string internalComputerLimitedList = reader.GetString(2);

                        ActionEntity actionEntity = ActionManager.GetAction(actionID);

                        ComputerActionPolicy computerActionPolicy = new ComputerActionPolicy((ComputerLimitedType)actionEntity.ComputerLimitedType, internalComputerLimitedList);
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