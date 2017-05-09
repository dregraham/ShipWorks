using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Applies a Signature Surcharge to the service rate based on the shipment
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges.IUpsSurcharge" />
    public class VerbalConfirmationSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureSurcharge"/> class.
        /// </summary>
        public VerbalConfirmationSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the surcharge to the service rate based on the shipment
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            int applicablePackages = shipment.Packages.Count(ChargeApplies);

            if (applicablePackages > 0)
            {
                decimal rate = (decimal) surcharges[UpsSurchargeType.VerbalConfirmation];
                decimal surcharge = rate * applicablePackages;

                serviceRate.AddAmount(surcharge, $"{applicablePackages} Package(s) with Verbal Confirmation");
            }
        }

        /// <summary>
        /// Returns true if charge applies
        /// </summary>
        private static bool ChargeApplies(UpsPackageEntity package) => package.VerbalConfirmationEnabled;
    }
}
