using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Surcharge for Saturday Delivery
    /// </summary>
    public class SaturdayDeliverySurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="surcharges"></param>
        public SaturdayDeliverySurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the Saturday Delivery surcharge to the rate if applicable 
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (shipment.SaturdayDelivery)
            {
                double surchargeAmount = surcharges[UpsSurchargeType.SaturdayDelivery] * shipment.Packages.Count;

                serviceRate.AddAmount((decimal) surchargeAmount,
                    EnumHelper.GetDescription(UpsSurchargeType.SaturdayDelivery));
            }
        }
    }
}