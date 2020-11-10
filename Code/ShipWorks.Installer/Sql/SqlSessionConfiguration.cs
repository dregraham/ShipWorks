using System;
using System.Data.SqlClient;
using System.Threading;
using log4net;
using ShipWorks.Installer.Security;

namespace ShipWorks.Installer.Sql
{
    /// <summary>
    /// Configuration settings storage for SqlSession instances
    /// </summary>
    public class SqlSessionConfiguration
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlSessionConfiguration));

        // Default connection timeout
        static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);

        // Used to track change to the connection string for logging purposes
        static AsyncLocal<string> lastConnectionString = new AsyncLocal<string>();

        // Instance of SQL Server the app uses to connect to
        string serverInstance = string.Empty;

        // The database name to connect to
        string databaseName = string.Empty;

        // SQL Server Username
        string username = string.Empty;

        // SQL Server Password
        string password = string.Empty;

        // Use windows authentication
        bool windowsAuth = false;

        // Indicates if the configuration is readonly
        bool frozen = false;

        /// <summary>
        /// Raised when the configuration changes
        /// </summary>
        public event EventHandler ConnectionChanged;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SqlSessionConfiguration()
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public SqlSessionConfiguration(SqlSessionConfiguration copy)
        {
            CopyFrom(copy);
        }

        /// <summary>
        /// Copy from the given configuration into this one
        /// </summary>
        public void CopyFrom(SqlSessionConfiguration copy)
        {
            if (copy == null)
            {
                throw new ArgumentNullException("copy");
            }

            if (frozen)
            {
                throw new InvalidOperationException("Cannot modify the current SqlSessionConfiguration.");
            }

            // Important to copy the raw values here, not the properties, as the properties do some processing on the data coming out
            // (decrypting, forcing windows auth, etc)
            this.serverInstance = copy.serverInstance;
            this.databaseName = copy.databaseName;
            this.username = copy.username;
            this.password = copy.password;
            this.windowsAuth = copy.windowsAuth;

            //OnConnectionChanged();
        }

        /// <summary>
        /// Called to raise the ConnectionChanged event
        /// </summary>
        void OnConnectionChanged() => ConnectionChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Clear the configuration
        /// </summary>
        public void Clear()
        {
            CopyFrom(new SqlSessionConfiguration());
        }

        /// <summary>
        /// Freeze the configuration from being changed
        /// </summary>
        public void Freeze()
        {
            this.frozen = true;
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
                if (frozen)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSessionConfiguration.");
                }

                serverInstance = value;

                OnConnectionChanged();
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
                if (frozen)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSessionConfiguration.");
                }

                databaseName = value;
                OnConnectionChanged();
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
                if (frozen)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSessionConfiguration.");
                }

                string tempDecrypt = Password;

                username = value;
                password = string.IsNullOrWhiteSpace(tempDecrypt) ? tempDecrypt : SecureText.Encrypt(tempDecrypt, username);

                OnConnectionChanged();
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
                if (frozen)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSessionConfiguration.");
                }

                password = string.IsNullOrWhiteSpace(value) ? value : SecureText.Encrypt(value, username);
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// Whether to use windows authentication
        /// </summary>
        public bool WindowsAuth
        {
            get
            {
                if (IsLocalDb())
                {
                    return true;
                }

                return windowsAuth;
            }
            set
            {
                if (frozen)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSessionConfiguration.");
                }

                windowsAuth = value;
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// Indicates if this session is connected to SQL Server Local DB
        /// </summary>
        public bool IsLocalDb()
        {
            return serverInstance.Contains(@"(LocalDB)", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Creates the connection string.
        /// </summary>
        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(ServerInstance))
            {
                return string.Empty;
            }

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();

            csb.ApplicationName = "ShipWorks Installer";
            csb.DataSource = ServerInstance;
            csb.InitialCatalog = DatabaseName;

            csb.IntegratedSecurity = WindowsAuth;

            if (!windowsAuth)
            {
                csb.UserID = Username;
                csb.Password = Password;
            }

            // Timeout for connect
            csb.ConnectTimeout = (int) defaultTimeout.TotalSeconds;

            // http://blogs.msdn.com/florinlazar/archive/2008/05/05/8460156.aspx
            csb.TransactionBinding = "Explicit Unbind";

            // Generate the connection string
            string connectionString = csb.ConnectionString;

            // If it's different than the last time log it
            if (connectionString != lastConnectionString.Value)
            {
                var logCsb = new SqlConnectionStringBuilder(connectionString);

                // Have to make sure the password is stripped
                if (!windowsAuth)
                {
                    logCsb.Password = "REDACTED";
                }

                log.Info(string.Format("ConnectionString: {0}", logCsb));

                lastConnectionString.Value = connectionString;
            }

            return connectionString;
        }

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            SqlSessionConfiguration other = obj as SqlSessionConfiguration;
            if ((object) other == null)
            {
                return false;
            }

            // Same server and database...
            if (this.ServerInstance == other.ServerInstance && this.DatabaseName == other.DatabaseName)
            {
                // If they are both windows auth, then it's equal (doesn't matter what user\pass may be stored)
                if (this.WindowsAuth && other.WindowsAuth)
                {
                    return true;
                }

                // Same auth (password) so we can get out
                if (!this.WindowsAuth && !other.WindowsAuth &&
                    this.Username == other.Username &&
                    this.Password == other.Password)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Operator==
        /// </summary>
        public static bool operator ==(SqlSessionConfiguration left, SqlSessionConfiguration right)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(left, right))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object) left == null) || ((object) right == null))
            {
                return false;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Operator!=
        /// </summary>
        public static bool operator !=(SqlSessionConfiguration left, SqlSessionConfiguration right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            return serverInstance.GetHashCode() + databaseName.GetHashCode() + username.GetHashCode() + windowsAuth.GetHashCode();
        }
    }
}
