﻿using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// DTO for ShippingProfileAndShortcut
    /// </summary>
    [Component]
    public class EditableShippingProfile : IEditableShippingProfile
    {
        private readonly IEditableShippingProfileRepository shippingProfileRepository;

        /// <summary>
        /// Constructor used when we don't have an existing ShippingProfileEntity or ShortcutEntity
        /// </summary>
        public EditableShippingProfile(ShippingProfileEntity profile, ShortcutEntity shortcut, IEditableShippingProfileRepository shippingProfileRepository)
        {
            this.shippingProfileRepository = shippingProfileRepository;
            ShippingProfileEntity = profile;
            Shortcut = shortcut;
        }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingProfileEntity ShippingProfileEntity { get; }

        /// <summary>
        /// Shortcut
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShortcutEntity Shortcut { get; }

        /// <summary>
        /// The associated Shortcut
        /// </summary>
        /// <remarks>
        /// This is the description of the ShortcutKey. Blank if no associated keyboard shortcut
        /// </remarks>
        [Obfuscation(Exclude = true)]
        public string ShortcutKey =>
            Shortcut?.VirtualKey != null && Shortcut.ModifierKeys != null ?
                new KeyboardShortcutData(Shortcut).ShortcutText :
                string.Empty;

        /// <summary>
        /// The barcode to apply the profile
        /// </summary>
        public string Barcode => Shortcut.Barcode;

        /// <summary>
        /// The profiles keyboard shortcut
        /// </summary>
        public KeyboardShortcutData KeyboardShortcut => new KeyboardShortcutData(Shortcut);

        /// <summary>
        /// The associated ShipmentType description. Blank if global
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ShipmentTypeDescription =>
            ShippingProfileEntity?.ShipmentType != null ?
                EnumHelper.GetDescription(ShippingProfileEntity.ShipmentType) :
                string.Empty;

        /// <summary>
        /// Validate that the shippintProfile can be saved
        /// </summary>
        public Result Validate(IShippingProfileManager profileManager, IShortcutManager shortcutManager)
        {
            Result result = Result.FromSuccess();

            if (string.IsNullOrWhiteSpace(ShippingProfileEntity.Name))
            {
                result = Result.FromError("Enter a name for the profile.");
            }
            else if (profileManager.Profiles.Any(profile =>
                profile.ShippingProfileID != ShippingProfileEntity.ShippingProfileID &&
                profile.Name == ShippingProfileEntity.Name))
            {
                result = Result.FromError("A profile with the chosen name already exists.");
            }
            else if (!Shortcut.Barcode.IsNullOrWhiteSpace() && shortcutManager.Shortcuts.Any(s =>
                         s.ShortcutID != Shortcut.ShortcutID && s.Barcode.Equals(Shortcut.Barcode, System.StringComparison.InvariantCultureIgnoreCase)))
            {
                result = Result.FromError($"The barcode \"{Shortcut.Barcode}\" is already in use.");
            }

            return result;
        }

        /// <summary>
        /// Change profile to be of specified ShipmentType
        /// </summary>
        public void ChangeProvider(ShipmentTypeCode? shipmentType)
        {
            ShippingProfileEntity.ShipmentType = shipmentType;
            ShippingProfileEntity.Packages.Clear();

            shippingProfileRepository.Load(this, true);
        }

        /// <summary>
        /// Change the shortcut for the profile
        /// </summary>
        public void ChangeShortcut(KeyboardShortcutData keyboardShortcut, string barcode)
        {
            Shortcut.VirtualKey = keyboardShortcut?.ActionKey;
            Shortcut.ModifierKeys = keyboardShortcut?.Modifiers;
            Shortcut.Action = KeyboardShortcutCommand.ApplyProfile;

            Shortcut.Barcode = barcode.Trim();
        }
    }
}