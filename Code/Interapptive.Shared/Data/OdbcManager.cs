using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Principal;
using Microsoft.Win32;
using log4net;

namespace Interapptive.Shared.Data
{
    ///<summary>
    /// Class to assist with creation and removal of ODBC DSN entries
    ///</summary>
    public abstract class OdbcManager : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(OdbcManager));
        protected const string OdbcIniRegPath = @"SOFTWARE\ODBC\ODBC.INI\";
        protected const string OdbcInstIniRegPath = @"SOFTWARE\ODBC\ODBCINST.INI\";
        private Collection<string> driverNames;

        /// <summary>
        /// Constructor
        /// </summary>
        protected OdbcManager()
        {
            driverNames = new Collection<string>();
        }

        /// <summary>
        /// The registry hive from which to start.
        /// </summary>
        protected static RegistryHive RootHive
        {
            get
            {
                // We'll use CurrentUser, as WorldShip can still read it when it's not in LocalMachine
                return RegistryHive.CurrentUser;
            }
        }

        /// <summary>
        /// The root odbc registry key.  
        /// </summary>
        protected abstract RegistryKey RootKey
        {
            get;
        }

        /// <summary>
        /// Gets a registry sub key 
        /// </summary>
        /// <param name="key">The key from which to get a sub key</param>
        /// <param name="name">Name of the sub key to get</param>
        /// <param name="writable">Should the sub key be opened in writable mode</param>
        /// <returns>RegistryKey for name or null if not found</returns>
        /// <exception cref="T:OdbcManagerException" />
        protected static RegistryKey OpenSubKey(RegistryKey key, string name, bool writable)
        {
            RegistryKey subKey;
            try
            {
                if (key == null)
                {
                    // Throw here so it will get converted below to an OdbcManagerException
                    throw new ArgumentNullException("key");
                }

                // Open a sub key
                subKey = key.OpenSubKey(name, writable);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is SecurityException ||
                    ex is ObjectDisposedException)
                {
                    log.Error("ShipWorks was unable to read the ODBC registry.", ex);
                    throw new OdbcManagerException(BuildErrorMessage("ShipWorks was unable to read the ODBC registry.", ex), ex);
                }

                throw;
            }

            return subKey;
        }

        /// <summary>
        /// Creates a registry key for the passed in key
        /// </summary>
        /// <param name="key">Registry key for which to create a sub key</param>
        /// <param name="name">Name of the sub key to create</param>
        /// <returns>RegistryKey</returns>
        /// <exception cref="T:OdbcManagerException" />
        protected static RegistryKey CreateSubKey(RegistryKey key, string name)
        {
            RegistryKey subKey;
            try
            {
                if (key == null)
                {
                    // Throw here so it will get converted below to an OdbcManagerException
                    throw new ArgumentNullException("key");
                }

                // Create a sub key
                subKey = key.CreateSubKey(name);

                if (subKey == null)
                {
                    log.Error("ShipWorks was unable to create an ODBC registry key.");
                    throw new OdbcManagerException("ShipWorks was unable to create a new ODBC registry entry.");
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is SecurityException ||
                    ex is ObjectDisposedException ||
                    ex is UnauthorizedAccessException ||
                    ex is IOException)
                {
                    log.Error("ShipWorks was unable to create an ODBC registry key.", ex);
                    throw new OdbcManagerException(BuildErrorMessage("ShipWorks was unable to create a new ODBC registry entry.", ex), ex);
                }

                throw;
            }

            return subKey;
        }

        /// <summary>
        /// Get a key's value
        /// </summary>
        /// <param name="key">RegistryKey for which to get the value</param>
        /// <param name="name">Name of the key's value to get</param>
        /// <exception cref="T:OdbcManagerException" />
        protected static object GetKeyValue(RegistryKey key, string name)
        {
            try
            {
                // Open a sub key
                return key.GetValue(name);
            }
            catch (Exception ex)
            {
                if (ex is SecurityException ||
                    ex is ObjectDisposedException ||
                    ex is UnauthorizedAccessException ||
                    ex is IOException)
                {
                    log.Error("ShipWorks was unable to read the ODBC registry.", ex);
                    throw new OdbcManagerException(BuildErrorMessage("ShipWorks was unable to read the ODBC registry.", ex), ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Set a RegistryKey's value
        /// </summary>
        /// <param name="key">RegistryKey for which to set a value</param>
        /// <param name="name">Name of the value to set</param>
        /// <param name="value">Value to set</param>
        /// <exception cref="T:OdbcManagerException" />
        protected static void SetKeyValue(RegistryKey key, string name, object value)
        {
            try
            {
                // Set the value for the key
                key.SetValue(name, value);
            }
            catch (Exception ex)
            {
                if (ex is SecurityException ||
                    ex is ObjectDisposedException ||
                    ex is UnauthorizedAccessException ||
                    ex is IOException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    log.Error("ShipWorks was unable to read the ODBC registry.", ex);
                    throw new OdbcManagerException(BuildErrorMessage("ShipWorks was unable to read the ODBC registry.", ex), ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Gets a list of configured ODBC driver names
        /// </summary>
        /// <exception cref="T:OdbcManagerException" />
        private Collection<string> DriverNames
        {
            get
            {
                if (driverNames == null || driverNames.Count == 0)
                {
                    driverNames = new Collection<string>();

                    try
                    {
                        // Lookup driver path from driver name
                        using (var odbcInstRegKey = OpenSubKey(Registry.LocalMachine, OdbcInstIniRegPath, false))
                        {
                            // It's not null, so get the sub key names
                            // If it were null, driverNames will have no entries because there are none.
                            if (odbcInstRegKey != null)
                            {
                                // Add each sub key, which is a driver name, to the list IF it has a valid driver path.
                                foreach (string driverName in odbcInstRegKey.GetSubKeyNames())
                                {
                                    if (!string.IsNullOrWhiteSpace(GetDriverPath(driverName)))
                                    {
                                        driverNames.Add(driverName.ToUpperInvariant());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is SecurityException ||
                            ex is ObjectDisposedException ||
                            ex is UnauthorizedAccessException ||
                            ex is IOException)
                        {
                            log.Error("ShipWorks was unable to read the ODBC registry.", ex);
                            throw new OdbcManagerException(BuildErrorMessage("ShipWorks was unable to find ODBC SQL Server drivers.", ex), ex);
                        }

                        throw;
                    }
                }

                return driverNames;
            }
        }

        /// <summary>
        /// Looks up a driver for the given driver name.
        /// </summary>
        /// <param name="driverName">Name of the driver to look up, "SQL Server" for example.</param>
        /// <returns>True if the driver name was found, otherwise false.</returns>
        /// <exception cref="T:OdbcManagerException" />
        public bool IsValidDriverName(string driverName)
        {
            // Lookup driver path from driver name
            return DriverNames.Contains(driverName.ToUpperInvariant());
        }

        /// <summary>
        /// Looks up the driver path for the given driver name.
        /// </summary>
        /// <param name="driverName">Name of the driver to look up, "SQL Server" for example.</param>
        /// <returns>The path to the driver's dll, or string.Empty if the driver path could not be found.</returns>
        /// <exception cref="T:OdbcManagerException" />
        private static string GetDriverPath(string driverName)
        {
            string driverPath = string.Empty;

            // Lookup driver path from driver name
            using (var driverKey = OpenSubKey(Registry.LocalMachine, OdbcInstIniRegPath + driverName, false))
            {
                if (driverKey == null)
                {
                    return string.Empty;
                }

                var driverValue = GetKeyValue(driverKey, "Driver");
                driverPath = driverValue == null ? string.Empty : driverValue.ToString();
            }

            return driverPath;
        }

        /// <summary>
        /// Creates a new DSN entry with the specified values. If the DSN exists, the values are updated.
        /// </summary>
        /// <param name="dsnName">Name of the DSN for use by client applications</param>
        /// <param name="description">Description of the DSN that appears in the ODBC control panel applet</param>
        /// <param name="server">Network name or IP address of database server</param>
        /// <param name="driverName">Name of the driver to use</param>
        /// <param name="trustedConnection">True to use NT authentication, false to require applications to supply username/password in the connection string</param>
        /// <param name="database">Name of the datbase to connect to</param>
        /// <param name="userName">User name used to connect to the database</param>
        /// <exception cref="T:OdbcManagerException" />
        [NDependIgnoreTooManyParams]
        public void CreateDsn(string dsnName, string description, string server, string driverName, bool trustedConnection, string database, string userName)
        {
            // Add value to odbc data sources
            using (var datasourcesKey = OpenSubKey(RootKey, OdbcIniRegPath + "ODBC Data Sources", true))
            {
                if (datasourcesKey == null)
                {
                    using (RegistryKey newDataSourcesKey = CreateSubKey(RootKey, OdbcIniRegPath + "ODBC Data Sources"))
                    {
                        if (newDataSourcesKey == null)
                        {
                            log.Error("Unable to create dsn because the ODBC Data Soruces registry key was not found or was unable to be opened.");
                            throw new OdbcManagerException("Unable to open the 'ODBC Data Sources' registry entry.");
                        }

                        SetKeyValue(newDataSourcesKey, dsnName, driverName);
                    }
                }
                else
                {
                    SetKeyValue(datasourcesKey, dsnName, driverName);
                }
            }

            // Create new key in odbc.ini with dsn name and add values
            using (var dsnKey = CreateSubKey(RootKey, OdbcIniRegPath + dsnName))
            {
                dsnKey.SetValue("Database", database);
                dsnKey.SetValue("Description", description);
                dsnKey.SetValue("Driver", GetDriverPath(driverName));
                dsnKey.SetValue("LastUser", userName);
                dsnKey.SetValue("Server", server);
                dsnKey.SetValue("Database", database);
                dsnKey.SetValue("Trusted_Connection", trustedConnection ? "Yes" : "No");
            }
        }

        ///<summary>
        /// Checks the registry to see if a DSN exists with the specified name
        ///</summary>
        ///<param name="dsnName">The name of the DSN for which to check.</param>
        ///<returns>True if the DSN entry exists.  False otherwise.</returns>
        ///<exception cref="OdbcManagerException" />
        public bool DsnExists(string dsnName)
        {
            bool exists = false;

            using (var subKey = OpenSubKey(RootKey, OdbcIniRegPath + dsnName, false))
            {
                exists = subKey != null;
            }

            return exists;
        }

        /// <summary>
        /// Helper method to build an error message
        /// </summary>
        private static string BuildErrorMessage(string firstMessage, Exception ex)
        {
            return string.Format("{0}  {1}", firstMessage, ex.Message);
        }

        /// <summary>
        /// Disposes RootKey
        /// </summary>
        public void Dispose()
        {
            if (RootKey != null)
            {
                RootKey.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
