using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Services;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// DTO for ShippingProfileAndShortcut
    /// </summary>
    [Component]
    public class ShippingProfile : IShippingProfile
    {
        private readonly IShippingProfileRepository shippingProfileRepository;
        private readonly IShippingProfileApplicationStrategyFactory strategyFactory;
        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;
        private readonly Func<ISecurityContext> securityContext;

        /// <summary>
        /// Constructor used when we don't have an existing ShippingProfileEntity or ShortcutEntity
        /// </summary>
        public ShippingProfile(
            IShippingProfileRepository shippingProfileRepository, 
            IShippingProfileApplicationStrategyFactory strategyFactory,
            IShippingManager shippingManager,
            IMessenger messenger,
            Func<ISecurityContext> securityContext)
        {
            this.shippingProfileRepository = shippingProfileRepository;
            this.strategyFactory = strategyFactory;
            this.shippingManager = shippingManager;
            this.messenger = messenger;
            this.securityContext = securityContext;
        }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingProfileEntity ShippingProfileEntity { get; set; }

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

            if (CanApply(shipmentList))
            {
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
            }
            
            return shipmentList.Select(s => shippingManager.GetShipmentAdapter(s));
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

        /// <summary>
        /// Is the profile applicable to the Shipment
        /// </summary>
        public bool IsApplicable(ShipmentTypeCode? shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.None:
                    return ShippingProfileEntity.ShipmentType != null;
                case ShipmentTypeCode.Amazon:
                    return ShippingProfileEntity.ShipmentType == null || ShippingProfileEntity.ShipmentType == ShipmentTypeCode.Amazon;
                default:
                    return ShippingProfileEntity.ShipmentType != ShipmentTypeCode.Amazon;
            }
        }

        /// <summary>
        /// Check to see if the profile can be applied
        /// </summary>
        private bool CanApply(IEnumerable<ShipmentEntity> shipments)
            => shipments.All(s => securityContext().HasPermission(PermissionType.ShipmentsCreateEditProcess, s.OrderID)) && 
                shipments.All(s => IsApplicable(s.ShipmentTypeCode));
    }
}