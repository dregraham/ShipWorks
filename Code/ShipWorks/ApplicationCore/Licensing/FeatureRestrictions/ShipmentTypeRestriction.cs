using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Editions;
using ShipWorks.Shipping;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    /// <summary>
    /// ShipmentType restriction
    /// </summary>
    public class ShipmentTypeRestriction : IFeatureRestriction
    {
        /// <summary>
        /// Works on the ShipmentType edition feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.ShipmentType;
        
        /// <summary>
        /// Checks to see if the given ShipmentTypeCode is restricted
        /// </summary>
        public EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            ShipmentTypeCode shipmentType = data as ShipmentTypeCode? ?? ShipmentTypeCode.None;

            if (shipmentType != ShipmentTypeCode.None && 
                capabilities.ShipmentTypeRestriction.ContainsKey(shipmentType) &&
                capabilities.ShipmentTypeRestriction[shipmentType].Contains(ShipmentTypeRestrictionType.Disabled))
            {
                return EditionRestrictionLevel.Hidden;
            }

            return EditionRestrictionLevel.None;
        }

        /// <summary>
        /// Handles ShipmentType feature restrictions
        /// </summary>
        /// <remarks>bool if the issue has been resolved</remarks>
        public bool Handle(IWin32Window owner, object data)
        {
            return false;
        }
    }
}