using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Exception thrown when there is a problem logging in to an email server
    /// </summary>
    public class EmailLogonException : EmailException
    {
        public EmailLogonException()
        {
            ErrorNumber = EmailExceptionErrorNumber.LogonFailed;
        }

        public EmailLogonException(string message)
            : base(message, EmailExceptionErrorNumber.LogonFailed)
        {

        }

        public EmailLogonException(string message, Exception inner)
            : base(message, inner, EmailExceptionErrorNumber.LogonFailed)
        {

        }

        public override string Message
        {
            get
            {
                return "There was an error logging on to the mail server. " + base.Message;
            }
        }
    }
}
