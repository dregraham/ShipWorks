using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
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
                ResultsetFields resultFields = new ResultsetFields(2);
                resultFields.DefineField(ActionQueueFields.ActionQueueID, 0, "ActionQueueID", "");
                resultFields.DefineField(ActionQueueFields.InternalComputerLimitedList, 1, "InternalComputerLimitedList", "");

                RelationPredicateBucket bucket = new RelationPredicateBucket(
                    (ActionQueueFields.ActionQueueType == (int) ActionManager.ActionQueueType) &
                    (ActionQueueFields.ActionQueueID > lastQueueID) &
                    (ActionQueueFields.Status == (int) ActionQueueStatus.Dispatched |
                     ActionQueueFields.Status == (int) ActionQueueStatus.Incomplete |
                     ActionQueueFields.Status == (int) ActionQueueStatus.ResumeFromPostponed));

                using (SqlDataReader reader = (SqlDataReader) adapter.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, 100, new SortExpression(ActionQueueFields.ActionQueueID | SortOperator.Ascending), true))
                {
                    List<long> keys = new List<long>();

                    while (reader.Read())
                    {
                        // reader.GetString(1) returns InternalComputerLimitedList
                        ComputerActionPolicy computerActionPolicy = new ComputerActionPolicy(reader.GetString(1));
                        if (computerActionPolicy.IsComputerAllowed(UserSession.Computer))
                        {
                            keys.Add(reader.GetInt64(0));
                        }
                    }

                    return keys;
                }
            }
        }
    }
}