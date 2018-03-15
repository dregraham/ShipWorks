using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shared.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// DTO for ShippingProfileAndShortcut
    /// </summary>
    [Component]
    public class ShippingProfile : IShippingProfile
    {
        private readonly IShippingProfileLoader profileLoader;
        private readonly IShippingProfileApplicationStrategyFactory strategyFactory;
        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;
        private ShippingProfileEntity shippingProfileEntity;

        /// <summary>
        /// Constructor used when we don't have an existing ShippingProfileEntity or ShortcutEntity
        /// </summary>
        public ShippingProfile(IShippingProfileLoader profileLoader,
            IShippingProfileApplicationStrategyFactory strategyFactory,
            IShippingManager shippingManager, IMessenger messenger)
        {
            this.profileLoader = profileLoader;
            this.strategyFactory = strategyFactory;
            this.shippingManager = shippingManager;
            this.messenger = messenger;
            ShippingProfileEntity = new ShippingProfileEntity
            {
                Name = string.Empty,
                ShipmentTypePrimary = false
            };

            Shortcut = new ShortcutEntity
            {
                Action = (int) KeyboardShortcutCommand.ApplyProfile
            };

            profileLoader.LoadProfileData(ShippingProfileEntity, false);
        }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingProfileEntity ShippingProfileEntity
        {
            get => shippingProfileEntity;
            set
            {
                profileLoader.LoadProfileData(value, true);
                shippingProfileEntity = value;
            }
        }

        /// <summary>
        /// Shortcut
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShortcutEntity Shortcut { get; set; }

        /// <summary>
        /// The associated Shortcut 
        /// </summary>
        /// <remarks>
        /// This is the description of the ShortcutKey. Blank if no associated keyboard shortcut
        /// </remarks>
        [Obfuscation(Exclude = true)]
        public string ShortcutKey => Shortcut?.Hotkey != null ? EnumHelper.GetDescription(Shortcut.Hotkey) : string.Empty;

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
                         s.ShortcutID != Shortcut.ShortcutID && s.Barcode == Shortcut.Barcode))
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

            profileLoader.LoadProfileData(ShippingProfileEntity, true);
        }

        /// <summary>
        /// Apply profile to shipment
        /// </summary>
        public void Apply(ShipmentEntity shipment)
            => Apply(new List<ShipmentEntity> { shipment });

        /// <summary>
        /// Apply profile to shipments
        /// </summary>
        public void Apply(List<ShipmentEntity> shipments)
        {
            List<ShipmentEntity> originalShipments = shipments.Select(s => EntityUtility.CloneEntity(s, false)).ToList();

            foreach (ShipmentEntity shipment in shipments)
            {
                if (ShippingProfileEntity.ShipmentType != null &&
                    shipment.ShipmentTypeCode != ShippingProfileEntity.ShipmentType.Value)
                {
                    shippingManager.ChangeShipmentType(ShippingProfileEntity.ShipmentType.Value, shipment);
                }

                IShippingProfileApplicationStrategy strategy = strategyFactory.Create(ShippingProfileEntity.ShipmentType);
                strategy.ApplyProfile(ShippingProfileEntity, shipment);
            }

            messenger.Send(new ProfileAppliedMessage(this, originalShipments, shipments));
        }
    }
}