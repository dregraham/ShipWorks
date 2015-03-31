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
    }
}