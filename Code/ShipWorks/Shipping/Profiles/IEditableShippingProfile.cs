using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Editable version of the shipping profile
    /// </summary>
    public interface IEditableShippingProfile
    {
        /// <summary>
        /// The barcode to apply the profile
        /// </summary>
        string Barcode { get; }

        /// <summary>
        /// The profiles keyboard shortcut
        /// </summary>
        KeyboardShortcutData KeyboardShortcut { get; }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        ShippingProfileEntity ShippingProfileEntity { get; }

        /// <summary>
        /// Shortcut
        /// </summary>
        ShortcutEntity Shortcut { get; }

        /// <summary>
        /// Change the shortcut for the profile
        /// </summary>
        void ChangeShortcut(KeyboardShortcutData keyboardShortcut, string barcode);

        /// <summary>
        /// Change profile to be of specified ShipmentType
        /// </summary>
        void ChangeProvider(ShipmentTypeCode? shipmentType);

        /// <summary>
        /// Validate that the shippintProfile can be saved
        /// </summary>
        Result Validate(IShippingProfileManager profileManager, IShortcutManager shortcutManager);
    }
}