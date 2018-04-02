﻿using System.Collections.Generic;
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
        ShippingProfileEntity ShippingProfileEntity { get; set; }

        /// <summary>
        /// Shortcut
        /// </summary>
        ShortcutEntity Shortcut { get; set; }

        /// <summary>
        /// The associated Shortcut 
        /// </summary>
        /// <remarks>
        /// This is the description of the ShortcutKey. Blank if no associated keyboard shortcut
        /// </remarks>
        string ShortcutKey { get; }

        /// <summary>
        /// The barcode to apply the profile
        /// </summary>
        string Barcode { get; }

        /// <summary>
        /// The profiles keyboard shortcut
        /// </summary>
        KeyboardShortcutData KeyboardShortcut { get; }

        /// <summary>
        /// Apply profile to shipments
        /// </summary>
        IEnumerable<ICarrierShipmentAdapter> Apply(IEnumerable<ShipmentEntity> shipment);

        /// <summary>
        /// Is the profile applicable to the given ShipmentTypeCode
        /// </summary>
        bool IsApplicable(ShipmentTypeCode? shipmentTypeCode);

        /// <summary>
        /// Apply profile to shipment
        /// </summary>
        ICarrierShipmentAdapter Apply(ShipmentEntity shipment);

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