using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Delegate for the ActionProcessed event
    /// </summary>
    public delegate void ActionProcessedEventHandler(object sender, ActionProcessedEventArgs e);

    /// <summary>
    /// EventArgs for the ActionProcessedEventHandler
    /// </summary>
    public class ActionProcessedEventArgs : EventArgs
    {
        long queueID;
        ActionRunnerResult result;

        ActionQueueEntity startState;
        ActionQueueEntity finishState;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionProcessedEventArgs(long queueID, ActionRunnerResult runResult, ActionQueueEntity startState, ActionQueueEntity finishState)
        {
            this.queueID = queueID;
            this.result = runResult;
            this.startState = startState;
            this.finishState = finishState;
        }

        /// <summary>
        /// The ActionQueueID
        /// </summary>
        public long QueueID
        {
            get { return queueID; }
        }

        /// <summary>
        /// The result of what the ActionRunner did for the queue
        /// </summary>
        public ActionRunnerResult Result
        {
            get { return result; }
        }

        /// <summary>
        /// The state of the queue before running.  Only valid if Result is Ran
        /// </summary>
        public ActionQueueEntity QueueAtStart
        {
            get { return startState; }
        }

        /// <summary>
        /// The state of the queue after runnning. Only valid if Result is Ran
        /// </summary>
        public ActionQueueEntity QueueAtFinish
        {
            get { return finishState; }
        }
    }
}
