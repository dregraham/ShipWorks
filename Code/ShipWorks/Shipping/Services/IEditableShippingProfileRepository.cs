using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// represents a ShippingProfile repository
    /// </summary>
    public interface IEditableShippingProfileRepository
    {
        /// <summary>
        /// Delete the ShippingProfile and its children
        /// </summary>
        Result Delete(IEditableShippingProfile shippingProfile);

        /// <summary>
        /// Get the ShippingProfileEntities corresponding ShippingProfile
        /// </summary>
        IEditableShippingProfile Get(long shippingProfileEntityId);

        /// <summary>
        /// Get all of the ShippingProfiles
        /// </summary>
        IEnumerable<IEditableShippingProfile> GetAll();

        /// <summary>
        /// Save the ShippingProfile and its children
        /// </summary>
        Result Save(IEditableShippingProfile shippingProfile);

        /// <summary>
        /// Load the given profile
        /// </summary>
        void Load(IEditableShippingProfile profile, bool refreshIfPresent);
    }
}