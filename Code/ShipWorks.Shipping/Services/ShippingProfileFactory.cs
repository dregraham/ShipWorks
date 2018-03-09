using Interapptive.Shared.ComponentRegistration;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileFactory(IShippingProfileLoader profileLoader,
            IShippingProfileApplicationStrategyFactory strategyFactory)
        {
            this.profileLoader = profileLoader;
            this.strategyFactory = strategyFactory;
        }

        /// <summary>
        /// Creates a new ShippingProfile with a new ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IShippingProfile Create() => new ShippingProfile(profileLoader, strategyFactory);
        
        /// <summary>
        /// Creates a ShippingProfile with an existing ShippingProfileEntity and ShortcutEntity
        /// </summary>
        public IShippingProfile Create(ShippingProfileEntity shippingProfileEntity, ShortcutEntity shortcut) =>
            new ShippingProfile(shippingProfileEntity, shortcut, profileLoader, strategyFactory);
    }
}