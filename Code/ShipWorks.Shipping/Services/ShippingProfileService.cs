using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityInterfaces;
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
        private readonly IEditableShippingProfileRepository editableShippingProfileRepository;
        private readonly IShippingProfileRepository shippingProfileRepository;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShippingProfileFactory shippingProfileFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileService(
            IShortcutManager shortcutManager,
            IEditableShippingProfileRepository editableShippingProfileRepository,
            IShippingProfileRepository shippingProfileRepository,
            IShipmentTypeManager shipmentTypeManager,
            IShippingProfileFactory shippingProfileFactory)
        {
            this.shortcutManager = shortcutManager;
            this.editableShippingProfileRepository = editableShippingProfileRepository;
            this.shippingProfileRepository = shippingProfileRepository;
            this.shipmentTypeManager = shipmentTypeManager;
            this.shippingProfileFactory = shippingProfileFactory;
        }

        /// <summary>
        /// Get the configured shipment types profiles
        /// </summary>
        public IEnumerable<IShippingProfile> GetConfiguredShipmentTypeProfiles() =>
            shippingProfileRepository
                .GetAll()
                .Where(x => ConfiguredShipmentTypeProfile(x.ShippingProfileEntity.ShipmentType));

        /// <summary>
        /// Get the configured shipment types profiles
        /// </summary>
        public IEnumerable<IEditableShippingProfile> GetEditableConfiguredShipmentTypeProfiles() =>
            editableShippingProfileRepository
                .GetAll()
                .Where(x => ConfiguredShipmentTypeProfile(x.ShippingProfileEntity.ShipmentType));

        /// <summary>
        /// Returns true if should show in grid
        /// </summary>
        /// <returns>Return true if global profile or the shipment type is configured</returns>
        private bool ConfiguredShipmentTypeProfile(ShipmentTypeCode? shipmentType) =>
            !shipmentType.HasValue || shipmentTypeManager.ConfiguredShipmentTypeCodes.Contains(shipmentType.Value);

        /// <summary>
        /// Get the available hotkeys for the given ShippingProfile
        /// </summary>
        public IEnumerable<KeyboardShortcutData> GetAvailableHotkeys(IEditableShippingProfile shippingProfile)
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
        public IShippingProfile Get(long profileId) =>
            shippingProfileRepository.Get(profileId);

        /// <summary>
        /// Get the shipping profile
        /// </summary>
        public IShippingProfile Get(IShippingProfileEntity profile) =>
            shippingProfileRepository.Get(profile);

        /// <summary>
        /// Get the shipping profile
        /// </summary>
        public IEditableShippingProfile GetEditable(long shippingProfileEntityId) =>
            editableShippingProfileRepository.Get(shippingProfileEntityId);

        /// <summary>
        /// Delete the shipping profile
        /// </summary>
        public Result Delete(IEditableShippingProfile shippingProfile) =>
            editableShippingProfileRepository.Delete(shippingProfile);

        /// <summary>
        /// Save the shipping profile
        /// </summary>
        public Result Save(IEditableShippingProfile shippingProfile) =>
            editableShippingProfileRepository.Save(shippingProfile);

        /// <summary>
        /// Create an empty ShippingProfile
        /// </summary>
        public IEditableShippingProfile CreateEmptyShippingProfile() =>
            shippingProfileFactory.CreateEditable();
    }
}
