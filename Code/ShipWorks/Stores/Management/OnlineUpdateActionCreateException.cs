using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Exception to represent errors when creating online action update tasks
    /// </summary>
    [Serializable]
    public class OnlineUpdateActionCreateException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateActionCreateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateActionCreateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
