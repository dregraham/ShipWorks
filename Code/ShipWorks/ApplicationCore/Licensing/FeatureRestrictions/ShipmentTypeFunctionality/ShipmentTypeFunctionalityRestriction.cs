using System.Linq;
using ShipWorks.Editions;
using ShipWorks.Shipping;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.ShipmentTypeFunctionality
{
    /// <summary>
    /// An abstract class intended to be the base class for restrictions related to 
    /// the shipment type functionality of the ILicenseCapabilities.
    /// </summary>
    /// <seealso cref="ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.FeatureRestriction" />
    public abstract class ShipmentTypeFunctionalityRestriction : FeatureRestriction
    {
        private readonly ShipmentTypeRestrictionType restrictionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentTypeFunctionalityRestriction"/> class.
        /// </summary>
        /// <param name="restrictionType">The type of shipment type restriction that should be checked.</param>
        protected ShipmentTypeFunctionalityRestriction(ShipmentTypeRestrictionType restrictionType)
        {
            this.restrictionType = restrictionType;
        }

        /// <summary>
        /// Checks the license capabilities to see account conversion for the given shipment type is restricted.
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            ShipmentTypeCode shipmentType = data as ShipmentTypeCode? ?? ShipmentTypeCode.None;

            if (shipmentType != ShipmentTypeCode.None &&
                capabilities.ShipmentTypeRestriction.ContainsKey(shipmentType) &&
                capabilities.ShipmentTypeRestriction[shipmentType].Contains(restrictionType))
            {
                return EditionRestrictionLevel.Forbidden;
            }

            return EditionRestrictionLevel.None;
        }
    }
}
