﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using log4net;
using Microsoft.Win32;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Utility class for enumerating and working with sql instance names
    /// </summary>
    public static class SqlInstanceUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlInstanceUtility));
        private const string LocalDBInstalledRegistryKey = @"SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions";

        /// <summary>
        /// The name given to the special default instance of sql server
        /// </summary>
        public static string DefaultInstanceName
        {
            get { return "MSSQLSERVER"; }
        }

        /// <summary>
        /// The default password ShipWorks uses for sa when it installs new SQL instances
        /// </summary>
        public static string ShipWorksSaPassword
        {
            get { return "ShipW@rks1"; }
        }

        /// <summary>
        /// Get the server instance connection string for LocalDB
        /// </summary>
        public static string LocalDbServerInstance
        {
            get
            {
                //See if 2017, 2016 or 2014 LocalDB exists
                if (RegistryHelper.RegistrySubKeyExists(Registry.LocalMachine, $"{LocalDBInstalledRegistryKey}\\14.0") ||
                    RegistryHelper.RegistrySubKeyExists(Registry.LocalMachine, $"{LocalDBInstalledRegistryKey}\\13.0") ||
                    RegistryHelper.RegistrySubKeyExists(Registry.LocalMachine, $"{LocalDBInstalledRegistryKey}\\12.0"))
                {
                    return @"(LocalDB)\MSSQLLocalDB";
                }

                return @"(LocalDB)\V11.0";
            }
        }

        /// <summary>
        /// Get the server instance that ShipWorks should connect to in simple\automatic mode.  This either returns the LocalDb server instance,
        /// or the SQL Server instance that was upgraded to from LocalDB
        /// </summary>
        public static string AutomaticServerInstance
        {
            get
            {
                // First, see if there is already an instance name recorded
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Interapptive\ShipWorks\Database"))
                {
                    if (key != null)
                    {
                        string value = key.GetValue("Automatic") as string;

                        if (value != null)
                        {
                            // Only if it's installed...
                            if (SqlInstanceUtility.IsSqlInstanceInstalled(value))
                            {
                                string serverInstance = Environment.MachineName + "\\" + value;

                                // ... and we can connect to it, do we try to reuse an existing instance
                                if (SqlInstanceUtility.DetermineCredentials(serverInstance) != null)
                                {
                                    return serverInstance;
                                }
                            }
                        }
                    }
                }

                // If there was no automatic full instance, fallback on the LocalDb server instance
                return LocalDbServerInstance;
            }
        }

        /// <summary>
        /// Extracts just the server name part from the SERVER\Instance representation.
        /// </summary>
        public static string ExtractServerName(string serverInstance)
        {
            MethodConditions.EnsureArgumentIsNotNull(serverInstance, nameof(serverInstance));

            string server;

            // Extract just the instance name
            string[] parts = serverInstance.Split('\\');
            if (parts.Length < 1)
            {
                server = "";
            }
            else
            {
                server = parts[0];
            }

            return server;
        }

        /// <summary>
        /// Extracts just the instance name part from the SERVER\Instance representation.  If there is no instance name, just the server name,
        /// the empty string is returned.
        /// </summary>
        public static string ExtractInstanceName(string serverInstance)
        {
            MethodConditions.EnsureArgumentIsNotNull(serverInstance, nameof(serverInstance));

            string instance;

            // Extract just the instance name
            string[] parts = serverInstance.Split('\\');
            if (parts.Length < 2 || parts[1].Length == 0)
            {
                instance = "";
            }
            else
            {
                instance = parts[1];
            }

            return instance;
        }

        /// <summary>
        /// Get the instanceID of the given sql server instance name
        /// </summary>
        public static string GetInstanceID(string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
            {
                instanceName = DefaultInstanceName;
            }

            RegistryKey key = null;

            try
            {
                key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL");

                if (key == null && MyComputer.Is64BitProcess)
                {
                    // Try the 32 bit node
                    key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\Instance Names\SQL");
                }

                if (key == null)
                {
                    throw new NotFoundException("Could not find Instance Names\\SQL key");
                }

                string instanceID = key.GetValue(instanceName) as string;
                if (string.IsNullOrEmpty(instanceID))
                {
                    throw new NotFoundException(string.Format("Could not find key for instance name {0}", instanceName));
                }

                log.InfoFormat("Found instanceID '{0}' for '{1}'", instanceID, instanceName);

                return instanceID;
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
            }
        }

        /// <summary>
        /// Determines if the given sql server instance is installed.
        /// </summary>
        public static bool IsSqlInstanceInstalled(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = DefaultInstanceName;
            }

            if (IsSqlInstanceInstalled(name, @"SOFTWARE\Microsoft\Microsoft SQL Server"))
            {
                return true;
            }

            if (MyComputer.Is64BitProcess)
            {
                return IsSqlInstanceInstalled(name, @"SOFTWARE\WOW6432Node\Microsoft\Microsoft SQL Server");
            }

            return false;
        }

        /// <summary>
        /// Determnins if SQL Server LocalDB is installed
        /// </summary>
        public static bool IsLocalDbInstalled()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions\11.0"))
            {
                return key != null;
            }
        }

        /// <summary>
        /// Determines if the given instance name is installed by looking in the specified registry which is a known parent of a SQL Server "InstalledInstances" value
        /// </summary>
        private static bool IsSqlInstanceInstalled(string name, string registrySubKey)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registrySubKey))
            {
                if (key != null)
                {
                    string[] instances = key.GetValue("InstalledInstances") as string[];

                    if (instances != null)
                    {
                        foreach (string instance in instances)
                        {
                            if (string.Equals(instance, name, StringComparison.CurrentCultureIgnoreCase))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// See if we can figure out the credentials necessary to connect to the given instance.  If provided, the configuration given in firstTry will be attempted first
        /// </summary>
        public static SqlSessionConfiguration DetermineCredentials(string instance, SqlSessionConfiguration firstTry = null)
        {
            List<SqlSessionConfiguration> configsToTry = new List<SqlSessionConfiguration>();

            // If firstTry was given, and we are connecting to the same instance it represents, use it's config first as it will likely work.
            if ((firstTry != null) &&
                (firstTry.WindowsAuth || !string.IsNullOrWhiteSpace(firstTry.Username)) &&
                (firstTry.ServerInstance == instance))
            {
                configsToTry.Add(new SqlSessionConfiguration(firstTry));
            }

            // Then we'll try the sa account with the password we create - we know that'd be an admin
            configsToTry.Add(new SqlSessionConfiguration()
            {
                Username = "sa",
                Password = SqlInstanceUtility.ShipWorksSaPassword,
                WindowsAuth = false
            });


            // Then we'll try windows auth
            configsToTry.Add(new SqlSessionConfiguration()
            {
                WindowsAuth = true
            });

            foreach (SqlSessionConfiguration config in configsToTry)
            {
                try
                {
                    config.ServerInstance = instance;
                    config.DatabaseName = "";

                    SqlSession session = new SqlSession(config);
                    session.TestConnection(TimeSpan.FromSeconds(3));

                    return config;
                }
                catch (SqlException ex)
                {
                    log.Info("Failed to connect.", ex);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a list of all SQL Server instances running on the LAN
        /// </summary>
        public static string[] GetRunningSqlServers()
        {
            SqlDataSourceEnumerator sqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance;
            DataTable table = sqlDataSourceEnumerator.GetDataSources();

            IEnumerable<string> servers = table
                .Rows
                .Cast<DataRow>()
                .Where(dr => !string.IsNullOrWhiteSpace(dr["ServerName"]?.ToString()))
                .Select(dr =>
                {
                    string backSlash = string.IsNullOrWhiteSpace(dr["InstanceName"]?.ToString()) ? string.Empty : @"\";
                    return $"{dr["ServerName"]}{backSlash}{dr["InstanceName"]}";
                });

            return servers.OrderBy(a => a).ToArray();
        }
    }
}
