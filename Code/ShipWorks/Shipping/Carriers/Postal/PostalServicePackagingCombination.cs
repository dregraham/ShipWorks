using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Helper class for combinations of PostalServiceType and PostalPackagingType
    /// </summary>
    public class PostalServicePackagingCombination
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
        public PostalServiceType ServiceType;

        /// <summary>
        /// The PostalPackagingType
        /// </summary>
        public PostalPackagingType PackagingType;
    }
}
