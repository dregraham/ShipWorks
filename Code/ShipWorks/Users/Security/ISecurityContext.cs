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
        /// <remarks>
        /// If the PermissionType is related to orders, then the ObjectID will be automatically translated
        /// to a StoreID, such as an OrderItemID would be translated to its order's StoreID.
        /// </remarks>
        void DemandPermission(PermissionType shipmentsCreateEditProcess, long? objectID);

        /// <summary>
        /// Checks whether the current user has the specified permission
        /// </summary>
        bool HasPermission(PermissionType shipmentsCreateEditProcess, long? objectID);
    }
}