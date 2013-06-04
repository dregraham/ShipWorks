using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Exception thrown when an error occurrs related to filter conditions
    /// </summary>
    class FilterConditionException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FilterConditionException()
        {

        }

        public FilterConditionException(string message)
            : base(message)
        {

        }

        public FilterConditionException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
