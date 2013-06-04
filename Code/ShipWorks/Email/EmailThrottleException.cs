using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Email
{
    /// <summary>
    /// Exception that is thrown when too many email messages have already been sent for a given duration.
    /// </summary>
    class EmailThrottleException : EmailException
    {
        public EmailThrottleException()
        {

        }

        public EmailThrottleException(string message)
            : base(message)
        {

        }

        public EmailThrottleException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
