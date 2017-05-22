using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Provides an action queue list that is based on a defined unchanging list of queues that had errors
    /// </summary>
    public class ActionQueueGatewayErrorList : ActionQueueGateway
    {
        List<long> queueList;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionQueueGatewayErrorList(List<long> queueList)
        {
            if (queueList == null)
            {
                throw new ArgumentNullException("queueList");
            }

            this.queueList = queueList;
        }

        /// <summary>
        /// Indiciates if any new queue's arrive in the gateway while the gateway is being processed
        /// </summary>
        public override bool CanNewQueuesArrive => false;

        /// <summary>
        /// This is a Error Action Queue Gateway
        /// </summary>
        public override ActionQueueGatewayType GatewayType => ActionQueueGatewayType.Error;

        /// <summary>
        /// Get the next set of queue ID's to process
        /// </summary>
        public override List<long> GetNextQueuePage(long lastQueueID)
        {
            IEnumerable<long> list;

            if (queueList.Contains(lastQueueID))
            {
                // Skip until we find it, and then skip past it
                list = queueList.SkipWhile(q => q != lastQueueID).Skip(1);
            }
            else
            {
                list = queueList;
            }

            return list.ToList();
        }

        /// <summary>
        /// Checks the queue to see if there's any work to do.
        /// </summary>
        public override bool AnyWorkToDo(DbConnection sqlConnection)
        {
            return queueList.Any();
        }
    }
}
