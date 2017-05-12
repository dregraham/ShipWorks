using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters
{
    /// <summary>
    /// Get eligible services based on the shipment
    /// </summary>
    public class ShipmentServiceFilter : IServiceFilter
    {
        /// <summary>
        /// Get eligible services based on the shipment
        /// </summary>
        public IEnumerable<UpsServiceType> GetEligibleServices(UpsShipmentEntity shipment, IEnumerable<UpsServiceType> services)
        {
            // Remove ground when the ship date is saturday
            return shipment.Shipment.ShipDate.DayOfWeek == DayOfWeek.Saturday
                ? services.Except(new[] {UpsServiceType.UpsGround, UpsServiceType.Ups3DaySelect})
                : services;
        }
    }
}