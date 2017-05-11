using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Surcharge for Saturday Pickup
    /// </summary>
    public class SaturdayPickupSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="surcharges"></param>
        public SaturdayPickupSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the Saturday pickup surcharge to the rate if applicable 
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (shipment.Shipment.ShipDate.DayOfWeek == DayOfWeek.Saturday)
            {
                double surchargeAmount = surcharges[UpsSurchargeType.SaturdayPickup] * shipment.Packages.Count;

                serviceRate.AddAmount((decimal) surchargeAmount,
                    EnumHelper.GetDescription(UpsSurchargeType.SaturdayPickup));
            }
        }
    }
}