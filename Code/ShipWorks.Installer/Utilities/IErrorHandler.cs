using System;

namespace ShipWorks.Installer.Utilities
{
    /// <summary>
    /// Interface for classes that handle errors
    /// </summary>
    public interface IErrorHandler
    {
        /// <summary>
        /// Handle an error
        /// </summary>
        void HandleError(Exception ex);
    }
}
