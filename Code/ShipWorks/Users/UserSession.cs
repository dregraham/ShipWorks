using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Email.Accounts;
using ShipWorks.FileTransfer;
using ShipWorks.Filters;
using ShipWorks.Filters.Grid;
using ShipWorks.Filters.Search;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Settings.Printing;
using ShipWorks.Stores.Communication;
using ShipWorks.Templates;
using ShipWorks.Templates.Media;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Security;

namespace ShipWorks.Users
{
    /// <summary>
    /// Class for managing the current session of ShipWorks
    /// </summary>
    public static class UserSession
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(UserSession));

        // The currently logged in user and his security context
        private static UserEntity loggedInUser;
        private static SecurityContext securityContext;

        // The current running computer
        private static ComputerEntity thisComputer;

        // Uniquely identifies the database to which we are connected
        private static string databaseID = null;

        // The username of the last user to successfully logon
        private static string lastUsername = "";
        private static string lastPassword = "";
        private static bool lastRemember;
        private static ILifetimeScope lifetimeScope;

        /// <summary>
        /// One-time initialization of the session
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            InitializeForCurrentDatabase(Program.ExecutionMode);
        }

        /// <summary>
        /// One-time initialization of the session overloaded to be used for integration testing purposes.
        /// </summary>
        public static void InitializeForCurrentDatabase(ExecutionMode executionMode)
        {
            if (executionMode == null)
            {
                // Fall back to use the program's execution mode if none is provided
                executionMode = Program.ExecutionMode;
            }

            // Reset any cached entity data
            ShippingManager.InitializeForCurrentDatabase();

            // Initialize database scope things
            ConfigurationData.InitializeForCurrentDatabase();
            DataResourceManager.InitializeForCurrentDatabase();

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                foreach (IInitializeForCurrentDatabase service in scope.Resolve<IEnumerable<IInitializeForCurrentDatabase>>())
                {
                    service.InitializeForCurrentDatabase(executionMode);
                }
            }

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

                // If there is no UI, then we just use the SuperUser
                if (!executionMode.IsUISupported)
                {
                    loggedInUser = SuperUser.Instance;
                }
                else
                {
                    // If we were logged in, try to automatically log back in
                    if (wasLoggedIn)
                    {
                        Logon(lastUsername, lastPassword, lastRemember, lastDatabaseID != databaseID);
                    }
                }
            }
        }

        /// <summary>
        /// Required initialization that must take place after a user logs in to get various resources and managers ready
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void InitializeForCurrentSession(ExecutionMode executionMode)
        {
            ServiceStatusManager.InitializeForCurrentSession();
            ObjectLabelManager.InitializeForCurrentSession();

            GridColumnDefinitionManager.InitializeForCurrentUser();

            FilterContentManager.InitializeForCurrentSession();
            FtpAccountManager.InitializeForCurrentSession();
            TemplateManager.InitializeForCurrentSession();
            LabelSheetManager.InitializeForCurrentSession();
            EmailAccountManager.InitializeForCurrentSession();
            SearchManager.InitializeForCurrentSession();

            ServerMessageManager.InitializeForCurrentUser();

            DownloadManager.InitializeForCurrentSession();
            FilterLayoutContext.InitializeForCurrentSession();
            FilterNodeColumnManager.InitializeForCurrentSession();
            ShippingOriginManager.InitializeForCurrentSession();
            UspsAccountManager.InitializeForCurrentSession();
            EndiciaAccountManager.InitializeForCurrentSession();
            DimensionsManager.InitializeForCurrentSession();
            ShippingProfileManager.InitializeForCurrentSession();
            FedExAccountManager.InitializeForCurrentSession();
            UpsAccountManager.InitializeForCurrentSession();
            ShippingDefaultsRuleManager.InitializeForCurrentSession();
            ShippingPrintOutputManager.InitializeForCurrentSession();
            OnTracAccountManager.InitializeForCurrentSession();
            iParcelAccountManager.InitializeForCurrentSession();

            lifetimeScope?.Dispose();
            lifetimeScope = IoC.BeginLifetimeScope();

            foreach (IInitializeForCurrentSession service in lifetimeScope.Resolve<IEnumerable<IInitializeForCurrentSession>>())
            {
                service.InitializeForCurrentSession();
            }

            if (executionMode.IsUISupported)
            {
                foreach (IInitializeForCurrentUISession service in lifetimeScope.Resolve<IEnumerable<IInitializeForCurrentUISession>>())
                {
                    service.InitializeForCurrentSession();
                }
            }

            // Update restrictions so that they are ready for any code that needs them early
            EditionManager.UpdateRestrictions();
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

            lifetimeScope?.Dispose();
            lifetimeScope = null;
        }

        /// <summary>
        /// The computer on which the user is currently running.
        /// </summary>
        public static ComputerEntity Computer => thisComputer;

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
                // The behavior scope is active, or the SuperUser is actually logged in, use the super user security context
                if (AuditBehaviorScope.IsSuperUserActive || (User != null && User.UserID == SuperUser.UserID))
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
                if (AuditBehaviorScope.ActiveState != AuditState.Enabled || AuditReason.ReasonType != AuditReasonType.Default || DeletionService.IsDeletingStore)
                {
                    // Default to enabled
                    char auditEnabledFlag = 'E';

                    // 'S' means disabled due to store delete, 'D' means just disabled
                    if (DeletionService.IsDeletingStore)
                    {
                        auditEnabledFlag = 'S';
                    }
                    else
                    {
                        switch (AuditBehaviorScope.ActiveState)
                        {
                            case AuditState.Disabled: auditEnabledFlag = 'D'; break;
                            case AuditState.NoDetails: auditEnabledFlag = 'P'; break;
                        }
                    }

                    string additional = string.Format("{0}{1:X1}{2}",
                                                      auditEnabledFlag,
                                                      (int) AuditReason.ReasonType,
                                                      AuditReason.ReasonDetail);

                    // SQL Server limits this to a max of 128.  The "- 2" is to make room for our terminator
                    workStationID += additional.Truncate(128 - workStationID.Length - 2) + "@;";
                }

                return workStationID;
            }
        }

        /// <summary>
        /// An identifier that uniquely identifies the database to which the current session is connected.
        /// </summary>
        public static string DatabaseID => databaseID;

        /// <summary>
        /// Indicates if a user is currently logged on to ShipWorks
        /// </summary>
        public static bool IsLoggedOn => User != null;

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
            loggedInUser = null;
            UserEntity user = UserUtility.GetShipWorksUser(username, password);

            log.InfoFormat("Login for user '{0}' {1}.", username, user == null ? "failed" : "succeeded");

            // If we got a user, its the one we need.
            if (user != null)
            {
                // Implements "Remember Me"
                SaveLastUser(username, password, remember);

                Logon(user, audit);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Log in the specified user with the specified computer
        /// </summary>
        public static void Logon(UserEntity user, ComputerEntity computer, bool audit)
        {
            thisComputer = computer;
            Logon(user, audit);
        }

        /// <summary>
        /// Log in the specified user
        /// </summary>
        public static void Logon(UserEntity user, bool audit)
        {
            loggedInUser = user;

            // Load the user's security context
            securityContext = new SecurityContext(user);

            // Audit the logon
            if (audit)
            {
                AuditUtility.Audit(AuditActionType.Logon);
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

            Messenger.Current.Send(new WindowResettingMessage(Program.MainForm));

            log.InfoFormat("Logging off '{0}'.", loggedInUser.Username);

            // Nothing to do to logoff the SuperUser
            if (loggedInUser.UserID != SuperUser.UserID)
            {
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
            }

            loggedInUser = null;
            securityContext = null;

            if (Program.ExecutionMode.IsUISupported)
            {
                foreach (IInitializeForCurrentUISession service in lifetimeScope.Resolve<IEnumerable<IInitializeForCurrentUISession>>())
                {
                    service.EndSession();
                }
            }
        }

        /// <summary>
        /// The last successful username to logon to the system.
        /// </summary>
        public static string LastSuccessfulUsername => lastUsername;

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
                // The guid isn't enough.  They could restore the database to a different path, essentially copying it.  In which
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
        public static string SettingsFilename => Path.Combine(DataPath.WindowsUserSettings, "user.xml");
    }
}