using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.IO.KeyboardShortcuts;

namespace ShipWorks.Shipping.Profiles
{
    public interface IShippingProfileService
    {
        /// <summary>
        /// Get all of the ShippingProfiles
        /// </summary>
        /// <returns></returns>
        IEnumerable<ShippingProfile> GetAll();

        /// <summary>
        /// Get the ShippingProfileEntities corrisponding ShippingProfile
        /// </summary>
        ShippingProfile Get(long shippingProfileEntityId);

        /// <summary>
        /// Create an empty ShippingProfile
        /// </summary>
        ShippingProfile Create();

        /// <summary>
        /// Save the ShippingProfile and its children 
        /// </summary>
        Result Save(ShippingProfile shippingProfile);

        /// <summary>
        /// Delete the ShippingProfile and its children
        /// </summary>
        Result Delete(ShippingProfile shippingProfile);

        /// <summary>
        /// Get the available hotkeys for the given ShippingProfile
        /// </summary>
        IEnumerable<Hotkey> GetAvailableHotKeys(ShippingProfile shippingProfile);
    }
}
