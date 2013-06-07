using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Users;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;


namespace ShipWorks.Data.Connection
{
    public class SqlSessionConfiguration : ISqlSessionConfiguration
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlSessionConfiguration));

        // Default connection timeout
        static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);


        // Used to track change to the connection string for logging purposes
        [ThreadStatic]
        static string lastConnectionString;

        // File from which to save and restore settings
        static string filename;

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


        // Instance of SQL Server the app uses to connect to
        string serverInstance = string.Empty;

        // The database name to connect to
        string databaseName = string.Empty;

        // SQL Server Username
        string username = string.Empty;

        // SQL Server Password
        string password = string.Empty;

        // Remember password
        bool rememberPassword = false;

        // Use windows authentication
        bool windowsAuth = false;

        bool frozen = false;


        public SqlSessionConfiguration() { }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public SqlSessionConfiguration(SqlSessionConfiguration copy)
        {
            CopyFrom(copy);
        }


        public void CopyFrom(ISqlSessionConfiguration copy)
        {
            if (null == copy)
                throw new ArgumentNullException("copy");

            if (frozen)
            {
                throw new InvalidOperationException("Cannot modify the current SqlSessionConfiguration.");
            }

            this.serverInstance = copy.ServerInstance;
            this.databaseName = copy.DatabaseName;
            this.username = copy.Username;
            this.password = copy.Password;
            this.rememberPassword = copy.RememberPassword;
            this.windowsAuth = copy.WindowsAuth;

            OnConnectionChanged();
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
                password = SecureText.Encrypt(tempDecrypt, username);

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

                password = SecureText.Encrypt(value, username);
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// Set whether the password should be saved to disk.
        /// </summary>
        public bool RememberPassword
        {
            get
            {
                return rememberPassword;
            }
            set
            {
                if (frozen)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSessionConfiguration.");
                }

                rememberPassword = value;
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
                if (frozen)
                {
                    throw new InvalidOperationException("Cannot modify the current SqlSessionConfiguration.");
                }

                windowsAuth = value;
                OnConnectionChanged();
            }
        }


        public event EventHandler ConnectionChanged;

        void OnConnectionChanged()
        {
            var handlers = this.ConnectionChanged;
            if (null != handlers)
                handlers(this, EventArgs.Empty);
        }


        public void Clear()
        {
            CopyFrom(new SqlSessionConfiguration());
        }

        public void Freeze()
        {
            this.frozen = true;
        }


        /// <summary>
        /// Creates the connection string.
        /// </summary>
        public string GetConnectionString()
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
            csb.ConnectTimeout = (int)defaultTimeout.TotalSeconds;

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
        /// Loads configuration state from disk.
        /// </summary>
        public void Load()
        {
            Clear();

            log.InfoFormat("Loading SqlSessionConfiguration from {0}.", SettingsFile);

            // If the file does not exist, do nothing
            if (!File.Exists(SettingsFile))
            {
                log.Info("SqlSessionConfiguration file not found.");
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
            rememberPassword = XPathUtility.Evaluate(xpath, "//Remember", false);
            windowsAuth = XPathUtility.Evaluate(xpath, "//WindowsAuth", false);

            OnConnectionChanged();
        }

        /// <summary>
        /// Saves the configuration state to disk.
        /// </summary>
        public void Save()
        {
            using (var xmlWriter = new XmlTextWriter(SettingsFile, Encoding.UTF8))
            {
                xmlWriter.Formatting = Formatting.Indented;

                xmlWriter.WriteStartDocument();

                InternalSave(xmlWriter);

                // Close
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
        }

        /// <summary>
        /// Save the settings to the given xml writer
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
            xmlWriter.WriteElementString("Password", rememberPassword ? password : "");
            xmlWriter.WriteElementString("Remember", rememberPassword.ToString());
            xmlWriter.WriteElementString("WindowsAuth", windowsAuth.ToString());
            xmlWriter.WriteEndElement();

            // Close
            xmlWriter.WriteEndElement();
        }
    }
}
