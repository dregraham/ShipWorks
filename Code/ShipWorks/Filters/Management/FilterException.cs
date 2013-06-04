using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Base for all exceptions related to filters that we handle
    /// </summary>
    [Serializable]
    public class FilterException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FilterException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
