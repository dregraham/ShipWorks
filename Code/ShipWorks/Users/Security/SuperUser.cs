using System;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Utility class for dealing with the buitin ShipWorks SuperUser
    /// </summary>
    public static class SuperUser
    {
        static UserEntity instance;
        static SecurityContext securityContext;

        /// <summary>
        /// Constructor
        /// </summary>
        static SuperUser()
        {
            instance = new UserEntity();
            instance.UserID = UserEntity.SuperUserID;
            instance.Username = UserEntity.SuperUserInternalName;
            instance.IsAdmin = true;

            instance.IsNew = false;
            instance.IsDirty = false;
            instance.Fields.State = EntityState.Fetched;

            securityContext = new SecurityContext(null, true);
        }

        /// <summary>
        /// Create the Super User account.  Used when a database is first created.
        /// </summary>
        public static void Create(Func<SqlConnection> openSqlConnection, SqlAdapter adapter)
        {
            // We don't need to go through UserUtility since we don't need all the extras, like user settings, My Filters, etc.
            UserEntity user = new UserEntity();
            user.UserID = UserEntity.SuperUserID;
            user.Username = UserEntity.SuperUserInternalName;
            user.Password = UserUtility.HashPassword(Guid.NewGuid().ToString());

            user.Email = "";
            user.IsAdmin = true;
            user.IsDeleted = false;

            adapter.IdentityInsert = true;
            adapter.SaveEntity(user, false, false);
            adapter.IdentityInsert = false;

            // Now we have to make sure and reset our seeding back to normal, since the identity insert will have goofed it up
            using (SqlConnection con = openSqlConnection())
            {
                // If there are existing users, we use the current MAX - b\c the next seed given is 1000+ what we reseed to.
                // Which is also why if there are no users, use just use the raw seed (like 2, not 1002, b\c the first seed will then be given as 1002)
                SqlCommand getSeedCmd = SqlCommandProvider.Create(con, string.Format(@"
                    SELECT COALESCE(MAX(UserID), {0})
                      FROM [User]
                      WHERE UserID != {1}", EntityUtility.GetEntitySeed(EntityType.UserEntity), UserEntity.SuperUserID));
                long seedID = (long) getSeedCmd.ExecuteScalar();

                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = string.Format("DBCC CHECKIDENT ('{0}', RESEED, {1})", SqlAdapter.GetTableName(EntityType.UserEntity), seedID);
                SqlCommandProvider.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// The single global instance of the super user
        /// </summary>
        public static UserEntity Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// The security context of the super user.  All access.
        /// </summary>
        public static SecurityContext SecurityContext
        {
            get { return securityContext; }
        }

        /// <summary>
        /// The constant userID for the super user
        /// </summary>
        public static long UserID
        {
            get { return UserEntity.SuperUserID; }
        }

        /// <summary>
        /// The display name of the super user
        /// </summary>
        public static string DisplayName
        {
            get { return UserEntity.SuperUserDisplayName; }
        }
    }
}
