using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Exception that is thrown when a concurrency vioation occurrs while saving filters
    /// </summary>
    class FilterConcurrencyException : FilterException
    {
        public FilterConcurrencyException()
        {

        }

        public FilterConcurrencyException(string message)
            : base(message)
        {

        }

        public FilterConcurrencyException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
