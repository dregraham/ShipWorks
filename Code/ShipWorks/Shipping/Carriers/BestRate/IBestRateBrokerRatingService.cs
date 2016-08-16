using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Interface for getting rates from best rate brokers
    /// </summary>
    public interface IBestRateBrokerRatingService
    {
        /// <summary>
        /// Called to get the latest rates for the shipment
        /// </summary>
        IEnumerable<RateGroup> GetRates(ShipmentEntity shipment, List<BrokerException> exceptionHandler);

        RateGroup CompileBestRates(ShipmentEntity shipment, IEnumerable<RateGroup> rateGroups);
    }
}