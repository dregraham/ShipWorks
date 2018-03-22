using System.Collections.Generic;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Represents a ShippingProfile
    /// </summary>
    public interface IShippingProfile
    {
        /// <summary>
        /// The associated ShipmentType description. Blank if global
        /// </summary>
        string ShipmentTypeDescription { get; }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        ShippingProfileEntity ShippingProfileEntity { get; }

        /// <summary>
        /// Shortcut
        /// </summary>
        ShortcutEntity Shortcut { get; }

        /// <summary>
        /// The associated Shortcut 
        /// </summary>
        /// <remarks>
        /// This is the description of the ShortcutKey. Blank if no associated keyboard shortcut
        /// </remarks>
        string ShortcutKey { get; }

        /// <summary>
        /// Apply profile to shipments
        /// </summary>
        IEnumerable<ICarrierShipmentAdapter> Apply(IEnumerable<ShipmentEntity> shipment);

        /// <summary>
        /// Apply profile to shipment
        /// </summary>
        ICarrierShipmentAdapter Apply(ShipmentEntity shipment);

        /// <summary>
        /// Change the keybaord shortcut
        /// </summary>
        void ChangeKeyboardShortcut(KeyboardShortcutData keyboardShortcut);

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