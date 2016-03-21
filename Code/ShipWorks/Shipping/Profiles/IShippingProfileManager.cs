using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Manages shipment profiles
    /// </summary>
    public interface IShippingProfileManager
    {
        /// <summary>
        /// Get the default profile for the given shipment type
        /// </summary>
        ShippingProfileEntity GetDefaultProfile(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Gets or creates a profile with the default settings for the shipment type
        /// </summary>
        ShippingProfileEntity GetOrCreatePrimaryProfile(ShipmentType shipmentType);

        /// <summary>
        /// Get profiles for the given shipment type
        /// </summary>
        IEnumerable<ShippingProfileEntity> GetProfilesFor(ShipmentTypeCode value);
    }
}