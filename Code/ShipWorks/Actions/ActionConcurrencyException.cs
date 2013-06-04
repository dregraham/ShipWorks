using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Exception thrown when there is a optimistic concurrency problem when saving actions
    /// </summary>
    public class ActionConcurrencyException : ActionException
    {
        public ActionConcurrencyException()
        {

        }

        public ActionConcurrencyException(string message)
            : base(message)
        {

        }

        public ActionConcurrencyException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
