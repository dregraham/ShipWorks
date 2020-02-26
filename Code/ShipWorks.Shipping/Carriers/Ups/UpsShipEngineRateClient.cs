using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Client for retreiving UPS rates via ShipEngine
    /// </summary>
    [KeyedComponent(typeof(IUpsRateClient), UpsRatingMethod.ShipEngine)]
    class UpsShipEngineRateClient : IUpsRateClient
    {
        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        public GenericResult<List<UpsServiceRate>> GetRates(ShipmentEntity shipment)
        {
            throw new System.NotImplementedException();
        }
    }
}
