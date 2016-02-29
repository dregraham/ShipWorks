using System.Text.RegularExpressions;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    /// <summary>
    /// Restricts Postal shipments to Apo Fpo and PoBox
    /// </summary>
    public class PostalApoFpoPoboxOnlyRestriction : FeatureRestriction
    {
        /// <summary>
        /// Returns the Edition Feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.PostalApoFpoPoboxOnly;

        /// <summary>
        /// Checks to see if we should restrict postal shipments to just APO FPO or PoBox
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            ShipmentEntity shipment = data as ShipmentEntity;
    
            // For some reason the data passed is not a shipment
            if (shipment == null)
            {
                return EditionRestrictionLevel.None;
            }

            // Not a postal shipment so we dont care
            if (!PostalUtility.IsPostalShipmentType((ShipmentTypeCode)shipment.ShipmentType))
            {
                return EditionRestrictionLevel.None;
            }

            // APO/FPO shipments are allowed
            if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
            {
                return EditionRestrictionLevel.None;
            }

            // PoBox shipments are allowed
            if (Regex.Match(shipment.ShipStreet1 + " " + shipment.ShipStreet2 + " " + shipment.ShipStreet3, "P.{0,2}O.{0,2}[ ]?Box", RegexOptions.IgnoreCase).Success)
            {
                return EditionRestrictionLevel.None;
            }

            // All services are allowed
            if (capabilities.PostalAvailability == BrownPostalAvailability.AllServices)
            {
                return EditionRestrictionLevel.None;
            }

            return EditionRestrictionLevel.Forbidden;
        }
    }
}