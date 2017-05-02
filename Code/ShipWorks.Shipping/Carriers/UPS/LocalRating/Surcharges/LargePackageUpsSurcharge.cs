using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    public class LargePackageUpsSurcharge : IUpsSurcharge
    {
        private readonly Dictionary<UpsSurchargeType, decimal> surcharges;

        /// <summary>
        /// Initializes a new instance of the <see cref="LargePackageUpsSurcharge"/> class.
        /// </summary>
        public LargePackageUpsSurcharge(Dictionary<UpsSurchargeType, decimal> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the large package surcharge if applicable.
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, UpsLocalServiceRate serviceRate)
        {
            if (IsLargePackage(shipment))
            {
                serviceRate.AddAmount(surcharges[UpsSurchargeType.LargePackage], "Large Package");
            }
        }

        /// <summary>
        /// Determines if any packages are considered large.
        /// </summary>
        private bool IsLargePackage(UpsShipmentEntity shipment)
        {
            return shipment.Packages.Any(p => p.IsLargePackage);
        }
    }
}