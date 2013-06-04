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

        }

        public EmailException(string message)
            : base(message)
        {

        }

        public EmailException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
