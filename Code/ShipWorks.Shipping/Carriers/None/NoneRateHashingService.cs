using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.None
{
    public class NoneRateHashingService : IRateHashingService
    {
        public RatingFields RatingFields { get; }
        public string GetRatingHash(ShipmentEntity shipment)
        {
            return string.Empty;
        }
    }
}
