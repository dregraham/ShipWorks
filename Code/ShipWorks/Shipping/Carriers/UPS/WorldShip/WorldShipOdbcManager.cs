using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using Interapptive.Shared.Data;
using Microsoft.Win32;
using log4net;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// WorldShip implementation of the OdbcManager class.  
    /// </summary>
    public class WorldShipOdbcManager : OdbcManager
    {
        private RegistryKey key;
        static readonly ILog log = LogManager.GetLogger(typeof(WorldShipOdbcManager));

        /// <summary>
        /// The root odbc registry key for UPS WorldShip entries.  
        /// It checks the 32 bit registry key first.  If no ups entries are found, it checks the 64 bit registry key.
        /// If neither 32 or 64 bit are found, 32 bit is returned (as WorldShip tends to install as 32 bit now).
        /// </summary>
        protected override RegistryKey RootKey
        {
            get
            {
                if (key == null)
                {
                    // Check to see if the ups DSNs are in the Wow6432Node
                    RegistryKey localKey = FindUpsEntriesHive(RegistryView.Registry32);
                    if (localKey != null)
                    {
                        key = localKey;
                        return key;
                    }

                    // None found in the 32 node, check the 64 one
                    localKey = FindUpsEntriesHive(RegistryView.Registry64);
                    if (localKey != null)
                    {
                        key = localKey;
                        return key;
                    }

                    // No ups keys found in either 32 or 64, default to 32
                    if (key == null)
                    {
                        // Assume that WS is running as 32 bit, or will be at some point
                        key = RegistryKey.OpenBaseKey(RootHive, RegistryView.Registry32);
                    }
                }

                return key;
            }
        }

        /// <summary>
        /// Finds ups dsn entries for the specified registry view, and returns the hive in which it was found.
        /// </summary>
        /// <param name="regView">The registry view to query</param>
        /// <returns>If a ups dsn is found, return the registry hive it which it was found, otherwise null.</returns>
        private static RegistryKey FindUpsEntriesHive(RegistryView regView)
        {
            // Open the hive for the requested registry view
            try
            {
                using (RegistryKey localKey = RegistryKey.OpenBaseKey(RootHive, regView))
                {
                    const string keyPath = OdbcIniRegPath + "ODBC Data Sources";

                    // If the odbc data sources key exists, open it
                    using (RegistryKey odbcDataSources = localKey.OpenSubKey(keyPath))
                    {
                        if (odbcDataSources != null)
                        {
                            // Get a list of datasources defined in the key
                            List<string> dsnNames = odbcDataSources.GetValueNames().ToList();

                            // See if any of them contain UPS 
                            if (dsnNames.Any(k => k.ToUpperInvariant().Contains("UPS ")))
                            {
                                // At least one exists, return the hive.
                                return localKey;
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
                    throw new OdbcManagerException("ShipWorks was unable to read the ODBC registry.", ex);
                }

                throw;
            }

            return null;
        }
    }
}
