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
        IEnumerable<Hotkey> GetAvailableHotkeys(ShippingProfile shippingProfile);

        /// <summary>
        /// Load the given profile
        /// </summary>
        void LoadProfileData(ShippingProfile profile, bool refreshIfPresent);
    }
}
