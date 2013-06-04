using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Represents a running background operation
    /// </summary>
    public class ApplicationBusyToken : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationBusyToken()
        {

        }

        /// <summary>
        /// Mark the operation as completed
        /// </summary>
        public void Dispose()
        {
            ApplicationBusyManager.OperationComplete(this);
        }
    }
}
