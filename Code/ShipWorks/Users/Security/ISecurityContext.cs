using Interapptive.Shared.Utility;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Encapsulates user permission settings and their modifications.
    /// </summary>
    public interface ISecurityContext
    {
        /// <summary>
        /// Determines if the current user has the specified permission, and if not, throws a PermissionException.
        /// </summary>
        void DemandPermission(PermissionType type);

        /// <summary>
        /// Determines if the current user has the specified permission, and if not, throws a PermissionException.
        /// </summary>
        /// <remarks>
        /// If the PermissionType is related to orders, then the ObjectID will be automatically translated
        /// to a StoreID, such as an OrderItemID would be translated to its order's StoreID.
        /// </remarks>
        void DemandPermission(PermissionType permission, long? objectID);

        /// <summary>
        /// Determines if the current user has the specified permission
        /// </summary>
        /// <remarks>
        /// If the PermissionType is related to orders, then the ObjectID will be automatically translated
        /// to a StoreID, such as an OrderItemID would be translated to its order's StoreID.
        /// </remarks>
        Result RequestPermission(PermissionType permission, long? objectID);

        /// <summary>
        /// Checks whether the current user has the specified permission
        /// </summary>
        bool HasPermission(PermissionType permission);

        /// <summary>
        /// Checks whether the current user has the specified permission
        /// </summary>
        bool HasPermission(PermissionType permission, long? objectID);

        /// <summary>
        /// Determines how many stores the user is allowed to do the given permission for
        /// </summary>
        StorePermissionCoverage GetRelatedObjectPermissionCoverage(PermissionType type);

        /// <summary>
        /// Determines how many stores the user is allowed to do the given permission for
        /// </summary>
        StorePermissionCoverage GetStorePermissionCoverage(PermissionType permissionType);

        /// <summary>
        /// Clear all the cached permissions
        /// </summary>
        void ClearPermissionCache();
    }
}