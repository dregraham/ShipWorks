using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Factory for creating ShippingProfile
    /// </summary>
    [Component]
    public class ShippingProfileFactory : IShippingProfileFactory
    {
        private readonly IShippingProfileLoader profileLoader;
        private readonly IShippingProfileApplicationStrategyFactory strategyFactory;
        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileFactory(IShippingProfileLoader profileLoader,
            IShippingProfileApplicationStrategyFactory strategyFactory,
            IShippingManager shippingManager,
            IMessenger messenger)
        {
            this.profileLoader = profileLoader;
            this.strategyFactory = strategyFactory;
            this.shippingManager = shippingManager;
            this.messenger = messenger;
        }

        /// <summary>
        /// Creates a new ShippingProfile with a new ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IShippingProfile Create() => new ShippingProfile(profileLoader, strategyFactory, shippingManager, messenger);

        /// <summary>
        /// Creates a ShippingProfile with an existing ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IShippingProfile Create(ShippingProfileEntity shippingProfileEntity, ShortcutEntity shortcut) =>
            new ShippingProfile(profileLoader, strategyFactory, shippingManager, messenger)
            {
                ShippingProfileEntity = shippingProfileEntity,
                Shortcut = shortcut,
            };
    }
}