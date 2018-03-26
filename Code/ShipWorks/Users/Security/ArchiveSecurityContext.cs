using System.Collections.Generic;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Security context used when logged into an archive database
    /// </summary>
    public class ArchiveSecurityContext : BaseSecurityContext
    {
        readonly ISecurityContext baseContext;
        readonly HashSet<PermissionType> allowedPermissions = new HashSet<PermissionType>
        {
            PermissionType.ManageFilters,
            PermissionType.ManageUsers,
            PermissionType.ManageTemplates,
            PermissionType.OrdersViewPaymentData,
            PermissionType.DatabaseBackup,
            PermissionType.DatabaseSetup,
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveSecurityContext(ISecurityContext baseContext)
        {
            this.baseContext = baseContext;
        }

        /// <summary>
        /// Checks whether the current user has the specified permission
        /// </summary>
        public override bool HasPermission(PermissionType permission, long? objectID)
        {
            return allowedPermissions.Contains(permission) && baseContext.HasPermission(permission, objectID);
        }

        /// <summary>
        /// Determines how many stores the user is allowed to do the given permission for
        /// </summary>
        public override StorePermissionCoverage GetRelatedObjectPermissionCoverage(PermissionType type) =>
            baseContext.GetRelatedObjectPermissionCoverage(type);

        /// <summary>
        /// Determines how many stores the user is allowed to do the given permission for
        /// </summary>
        public override StorePermissionCoverage GetStorePermissionCoverage(PermissionType permissionType) =>
            baseContext.GetRelatedObjectPermissionCoverage(permissionType);

        /// <summary>
        /// Clear all the cached permissions
        /// </summary>
        public override void ClearPermissionCache() =>
            baseContext.ClearPermissionCache();
    }
}
