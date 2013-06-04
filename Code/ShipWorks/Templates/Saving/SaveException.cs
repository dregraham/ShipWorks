using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Saving
{
    /// <summary>
    /// Root exception for known exceptions that occur while saving templates.
    /// </summary>
    public class SaveException : Exception
    {
        public SaveException()
        {

        }

        public SaveException(string message)
            : base(message)
        {

        }

        public SaveException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
