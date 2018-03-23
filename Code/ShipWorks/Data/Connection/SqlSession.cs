using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Class for managing the login session and connectivity to SQL Server
    /// </summary>
    public class SqlSession
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlSession));

        // Global instance of the SQL Session
        static SqlSession current;

        readonly SqlSessionConfiguration configuration;

        // Cached properties of the server
        string serverMachineName;
        Version serverVersion;

        private Guid databaseId = Guid.Empty;

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
            : this(new SqlSessionConfiguration())
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public SqlSession(SqlSession copy)
            : this(copy.Configuration)
        {

        }

        /// <summary>
        /// Constructor with configuration
        /// </summary>
        public SqlSession(SqlSessionConfiguration configuration)
        {
            this.configuration = new SqlSessionConfiguration(configuration);

            Configuration.ConnectionChanged += delegate { ConnectionChanged(); };
        }

        /// <summary>
        /// One-time initialization of the SqlSession.
        /// </summary>
        public static void Initialize()
        {
            SqlSession session = new SqlSession();
            session.Configuration.Load();

            // Set the loaded session as current
            Current = session;

            if (IsConfigured && Current.CanConnect())
            {
                // If there is a crash, we are not allowed to ask the database for these values, so ask now so that they get cached.
                Current.GetServerMachineName();
                Current.GetServerVersion();
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

                if (string.IsNullOrEmpty(current.Configuration.DatabaseName) || string.IsNullOrEmpty(current.Configuration.ServerInstance))
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
                current.Configuration.Freeze();
            }
        }

        /// <summary>
        /// Returns DatabaseGuid of database.
        /// </summary>
        public Guid DatabaseIdentifier
        {
            get
            {
                if (databaseId == Guid.Empty)
                {
                    databaseId = GetDatabaseId();
                }

                return databaseId;
            }
        }

        /// <summary>
        /// Returns DatabaseGuid of database.
        /// </summary>
        private static Guid GetDatabaseId()
        {
            try
            {
                using (DbConnection con = Current.OpenConnection())
                {
                    return DbCommandProvider.ExecuteScalar<Guid>(con, "exec GetDatabaseGuid");
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseIdentifierException(ex);
            }
        }

        /// <summary>
        /// Needs to be called whenver a core property of teh connection string changes
        /// </summary>
        private void ConnectionChanged()
        {
            // This forces the time to be regotten the next time asked for
            SqlDateTimeProvider.Current.ResetCache();

            // Set cached properties so that they will get re-populated when asked for.
            serverVersion = null;
            serverMachineName = string.Empty;
        }

        /// <summary>
        /// Open a connection using the current properties of the SqlSession
        /// </summary>
        public DbConnection OpenConnection()
        {
            DbConnection con = DataAccessAdapter.CreateConnection(Configuration.GetConnectionString());

            ConnectionMonitor.OpenConnection(con);

            return con;
        }

        /// <summary>
        /// Open a connection using the current properties of the SqlSession, but with
        /// a timeout based on timeoutInSeconds
        /// </summary>
        public DbConnection OpenConnection(int timeoutInSeconds)
        {
            string sqlConnectionString = ConnectionStringWithTimeout(timeoutInSeconds);
            DbConnection con = DataAccessAdapter.CreateConnection(Configuration.GetConnectionString());

            ConnectionMonitor.OpenConnection(con);

            return con;
        }

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        public bool TestConnection()
        {
            return TestConnection(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        public bool TestConnection(TimeSpan timeout)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(Configuration.GetConnectionString());
            csb.ConnectTimeout = (int) timeout.TotalSeconds;

            // First check to see if the database is in single user mode
            string originalDatabaseName = csb.InitialCatalog;
            csb.InitialCatalog = "master";

            using (DbConnection con = DataAccessAdapter.CreateConnection(csb.ToString()))
            {
                con.Open();

                if (SqlUtility.IsSingleUser(con, originalDatabaseName))
                {
                    return false;
                }

                con.Close();
            }

            // The db isn't single user, so try connecting to it.
            csb.InitialCatalog = originalDatabaseName;
            using (DbConnection con = DataAccessAdapter.CreateConnection(csb.ToString()))
            {
                con.Open();

                ConnectionMonitor.ValidateOpenConnection(con);

                con.Close();
            }

            return true;
        }

        /// <summary>
        /// Returns a flag indicating if a connection can be made to SQL Server.
        /// </summary>
        public bool CanConnect()
        {
            try
            {
                return TestConnection();
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if this session is a connection to a server instance on the local machine
        /// </summary>
        public bool IsLocalServer()
        {
            return GetServerMachineName() == Environment.MachineName;
        }

        /// <summary>
        /// Indicates if we are connected to an instance of SQL Server 2008 or better
        /// </summary>
        public bool IsSqlServer2008OrLater()
        {
            using (DbConnection con = OpenConnection())
            {
                log.InfoFormat("Connected to '{0}', ServerVersion: '{1}'", Configuration.ServerInstance, con.ServerVersion);

                // If its an old version of SQL Server
                return new Version(con.ServerVersion) >= new Version(10, 0);
            }
        }

        /// <summary>
        /// Indicates if we are connected to a 64bit instance of SQL Server
        /// </summary>
        public bool Is64Bit()
        {
            using (DbConnection con = OpenConnection())
            {
                string version = (string) DbCommandProvider.ExecuteScalar(con, "SELECT @@Version");
                log.InfoFormat("SQL Server version: {0}", version);

                return !version.ToLower().Contains("x86");
            }
        }

        /// <summary>
        /// Gets the running version of the SQL Server database engine
        /// </summary>
        public Version GetServerVersion()
        {
            if (serverVersion == null)
            {
                using (DbConnection con = OpenConnection())
                {
                    serverVersion = new Version(con.ServerVersion);
                }
            }

            return serverVersion;
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
                    using (DbConnection con = OpenConnection())
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
        [NDependIgnoreLongMethod]
        public List<string> DetermineMissingPermissions(SqlSessionPermissionSet permissionSet)
        {
            List<string> missing = new List<string>();

            // If it's not a 2008 and later database, the functions we use to check don't even exist
            if (!IsSqlServer2008OrLater())
            {
                return missing;
            }

            // We always need the server level stuff
            if (true)
            {
                // Make sure we can view server state
                try
                {
                    using (DbConnection con = OpenConnection())
                    {
                        DbCommandProvider.ExecuteNonQuery(con, "SELECT transaction_id FROM sys.dm_tran_current_transaction");
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
                    using (DbConnection con = OpenConnection())
                    {
                        DbCommandProvider.ExecuteScalar(con, "SELECT SystemDataID FROM SystemData");
                    }

                    // Now try to write it
                    using (DbConnection con = OpenConnection())
                    {
                        DbCommandProvider.ExecuteScalar(con, "UPDATE SystemData SET TemplateVersion = TemplateVersion");
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
                    using (DbConnection con = OpenConnection())
                    {
                        // Check for being able to execute sprocs
                        if (!SqlUtility.CheckDatabasePermission(con, "EXECUTE"))
                        {
                            missing.Add("'EXECUTE' permission on all objects.");
                        }
                    }

                    // Have to be able to VIEW CHANGE TRACKING on change-tracked tables
                    using (DbConnection con = OpenConnection())
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
                using (DbConnection con = OpenConnection())
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
            using (DbConnection con = OpenConnection())
            {
                return 0 != (int) DbCommandProvider.ExecuteScalar(con, "select value_in_use from sys.configurations where name = 'clr enabled'");
            }
        }

        /// <summary>
        /// Get the machine name of the server that sql server is installed
        /// </summary>
        public string GetServerMachineName()
        {
            if (string.IsNullOrWhiteSpace(serverMachineName))
            {
                using (DbConnection con = OpenConnection())
                {
                    serverMachineName = (string) DbCommandProvider.ExecuteScalar(con, "SELECT SERVERPROPERTY( 'MachineName' )");
                }
            }

            return serverMachineName;
        }

        /// <summary>
        /// The current configuration of the session
        /// </summary>
        public SqlSessionConfiguration Configuration
        {
            get { return configuration; }
        }

        /// <summary>
        /// Saves the state of the SqlSession object and sets it as the current sql session.
        /// </summary>
        public void SaveAsCurrent()
        {
            Configuration.Save();

            Current = this;
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

        /// <summary>
        /// Gets a connection string, based on specified ConnectionString, and modifies it to have a new
        /// number of minutes for the timeout.
        /// </summary>
        private static string ConnectionStringWithTimeout(string sqlConnectionString, int timeoutSeconds)
        {
            if (!string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(SqlAdapter.Default.ConnectionString)
                {
                    ConnectTimeout = timeoutSeconds
                };

                sqlConnectionString = sqlConnectionStringBuilder.ConnectionString;
            }

            return sqlConnectionString;
        }

        /// <summary>
        /// Gets a connection string, based on SqlSession.Current.Configuration.GetConnectionString(), and modifies it to have a new
        /// number of minutes for the timeout.
        /// </summary>
        private static string ConnectionStringWithTimeout(int timeoutSeconds)
        {
            string sqlConnectionString = Current.Configuration.GetConnectionString();

            return ConnectionStringWithTimeout(sqlConnectionString, timeoutSeconds);
        }
    }
}
