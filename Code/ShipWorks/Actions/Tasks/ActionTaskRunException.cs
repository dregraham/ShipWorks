using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Exception thrown by tasks when a problem happens during execution
    /// </summary>
    public class ActionTaskRunException : ActionException
    {
        public ActionTaskRunException()
        {

        }

        public ActionTaskRunException(string message)
            : base(message)
        {

        }

        public ActionTaskRunException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
