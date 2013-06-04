using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Exception thrown when filters couldn't be updated before processing actions that required it.
    /// </summary>
    class ActionRunnerFilterUpdateException : ActionException
    {
        public ActionRunnerFilterUpdateException()
        {

        }

        public ActionRunnerFilterUpdateException(string message)
            : base(message)
        {

        }

        public ActionRunnerFilterUpdateException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
