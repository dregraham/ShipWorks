using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    public class LargePackageUpsSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Initializes a new instance of the <see cref="LargePackageUpsSurcharge"/> class.
        /// </summary>
        public LargePackageUpsSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the large package surcharge if applicable.
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, UpsLocalServiceRate serviceRate)
        {
            int numberOfLargePackages = shipment.Packages.Count(p => p.IsLargePackage);
            if (numberOfLargePackages > 0)
            {
                decimal surcharge = numberOfLargePackages * (decimal) surcharges[UpsSurchargeType.LargePackage];

                serviceRate.AddAmount(surcharge, $"{numberOfLargePackages} Large Package(s)");
            }
        }
    }
}