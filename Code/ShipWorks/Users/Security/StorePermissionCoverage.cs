using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Indicates how many stores a user has permission to do something in
    /// </summary>
    public enum StorePermissionCoverage
    {
        /// <summary>
        /// User is granted the PermissionType on all stores
        /// </summary>
        All,

        /// <summary>
        /// User is not granted the permission type in any store
        /// </summary>
        None,

        /// <summary>
        /// User is granted the permission in some stores
        /// </summary>
        Some
    }
}
