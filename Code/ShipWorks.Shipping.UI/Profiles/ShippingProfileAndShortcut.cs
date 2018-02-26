using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.UI.Profiles
{
    /// <summary>
    /// DTO for ShippingProfileAndShortcut
    /// </summary>
    public class ShippingProfileAndShortcut
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileAndShortcut(ShippingProfileEntity shippingProfile, ShortcutEntity shortcut)
        {
            ShippingProfile = shippingProfile;

            string shortcutText = string.Empty;
            if (shortcut?.Hotkey != null)
            {
                shortcutText = EnumHelper.GetDescription(shortcut.Hotkey);
            }

            ShortcutKey = shortcutText;

            string shipmentTypeDescription = string.Empty;
            if (shippingProfile.ShipmentType != null)
            {
                shipmentTypeDescription = EnumHelper.GetDescription(shippingProfile.ShipmentType);
            }
            ShipmentTypeDescription = shipmentTypeDescription;
        }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        public ShippingProfileEntity ShippingProfile { get; set; }

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