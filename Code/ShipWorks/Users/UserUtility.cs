using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Grid;
using ShipWorks.Stores;
using ShipWorks.UI.Controls;
using ShipWorks.Users.Security;

namespace ShipWorks.Users
{
    /// <summary>
    /// Utility class for working with users.
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what ShipWorks currently uses")]
    public static class UserUtility
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(UserUtility));

        /// <summary>
        /// Determines if any admin users exist in the system.
        /// </summary>
        public static bool HasAdminUsers()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandText = "SELECT COUNT(*) FROM [User] WHERE IsAdmin = 1 and IsDeleted = 0 and UserID != @superID";
                cmd.AddParameterWithValue("@superID", SuperUser.UserID);

                return (int) DbCommandProvider.ExecuteScalar(cmd) > 0;
            }
        }

        /// <summary>
        /// Returns true if the username is already in use.  False otherwise.
        /// </summary>
        public static bool IsUsernameTaken(string username)
        {
            return IsUsernameTaken(username, null);
        }

        /// <summary>
        /// Returns true if the username is already in use.  False otherwise.
        /// </summary>
        public static bool IsUsernameTaken(string username, UserEntity ignoreUser)
        {
            IPredicate filter = UserFields.Username == username;

            if (ignoreUser != null)
            {
                filter = new PredicateExpression(filter, PredicateExpressionOperator.And, UserFields.UserID != ignoreUser.UserID);
            }

            int count = UserCollection.GetCount(SqlAdapter.Default, filter);

            Debug.Assert(count < 2);

            return count > 0;
        }

        /// <summary>
        /// Create a new ShipWorks user
        /// </summary>
        public static UserEntity CreateUser(string username, string email, string password, bool admin)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                UserEntity user = CreateUser(username, email, password, admin, adapter);

                adapter.Commit();

                return user;
            }
        }

        /// <summary>
        /// Create a new ShipWorks user
        /// </summary>
        public static UserEntity CreateUser(string username, string email, string password, bool admin, SqlAdapter adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            if (!adapter.InSystemTransaction)
            {
                throw new InvalidOperationException("A transaction must be in progress for CreateUser.");
            }

            UserEntity user = new UserEntity
            {
                Username = username,
                Password = HashPassword(password),
                Email = email,
                IsAdmin = admin,
                IsDeleted = false
            };

            try
            {
                adapter.SaveAndRefetch(user);

                // setup the new user
                ConfigureNewUser(adapter, user);

                return user;
            }
            catch (ORMQueryExecutionException ex)
            {
                if (ex.Message.Contains("IX_User_Username"))
                {
                    log.ErrorFormat("User '{0}' already exists.", username);

                    throw new DuplicateNameException($"The username '{username}' already exists.", ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Perform necessary configuration on a new ShipWorks user
        /// </summary>
        public static void ConfigureNewUser(SqlAdapter adapter, UserEntity user)
        {
            // validate
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            FilterLayoutEntity myFiltersOrders = FilterHelper.CreateMyLayout(user, FilterTarget.Orders);
            adapter.SaveEntity(myFiltersOrders);

            FilterLayoutEntity myFiltersCustomers = FilterHelper.CreateMyLayout(user, FilterTarget.Customers);
            adapter.SaveEntity(myFiltersCustomers);

            CreateDefaultSettings(user, adapter);
        }

        /// <summary>
        /// Create the default permissions that a new users will get.  This is unsaved, and not associated
        /// with any user after creation.
        /// </summary>
        public static PermissionSet CreateDefaultPermissionSet()
        {
            PermissionSet permissions = new PermissionSet();

            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                permissions.AddPermissions(CreateDefaultStorePermissionSet(store.StoreID));
            }

            return permissions;
        }

        /// <summary>
        /// Create the default permission set for the given store
        /// </summary>
        public static PermissionSet CreateDefaultStorePermissionSet(long storeID)
        {
            PermissionSet permissions = new PermissionSet();

            permissions.AddPermission(PermissionType.OrdersEditNotes, storeID);
            permissions.AddPermission(PermissionType.OrdersEditStatus, storeID);
            permissions.AddPermission(PermissionType.OrdersSendEmail, storeID);

            permissions.AddPermission(PermissionType.ShipmentsCreateEditProcess, storeID);

            return permissions;
        }

        /// <summary>
        /// Create the default settings for a new user
        /// </summary>
        private static void CreateDefaultSettings(UserEntity user, SqlAdapter adapter)
        {
            UserSettingsEntity settings = new UserSettingsEntity();
            settings.User = user;

            settings.DisplayColorScheme = (int) ColorScheme.Blue;
            settings.DisplaySystemTray = false;

            settings.WindowLayout = WindowLayoutProvider.GetDefaultLayout();
            settings.GridMenuLayout = null;

            settings.FilterInitialUseLastActive = false;
            settings.OrderFilterLastActive = BuiltinFilter.GetTopLevelKey(FilterTarget.Orders);
            settings.FilterInitialSpecified = settings.OrderFilterLastActive;
            settings.FilterInitialSortType = (int) FilterInitialSortType.CurrentSort;
            settings.OrderFilterExpandedFolders = null;

            settings.CustomerFilterLastActive = BuiltinFilter.GetTopLevelKey(FilterTarget.Customers);
            settings.CustomerFilterExpandedFolders = null;

            settings.ShippingWeightFormat = (int) WeightDisplayFormat.FractionalPounds;

            settings.TemplateLastSelected = 0;

            adapter.SaveAndRefetch(settings);
        }

        /// <summary>
        /// Gets the UserEntity corresponding to the given username and password.  null if not such user is found.
        /// </summary>
        public static UserEntity GetShipWorksUser(string username, string password)
        {
            PrefetchPath2 settingsPrefetch = new PrefetchPath2(EntityType.UserEntity) { UserEntity.PrefetchPathSettings };

            UserCollection users = UserCollection.Fetch(SqlAdapter.Default,
                UserFields.Username == username & UserFields.Password == HashPassword(password) &
                UserFields.IsDeleted == false, settingsPrefetch);

            // If we got a user, its the one we need.
            if (users.Count == 1)
            {
                UserEntity user = users[0];

                if (user.Settings == null)
                {
                    throw new NotFoundException($"Could not find settings for user '{username}'.");
                }

                return user;
            }

            return null;
        }

        /// <summary>
        /// Get the UserID of the ShipWorks user with the given username and password.  -1 if no such user is found.
        /// </summary>
        public static long GetShipWorksUserID(string username, string password)
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandText = "SELECT UserID FROM [User] WHERE Username = @Username and Password = @Password and IsDeleted = 0";
                cmd.AddParameterWithValue("@Username", username);
                cmd.AddParameterWithValue("@Password", HashPassword(password));

                object result = DbCommandProvider.ExecuteScalar(cmd);
                if (result == null || result is DBNull)
                {
                    return -1;
                }

                return (long) result;
            }
        }

        /// <summary>
        /// Create a hash of a user password.
        /// </summary>
        public static string HashPassword(string password)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(password);

            MD5 md5 = new MD5CryptoServiceProvider();

            // Generate the hash
            string result = Convert.ToBase64String(md5.ComputeHash(plainBytes));

            return result.Substring(0, Math.Min(32, result.Length));
        }
    }
}
