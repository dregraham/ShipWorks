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
            // Get the rating hash for the shipment that is currently loaded
            string currentRateHash = rateHashingServiceLookup[shipment.ShipmentTypeCode].GetRatingHash(shipment);
            
            return ratesRetriever.GetRates(shipment);
        }
    }
}