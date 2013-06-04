using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Exception thrown when there is a problem logging in to an email server
    /// </summary>
    public class EmailLogonException : EmailException
    {
        public EmailLogonException()
        {

        }

        public EmailLogonException(string message)
            : base(message)
        {

        }

        public EmailLogonException(string message, Exception inner)
            : base(message, inner)
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
