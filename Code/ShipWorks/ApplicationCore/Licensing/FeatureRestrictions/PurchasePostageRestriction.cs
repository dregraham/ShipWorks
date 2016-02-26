using System.Linq;
using ShipWorks.Editions;
using ShipWorks.Shipping;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class PurchasePostageRestriction : FeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.PurchasePostage;

        /// <summary>
        /// Checks the license capabilities to see if purchasing postage for the given shipment type is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            ShipmentTypeCode shipmentType = data as ShipmentTypeCode? ?? ShipmentTypeCode.None;

            if (shipmentType != ShipmentTypeCode.None &&
                capabilities.ShipmentTypeRestriction.ContainsKey(shipmentType) &&
                capabilities.ShipmentTypeRestriction[shipmentType].Contains(ShipmentTypeRestrictionType.Purchasing))
            {
                return EditionRestrictionLevel.Forbidden;
            }

            return EditionRestrictionLevel.None;
        }
    }
}