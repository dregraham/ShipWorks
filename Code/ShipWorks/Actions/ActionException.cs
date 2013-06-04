using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Base exception for all handleable exceptions thrown related to actions
    /// </summary>
    public class ActionException : Exception
    {
        public ActionException()
        {

        }

        public ActionException(string message)
            : base(message)
        {

        }

        public ActionException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
