using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;
using ShipWorks.Templates.Printing;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// DTO for ShippingProfileAndShortcut
    /// </summary>
    [Component]
    public class ShippingProfile : IShippingProfile
    {
        private readonly IShippingProfileApplicationStrategyFactory strategyFactory;
        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;
        private readonly Func<ISecurityContext> securityContext;

        /// <summary>
        /// Constructor used when we don't have an existing ShippingProfileEntity or ShortcutEntity
        /// </summary>
        public ShippingProfile(
            IShippingProfileEntity profile,
            IShortcutEntity shortcut,
            IShippingProfileApplicationStrategyFactory strategyFactory,
            IShippingManager shippingManager,
            IMessenger messenger,
            Func<ISecurityContext> securityContext)
        {
            this.strategyFactory = strategyFactory;
            this.shippingManager = shippingManager;
            this.messenger = messenger;
            this.securityContext = securityContext;

            ShippingProfileEntity = profile;
            Shortcut = shortcut;
        }

        /// <summary>
        /// Shipping Profile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IShippingProfileEntity ShippingProfileEntity { get; }

        /// <summary>
        /// Shortcut
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IShortcutEntity Shortcut { get; }

        /// <summary>
        /// Does this profile have a shortcut or barcode
        /// </summary>
        public bool HasShortcutOrBarcode =>
            !string.IsNullOrWhiteSpace(Shortcut.Barcode) || !string.IsNullOrWhiteSpace(ShortcutKey);

        /// <summary>
        /// The associated Shortcut
        /// </summary>
        /// <remarks>
        /// This is the description of the ShortcutKey. Blank if no associated keyboard shortcut
        /// </remarks>
        private string ShortcutKey =>
            Shortcut?.VirtualKey != null && Shortcut.ModifierKeys != null ?
                new KeyboardShortcutData(Shortcut).ShortcutText :
                string.Empty;

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
        /// Get a printable barcode of this profile
        /// </summary>
        public PrintableBarcode ToPrintableBarcode() =>
            new PrintableBarcode(ShippingProfileEntity.Name, Shortcut.Barcode, ShortcutKey);

        /// <summary>
        /// Check to see if the profile can be applied
        /// </summary>
        private bool CanApply(IEnumerable<ShipmentEntity> shipments) =>
            shipments.All(s => securityContext().HasPermission(PermissionType.ShipmentsCreateEditProcess, s.OrderID)) &&
                shipments.All(s => IsApplicable(s.ShipmentTypeCode));
    }
}