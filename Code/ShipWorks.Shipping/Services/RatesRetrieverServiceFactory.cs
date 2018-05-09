using System;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Use the correct rates retriever service based on whether this is an archive database or not
    /// </summary>
    public class RatesRetrieverServiceFactory : IInitializeForCurrentUISession
    {
        private readonly IRatesRetrieverService service;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatesRetrieverServiceFactory(
            IConfigurationData configurationData,
            Func<RatesRetrieverService> getLiveService,
            Func<ArchiveRatesRetrieverService> getArchiveService)
        {
            service = configurationData.IsArchive() ?
                (IRatesRetrieverService) getArchiveService() :
                getLiveService();
        }

        /// <summary>
        /// Initialize the service for the current session
        /// </summary>
        public void InitializeForCurrentSession() => service.InitializeForCurrentSession();

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession() => service.EndSession();

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        public void Dispose() => service.Dispose();
    }
}
