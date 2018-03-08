using System.Linq;
using System.Reflection;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfile(ShippingProfileEntity shippingProfileEntity, 
            ShortcutEntity shortcut,
            IShippingProfileLoader profileLoader,
            IShippingProfileApplicationStrategyFactory strategyFactory)
        {
            this.profileLoader = profileLoader;
            this.strategyFactory = strategyFactory;
            ShippingProfileEntity = shippingProfileEntity;
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
        {
            IShippingProfileApplicationStrategy strategy = strategyFactory.Create(ShippingProfileEntity.ShipmentType);
            strategy.ApplyProfile(ShippingProfileEntity, shipment);
        }

        public void LoadProfileData(bool refreshIfPresent)
        {
            profileLoader.LoadProfileData(ShippingProfileEntity, refreshIfPresent);
        }
    }
}