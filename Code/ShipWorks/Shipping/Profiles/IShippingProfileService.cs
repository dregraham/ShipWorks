using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Interface for Service that manages ShippingProfiles and their shortcuts
    /// </summary>
    public interface IShippingProfileService
    {
        /// <summary>
        /// Create an empty ShippingProfile
        /// </summary>
        IShippingProfile CreateEmptyShippingProfile();

        /// <summary>
        /// Get the available hotkeys for the given ShippingProfile
        /// </summary>
        IEnumerable<KeyboardShortcutData> GetAvailableHotkeys(IShippingProfile shippingProfile);

        /// <summary>
        /// Get the configured shipment types profiles
        /// </summary>
        IEnumerable<IShippingProfile> GetConfiguredShipmentTypeProfiles();

        /// <summary>
        /// Get the ShippingProfileEntities corrisponding ShippingProfile
        /// </summary>
        IShippingProfile Get(long shippingProfileEntityId);

        /// <summary>
        /// Delete the ShippingProfile and its children
        /// </summary>
        Result Delete(IShippingProfile shippingProfile);

        /// <summary>
        /// Save the ShippingProfile and its children
        /// </summary>
        Result Save(IShippingProfile shippingProfile);
    }
}
