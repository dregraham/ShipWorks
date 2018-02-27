using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// DTO for ShippingProfileAndShortcut
    /// </summary>
    public class ShippingProfile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfile(ShippingProfileEntity shippingProfileEntity, IShortcutEntity shortcut)
        {
            ShippingProfileEntity = shippingProfileEntity;

            string shortcutText = string.Empty;
            if (shortcut?.Hotkey != null)
            {
                shortcutText = EnumHelper.GetDescription(shortcut.Hotkey);
            }

            ShortcutKey = shortcutText;

            string shipmentTypeDescription = string.Empty;
            if (shippingProfileEntity.ShipmentType != null)
            {
                shipmentTypeDescription = EnumHelper.GetDescription(shippingProfileEntity.ShipmentType);
            }
            ShipmentTypeDescription = shipmentTypeDescription;
        }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        public ShippingProfileEntity ShippingProfileEntity { get; set; }

        /// <summary>
        /// The associated Shortcut 
        /// </summary>
        /// <remarks>
        /// This is the description of the ShortcutKey. Blank if no associated keyboard shortcut
        /// </remarks>
        public string ShortcutKey { get; }

        /// <summary>
        /// The associated ShipmentType description. Blank if global
        /// </summary>
        public string ShipmentTypeDescription { get; }
    }
}