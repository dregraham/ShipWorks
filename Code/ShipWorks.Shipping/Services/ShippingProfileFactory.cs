using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
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
        private readonly IShippingProfileLoader profileLoader;
        private readonly IShippingProfileApplicationStrategyFactory strategyFactory;
        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;
        private readonly ISecurityContext securityContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileFactory(IShippingProfileLoader profileLoader,
            IShippingProfileApplicationStrategyFactory strategyFactory,
            IShippingManager shippingManager,
            IMessenger messenger, 
            ISecurityContext securityContext)
        {
            this.profileLoader = profileLoader;
            this.strategyFactory = strategyFactory;
            this.shippingManager = shippingManager;
            this.messenger = messenger;
            this.securityContext = securityContext;
        }

        /// <summary>
        /// Creates a new ShippingProfile with a new ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IShippingProfile Create() => new ShippingProfile(profileLoader, strategyFactory, shippingManager, messenger, securityContext);

        /// <summary>
        /// Creates a ShippingProfile with an existing ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IShippingProfile Create(ShippingProfileEntity shippingProfileEntity, ShortcutEntity shortcut) =>
            new ShippingProfile(profileLoader, strategyFactory, shippingManager, messenger, securityContext)
            {
                ShippingProfileEntity = shippingProfileEntity,
                Shortcut = shortcut,
            };
    }
}