using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Surcharge for COD
    /// </summary>
    public class CODSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        public CODSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the COD surcharge to the rate if applicable 
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (shipment.CodEnabled)
            {
                double surchargeAmount = surcharges[UpsSurchargeType.CollectonDelivery] * shipment.Packages.Count;

                serviceRate.AddAmount((decimal) surchargeAmount,
                    EnumHelper.GetDescription(UpsSurchargeType.CollectonDelivery));
            }
        }
    }
}