using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// represents a ShippingProfile repository
    /// </summary>
    public interface IShippingProfileRepository
    {
        /// <summary>
        /// Delete the ShippingProfile and its children
        /// </summary>
        Result Delete(IShippingProfile shippingProfile);

        /// <summary>
        /// Get the ShippingProfileEntities corrisponding ShippingProfile
        /// </summary>
        IShippingProfile Get(long shippingProfileEntityId);

        /// <summary>
        /// Get all of the ShippingProfiles
        /// </summary>
        IEnumerable<IShippingProfile> GetAll();

        /// <summary>
        /// Save the ShippingProfile and its children 
        /// </summary>
        Result Save(IShippingProfile shippingProfile);
    }
}