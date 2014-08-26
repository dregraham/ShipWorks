using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;

namespace ShipWorks.Email
{
    /// <summary>
    /// Exception that is thrown when too many email messages have already been sent for a given duration.
    /// </summary>
    class EmailThrottleException : EmailException
    {
        public EmailThrottleException()
        {
            ErrorNumber = EmailExceptionErrorNumber.MaxEmailsPerHourReached;
        }

        public EmailThrottleException(string message)
            : base(message, EmailExceptionErrorNumber.MaxEmailsPerHourReached)
        {

        }

        public EmailThrottleException(string message, Exception inner)
            : base(message, inner, EmailExceptionErrorNumber.MaxEmailsPerHourReached)
        {

        }
    }
}
