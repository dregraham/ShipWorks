using System.Collections.Generic;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Client to retrieve UPS rates
    /// </summary>
    public interface IUpsRateClient
    {
        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        List<UpsServiceRate> GetRates(ShipmentEntity shipment);
    }
}