using System.Collections.Generic;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    ///  Returns a collection of surcharges to be applied on a base UPS Rate.
    /// </summary>
    [Component]
    public class UpsSurchargeFactory : IUpsSurchargeFactory
    {
        /// <summary>
        /// Gets the specified surcharges to be applied to a shipment
        /// </summary>
        public IEnumerable<IUpsSurcharge> Get(IDictionary<UpsSurchargeType, double> surchargeEntityLookup, IUpsLocalRatingZoneFileEntity zoneFileEntity)
        {
            return new IUpsSurcharge[]
            {
                new DeliveryAreaSurcharge(surchargeEntityLookup, zoneFileEntity), 
                new LargePackageUpsSurcharge(surchargeEntityLookup)
            };
        }
    }
}