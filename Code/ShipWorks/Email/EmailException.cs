using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Email
{
    /// <summary>
    /// Base for handling common email related known exceptions
    /// </summary>
    public class EmailException : Exception
    {
        public EmailException()
        {
            ErrorNumber = EmailExceptionErrorNumber.None;
        }

        public EmailException(string message)
            : base(message)
        {
            ErrorNumber = EmailExceptionErrorNumber.None;
        }

        public EmailException(string message, EmailExceptionErrorNumber errorNumber)
            : base(message)
        {
            ErrorNumber = errorNumber;
        }

        public EmailException(string message, Exception inner)
            : this(message, inner, EmailExceptionErrorNumber.None)
        {

        }

        public EmailException(string message, Exception inner, EmailExceptionErrorNumber errorNumber)
            : base(message, inner)
        {
            ErrorNumber = errorNumber;
        }

        /// <summary>
        /// Error number that represents the reason for EmailException that occurred.
        /// </summary>
        public EmailExceptionErrorNumber ErrorNumber
        {
            get; 
            set;
        }

        /// <summary>
        /// Determines if an immediate retry in sending is allowed based on the error number.
        /// </summary>
        public bool RetryAllowed
        {
            get
            {
                switch (ErrorNumber)
                {
                    case EmailExceptionErrorNumber.None:
                    case EmailExceptionErrorNumber.LogonFailed:
                    case EmailExceptionErrorNumber.DelaySending:
                    case EmailExceptionErrorNumber.MaxEmailsPerHourReached:
                    case EmailExceptionErrorNumber.EmailAccountChanged:
                        return true;
                    case EmailExceptionErrorNumber.MissingToField:
                    case EmailExceptionErrorNumber.MissingFromField:
                    case EmailExceptionErrorNumber.EmailBodyProcessingFailed:
                    case EmailExceptionErrorNumber.InvalidEmailAddress:
                    case EmailExceptionErrorNumber.InvalidTemplateSelected:
                    case EmailExceptionErrorNumber.IndeterminateTemplateSettings:
                    case EmailExceptionErrorNumber.InvalidEmailAccount:
                    case EmailExceptionErrorNumber.NoEmailAccountsConfigured:
                        return false;
                    default:
                        return true;
                }
            }
        }
    }
}
