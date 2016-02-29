using System.Text.RegularExpressions;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class PostalApoFpoPoboxOnlyRestriction : FeatureRestriction
    {
        public override EditionFeature EditionFeature => EditionFeature.PostalApoFpoPoboxOnly;


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

            // Apo Fpo PoBox only and we got this far so the shipment must not be
            // one of the three, return forbidden 
            if (capabilities.PostalAvailability == BrownPostalAvailability.ApoFpoPobox)
            {
                return EditionRestrictionLevel.Forbidden;
            }


            if (capabilities.PostalAvailability == BrownPostalAvailability.None)
            {
                return EditionRestrictionLevel.Forbidden;
            }

            return EditionRestrictionLevel.Forbidden;
            
        }
    }
}