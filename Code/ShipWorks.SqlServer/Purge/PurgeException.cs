using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.SqlServer.Purge
{
    public class PurgeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeException"/> class. 
        /// </summary>
        public PurgeException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeException"/> class. 
        /// </summary>
        public PurgeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
