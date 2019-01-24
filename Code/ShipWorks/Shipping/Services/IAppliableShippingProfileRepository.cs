using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Repository for profiles that can be applied to shipments
    /// </summary>
    public interface IShippingProfileRepository
    {
        /// <summary>
        /// Get the ShippingProfileEntities corresponding ShippingProfile
        /// </summary>
        IShippingProfile Get(long profileId);

        /// <summary>
        /// Get the ShippingProfileEntities corresponding ShippingProfile
        /// </summary>
        IShippingProfile Get(IShippingProfileEntity profile);

        /// <summary>
        /// Get all of the ShippingProfiles
        /// </summary>
        IEnumerable<IShippingProfile> GetAll();
    }
}