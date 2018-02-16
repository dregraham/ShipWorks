using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Interface for loading and saving profile data
    /// </summary>
    [Service(SingleInstance = true)]
    public interface IShippingProfileLoader
    {
        /// <summary>
        /// Load the given profile
        /// </summary>
        void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent);
    }
}