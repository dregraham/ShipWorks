using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.Api
{
    /// <summary>
    /// An implementation of the IPromoClientFactory that creates the "live" implementation
    /// of the IUpsApiPromoClient.
    /// </summary>
    public class UpsPromoWebClientFactory : IPromoClientFactory
    {
        private readonly ILogEntryFactory logEntryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPromoWebClientFactory"/> class.
        /// </summary>
        public UpsPromoWebClientFactory(ILogEntryFactory logEntryFactory)
        {
            this.logEntryFactory = logEntryFactory;
        }

        /// <summary>
        /// Uses the UpsPromo provided to create a web client for communicating with 
        /// the UPS Promo API.
        /// </summary>
        /// <param name="promo">The UPS promo that the client is being created for.</param>
        /// <returns>A concrete UpsApiPromoClient instance.</returns>
        public IUpsApiPromoClient CreatePromoClient(UpsPromo promo)
        {
            return new UpsApiPromoClient(promo, logEntryFactory);
        }
    }
}
