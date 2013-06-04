using System;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Threading;
using Interapptive.Shared;
using log4net;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Data;
using System.Collections.Generic;
using Interapptive.Shared.UI;
using ShipWorks.Data.Administration;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Class for managing the login session and connectivity to SQL Server
    /// </summary>
    public class SqlSession
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlSession));

        // File from which to save and restore settings
        static string filename;

        // Global instance of the SQL Session
        static SqlSession current;

        // Instance of SQL Server the app uses to connect to
        string serverInstance = "";

        // The database name to connect to
        string databaseName = "";

        // SQL Server Username
        string username = "";

        // SQL Server Password
        string password = "";

        // Remember password
        bool remember = false;

        // Use windows authentication
        bool windowsAuth = false;

        // Cached properties of the server
        DateTime serverLocalDate;
        Stopwatch timeSinceTimeTaken;

        // Used to track change to the connection string for logging purposes
        [ThreadStatic]
        static string lastConnectionString;

        // Default connection timeout
        static TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Static constructor
        /// </summary>
        static SqlSession()
        {
            int labelLenth = new ObjectLabelEntity().Fields["Label"].MaxLength;
            if (labelLenth != 100)
            {
                throw new NotSupportedException("Code in the OrderItemLabelTrigger, OrderItemAttributeLabelTrigger, TemplateFolderLabelTrigger, TemplateLabelTrigger assumes a label length of 100 for truncation.  That needs updated now that the length has changed.");
            }

            if (new AuditEntity().Fields["ReasonDetail"].MaxLength != 100)
            {
                throw new NotSupportedException("Have to updated hard-coded value in ShipWorks.SqlServer.General.UserContext.");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlSession()
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public SqlSession(SqlSession copy)
        {
            this.serverInstance = copy.serverInstance;
            this.databaseName = copy.databaseName;
            this.username = copy.username;
            this.password = copy.password;
            this.remember = copy.remember;
            this.windowsAuth = copy.windowsAuth;
        }

        /// <summary>
        /// One-time initializtion of the SqlSession.
        /// </summary>
        public static void Initialize()
        {
            SqlSession session = new SqlSession();
            session.Load();

            // Set the loaded session as current
            Current = session;
        }

        /// <summary>
        /// The file the settings are stored in.
        /// </summary>
        public static string SettingsFile
        {
            get
            {
                if (filename == null)
                {
                    filename = Path.Combine(DataPath.InstanceSettings, "sqlsession.xml");
                }

                return filename;
            }
        }

        /// <summary>
        /// Gets the global instance of the SqlSession object.  If the server and database have not been configured, null is returned.
        /// </summary>
        public static SqlSession Current
        {
            get
            {
                // If there is one in scope, it overrides what we know as current
                if (SqlSessionScope.ScopedSqlSession != null)
                {
                    return SqlSessionScope.ScopedSqlSession;
                }

                if (current == null)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(current.DatabaseName) || string.IsNullOrEmpty(current.ServerInstance))
                {
                    return null;
                }

                return current;
            }

            // Only we can change this internally
            private set
            {
                // Current is immutable, so if its the same object, its the same
                if (current == value)
                {
                    return;
                }

                current = value;
            }
        }

        /// <summary>
        /// Create the connection string based on the sql session
        /// </summary>
        public string GetConnectionString()
        {
            string cs = InternalGetConnectionString(defaultTimeout);

            return cs;
        }

        /// <summary>
        /// Needs to be called whenver a core property of teh connection string changes
        /// </summary>
        private void ConnectionChanged()
        {
            // This forces the time to be regotten the next time asked for
            timeSinceTimeTaken = null;
        }

        /// <summary>
        /// Get the connection string that specified the given timeout value
        /// </summary>
        private string InternalGetConnectionString(TimeSpan timeout)
        {
            if (string.IsNullOrEmpty(ServerInstance))
            {
                return "";
            }

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();

            csb.ApplicationName = "ShipWorks";
            csb.DataSource = ServerInstance;
            csb.InitialCatalog = DatabaseName;

            csb.IntegratedSecurity = WindowsAuth;

            if (!windowsAuth)
            {
                csb.UserID = Username;
                csb.Password = Password;
            }

            // The WorkstationID is how we pass the UserID and ComputerID to use for auditing on this connection
            csb.WorkstationID = UserSession.WorkstationID;

            // Timeout for connect
            csb.ConnectTimeout = (int) timeout.TotalSeconds;

            // http://blogs.msdn.com/florinlazar/archive/2008/05/05/8460156.aspx
            csb.TransactionBinding = "Explicit Unbind";

            // Generate the connection string
            string connectionString = csb.ConnectionString;

            // If it's different than the last time log it
            if (connectionString != lastConnectionString)
            {
                var logCsb = new SqlConnectionStringBuilder(connectionString);

                // Have to make sure the password is stripped
                if (!windowsAuth)
                {
                    logCsb.Password = "";
                }

                log.InfoFormat("ConnectionString: {0}", logCsb);

                lastConnectionString = connectionString;
            }

            return connectionString;
        }
        
        /// <summary>
        /// Open a connection using the current properties of the SqlSession
        /// </summary>
        public SqlConnection OpenConnection()
        {
            SqlConnection con = new SqlConnection(GetConnectionString());

            ConnectionMonitor.OpenConnection(con);

            return con;
        }

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        public void TestConnection()
        {
            TestConnection(defaultTimeout);
        }

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        private void TestConnection(TimeSpan timeout)
        {
            using (SqlConnection con = new SqlConnection(InternalGetConnectionString(timeout)))
            {
                con.Open();

                ConnectionMonitor.ValidateOpenConnection(con);

                con.Close();
            }
        }

        /// <summary>
        /// Returns a flag indicating if a connection can be made to SQL Server.
        /// </summary>
        public bool CanConnect()
        {
            try
            {
                TestConnection();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if this session is
        /// </summary>
        public bool IsLocalServer()
        {
            return GetServerMachineName() == Environment.MachineName;
        }

        /// <summary>
        /// Indicates if we are connected to an instance of SQL Server 2008 or better
        /// </summary>
        public bool IsSqlServer2008()
        {
            using (SqlConnection con = OpenConnection())
            {
                log.InfoFormat("Connected to '{0}', ServerVersion: '{1}'", ServerInstance, con.ServerVersion);

                // If its an old version of SQL Server
                return new Version(con.ServerVersion) >= new Version(10, 0);
            }
        }

        /// <summary>
        /// Indicates if we are connected to a 64bit instance of SQL Server
        /// </summary>
        public bool Is64Bit()
        {
            using (SqlConnection con = OpenConnection())
            {
                string version = (string) SqlCommandProvider.ExecuteScalar(con, "SELECT @@Version");
                log.InfoFormat("SQL Server version: {0}", version);

                return !version.ToLower().Contains("x86");
            }
        }

        /// <summary>
        /// Gets the running version of the SQL Server database engine
        /// </summary>
        public Version GetServerVersion()
        {
            using (SqlConnection con = OpenConnection())
            {
                return new Version(con.ServerVersion);
            }
        }

        /// <summary>
        /// Checks to see if the user on the current connection has the minimum permissions required to run ShipWorks
        /// </summary>
        public bool CheckPermissions(SqlSessionPermissionSet permissionSet, IWin32Window owner)
        {
            List<string> missing = DetermineMissingPermissions(permissionSet);

            if (missing.Count > 0)
            {
                string sqlUser = "Unknown";

                try
                {
                    using (SqlConnection con = OpenConnection())
                    {
                        sqlUser = SqlUtility.GetUsername(con);
                    }
                }
                catch (SqlException)
                {

                }

                string message = string.Format("The SQL Server user '{0}' does not have the permissions needed by ShipWorks to continue:\n\n", sqlUser);

                foreach (string error in missing)
                {
                    message += "     " + error + "\n";
                }

                MessageHelper.ShowError(owner, message);

                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the list of any missing permissions minimally required to run ShipWorks
        /// </summary>
        public List<string> DetermineMissingPermissions(SqlSessionPermissionSet permissionSet)
        {
            List<string> missing = new List<string>();

            // We always need the server level stuff
            if (true)
            {
                // Make sure we can view server state
                try
                {
                    using (SqlConnection con = OpenConnection())
                    {
                        SqlCommandProvider.ExecuteNonQuery(con, "SELECT transaction_id FROM sys.dm_tran_current_transaction");
                    }
                }
                catch (SqlException ex)
                {
                    log.Error("Failed checking for dm_tran_current_transaction permission.", ex);

                    missing.Add("'VIEW SERVER STATE' permission on the server.");
                }
            }

            // For standard we need read\write\execute
            if (permissionSet == SqlSessionPermissionSet.Standard)
            {
                // It's possible that the database isn't a ready-to-go ShipWorks database (needs upgraded, isn't a ShipWorks database, whatever). If that's the case, we don't need
                // to keep checking various permissions once we realize that.
                bool isValidDatabase = true;

                // Make sure we can read and write data
                try
                {
                    // Try to read it
                    using (SqlConnection con = OpenConnection())
                    {
                        SqlCommandProvider.ExecuteScalar(con, "SELECT SystemDataID FROM SystemData");
                    }

                    // Now try to write it
                    using (SqlConnection con = OpenConnection())
                    {
                        SqlCommandProvider.ExecuteScalar(con, "UPDATE SystemData SET TemplateVersion = TemplateVersion");
                    }
                }
                catch (SqlException ex)
                {
                    // We are specifically looking for 229 - "The SELECT\UPDATE\etc permission was denied."  208 for example would be that the object does not exist, which would mean 
                    // that we just haven't setup the ShipWorks database yet
                    if (ex.Number == 229)
                    {
                        log.Error("Failed checking getting schema version (testing for read and write).", ex);

                        missing.Add("'SELECT', 'UPDATE', 'INSERT', 'DELETE' on all tables.");
                    }
                    else if (ex.Number == 208)
                    {
                        log.Warn("The database being checked does not contain SystemData, and is assumed to be an invalid ShipWorks database.");

                        isValidDatabase = false;
                    }
                }

                // Only keep checking if it's a valid ShipWorks database
                if (isValidDatabase)
                {
                    // Have to be able to execute stored procedures and functions
                    using (SqlConnection con = OpenConnection())
                    {
                        // Check for being able to execute sprocs
                        if (!SqlUtility.CheckDatabasePermission(con, "EXECUTE"))
                        {
                            missing.Add("'EXECUTE' permission on all objects.");
                        }
                    }

                    // Have to be able to VIEW CHANGE TRACKING on change-tracked tables
                    using (SqlConnection con = OpenConnection())
                    {
                        // Check for being able to view change tracking.  We just check the 'User' table, but assume that if they don't have it on that they won't on any.
                        if (!SqlUtility.CheckObjectPermission(con, "User", "VIEW CHANGE TRACKING"))
                        {
                            missing.Add("'VIEW CHANGE TRACKING' permission on all tables.");
                        }
                    }
                }
            }

            // For upgrading we need alter
            if (permissionSet == SqlSessionPermissionSet.Upgrade)
            {
                using (SqlConnection con = OpenConnection())
                {
                    // Check for being able to execute sprocs
                    if (!SqlUtility.CheckDatabasePermission(con, "ALTER"))
                    {
                        missing.Add("ShipWorks requires the 'ALTER' permission.");
                    }
                }
            }

            return missing;
        }

        /// <summary>
        /// Checks to see if CLR is enabled on the server
        /// </summary>
        public bool IsClrEnabled()
        {
            using (SqlConnection con = OpenConnection())
            {
                return 0 != (int) SqlCommandProvider.ExecuteScalar(con, "select value_in_use from sys.configurations where name = 'clr enabled'");
            }
        }

        /// <summary>
        /// Get the latest time information from the server. Uses a cache mechanism for efficiency, so
        /// we don't go to the server every invocation.
        /// 
        /// If the time has been retrieved from the server withing the past 30 minutes, then the current time
        /// is estimated by adding the last retrieved time plus the elapsed time.
        /// </summary>
        public DateTime GetLocalDate()
        {
            if (timeSinceTimeTaken != null)
            {
                if (timeSinceTimeTaken.Elapsed < TimeSpan.FromMinutes(30))
                {
                    return serverLocalDate + timeSinceTimeTaken.Elapsed;
                }
            }

            timeSinceTimeTaken = Stopwatch.StartNew();

            using (SqlConnection con = OpenConnection())
            {
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = "SELECT GETDATE() as 'LocalDate'";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    reader.Read();

                    serverLocalDate = (DateTime) reader["LocalDate"];

                    log.InfoFormat("Server LocalDate ({0})", serverLocalDate);

                    return serverLocalDate;
                }
            }
        }

        /// <summary>
        /// Get the machine name of the server that sql server is installed 
        /// </summary>
        public string GetServerMachineName()
        {
            using (SqlConnection con = OpenConnection())
            {
                return (string) SqlCommandProvider.ExecuteScalar(con, "SELECT SERVERPROPERTY( 'MachineName' )");
            }
        }

        /// <summary>
        /// The SQL Server instance to which we connect
        /// </summary>
        public string ServerInstance
        {
            get
            {
                return serverInstance;
            }
            set
            {
                if (this == current)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSession.");
                }

                serverInstance = value;
                ConnectionChanged();
            }
        }

        /// <summary>
        /// The name of the database to which to connect
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return databaseName;
            }
            set
            {
                if (this == current)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSession.");
                }

                databaseName = value;
                ConnectionChanged();
            }
        }

        /// <summary>
        /// The username
        /// </summary>
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (this == current)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSession.");
                }

                string tempDecrypt = Password;

                username = value;
                password = SecureText.Encrypt(tempDecrypt, username);

                ConnectionChanged();
            }
        }

        /// <summary>
        /// The password
        /// </summary>
        public string Password
        {
            get
            {
                return SecureText.Decrypt(password, username);
            }
            set
            {
                if (this == current)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSession.");
                }

                password = SecureText.Encrypt(value, username);
                ConnectionChanged();
            }
        }

        /// <summary>
        /// Set whether the password should be saved to disk.
        /// </summary>
        public bool RememberPassword
        {
            get
            {
                return remember;
            }
            set
            {
                if (this == current)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSession.");
                }

                remember = value;
            }
        }

        /// <summary>
        /// Whether to use windows authentication
        /// </summary>
        public bool WindowsAuth
        {
            get
            {
                return windowsAuth;
            }
            set
            {
                if (this == current)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSession.");
                }

                windowsAuth = value;
                ConnectionChanged();
            }
        }

        /// <summary>
        /// Clear all the fields
        /// </summary>
        private void Clear()
        {
            if (this == current)
            {
                throw new InvalidOperationException("Cannot modify the current SqlSession.");
            }

            serverInstance = string.Empty;
            databaseName = string.Empty;
            username = string.Empty;
            password = string.Empty;
            remember = false;
            windowsAuth = false;

            ConnectionChanged();
        }

        /// <summary>
        /// Load SQL Session state from disk
        /// </summary>
        private void Load()
        {
            Clear();

            log.InfoFormat("Loading SqlSession from {0}.", SettingsFile);

            // If the file does not exist, do nothing
            if (!File.Exists(SettingsFile))
            {
                log.Info("SqlSession file not found.");
                return;
            }

            InternalLoad(File.ReadAllText(SettingsFile));
        }

        /// <summary>
        /// Load the settings based on the given xml string
        /// </summary>
        private void InternalLoad(string xml)
        {
            XmlDocument xmlSettings = new XmlDocument();

            try
            {
                xmlSettings.LoadXml(xml);
            }
            catch (XmlException ex)
            {
                log.Error("Could not load SqlSession XML", ex);

                return;
            }

            XPathNavigator xpath = xmlSettings.CreateNavigator();

            // Load the settings
            serverInstance = XPathUtility.Evaluate(xpath, "//Instance", "");
            databaseName = XPathUtility.Evaluate(xpath, "//Database", "");
            username = XPathUtility.Evaluate(xpath, "//Username", "");
            password = XPathUtility.Evaluate(xpath, "//Password", "");
            remember = XPathUtility.Evaluate(xpath, "//Remember", false);
            windowsAuth = XPathUtility.Evaluate(xpath, "//WindowsAuth", false);

            ConnectionChanged();
        }

        /// <summary>
        /// Saves the state of the SqlSession object and sets it as the current sql session.
        /// </summary>
        public void SaveAsCurrent()
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(SettingsFile, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            xmlWriter.WriteStartDocument();

            InternalSave(xmlWriter);

            // Close
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            Current = this;
        }

        /// <summary>
        /// Save SQL Session state to the given xml writer
        /// </summary>
        private void InternalSave(XmlTextWriter xmlWriter)
        {
            // Open
            xmlWriter.WriteStartElement("SqlSession");

            // Server
            xmlWriter.WriteStartElement("Server");
            xmlWriter.WriteElementString("Instance", serverInstance);
            xmlWriter.WriteElementString("Database", databaseName);
            xmlWriter.WriteEndElement();

            // Credentials
            xmlWriter.WriteStartElement("Credentials");
            xmlWriter.WriteElementString("Username", username);
            xmlWriter.WriteElementString("Password", remember ? password : "");
            xmlWriter.WriteElementString("Remember", remember.ToString());
            xmlWriter.WriteElementString("WindowsAuth", windowsAuth.ToString());
            xmlWriter.WriteEndElement();

            // Close
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// The state of the configuration used to connect to SQL Server
        /// </summary>
        public static bool IsConfigured
        {
            get
            {
                return SqlSession.Current != null;
            }
        }
    }
}
