using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.IO.KeyboardShortcuts;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Interface for Service that manages ShippingProfiles and their shortcuts
    /// </summary>
    public interface IShippingProfileService
    {
        /// <summary>
        /// Get all of the ShippingProfiles
        /// </summary>
        IEnumerable<IShippingProfile> GetAll();

        /// <summary>
        /// Get the ShippingProfileEntities corrisponding ShippingProfile
        /// </summary>
        IShippingProfile Get(long shippingProfileEntityId);

        /// <summary>
        /// Create an empty ShippingProfile
        /// </summary>
        IShippingProfile CreateEmptyShippingProfile();

        /// <summary>
        /// Save the ShippingProfile and its children 
        /// </summary>
        Result Save(IShippingProfile shippingProfile);

        /// <summary>
        /// Delete the ShippingProfile and its children
        /// </summary>
        Result Delete(IShippingProfile shippingProfile);

        /// <summary>
        /// Get the available hotkeys for the given ShippingProfile
        /// </summary>
        IEnumerable<Hotkey> GetAvailableHotkeys(IShippingProfile shippingProfile);
    }
}
