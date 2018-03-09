using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Interface for factory that creates a ShippingProfile
    /// </summary>
    public interface IShippingProfileFactory
    {
        /// <summary>
        /// Creates a new ShippingProfile with a new ShippingProfileEntity and ShortcutEntity
        /// </summary>
        IShippingProfile Create();

        /// <summary>
        /// Creates a ShippingProfile with an existing ShippingProfileEntity and ShortcutEntity
        /// </summary>
        IShippingProfile Create(ShippingProfileEntity shippingProfileEntity, ShortcutEntity shortcut);
    }
}