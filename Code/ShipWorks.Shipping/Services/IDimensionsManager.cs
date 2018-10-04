using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Interface for DimensionsManager
    /// </summary>
    public interface IDimensionsManager
    {
        /// <summary>
        /// Return all the dimensions profiles plus the "Enter Dimensions" based on default package adapter.
        /// </summary>
        IEnumerable<DimensionsProfileEntity> Profiles(IPackageAdapter defaultPackageAdapter);

        /// <summary>
        /// Get the profile with the specified ID, or null if not found.
        /// </summary>
        DimensionsProfileEntity GetProfile(long dimensionsProfileID);
    }
}
