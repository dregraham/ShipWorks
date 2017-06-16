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
    public class DryIceSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureSurcharge"/> class.
        /// </summary>
        public DryIceSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the surcharge to the service rate based on the shipment
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            int dryIcePackages = shipment.Packages.Count(ChargeApplies);

            if (dryIcePackages > 0)
            {
                decimal dryIceRate = (decimal) surcharges[UpsSurchargeType.DryIce];
                decimal surcharge = dryIceRate * dryIcePackages;

                serviceRate.AddAmount(surcharge, $"{dryIcePackages} Package(s) with dry ice");
            }
        }

        /// <summary>
        /// Returns true if charge applies
        /// </summary>
        private static bool ChargeApplies(UpsPackageEntity package)
        {
            bool enabled = package.DryIceEnabled;
            UpsDryIceRegulationSet regulationSet = (UpsDryIceRegulationSet)package.DryIceRegulationSet;
            bool forMedical = package.DryIceIsForMedicalUse;
            double weight = package.DryIceWeight;

            if (enabled && regulationSet == UpsDryIceRegulationSet.Cfr && !forMedical && weight > 5.5)
            {
                return true;
            }

            if (enabled && regulationSet == UpsDryIceRegulationSet.Iata)
            {
                return true;
            }

            return false;
        }
    }
}
