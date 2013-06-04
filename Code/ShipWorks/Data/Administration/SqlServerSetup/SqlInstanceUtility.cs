﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using Interapptive.Shared.Utility;
using log4net;
using Interapptive.Shared;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Utility class for enumerating and working with sql instance names
    /// </summary>
    public static class SqlInstanceUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlInstanceUtility));

        /// <summary>
        /// The name given to the special default instance of sql server
        /// </summary>
        public static string DefaultInstanceName
        {
            get { return "MSSQLSERVER"; }
        }

        /// <summary>
        /// Extracts just the instance name part from the SERVER\Instance representation.  If there is no instance name, just the server name,
        /// the empty string is returned.
        /// </summary>
        public static string ExtractInstanceName(string serverInstance)
        {
            if (serverInstance == null)
            {
                throw new ArgumentNullException("serverInstance");
            }

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
        /// Returns a list of all SQL Server instances running on the LAN
        /// </summary>
        public static string[] GetRunningSqlServers()
        {
            // Will get the input through p/invoke
            StringBuilder servers = new StringBuilder(5000);

            // The final result set
            string[] serverList = new string[0];

            // P/Invoke
            if (SqlEnumServers(servers, servers.MaxCapacity))
            {
                string[] rawList = servers.ToString().Split('\n');

                // Create the server list
                serverList = new string[rawList.Length];

                // Copy them all in, extracting only the server and instance name
                for (int i = 0; i < rawList.Length; i++)
                {
                    string server = rawList[i];

                    if (server.IndexOf(";") != -1)
                    {
                        serverList[i] = server.Substring(0, server.IndexOf(";"));
                    }
                    else
                    {
                        serverList[i] = server;
                    }
                }
            }

            return serverList;
        }

        [DllImport("ShipWorks.Native.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SqlEnumServers(StringBuilder servers, int maxCount);
    }
}
