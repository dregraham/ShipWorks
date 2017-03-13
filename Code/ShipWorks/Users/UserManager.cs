using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using System.ComponentModel;
using ShipWorks.Users.Security;
using ShipWorks.Data;

namespace ShipWorks.Users
{
    /// <summary>
    /// Manages and caches users
    /// </summary>
    public static class UserManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(UserManager));

        static TableSynchronizer<UserEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            synchronizer = new TableSynchronizer<UserEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (synchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (synchronizer)
            {
                if (synchronizer.Synchronize())
                {
                    synchronizer.EntityCollection.Sort((int) UserFieldIndex.Username, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// All users in ShipWorks
        /// </summary>
        private static List<UserEntity> Users
        {
            get
            {
                lock (synchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return EntityUtility.CloneEntityCollection(synchronizer.EntityCollection.Where(u => u.UserID != SuperUser.UserID));
                }
            }
        }

        /// <summary>
        /// Get the user entity with the given ID, or null if it does not exist
        /// </summary>
        public static UserEntity GetUser(long userID)
        {
            if (userID == SuperUser.UserID)
            {
                return SuperUser.Instance;
            }

            return Users.SingleOrDefault(u => u.UserID == userID);
        }

        /// <summary>
        /// Gets all the active ShipWorks users
        /// </summary>
        public static List<UserEntity> GetUsers(bool includeDeleted)
        {
            if (includeDeleted)
            {
                return Users;
            }
            else
            {
                return Users.Where(u => !u.IsDeleted).ToList();
            }
        }
    }
}
