using System;
using log4net;

namespace ShipWorks.Installer.Utilities
{
    /// <summary>
    /// Generic error handler
    /// </summary>
    public class ErrorHandler : IErrorHandler
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorHandler(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Handle the error
        /// </summary>
        public void HandleError(Exception ex)
        {
            log?.Error(ex);
        }
    }
}
