using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;
using log4net;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service for managing ShippingProfiles and their shortcuts
    /// </summary>
    [Component]
    public class ShippingProfileService : IShippingProfileService
    {
        private readonly IShortcutManager shortcutManager;
        private readonly IShippingProfileRepository shippingProfileRepository;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly Func<IShippingProfile> shippingProfileFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileService(IShortcutManager shortcutManager,
            IShippingProfileRepository shippingProfileRepository,
            IShipmentTypeManager shipmentTypeManager,
            Func<IShippingProfile> shippingProfileFactory)
        {
            this.shortcutManager = shortcutManager;
            this.shippingProfileRepository = shippingProfileRepository;
            this.shipmentTypeManager = shipmentTypeManager;
            this.shippingProfileFactory = shippingProfileFactory;
        }

        /// <summary>
        /// Get the configured shipment types profiles
        /// </summary>
        public IEnumerable<IShippingProfile> GetConfiguredShipmentTypeProfiles() => 
            shippingProfileRepository.GetAll().Where(ConfiguredShipmentTypeProfile);

        /// <summary>
        /// Returns true if should show in grid
        /// </summary>
        private bool ConfiguredShipmentTypeProfile(IShippingProfile shippingProfile)
        {
            ShipmentTypeCode? shipmentType = shippingProfile.ShippingProfileEntity.ShipmentType;

            // Return true if glbal profile or the shipment type is configured
            return !shipmentType.HasValue || shipmentTypeManager.ConfiguredShipmentTypeCodes.Contains(shipmentType.Value);
        }

        /// <summary>
        /// Get the available hotkeys for the given ShippingProfile
        /// </summary>
        public IEnumerable<KeyboardShortcutData> GetAvailableHotkeys(IShippingProfile shippingProfile)
        {
            IList<KeyboardShortcutData> availableHotkeys = shortcutManager.GetAvailableHotkeys().ToList();
            KeyboardShortcutData profilesKeyboardShortcut = shippingProfile.KeyboardShortcut;
            if ((profilesKeyboardShortcut?.Modifiers.HasValue ?? false) && (profilesKeyboardShortcut?.ActionKey.HasValue ?? false))
            {
                availableHotkeys.Add(profilesKeyboardShortcut);
            }

            return availableHotkeys;
        }

        /// <summary>
        /// Get the shipping profile
        /// </summary>
        public IShippingProfile Get(long shippingProfileEntityId) =>
            shippingProfileRepository.Get(shippingProfileEntityId);
        
        /// <summary>
        /// Delete the shipping profile
        /// </summary>
        public Result Delete(IShippingProfile shippingProfile) => 
            shippingProfileRepository.Delete(shippingProfile);

        /// <summary>
        /// Save the shipping profile
        /// </summary>
        public Result Save(IShippingProfile shippingProfile) =>
            shippingProfileRepository.Save(shippingProfile);
        
        /// <summary>
        /// Create an empty ShippingProfile
        /// </summary>
        public IShippingProfile CreateEmptyShippingProfile() =>
            shippingProfileFactory();
    }
}
