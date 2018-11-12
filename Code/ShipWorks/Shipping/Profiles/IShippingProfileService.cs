using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityInterfaces;

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
        IEditableShippingProfile CreateEmptyShippingProfile();

        /// <summary>
        /// Get the available hotkeys for the given ShippingProfile
        /// </summary>
        IEnumerable<KeyboardShortcutData> GetAvailableHotkeys(IEditableShippingProfile shippingProfile);

        /// <summary>
        /// Get the configured shipment types profiles
        /// </summary>
        IEnumerable<IShippingProfile> GetConfiguredShipmentTypeProfiles();

        /// <summary>
        /// Get the configured shipment types profiles
        /// </summary>
        IEnumerable<IEditableShippingProfile> GetEditableConfiguredShipmentTypeProfiles();

        /// <summary>
        /// Get the ShippingProfileEntities corresponding ShippingProfile
        /// </summary>
        IEditableShippingProfile GetEditable(long shippingProfileEntityId);

        /// <summary>
        /// Get the shipping profile
        /// </summary>
        IShippingProfile Get(long profileId);

        /// <summary>
        /// Get the ShippingProfileEntities corresponding ShippingProfile
        /// </summary>
        IShippingProfile Get(IShippingProfileEntity profile);

        /// <summary>
        /// Delete the ShippingProfile and its children
        /// </summary>
        Result Delete(IEditableShippingProfile shippingProfile);

        /// <summary>
        /// Save the ShippingProfile and its children
        /// </summary>
        Result Save(IEditableShippingProfile shippingProfile);
    }
}
