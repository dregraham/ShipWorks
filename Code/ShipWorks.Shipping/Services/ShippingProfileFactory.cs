using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Factory for creating ShippingProfile
    /// </summary>
    [Component]
    public class ShippingProfileFactory : IShippingProfileFactory
    {
        private readonly Func<IShippingProfileRepository> shippingProfileRepository;
        private readonly IShippingProfileApplicationStrategyFactory strategyFactory;
        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;
        private readonly Func<ISecurityContext> securityContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileFactory(Func<IShippingProfileRepository> shippingProfileRepository,
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
        /// Creates a new ShippingProfile with a new ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IShippingProfile Create()
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

            IShippingProfile profile = Create(shippingProfileEntity, shortcut);

            shippingProfileRepository().Load(profile, false);

            return profile;
        }

        /// <summary>
        /// Creates a ShippingProfile with an existing ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IShippingProfile Create(ShippingProfileEntity shippingProfileEntity, ShortcutEntity shortcut) =>
            new ShippingProfile(shippingProfileRepository(), strategyFactory, shippingManager, messenger, securityContext)
            {
                ShippingProfileEntity = shippingProfileEntity,
                Shortcut = shortcut,
            };
    }
}