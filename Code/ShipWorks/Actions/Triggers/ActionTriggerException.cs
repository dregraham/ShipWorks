using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions.Triggers
{
    public class ActionTriggerException : Exception
    {
        public ActionTriggerException()
        {

        }

        public ActionTriggerException(string message) :
            base(message)
        {

        }

        public ActionTriggerException(string message, Exception inner) :
            base(message, inner)
        {

        }
    }
}
