using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Applies ThirdPartyBilling surcharge
    /// </summary>
    public class ThirdPartyBillingSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThirdPartyBillingSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the ThirdPartyBilling surcharge to the service rate based on the shipment
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (shipment.PayorType == (int) UpsPayorType.ThirdParty)
            {
                decimal thirdPartyBillingSurchargeRate = (decimal) surcharges[UpsSurchargeType.ThirdpartyBilling];
                decimal thridPartyBillingSurcharge = Math.Round(serviceRate.Amount * thirdPartyBillingSurchargeRate, 2, MidpointRounding.AwayFromZero);

                serviceRate.AddAmount(thridPartyBillingSurcharge, EnumHelper.GetDescription(UpsSurchargeType.ThirdpartyBilling));
            }
        }
    }
}
