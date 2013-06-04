using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Exception thrown by Sears.com store and actions
    /// </summary>
    public class SearsException : Exception
    {
        public SearsException()
        {

        }

        public SearsException(string message) :
            base(message)
        {

        }

        public SearsException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
