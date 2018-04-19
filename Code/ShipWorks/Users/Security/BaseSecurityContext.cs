using Interapptive.Shared.Utility;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Provide the basic functions of the SecurityContext
    /// </summary>
    public abstract class BaseSecurityContext : ISecurityContext
    {
        /// <summary>
        /// Determines if the current user has the specified permission, and if not, throws a PermissionException.
        /// </summary>
        public void DemandPermission(PermissionType type)
        {
            DemandPermission(type, null);
        }

        /// <summary>
        /// Determines if the current user has the specified permission, and if not, throws a PermissionException. If the PermissionType is
        /// related to orders, then the ObjectID will be automatically translated to a StoreID, such as an OrderItemID would
        /// be translated to its order's StoreID.
        /// </summary>
        public void DemandPermission(PermissionType type, long? objectID)
        {
            if (!HasPermission(type, objectID))
            {
                throw new PermissionException(UserSession.User, type);
            }
        }

        /// <summary>
        /// Determines if the current user has the specified permission
        /// </summary>
        /// <remarks>
        /// If the PermissionType is related to orders, then the ObjectID will be automatically translated to a StoreID, 
        /// such as an OrderItemID would be translated to its order's StoreID.
        /// </remarks>
        public Result RequestPermission(PermissionType type, long? objectID) =>
            HasPermission(type, objectID) ?
                Result.FromSuccess() :
                Result.FromError("User does not have permission");

        /// <summary>
        /// Determines if the user has the specified permission.  This takes into consideration
        /// permissions that imply other permissions (such as Edit Orders implies Edit Notes)
        /// </summary>
        public bool HasPermission(PermissionType type)
        {
            return HasPermission(type, null);
        }

        /// <summary>
        /// Determines if the user has the specified permission for the given object.   This takes into consideration
        /// permissions that imply other permissions (such as Edit Orders implies Edit Notes).  If the PermissionType is
        /// related to orders, then the ObjectID will be automatically translated to a StoreID, such as an OrderItemID would
        /// be translated to its order's StoreID.
        /// </summary>
        public abstract bool HasPermission(PermissionType permission, long? objectID);

        /// <summary>
        /// Determines how many stores the user is allowed to do the given permission for
        /// </summary>
        public abstract StorePermissionCoverage GetRelatedObjectPermissionCoverage(PermissionType type);

        /// <summary>
        /// Determines how many stores the user is allowed to do the given permission for
        /// </summary>
        public abstract StorePermissionCoverage GetStorePermissionCoverage(PermissionType permissionType);

        /// <summary>
        /// Clear all the cached permissions
        /// </summary>
        public abstract void ClearPermissionCache();
    }
}