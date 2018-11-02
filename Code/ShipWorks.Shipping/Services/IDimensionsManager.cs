using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

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
        IEnumerable<IDimensionsProfileEntity> ProfilesReadOnly(IPackageAdapter defaultPackageAdapter);

        /// <summary>
        /// Get the profile with the specified ID, or null if not found.
        /// </summary>
        IDimensionsProfileEntity GetProfileReadOnly(long dimensionsProfileID);
    }
}
