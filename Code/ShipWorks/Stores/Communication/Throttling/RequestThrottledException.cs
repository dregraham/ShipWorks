using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Communication.Throttling
{
    /// <summary>
    /// Request throttled exception
    /// </summary>
    [Serializable]
    public class RequestThrottledException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RequestThrottledException(string message) :
            base(message, null)
        {

        }
    }
}
