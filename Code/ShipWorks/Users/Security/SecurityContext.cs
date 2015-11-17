using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.Stores;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model;
using ShipWorks.Email;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Encapsulates user permission settings and their modifications.
    /// </summary>
    public class SecurityContext
    {
        PermissionSet permissionSet;
        bool isAdmin;

        Dictionary<PermissionIdentifier, bool> permissionCache = new Dictionary<PermissionIdentifier, bool>();
        Dictionary<PermissionType, StorePermissionCoverage> storePermissionCountCache = new Dictionary<PermissionType, StorePermissionCoverage>();

        #region class PermissionIdentifier

        /// <summary>
        /// Used internall for the caching lookup
        /// </summary>
        class PermissionIdentifier
        {
            PermissionType permissionType;
            long? objectID;

            /// <summary>
            /// Constructor
            /// </summary>
            public PermissionIdentifier(PermissionType permissionType, long? objectID)
            {
                this.permissionType = permissionType;
                this.objectID = objectID;
            }

            /// <summary>
            /// Equals
            /// </summary>
            public override bool Equals(object obj)
            {
                PermissionIdentifier other = obj as PermissionIdentifier;
                if ((object) other == null)
                {
                    return false;
                }

                return other.permissionType == permissionType && other.objectID == objectID;
            }

            /// <summary>
            /// Operator==
            /// </summary>
             public static bool operator ==(PermissionIdentifier left, PermissionIdentifier right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Operator!=
            /// </summary>
            public static bool operator !=(PermissionIdentifier left, PermissionIdentifier right)
            {
                return !(left.Equals(right));
            }

            /// <summary>
            /// Hash code
            /// </summary>
            public override int GetHashCode()
            {
                return permissionType.GetHashCode() + objectID.GetHashCode();
            }
        }

        #endregion

        /// <summary>
        /// Create a security context from the permissions in the database for the given user.
        /// </summary>
        public SecurityContext(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this.isAdmin = user.IsAdmin;

            permissionSet = new PermissionSet();
            permissionSet.Load(user.UserID);
        }

        /// <summary>
        /// Create a security context with the specified permissions
        /// </summary>
        public SecurityContext(PermissionSet permissionSet, bool isAdmin)
        {
            this.isAdmin = isAdmin;
            this.permissionSet = permissionSet;
        }

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
        [NDependIgnoreLongMethod]
        public bool HasPermission(PermissionType type, long? objectID)
        {
            if (isAdmin)
            {
                return true;
            }

            PermissionScope scope = PermissionHelper.GetScope(type);

            // Has to be an entity for anything other than global
            if (scope != PermissionScope.Global && objectID == null)
            {
                throw new ArgumentException("Cannot pass a null entity to a non-global permission.");
            }

            // First see if the permission depends on the actual entity that was passed, and translate as necesary
            if (scope == PermissionScope.IndirectEntityType)
            {
                type = PermissionHelper.GetIndirectEntityActualPermission(type, EntityUtility.GetEntityType(objectID.Value));
                scope = PermissionHelper.GetScope(type);

                // If Global, we no longer need the object
                if (scope == PermissionScope.Global)
                {
                    objectID = null;
                }
            }

            // For store permissions we have an early-out if they can do the permission for all stores or no stores
            if (scope != PermissionScope.Global)
            {
                // "Some" is the default for "dont early out"
                StorePermissionCoverage coverage = StorePermissionCoverage.Some;

                if (scope == PermissionScope.Store)
                {
                    // Only need to do the shortcut to avoid translating from child entities up to the StoreID... so we don't need to do 
                    // it (and can't, or it would recurse) for StoreEntity
                    if (EntityUtility.GetEntityType(objectID.Value) != EntityType.StoreEntity)
                    {
                        coverage = GetStorePermissionCoverage(type);
                    }
                }
                else
                {
                    Debug.Assert(scope == PermissionScope.IndirectRelatedObject);

                    coverage = GetRelatedObjectPermissionCoverage(type);
                }

                // If the user can do it for all stores, no need to check what's selected
                if (coverage == StorePermissionCoverage.All)
                {
                    return true;
                }

                // If the user can't do it for any stores, no need to check what's selected
                if (coverage == StorePermissionCoverage.None)
                {
                    return false;
                }
            }

            bool hasPermission;

            // See if we've cached this permission check
            if (permissionCache.TryGetValue(new PermissionIdentifier(type, objectID), out hasPermission))
            {
                return hasPermission;
            }
            
            // See if the security depends not on the passed in object - but on the object's that is is related to
            if (PermissionHelper.GetScope(type) == PermissionScope.IndirectRelatedObject)
            {
                hasPermission = HasIndirectRelatedObjectPermission(type, objectID.Value);
            }
            else
            {
                long? scopeID = objectID;

                // Translate the given objectID into an ID usable for the permission scope
                if (PermissionHelper.GetScope(type) == PermissionScope.Store)
                {
                    scopeID = TranslateStoreScope(objectID);
                }

                // If its a store scope, but we weren't able to nail down a StoreID (for example, a customer with multiple orders in multiple stores), then 
                // we have to say no.  If they had access to all stores - we'd have already said yes.
                if (PermissionHelper.GetScope(type) == PermissionScope.Store && scopeID == null)
                {
                    hasPermission = false;
                }
                else
                {
                    // See if they have the specific permission requested
                    hasPermission = permissionSet.HasPermission(type, scopeID);

                    // If not a direct permission, check up for implied permissions (like if you can edit orders, its implied you can edit notes)
                    if (!hasPermission)
                    {
                        hasPermission = HasImpliedPermission(type, scopeID);
                    }

                    // If the scopeID is different from the objectID, cache it too
                    if (scopeID != objectID)
                    {
                        permissionCache[new PermissionIdentifier(type, scopeID)] = hasPermission;
                    }
                }
            }

            // Cache this permission request and its result
            permissionCache[new PermissionIdentifier(type, objectID)] = hasPermission;

            return hasPermission;
        }

        /// <summary>
        /// Indicates if the objects related to the given EntityID have the specified permission. For example, a NoteID... what you really
        /// want to check is the Customer or Order that is related to the note.
        /// </summary>
        private bool HasIndirectRelatedObjectPermission(PermissionType type, long entityID)
        {
            switch (type)
            {
                case PermissionType.RelatedObjectEditNotes:
                    {
                        if (EntityUtility.GetEntityType(entityID) != EntityType.NoteEntity)
                        {
                            throw new InvalidOperationException("Wrong EntityType passed for RelatedObjectEditNotes.");
                        }

                        NoteEntity note = (NoteEntity) DataProvider.GetEntity(entityID);

                        return note != null && HasPermission(PermissionType.EntityTypeEditNotes, note.ObjectID);
                    }

                case PermissionType.RelatedObjectSendEmail:
                    {
                        // For email we have to check every related key
                        foreach (long key in EmailUtility.GetRelatedKeys(entityID, EmailOutboundRelationType.RelatedObject))
                        {
                            if (!HasPermission(PermissionType.EntityTypeSendEmail, key))
                            {
                                return false;
                            }
                        }

                        return true;
                    }

                default:
                    throw new InvalidOperationException("Unhandled indirect entity permission type.");
            }
        }

        /// <summary>
        /// Determines if the given permission is available through another permission which implies it.
        /// </summary>
        private bool HasImpliedPermission(PermissionType type, long? objectID)
        {
            // Create and edit customers - if there allowed to create orders for any store
            if (type == PermissionType.CustomersCreateEdit)
            {
                return StoreManager.GetAllStores().Any(s => HasPermission(PermissionType.OrdersModify, s.StoreID));
            }

            // Delete customers - only if you can delete orders from every store
            if (type == PermissionType.CustomersDelete)
            {
                return StoreManager.GetAllStores().All(s => HasPermission(PermissionType.OrdersModify, s.StoreID));
            }

            // Send email to customers - only if you can send email for every store
            if (type == PermissionType.CustomersSendEmail)
            {
                return StoreManager.GetAllStores().All(s => HasPermission(PermissionType.OrdersSendEmail, s.StoreID));
            }

            // Edit customer notes - only if you can edit notes for at least one order
            if (type == PermissionType.CustomersEditNotes)
            {
                return StoreManager.GetAllStores().Any(s => HasPermission(PermissionType.OrdersEditNotes, s.StoreID));
            }

            // Add orders to customers - if you can add orders to any store
            if (type == PermissionType.CustomersAddOrder)
            {
                return StoreManager.GetAllStores().Any(s => HasPermission(PermissionType.OrdersModify, s.StoreID));
            }

            switch (type)
            {
                case PermissionType.OrdersEditNotes:
                case PermissionType.OrdersEditStatus:
                    return HasPermission(PermissionType.OrdersModify, objectID);

                case PermissionType.CustomersEditNotes:
                    return HasPermission(PermissionType.CustomersCreateEdit, objectID);
            }

            return false;
        }

        /// <summary>
        /// Translate the given objectID into an ID usable for the specified scope
        /// </summary>
        private static long? TranslateStoreScope(long? objectID)
        {
            if (objectID == null)
            {
                throw new ArgumentNullException("objectID");
            }

            List<long> keys = DataProvider.GetRelatedKeys(objectID.Value, EntityType.StoreEntity);
            if (keys.Count > 1)
            {
                // More than one store relates to this object (likely a customer, with orders in multiple stores)
                return null;
            }

            // No stores? Maybe the store was just deleted
            if (keys.Count == 0)
            {
                return null;
            }

            return keys[0];
        }

        /// <summary>
        /// Determines if the given user has the specified permission.  This static method goes to the database every time.
        /// </summary>
        public static bool HasPermission(long userID, PermissionType type)
        {
            UserEntity user = new UserEntity(userID);
            SqlAdapter.Default.FetchEntity(user);

            if (user.IsAdmin)
            {
                return true;
            }

            PermissionCollection permissions = PermissionCollection.Fetch(SqlAdapter.Default,
                PermissionFields.UserID == userID & PermissionFields.PermissionType == (int) type);

            return permissions != null && permissions.Count > 0;
        }

        /// <summary>
        /// Clear all the cached permissions
        /// </summary>
        public void ClearPermissionCache()
        {
            permissionCache.Clear();
            storePermissionCountCache.Clear();
        }

        /// <summary>
        /// Determines how many stores the user is allowed to do the given permission for
        /// </summary>
        public StorePermissionCoverage GetRelatedObjectPermissionCoverage(PermissionType type)
        {
            if (PermissionHelper.GetScope(type) != PermissionScope.IndirectRelatedObject)
            {
                throw new InvalidOperationException("Only intended to be calld for related object scoped permissions.");
            }

            switch (type)
            {
                case PermissionType.RelatedObjectEditNotes:
                    {
                        bool canGlobal = HasPermission(PermissionType.CustomersEditNotes);
                        StorePermissionCoverage coverage = GetStorePermissionCoverage(PermissionType.OrdersEditNotes);

                        if (canGlobal && coverage == StorePermissionCoverage.All)
                        {
                            return StorePermissionCoverage.All;
                        }

                        if (!canGlobal && coverage == StorePermissionCoverage.None)
                        {
                            return StorePermissionCoverage.None;
                        }

                        return StorePermissionCoverage.Some;
                    }

                case PermissionType.RelatedObjectSendEmail:
                    {
                        bool canGlobal = HasPermission(PermissionType.CustomersSendEmail);
                        StorePermissionCoverage coverage = GetStorePermissionCoverage(PermissionType.OrdersSendEmail);

                        if (canGlobal && coverage == StorePermissionCoverage.All)
                        {
                            return StorePermissionCoverage.All;
                        }

                        if (!canGlobal && coverage == StorePermissionCoverage.None)
                        {
                            return StorePermissionCoverage.None;
                        }

                        return StorePermissionCoverage.Some;
                    }

                default:
                    throw new InvalidOperationException("Unhandled indirect related permission type.");
            }
        }

        /// <summary>
        /// Determines how many stores the user is allowed to do the given permission for
        /// </summary>
        public StorePermissionCoverage GetStorePermissionCoverage(PermissionType permissionType)
        {
            if (PermissionHelper.GetScope(permissionType) != PermissionScope.Store)
            {
                throw new InvalidOperationException("Only intended to be calld for Store scoped permissions.");
            }

            lock (storePermissionCountCache)
            {
                StorePermissionCoverage result;
                if (!storePermissionCountCache.TryGetValue(permissionType, out result))
                {
                    // Determine how many stores the user can do it in
                    int canCount = StoreManager.GetAllStores().Count(s => HasPermission(permissionType, s.StoreID));

                    if (canCount == 0)
                    {
                        result = StorePermissionCoverage.None;
                    }

                    else
                    {
                        if (canCount < StoreManager.GetAllStores().Count)
                        {
                            result = StorePermissionCoverage.Some;
                        }
                        else
                        {
                            result = StorePermissionCoverage.All;
                        }
                    }

                    storePermissionCountCache[permissionType] = result;
                }

                return result;
            }
        }
    }
}
