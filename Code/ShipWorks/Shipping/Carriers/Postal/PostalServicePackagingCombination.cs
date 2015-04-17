using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Helper class for combinations of PostalServiceType and PostalPackagingType
    /// </summary>
    public class PostalServicePackagingCombination : IEquatable<PostalServicePackagingCombination>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalServicePackagingCombination(PostalServiceType postalServiceType, PostalPackagingType postalPackagingType)
        {
            ServiceType = postalServiceType;
            PackagingType = postalPackagingType;
        }

        /// <summary>
        /// The PostalServiceType
        /// </summary>
        public readonly PostalServiceType ServiceType;

        /// <summary>
        /// The PostalPackagingType
        /// </summary>
        public readonly PostalPackagingType PackagingType;

        public bool Equals(PostalServicePackagingCombination other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            return ServiceType.Equals(other.ServiceType) && PackagingType.Equals(other.PackagingType);
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.
        public override int GetHashCode()
        {
            int hashProductName = PackagingType.GetHashCode();

            int hashProductCode = ServiceType.GetHashCode();

            return hashProductName ^ hashProductCode;
        }
    }
}
