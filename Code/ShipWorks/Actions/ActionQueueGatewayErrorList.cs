using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// Indiciates if any new queue's arrive in the gateway while the gateway is being processed
        /// </summary>
        public override bool CanNewQueuesArrive
        {
            get { return false; }
        }
    }
}
