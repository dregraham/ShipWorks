using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

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
        public ShippingProfileAndShortcut(ShippingProfileEntity shippingProfile, IShortcutEntity shortcut)
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
        [Obfuscation(Exclude = true)]
        public string ShortcutKey { get; }

        /// <summary>
        /// The associated ShipmentType description. Blank if global
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ShipmentTypeDescription { get; }
    }
}