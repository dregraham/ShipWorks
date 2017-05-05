using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Surcharge for Carbon Neutral
    /// </summary>
    public class CarbonNeutralSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="surcharges"></param>
        public CarbonNeutralSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the carbon neutral surcharge to the rate if applicable 
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (shipment.CarbonNeutral)
            {
                int packageCount = shipment.Packages.Count;
                
                if (serviceRate.Service == UpsServiceType.UpsGround)
                {
                    serviceRate.AddAmount((decimal)surcharges[UpsSurchargeType.CarbonNeutralGround] * packageCount,
                        EnumHelper.GetDescription(UpsSurchargeType.CarbonNeutralGround));
                }
                else
                {
                    serviceRate.AddAmount((decimal)surcharges[UpsSurchargeType.CarbonNeutralAir] * packageCount,
                        EnumHelper.GetDescription(UpsSurchargeType.CarbonNeutralAir));
                }
            }
        }
    }
}