using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using System.Security.Cryptography;
using System.Xml;
using Interapptive.Shared;
using System.IO;
using System.Xml.XPath;
using ShipWorks.Data.Adapter.Custom;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using ShipWorks.Users.Security;
using ShipWorks.Users.Audit;
using Interapptive.Shared.Data;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Actions;
using ShipWorks.Stores;
using ShipWorks.Filters;
using ShipWorks.Templates;
using ShipWorks.Templates.Media;
using ShipWorks.Filters.Search;
using ShipWorks.Email.Accounts;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.Filters.Grid;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Printing;
using ShipWorks.Shipping;
using ShipWorks.FileTransfer;
using ShipWorks.Shipping.Carriers.EquaShip;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.ApplicationCore.WindowsServices;

namespace ShipWorks.Users
{
    /// <summary>
    /// Class for managing the current session of ShipWorks
    /// </summary>
    public static class UserSession
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(UserSession));

        // The currently logged in user and his security context
        static UserEntity loggedInUser;
        static SecurityContext securityContext;

        // The current running computer
        static ComputerEntity thisComputer;

        // Uniquely identifies the database to which we are connected
        static string databaseID = null;

        // The username of the last user to successfully logon
        static string lastUsername = "";
        static string lastPassword = "";
        static bool lastRemember;

        /// <summary>
        /// One-time initialization of the session
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            SystemData.InitializeForCurrentDatabase();

            // Reset any cached entity data
            DataProvider.InitializeForCurrentDatabase();
            ShippingManager.InitializeForCurrentDatabase();

            // Initialize database scope things
            ConfigurationData.InitializeForCurrentDatabase();
            ShippingSettings.InitializeForCurrentDatabase();
            DataResourceManager.InitializeForCurrentDatabase();

            bool wasLoggedIn = (User != null);

            Reset();

            if (!SqlSession.IsConfigured)
            {
                log.Info("ShipWorksSession initialized (SqlSession not configured)");
            }
            else
            {
                log.InfoFormat("ShipWorksSession initialized ({0}, {1})", SqlSession.Current.Configuration.ServerInstance, SqlSession.Current.Configuration.DatabaseName);

                // Ensure this computer is registered on the current connection.
                thisComputer = ComputerManager.RegisterThisComputer();

                string lastDatabaseID = databaseID;

                // Get the unique id of the database to which we are attached
                AcquireDatabaseID();

                // If we were logged in, try to automatically log back in
                if (wasLoggedIn)
                {
                    Logon(lastUsername, lastPassword, lastRemember, lastDatabaseID != databaseID);
                }
            }
        }

        /// <summary>
        /// Required initialization that must take place after a user logs in to get various resources and managers ready
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            ComputerManager.InitializeForCurrentUser();
            WindowsServiceManager.InitializeForCurrentUser();

            ObjectLabelManager.InitializeForCurrentUser();

            if (UserSession.IsLoggedOn)
                GridColumnDefinitionManager.InitializeForCurrentUser();

            FilterContentManager.InitializeForCurrentUser();
            ActionManager.InitializeForCurrentUser();
            FtpAccountManager.InitializeForCurrentUser();
            StoreManager.InitializeForCurrentUser();
            TemplateManager.InitializeForCurrentUser();
            LabelSheetManager.InitializeForCurrentUser();
            EmailAccountManager.InitializeForCurrentUser();
            SearchManager.InitializeForCurrentUser();

            if (UserSession.IsLoggedOn)
                ServerMessageManager.InitializeForCurrentUser();

            DownloadManager.InitializeForCurrentUser();

            if (UserSession.IsLoggedOn)
                FilterLayoutContext.InitializeForCurrentUser();

            FilterNodeColumnManager.InitializeForCurrentUser();
            ShippingOriginManager.InitializeForCurrentUser();
            StampsAccountManager.InitializeForCurrentUser();
            EndiciaAccountManager.InitializeForCurrentUser();
            DimensionsManager.InitializeForCurrentUser();
            ShippingProfileManager.InitializeForCurrentUser();
            FedExAccountManager.InitializeForCurrentUser();
            UpsAccountManager.InitializeForCurrentUser();
            ShippingDefaultsRuleManager.InitializeForCurrentUser();
            ShippingPrintOutputManager.InitializeForCurrentUser();
            ShippingProviderRuleManager.InitializeForCurrentUser();
            EquaShipAccountManager.InitializeForCurrentUser();
            OnTracAccountManager.InitializeForCurrentUser();
            iParcelAccountManager.InitializeForCurrentUser();
        }

        /// <summary>
        /// Rest the session
        /// </summary>
        public static void Reset()
        {
            thisComputer = null;
            loggedInUser = null;
            securityContext = null;
            databaseID = null;
        }

        /// <summary>
        /// The computer on which the user is currently running.
        /// </summary>
        public static ComputerEntity Computer
        {
            get { return thisComputer; }
        }

        /// <summary>
        /// Get the currently logged in user.  Null if no user is logged in.
        /// </summary>
        public static UserEntity User
        {
            get 
            {
                if (AuditBehaviorScope.IsSuperUserActive)
                {
                    return SuperUser.Instance;
                }

                return loggedInUser; 
            }
        }

        /// <summary>
        /// The SecurityContext of the logged on user.  Only valid of IsLoggedOn is true.
        /// </summary>
        public static SecurityContext Security
        {
            get 
            {
                if (AuditBehaviorScope.IsSuperUserActive)
                {
                    return SuperUser.SecurityContext;
                }

                return securityContext; 
            }
        }

        /// <summary>
        /// The AuditReason that is currently active.  This is used for auditing purposes so we know where
        /// they are when they changed something.
        /// </summary>
        public static AuditReason AuditReason
        {
            get
            {
                AuditReason reason = new AuditReason(AuditReasonType.Default);

                // The behavior scope has priority
                if (AuditBehaviorScope.ActiveReason != null)
                {
                    reason = AuditBehaviorScope.ActiveReason;
                }

                return reason;
            }
        }

        /// <summary>
        /// The WorkstationID is how we pass the UserID and ComputerID to use for auditing on this connection.
        /// </summary>
        public static string WorkstationID
        {
            get
            {
                // The basics that we always have to have are the user and computer
                string basics = string.Format("{0:X5}{1:X5}",
                    (UserSession.User != null) ? (UserSession.User.UserID - 2) / 1000 : 0,
                    (UserSession.Computer != null) ? (UserSession.Computer.ComputerID - 1) / 1000 : 0);

                // We start with just the basics
                string workStationID = basics;

                // If we auditing is disabled, or reason is specified, then we need to add that on
                if (AuditBehaviorScope.IsDisabled || AuditReason.ReasonType != AuditReasonType.Default)
                {
                    // Default to enabled
                    char auditEnabledFlag = 'E';

                    if (AuditBehaviorScope.IsDisabled)
                    {
                        // 'S' means disabled due to store delete, 'D' means just disabled
                        auditEnabledFlag = DeletionService.IsDeletingStore ? 'S' : 'D';
                    }

                    string additional = string.Format("{0}{1:X1}{2}",
                        auditEnabledFlag, 
                        (int) AuditReason.ReasonType,
                        AuditReason.ReasonDetail);

                    // SQL Server limits this to a max of 128.  The "- 2" is to make room for our terminator
                    workStationID += StringUtility.Truncate(additional, 128 - workStationID.Length - 2) + "@;";
                }

                return workStationID;
            }
        }

        /// <summary>
        /// An identifier that uniquely identifies the database to which the current session is connected.
        /// </summary>
        public static string DatabaseID
        {
            get
            {
                return databaseID;
            }
        }

        /// <summary>
        /// Indiciates if a user is currently logged on to ShipWorks
        /// </summary>
        public static bool IsLoggedOn
        {
            get { return User != null; }
        }

        /// <summary>
        /// Attempt to automatically logon to the system as the last user to use ShipWorks.
        /// </summary>
        public static bool LogonLastUser()
        {
            loggedInUser = null;
            securityContext = null;

            lastUsername = "";
            lastPassword = "";
            lastRemember = false;

            lastRemember = GetSavedUserCredentials(out lastUsername, out lastPassword);
            if (lastRemember)
            {
                return Logon(lastUsername, lastPassword, lastRemember);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the user credentials that were saved as a part of "Log me in automatically"
        /// </summary>
        public static bool GetSavedUserCredentials(out string username, out string password)
        {
            username = "";
            password = "";

            // If the file does not exist, do nothing
            if (!File.Exists(SettingsFilename))
            {
                return false;
            }

            try
            {
                XmlDocument xmlSettings = new XmlDocument();
                xmlSettings.Load(SettingsFilename);

                // Create the XPath to read the settings
                XPathNavigator xpath = xmlSettings.CreateNavigator();

                // Load the settings
                username = XPathUtility.Evaluate(xpath, "//Username", "");

                bool remember = XPathUtility.Evaluate(xpath, "//Remember", false);
                if (remember)
                {
                    password = SecureText.Decrypt(XPathUtility.Evaluate(xpath, "//Password", ""), username);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (XmlException ex)
            {
                log.Error("Error reading saved user credentials.", ex);

                return false;
            }
        }

        /// <summary>
        /// Attempt to login the given ShipWorks user.
        /// </summary>
        public static bool Logon(string username, string password, bool remember)
        {
            return Logon(username, password, remember, true);
        }

        /// <summary>
        /// Attempt to login the given ShipWorks user.
        /// </summary>
        private static bool Logon(string username, string password, bool remember, bool audit)
        {
            loggedInUser = UserUtility.GetShipWorksUser(username, password);

            log.InfoFormat("Login for user '{0}' {1}.", username, loggedInUser == null ? "failed" : "succeeded");

            // If we got a user, its the one we need.
            if (loggedInUser != null)
            {
                // Implements "Remember Me"
                SaveLastUser(username, password, remember);

                // Load the user's security context
                securityContext = new SecurityContext(loggedInUser);

                // Audit the logon
                if (audit)
                {
                    AuditUtility.Audit(AuditActionType.Logon);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Logoff the currently logged on user.
        /// </summary>
        public static void Logoff(bool clearRememberMe)
        {
            if (loggedInUser == null)
            {
                throw new InvalidOperationException("No user is logged on.");
            }

            log.InfoFormat("Logging off '{0}'.", loggedInUser.Username);

            try
            {
                AuditUtility.Audit(AuditActionType.Logoff);
            }
            // Catch everything here since we may be trying to logoff due to a crash
            catch (Exception ex)
            {
                log.Error("Could not audit logoff to database.", ex);
            }

            if (clearRememberMe)
            {
                SaveLastUser(loggedInUser.Username, "", false);
            }

            loggedInUser = null;
            securityContext = null;
        }

        /// <summary>
        /// The last successful username to logon to the system.
        /// </summary>
        public static string LastSuccessfulUsername
        {
            get
            {
                return lastUsername;
            }
        }

        /// <summary>
        /// Save the given user information as the last user to use the system.
        /// </summary>
        private static void SaveLastUser(string username, string password, bool remember)
        {
            // Save this as the last user that got logged on
            lastUsername = username;
            lastPassword = password;
            lastRemember = remember;

            XmlTextWriter xmlWriter = new XmlTextWriter(SettingsFilename, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            xmlWriter.WriteStartDocument();

            // Open
            xmlWriter.WriteStartElement("User");

            // Credentials
            xmlWriter.WriteStartElement("Credentials");
            xmlWriter.WriteElementString("Username", username);

            // Dont save the the password if not remembering
            if (remember)
            {
                xmlWriter.WriteElementString("Remember", bool.TrueString);
                xmlWriter.WriteElementString("Password", username.Length > 0 ? SecureText.Encrypt(password, username) : "");
            }

            xmlWriter.WriteEndElement();

            // Close
            xmlWriter.WriteEndElement();

            // Close
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        /// <summary>
        /// Determine the unique identifier for the database we are connected to
        /// </summary>
        private static void AcquireDatabaseID()
        {
            // This will get the database guid.
            SystemDataEntity systemData = SystemData.Fetch();

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // The guid isnt enough.  They could restore the database to a different path, essentially copying it.  In which
                // case the guid will be the same, but the path will be different.
                string targetPhysDb = (string) SqlCommandProvider.ExecuteScalar(con, "select physical_name from sys.database_files where type_desc = 'ROWS'");

                // Of course, they could also copy the same database, on two different machines, that would have the same path. So to
                // uniqueify that, we use the machine name.
                string machineName = SqlSession.Current.GetServerMachineName();

                // Has to be usable in a path, so we replace / with !
                databaseID = string.Format("{0:B}-{1}", 
                    systemData.DatabaseID, 
                    "{" + 
                    UserUtility.HashPassword(targetPhysDb + machineName).Replace('/', '!') + 
                    "}");

                log.InfoFormat("Acquired DatabaseID: {0}", databaseID);
            }
        }

        /// <summary>
        /// Full path to the file where we store our data
        /// </summary>
        public static string SettingsFilename
        {
            get
            {
                return Path.Combine(DataPath.WindowsUserSettings, "user.xml");
            }
        }
    }
}
