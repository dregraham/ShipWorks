using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Wraps the static ShippingProfileManager with an instance that implements an interface
    /// </summary>
    public class ShippingProfileManagerWrapper : IShippingProfileManager
    {
        /// <summary>
        /// Get the default profile for the given shipment type
        /// </summary>
        public ShippingProfileEntity GetDefaultProfile(ShipmentTypeCode shipmentTypeCode)
        {
            return ShippingProfileManager.GetDefaultProfile(shipmentTypeCode);
        }

        /// <summary>
        /// Saves the given profile
        /// </summary>
        public void SaveProfile(ShippingProfileEntity profile)
        {
            ShippingProfileManager.SaveProfile(profile);
        }
    }
}