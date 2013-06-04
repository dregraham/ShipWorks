using System;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Base for all exceptions thrown by the OnTrac integration
    /// </summary>
    public class OnTracException : Exception
    {
        /// <summary>
        /// Exception
        /// </summary>
        public OnTracException()
        {
        }

        /// <summary>
        /// Exception
        /// </summary>
        public OnTracException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Exception
        /// </summary>
        public OnTracException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}