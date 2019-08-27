using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Factory for creating ShippingProfile
    /// </summary>
    [Component]
    public class ShippingProfileFactory : IShippingProfileFactory
    {
        private readonly Func<IEditableShippingProfileRepository> shippingProfileRepository;
        private readonly Func<IShippingProfileEntity, IShortcutEntity, IShippingProfile> createShippingProfile;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileFactory(Func<IEditableShippingProfileRepository> shippingProfileRepository,
            Func<IShippingProfileEntity, IShortcutEntity, IShippingProfile> createShippingProfile)
        {
            this.shippingProfileRepository = shippingProfileRepository;
            this.createShippingProfile = createShippingProfile;
        }

        /// <summary>
        /// Creates a new ShippingProfile with a new ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IEditableShippingProfile CreateEditable()
        {
            ShippingProfileEntity shippingProfileEntity = new ShippingProfileEntity
            {
                Name = string.Empty,
                ShipmentTypePrimary = false
            };

            ShortcutEntity shortcut = new ShortcutEntity
            {
                Action = KeyboardShortcutCommand.ApplyProfile
            };

            IEditableShippingProfile profile = CreateEditable(shippingProfileEntity, shortcut);

            shippingProfileRepository().Load(profile, false);

            return profile;
        }

        /// <summary>
        /// Creates a ShippingProfile with an existing ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IEditableShippingProfile CreateEditable(ShippingProfileEntity profile, ShortcutEntity shortcut) =>
            new EditableShippingProfile(profile, shortcut, shippingProfileRepository());

        /// <summary>
        /// Create a profile that can be applied to a shipment
        /// </summary>
        public IShippingProfile Create(IShippingProfileEntity profile, IShortcutEntity shortcut) =>
            createShippingProfile(profile, shortcut);
    }
}