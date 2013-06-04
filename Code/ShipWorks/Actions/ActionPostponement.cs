using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Encapsulates an ActionQueue that has been postponed.  
    /// </summary>
    public class ActionPostponement
    {
        string identifier;
        ActionQueueEntity queue;
        ActionQueueStepEntity step;
        object data;

        /// <summary>
        /// Create a new postponement instance
        /// </summary>
        public ActionPostponement(string identifier, ActionQueueStepEntity step, object data)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentException("identifier cannot be null or empty", "identifier");
            }

            if (step == null)
            {
                throw new ArgumentNullException("step");
            }

            this.identifier = identifier;
            this.queue = step.ActionQueue;
            this.step = step;
            this.data = data;
        }

        /// <summary>
        /// An identifier that should uniquily identify the postponement such that the step that does the postponing can always know - and is the
        /// only one to know - how to find what it has previously postponed.
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }

        /// <summary>
        /// The queue that has been postponed.  Exactly one step of the queue will be in the postponed state.
        /// </summary>
        public ActionQueueEntity Queue
        {
            get { return queue; }
        }

        /// <summary>
        /// The step of the queue that is the postponed step.
        /// and marked it as postponed.
        /// </summary>
        public ActionQueueStepEntity Step
        {
            get { return step; }
        }

        /// <summary>
        /// The saved state associated with the postponed step. Only means something to the ActionTask that did the postponing.
        /// </summary>
        public object Data
        {
            get { return data; }
        }
    }
}
