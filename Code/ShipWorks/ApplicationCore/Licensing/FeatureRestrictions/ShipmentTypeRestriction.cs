﻿using System.Linq;
using ShipWorks.Editions;
using ShipWorks.Shipping;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    /// <summary>
    /// ShipmentType restriction
    /// </summary>
    public class ShipmentTypeRestriction : FeatureRestriction
    {
        /// <summary>
        /// Works on the ShipmentType edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.ShipmentType;

        /// <summary>
        /// Checks to see if the given ShipmentTypeCode is restricted
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
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
    }
}