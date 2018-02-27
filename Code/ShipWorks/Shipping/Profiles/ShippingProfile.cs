using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

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
        public ShippingProfile(ShippingProfileEntity shippingProfileEntity, ShortcutEntity shortcut)
        {
            ShippingProfileEntity = shippingProfileEntity;
            Shortcut = shortcut;
        }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        public ShippingProfileEntity ShippingProfileEntity { get; }

        /// <summary>
        /// Shortcut
        /// </summary>
        public ShortcutEntity Shortcut { get; }

        /// <summary>
        /// The associated Shortcut 
        /// </summary>
        /// <remarks>
        /// This is the description of the ShortcutKey. Blank if no associated keyboard shortcut
        /// </remarks>
        public string ShortcutKey
        {
            get => Shortcut?.Hotkey != null ? EnumHelper.GetDescription(Shortcut.Hotkey) : string.Empty;
        }
        /// <summary>
        /// The associated ShipmentType description. Blank if global
        /// </summary>
        public string ShipmentTypeDescription
        {
            get => ShippingProfileEntity?.ShipmentType != null ? EnumHelper.GetDescription(ShippingProfileEntity.ShipmentType) : string.Empty;
        }
    }
}