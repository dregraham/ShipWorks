using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Rates retriever service that does nothing
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ArchiveRatesRetrieverService : IRatesRetrieverService
    {
        /// <summary>
        /// Initialize the service for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {

        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {

        }

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        public void Dispose()
        {

        }
    }
}
