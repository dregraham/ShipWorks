using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
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
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private ShippingProfileEntity shippingProfileEntity;

        /// <summary>
        /// Constructor used when we don't have an existing ShippingProfileEntity or ShortcutEntity
        /// </summary>
        public ShippingProfile(IShippingProfileLoader profileLoader,
            IShippingProfileApplicationStrategyFactory strategyFactory,
            IShippingManager shippingManager, IMessenger messenger,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory)
        {
            this.profileLoader = profileLoader;
            this.strategyFactory = strategyFactory;
            this.shippingManager = shippingManager;
            this.messenger = messenger;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            ShippingProfileEntity = new ShippingProfileEntity
            {
                Name = string.Empty,
                ShipmentTypePrimary = false
            };

            Shortcut = new ShortcutEntity
            {
                Action = KeyboardShortcutCommand.ApplyProfile
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
        public string ShortcutKey =>
            Shortcut?.VirtualKey != null && Shortcut.ModifierKeys != null ? 
                new KeyboardShortcutData(null, Shortcut.VirtualKey.Value, Shortcut.ModifierKeys.Value).ShortcutText :
                string.Empty;

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
        public ICarrierShipmentAdapter Apply(ShipmentEntity shipment) =>
            Apply(new[] { shipment }).First();

        /// <summary>
        /// Apply profile to shipments
        /// </summary>
        public IEnumerable<ICarrierShipmentAdapter> Apply(IEnumerable<ShipmentEntity> shipments)
        {
            List<ShipmentEntity> shipmentList = shipments.ToList();
            
            List<ShipmentEntity> originalShipments = shipmentList.Select(s => EntityUtility.CloneEntity(s, false)).ToList();
            IShippingProfileApplicationStrategy strategy = strategyFactory.Create(ShippingProfileEntity.ShipmentType);

            foreach (ShipmentEntity shipment in shipmentList)
            {
                if (ShippingProfileEntity.ShipmentType != null &&
                    shipment.ShipmentTypeCode != ShippingProfileEntity.ShipmentType.Value)
                {
                    shippingManager.ChangeShipmentType(ShippingProfileEntity.ShipmentType.Value, shipment);
                }
                strategy.ApplyProfile(ShippingProfileEntity, shipment);
            }

            messenger.Send(new ProfileAppliedMessage(this, originalShipments, shipmentList));

            return shipmentList.Select(s => shipmentAdapterFactory.Get(s));
        }

        /// <summary>
        /// Change the keybaord shortcut
        /// </summary>
        public void ChangeKeyboardShortcut(KeyboardShortcutData keyboardShortcut)
        {
            Shortcut.VirtualKey = keyboardShortcut?.ActionKey;
            Shortcut.ModifierKeys = keyboardShortcut?.Modifiers;
            Shortcut.Action = KeyboardShortcutCommand.ApplyProfile;
        }
    }
}