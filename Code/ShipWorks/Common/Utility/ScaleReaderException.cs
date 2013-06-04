using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Common.Utility
{
    /// <summary>
    /// Exception thrown when there is a problem reading from an external scale
    /// </summary>
    class ScaleReaderException : Exception
    {
        public ScaleReaderException()
        {

        }

        public ScaleReaderException(string message)
            : base(message)
        {

        }

        public ScaleReaderException(string message, Exception inner) :
            base(message, inner)
        {

        }
    }
}
