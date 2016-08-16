using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// The permissions a user possesses
    /// </summary>
    public class PermissionSet : IEnumerable
    {
        PermissionCollection permissions = new PermissionCollection();
        long userID = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public PermissionSet()
        {
            permissions.RemovedEntitiesTracker = new PermissionCollection();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public PermissionSet(PermissionSet other) : this()
        {
            CopyFrom(other);
        }

        /// <summary>
        /// Load the set of permissions for the specified user
        /// </summary>
        public void Load(long userID)
        {
            this.userID = userID;

            // Load the user's permission collection
            permissions = PermissionCollection.Fetch(SqlAdapter.Default, PermissionFields.UserID == userID);

            // Track removals for later deletion
            permissions.RemovedEntitiesTracker = new PermissionCollection();
        }

        /// <summary>
        /// Determines if the user has the specified permission
        /// </summary>
        public bool HasPermission(PermissionType type)
        {
            return HasPermission(type, null);
        }

        /// <summary>
        /// Determines if the user has the specified permission for the given object
        /// </summary>
        public bool HasPermission(PermissionType type, long? objectID)
        {
            if (type == PermissionType.AlwaysGrant)
            {
                return true;
            }

            if (type == PermissionType.InterapptiveOnly)
            {
                return InterapptiveOnly.IsInterapptiveUser;
            }

            return FindPermission(permissions, type, objectID) != null;
        }

        /// <summary>
        /// Add the specified permission to the user's list of allowed rights
        /// </summary>
        public void AddPermission(PermissionType type)
        {
            AddPermission(type, null);
        }

        /// <summary>
        /// Add the specified permission for the given object to the user's list of allowed rights
        /// </summary>
        public void AddPermission(PermissionType type, long? objectID)
        {
            if (type == PermissionType.AlwaysGrant)
            {
                throw new InvalidOperationException("Cannot add the always permission, it is implied.");
            }

            if (!HasPermission(type, objectID))
            {
                PermissionEntity removed = FindPermission((PermissionCollection) permissions.RemovedEntitiesTracker, type, objectID);
                if (removed != null)
                {
                    permissions.Add(removed);
                    permissions.RemovedEntitiesTracker.Remove(removed);
                }
                else
                {
                    ValidateScope(type, objectID);

                    PermissionEntity permission = new PermissionEntity();
                    permission.UserID = userID;
                    permission.PermissionType = (int) type;
                    permission.EntityID = objectID;

                    permissions.Add(permission);
                }
            }
        }

        /// <summary>
        /// Validate that the given objectID is valid given the scope of the PermissionType
        /// </summary>
        private void ValidateScope(PermissionType type, long? objectID)
        {
            PermissionScope scope = PermissionHelper.GetScope(type);

            if (scope == PermissionScope.Global && objectID != null)
            {
                throw new InvalidOperationException("Global scope should not specified an EntityID to secure.");
            }

            if (scope == PermissionScope.Store)
            {
                if (objectID == null)
                {
                    throw new InvalidOperationException("An EntityID must be specified for a permission of store scope.");
                }

                EntityType entityType = EntityUtility.GetEntityType(objectID.Value);
                if (entityType != EntityType.StoreEntity)
                {
                    throw new InvalidOperationException("The EntityID must represent a store key for a permission of store scope.");
                }
            }
        }

        /// <summary>
        /// Add all the permissions found in the specified other permission set
        /// </summary>
        public void AddPermissions(PermissionSet other)
        {
            foreach (PermissionEntity permission in other.permissions)
            {
                AddPermission((PermissionType) permission.PermissionType, permission.EntityID);
            }
        }

        /// <summary>
        /// Remove the specified permission from the user's list of allowed rights
        /// </summary>
        public void RemovePermission(PermissionType type)
        {
            RemovePermission(type, null);
        }

        /// <summary>
        /// Remove the specified permission for the given object from the user's list of allowed rights
        /// </summary>
        public void RemovePermission(PermissionType type, long? objectID)
        {
            PermissionEntity permission = FindPermission(permissions, type, objectID);

            if (permission != null)
            {
                permissions.Remove(permission);

                if (permission.IsNew)
                {
                    permissions.RemovedEntitiesTracker.Remove(permission);
                }
            }
        }

        /// <summary>
        /// Find the permission of the given type and the specified secured object
        /// </summary>
        private PermissionEntity FindPermission(PermissionCollection collection, PermissionType type, long? objectID)
        {
            List<int> indexes = collection.FindMatches(PermissionFields.PermissionType == (int) type & PermissionFields.EntityID == objectID);

            if (indexes != null && indexes.Count > 0)
            {
                Debug.Assert(indexes.Count == 1);

                return collection[indexes[0]];
            }

            return null;
        }

        /// <summary>
        /// Copy this permission set to that of the given user.  Any existing permissions of the user are deleted.
        /// </summary>
        public void CopyTo(long userID, SqlAdapter adapter)
        {
            CopyTo(userID, true, adapter);
        }

        /// <summary>
        /// Copy this permission set to that of the given user.  If deleteExisting is false, then all existing permissions are left, and
        /// this is affectively a union.
        /// </summary>
        public void CopyTo(long userID, bool deleteExisting, SqlAdapter adapter)
        {
            // Delete everything that is there now for the given user
            if (deleteExisting)
            {
                adapter.DeleteEntitiesDirectly(typeof(PermissionEntity), new RelationPredicateBucket(PermissionFields.UserID == userID));
            }

            // Now save everything we have to the given user
            foreach (PermissionEntity entity in permissions)
            {
                PermissionEntity clone = new PermissionEntity();
                clone.Fields = entity.Fields.CloneAsDirty();
                clone.UserID = userID;

                adapter.SaveEntity(clone);
            }
        }

        /// <summary>
        /// Copy the permissions of the given set into ourself
        /// </summary>
        public void CopyFrom(PermissionSet source)
        {
            // Start out by removing all our own permissions
            foreach (PermissionEntity permission in new List<PermissionEntity>(permissions))
            {
                RemovePermission((PermissionType) permission.PermissionType, permission.EntityID);
            }

            // Add back in all permissions we are copying from
            AddPermissions(source);
        }

        /// <summary>
        /// Save the current permission set of the user
        /// </summary>
        public void Save(SqlAdapter adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            if (!adapter.InSystemTransaction)
            {
                throw new InvalidOperationException("A transaction must be in progress on the permission adapter.");
            }

            if (userID == -1)
            {
                throw new InvalidOperationException("Cannot save a permission set that is not associated with a user.  Use CopyTo instead.");
            }

            try
            {
                adapter.SaveEntityCollection(permissions, true, true);
                adapter.DeleteEntityCollection(permissions.RemovedEntitiesTracker);
            }
            catch (ORMQueryExecutionException ex)
            {
                if (ex.Message.Contains("IX_Permission"))
                {
                    // Resave, this time completely
                    SaveFull(adapter);
                }
                else
                {
                    throw;
                }
            }
            catch (ORMConcurrencyException)
            {
                // Resave, this time completely
                SaveFull(adapter);
            }

            // We have to know when we can clear the removed collection, which is when the transaction actually committs
            adapter.TransactionCompleted += new TransactionCompletedEventHandler(OnSaveTransactionCompleted);
        }

        /// <summary>
        /// Do a full save - wipe out all existing permissions, and add them all back in.
        /// </summary>
        private void SaveFull(SqlAdapter adapter)
        {
            // Delete everything that is there now for the given user
            adapter.DeleteEntitiesDirectly(typeof(PermissionEntity), new RelationPredicateBucket(PermissionFields.UserID == userID));

            foreach (PermissionEntity permission in permissions)
            {
                permission.Fields = permission.Fields.CloneAsDirty();
                permission.IsNew = true;

                adapter.SaveAndRefetch(permission);
            }
        }

        /// <summary>
        /// The transaction that the save is taking place in has completed
        /// </summary>
        void OnSaveTransactionCompleted(object sender, TransactionEventArgs e)
        {
            SqlAdapter adapter = (SqlAdapter) sender;
            adapter.TransactionCompleted -= new TransactionCompletedEventHandler(OnSaveTransactionCompleted);

            if (e.Transaction.TransactionInformation.Status == TransactionStatus.Committed)
            {
                permissions.RemovedEntitiesTracker.Clear();
            }
        }

        /// <summary>
        /// Cancel any changes that have occurred since the last load or save
        /// </summary>
        public void CancelChanges()
        {
            // Add back in all the removed permissions
            foreach (PermissionEntity removed in permissions.RemovedEntitiesTracker)
            {
                permissions.Add(removed);
            }

            // Revert all the changes
            foreach (PermissionEntity edited in permissions.DirtyEntities)
            {
                if (edited.IsNew)
                {
                    permissions.Remove(edited);
                }
                else
                {
                    edited.RollbackChanges();
                }
            }

            // Clear the tracked removed
            permissions.RemovedEntitiesTracker.Clear();
        }

        /// <summary>
        /// Enumeration of permissions
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return permissions.GetEnumerator();
        }
    }
}
