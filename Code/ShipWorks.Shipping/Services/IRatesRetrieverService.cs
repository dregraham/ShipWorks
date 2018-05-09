using System;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service that retrieves rates when shipments change
    /// </summary>
    public interface IRatesRetrieverService : IDisposable
    {
        /// <summary>
        /// Initialize the service for the current session
        /// </summary>
        void InitializeForCurrentSession();

        /// <summary>
        /// End the current session
        /// </summary>
        void EndSession();
    }
}