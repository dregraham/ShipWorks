using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Thrown when an error occurs while trying to save an action
    /// </summary>
    public class ActionSaveException : ActionException
    {
        public ActionSaveException()
        {

        }

        public ActionSaveException(string message)
            : base(message)
        {

        }

        public ActionSaveException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
