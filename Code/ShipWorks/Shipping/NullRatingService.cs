using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Null Rating Service
    /// </summary>
    public class NullRatingService : IRatingService
    {
        /// <summary>
        /// GetRates not supported by carrier
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            throw new InvalidOperationException("Should not be called.");
        }
    }
}