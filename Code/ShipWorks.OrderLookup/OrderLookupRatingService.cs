using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Rating service for use in Order Lookup mode
    /// </summary>
    [Component(SingleInstance = true)]
    public class OrderLookupRatingService : IOrderLookupRatingService
    {
        private readonly IRatesRetriever ratesRetriever;
        private readonly IIndex<ShipmentTypeCode, IRateHashingService> rateHashingServiceLookup;
        private GenericResult<RateGroup> latestRates;
        private string latestRateHash;

        /// <summary>
        /// ctor
        /// </summary>
        public OrderLookupRatingService(IRatesRetriever ratesRetriever,
                                        IIndex<ShipmentTypeCode, IRateHashingService> rateHashingServiceLookup)
        {
            this.ratesRetriever = ratesRetriever;
            this.rateHashingServiceLookup = rateHashingServiceLookup;
        }

        /// <summary>
        /// Get rates for the given shipment
        /// </summary>
        public GenericResult<RateGroup> GetRates(ShipmentEntity shipment)
        {
            // If we don't have a rate hash, get one, and get rates.
            if (string.IsNullOrWhiteSpace(latestRateHash))
            {
                latestRateHash = rateHashingServiceLookup[shipment.ShipmentTypeCode].GetRatingHash(shipment);
                latestRates = ratesRetriever.GetRates(shipment);
                return latestRates;
            }

            // Get the rating hash for the shipment that is currently loaded
            string currentRateHash = rateHashingServiceLookup[shipment.ShipmentTypeCode].GetRatingHash(shipment);

            // if rate hashes match, just return latest rates
            if (latestRateHash == currentRateHash)
            {
                return latestRates;
            }
            
            // if the latest rate hash doesn't match the current one, set the current to latest, and get rates.
            latestRateHash = currentRateHash;
            latestRates = ratesRetriever.GetRates(shipment);
            return latestRates;
        }
    }
}