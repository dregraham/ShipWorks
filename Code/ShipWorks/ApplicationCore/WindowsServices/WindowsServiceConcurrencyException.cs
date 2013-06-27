using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Scheduling;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    /// <summary>
    /// Exception thrown when there is a optimistic concurrency problem when saving actions
    /// </summary>
    public class WindowsServiceConcurrencyException : SchedulingException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WindowsServiceConcurrencyException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowsServiceConcurrencyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowsServiceConcurrencyException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
