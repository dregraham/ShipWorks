using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Exception thrown when a problem occurs while loading or saving appearance \ layout information
    /// </summary>
    class AppearanceException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppearanceException()
        {

        }

        /// <summary>
        /// Create an instance of the exception specifying the message text.
        /// </summary>
        public AppearanceException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Create an instance specifying the message text and the responsible exception.
        /// </summary>
        public AppearanceException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
