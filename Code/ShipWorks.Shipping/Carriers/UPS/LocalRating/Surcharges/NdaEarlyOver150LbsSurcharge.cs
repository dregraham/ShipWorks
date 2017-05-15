using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Surcharge for shipment over 150 lbs, using NDA AM
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges.IUpsSurcharge" />
    public class NdaEarlyOver150LbsSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Initializes a new instance of the <see cref="NdaEarlyOver150LbsSurcharge"/> class.
        /// </summary>
        /// <param name="surcharges">The surcharges.</param>
        public NdaEarlyOver150LbsSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the surcharge to the service rate, if the surcharge is applicable
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            if (serviceRate.Service == UpsServiceType.UpsNextDayAirAM)
            {
                int applicablePackages = shipment.Packages.Count(p => p.BillableWeight > 150);
                if (applicablePackages > 0)
                {
                    decimal surcharge = applicablePackages * (decimal) surcharges[UpsSurchargeType.NdaEarlyOver150Lbs];
                    serviceRate.AddAmount(surcharge, $"{applicablePackages} NDA Early package(s) over 150LBS");
                }
            }
        }
    }
}
