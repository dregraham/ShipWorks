using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public interface IUpsRateClient
    {
        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        IEnumerable<UpsServiceRate> GetRates(UpsShipmentEntity shipment);
    }
}