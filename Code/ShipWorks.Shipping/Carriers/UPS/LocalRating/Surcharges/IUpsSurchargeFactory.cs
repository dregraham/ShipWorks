using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Returns a collection of surcharges
    /// </summary>
    public interface IUpsSurchargeFactory
    {
        /// <summary>
        /// Gets the specified surcharges to be applied to a shipment
        /// </summary>
        IEnumerable<IUpsSurcharge> Get(IDictionary<UpsSurchargeType, double> surchargeEntityLookup);
    }
}