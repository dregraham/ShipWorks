using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    public interface IShippingProfileFactory
    {
        /// <summary>
        /// Creates a new ShippingProfile
        /// </summary>
        /// <returns></returns>
        IShippingProfile Create();

        /// <summary>
        /// Creates a ShippingProfile
        /// </summary>
        IShippingProfile Create(ShippingProfileEntity shippingProfileEntity, ShortcutEntity shortcut);
    }
}