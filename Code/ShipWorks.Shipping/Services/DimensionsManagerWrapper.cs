using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wraps the Core DimensionsManager
    /// </summary>
    public class DimensionsManagerWrapper : IDimensionsManager
    {
        /// <summary>
        /// Return all the dimensions profiles plus the "Enter Dimensions" based on default package adapter.
        /// </summary>
        public IEnumerable<IDimensionsProfileEntity> ProfilesReadOnly(IPackageAdapter defaultPackageAdapter) =>
            DimensionsManager.ProfilesReadOnly.Append(CreateProfileFromPackageAdapter(defaultPackageAdapter).AsReadOnly());

        /// <summary>
        /// Get the profile with the specified ID, or null if not found.
        /// </summary>
        public IDimensionsProfileEntity GetProfileReadOnly(long dimensionsProfileID) =>
            DimensionsManager.GetProfileReadOnly(dimensionsProfileID);

        /// <summary>
        /// Create a dimensions profile from the given package adapter
        /// </summary>
        private static DimensionsProfileEntity CreateProfileFromPackageAdapter(IPackageAdapter defaultPackageAdapter) =>
            new DimensionsProfileEntity()
            {
                DimensionsProfileID = 0,
                Name = "Enter Dimensions",
                Length = defaultPackageAdapter?.DimsLength ?? 0,
                Width = defaultPackageAdapter?.DimsWidth ?? 0,
                Height = defaultPackageAdapter?.DimsHeight ?? 0,
                Weight = defaultPackageAdapter?.AdditionalWeight ?? 0
            };
    }
}
