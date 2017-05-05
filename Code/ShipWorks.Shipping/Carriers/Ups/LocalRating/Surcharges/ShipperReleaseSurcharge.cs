using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Surcharge for Shipper Release
    /// </summary>
    public class ShipperReleaseSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="surcharges"></param>
        public ShipperReleaseSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the shipper release surcharge to the rate if applicable 
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (shipment.ShipperRelease)
            {
                double surchargeAmount = surcharges[UpsSurchargeType.ShipperRelease] * shipment.Packages.Count;

                serviceRate.AddAmount((decimal) surchargeAmount,
                    EnumHelper.GetDescription(UpsSurchargeType.ShipperRelease));
            }
        }
    }
}