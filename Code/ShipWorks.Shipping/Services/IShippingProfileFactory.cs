using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
        IEditableShippingProfile CreateEditable();

        /// <summary>
        /// Creates a ShippingProfile with an existing ShippingProfileEntity and ShortcutEntity
        /// </summary>
        IEditableShippingProfile CreateEditable(ShippingProfileEntity shippingProfileEntity, ShortcutEntity shortcut);

        /// <summary>
        /// Create a profile that can be applied
        /// </summary>
        IShippingProfile Create(IShippingProfileEntity profile, IShortcutEntity shortcut);
    }
}