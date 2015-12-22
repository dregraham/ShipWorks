using System;
using System.IO;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.IO.Text.Ini;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using log4net;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Helper class for integrating ShipWorks with WorldShip
    /// </summary>
    public static class WorldShipIntegrator
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WorldShipIntegrator));
        const string ImportDatFilename = "Import from ShipWorks for Hands-Off.dat";
        const string ExportDatFilename = "Export to ShipWorks.dat";
        const string ImportForKeyedDatFilename = "Import from ShipWorks For Keyed Import.dat";

        /// <summary>
        /// Integrate ShipWorks with WorldShip
        /// Checks that WorldShip is installed, and if so:
        /// - System DSN, creates it if not found.  Does not change it if found.
        /// - If map files are not found, copies them.  Does not copy if found.
        /// - Checks WorldShip ini files and adds settings for DSN and dat files.  Does not modify if settings have values.
        /// </summary>
        /// <exception cref="WorldShipIntegratorException" />
        [NDependIgnoreLongMethod]
        public static bool IntegrateWithWorldShip(IWin32Window owner)
        {
            // Check to see if there are any WorldShipShipment entries in the db.
            int existingEntriesCount = WorldShipShipmentEntriesCount();
            if (existingEntriesCount > 0)
            {
                // See if the user wants to delete them
                DialogResult dialogResult = MessageHelper.ShowQuestion(owner, MessageBoxIcon.Question, MessageBoxButtons.YesNoCancel,
                    string.Format("ShipWorks found {0} WorldShip shipments that were processed in ShipWorks and are waiting to be processed in WorldShip.  " +
                    "{1}{1}If you choose to enable Hands-Off shipping within WorldShip, WorldShip will attempt to process each of this shipments.  If you don’t know what these shipments are, or do not remember processing them in ShipWorks, it is recommended that you remove them from the WorldShip processing list.  This does not remove anything from ShipWorks." +
                    "{1}{1}Remove these shipments from the WorldShip processing list?", existingEntriesCount, Environment.NewLine));

                // Cancel means don't do anything
                if (dialogResult == DialogResult.Cancel)
                {
                    return false;
                }
                
                // Yes means, delete the entries
                // No means, don't delete, but create mappings.
                if (dialogResult == DialogResult.Yes)
                {
                    DeleteExistingWorldShipShipmentEntries();
                }
            }

            // Get the ini location.  We'll use it to get the import/export location
            string worldShipIniFilename = WorldShipUtility.GetWorldShipIniPath();
            if (string.IsNullOrWhiteSpace(worldShipIniFilename))
            {
                // We didn't find WorldShip on this computer...throw to let the user know
                throw new WorldShipIntegratorException("ShipWorks could not find WorldShip installed on this computer.");
            }

            // Get the path of the Ini file
            string worldShipIniPath = Path.GetFullPath(worldShipIniFilename);

            try
            {
                // Attempt to create an ODBC DSN for WorldShip
                CreateDSN();
            }
            catch (OdbcManagerException ex)
            {
                log.Error("Unable to create WorldShip DSN.", ex);
                string msg = string.Format("Unable to create WorldShip DSN.{0}{0}{1}", Environment.NewLine, ex.Message);
                throw new WorldShipIntegratorException(msg, ex);
            }

            // Define the path/filenames for the dat files
            string datFilePath = Path.Combine(worldShipIniPath, "ImpExp\\Shipment");
            string exportDatPathAndFilename = Path.Combine(datFilePath, ExportDatFilename);
            string importDatPathAndFilename = Path.Combine(datFilePath, ImportDatFilename);
            string importForKeyedDataPathAndFilename = Path.Combine(datFilePath, ImportForKeyedDatFilename);

            try
            {
                // Create the maps 
                CreateMaps(exportDatPathAndFilename, importDatPathAndFilename, importForKeyedDataPathAndFilename);
            }
            catch (ResourceUtilityException ex)
            {
                log.Error("ShipWorks encountered an error creating WorldShip map dat files.", ex);
                throw new WorldShipIntegratorException(ex.Message, ex);
            }

            try
            {
                // Check WorldShip ini files and adds settings for DSN, settings, and dat files.  Does not modify if settings have values.
                IniFile wsIni = new IniFile(Path.Combine(worldShipIniPath, "wstdShipmain.ini"));
                wsIni.WriteValue("Preferences", "HandsOffMapName", ImportDatFilename.Replace(".dat", string.Empty));
                wsIni.WriteIniValueIfMissing("Preferences", "HandsOffWaitTime", "00:01");
                wsIni.WriteIniValueIfMissing("Preferences", "HandsOffCheckDuplicatePKey", "Yes");
                wsIni.WriteIniValueIfMissing("Preferences", "HandsOffProcessShipmentAutomatically", "1");
                wsIni.WriteIniValueIfMissing("Preferences", "HandsOffShipperKey", "1");

                // Update the auto export settings
                wsIni = new IniFile(Path.Combine(worldShipIniPath, "wstdAutoExportMaps.ini"));
                wsIni.WriteValue("AutoExportShipments", "ExportMapName", exportDatPathAndFilename);

                // Update the Shipuser settings
                wsIni = new IniFile(Path.Combine(worldShipIniPath, "wstdShipuser.ini"));
                wsIni.WriteValue("UPS OnLine Connect", "AutoExportRecent", ExportDatFilename.Replace(".dat", string.Empty));
                wsIni.WriteValue("UPS OnLine Connect", "KeyedRecent", ImportForKeyedDatFilename.Replace(".dat", string.Empty));
                wsIni.WriteValue("Database", "activeImportMap", importDatPathAndFilename);
            }
            catch (IniFileException ex)
            {
                log.Error("ShipWorks encountered an error modifying WorldShip ini files.", ex);
                throw new WorldShipIntegratorException("Unable to access WorldShip ini file.", ex);
            }

            return true;
        }

        /// <summary>
        /// Deletes all entries from WorldShipShipment
        /// </summary>
        private static void DeleteExistingWorldShipShipmentEntries()
        {
            using (WorldShipShipmentCollection worldShipShipmentEntitiesToDelete = new WorldShipShipmentCollection())
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Fetch the records to import
                    adapter.FetchEntityCollection(worldShipShipmentEntitiesToDelete, null, 0);

                    if (worldShipShipmentEntitiesToDelete.Count > 0)
                    {
                        adapter.DeleteEntityCollection(worldShipShipmentEntitiesToDelete);
                    }

                    adapter.Commit();
                }
            }
        }

        /// <summary>
        /// Determines if any WorldShipShipments are in the WorldShipShipment table.
        /// </summary>
        /// <returns>True if a WorldShipShipment record exists, false otherwise.</returns>
        private static int WorldShipShipmentEntriesCount()
        {
            int numberOfEntries;

            using (WorldShipShipmentCollection existingWorldShipShipmentEntities = new WorldShipShipmentCollection())
            {
                using (SqlAdapter adapter = new SqlAdapter(false))
                {
                    // Fetch the records to import
                    adapter.FetchEntityCollection(existingWorldShipShipmentEntities, null, 0);
                }

                numberOfEntries = existingWorldShipShipmentEntities.Count;
            }

            return numberOfEntries;
        }

        /// <summary>
        /// Creates and ODBC DSN for WorldShip
        /// </summary>
        /// <exception cref="OdbcManagerException" />
        /// <exception cref="WorldShipIntegratorException" />
        private static void CreateDSN()
        {
            // System DSN, creates it if not found.  Does not change it if found.
            const string worldShipDSNName = "ShipWorks WorldShip Connector";
            const string worldShipDsnDescription = "Used to connect WorldShip with ShipWorks v3";

            // Get an instance of the WorldShip ODBC manager
            using (WorldShipOdbcManager worldShipOdbcMgr = new WorldShipOdbcManager())
            {
                bool shipWorksDsnExists;
                try
                {
                    // See if our ShipWorks ODBC connection for world ship exists
                    shipWorksDsnExists = worldShipOdbcMgr.DsnExists(worldShipDSNName);
                }
                catch (OdbcManagerException ex)
                {
                    log.Error(ex.Message, ex);
                    string errorMessage = string.Format("Unable to look up WorldShip DSN.{0}{0}{1}", Environment.NewLine, ex.Message);
                    throw new WorldShipIntegratorException(errorMessage, ex);
                }

                // If the DSN doesn't exist, create it
                if (!shipWorksDsnExists)
                {
                    // Get the db connection info from ShipWorks
                    string dbName = SqlSession.Current.Configuration.DatabaseName;
                    string server = SqlSession.Current.Configuration.ServerInstance;
                    string userName = SqlSession.Current.Configuration.Username;
                    string driverName = FindValidSqlDriverName(worldShipOdbcMgr);

                    if (string.IsNullOrWhiteSpace(driverName))
                    {
                        string errorMessage = string.Format("Unable to find an ODBC SQL Server driver.");
                        log.ErrorFormat(errorMessage);
                        throw new WorldShipIntegratorException(errorMessage);
                    }

                    try
                    {
                        // Create the new DSN
                        // Defaulting to Windows Auth, as that should fit most customers
                        worldShipOdbcMgr.CreateDsn(worldShipDSNName, worldShipDsnDescription, server, driverName, true, dbName, userName);
                    }
                    catch (OdbcManagerException ex)
                    {
                        log.Error(ex.Message, ex);
                        throw new WorldShipIntegratorException(ex.Message, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Saves WorldShip mapping files to disk
        /// </summary>
        /// <param name="exportDatPathAndFilename">Full path and filename where the export dat file should be written</param>
        /// <param name="importDatPathAndFilename">Full path and filename where the import dat file should be written</param>
        /// <param name="importForKeyedDataPathAndFilename">Full path and filename where the keyed import dat file should be written</param>
        /// <exception cref="ResourceUtilityException" />
        private static void CreateMaps(string exportDatPathAndFilename, string importDatPathAndFilename, string importForKeyedDataPathAndFilename)
        {
            // Get the Export map from the resources
            SaveMapToDisk("ShipWorks.Shipping.Carriers.UPS.WorldShip.ExportToShipWorks.dat", exportDatPathAndFilename, true);

            // Save the Import map from the resources
            SaveMapToDisk("ShipWorks.Shipping.Carriers.UPS.WorldShip.ImportFromShipWorks.dat", importDatPathAndFilename, true);

            // Save the import map for keyed import
            // This one is the same as the regular import, with the exception that it's primary key is order number instead of ShipmentID
            SaveMapToDisk("ShipWorks.Shipping.Carriers.UPS.WorldShip.ImportFromShipWorksForKeyed.dat", importForKeyedDataPathAndFilename, true);
        }

        /// <summary>
        /// Iterates through the list of supported sql server driver names, asking the ODBC Manager if it is a valid driver name.
        /// </summary>
        /// <param name="worldShipOdbcMgr">The WorldShipOdbcManager instance to query. </param>
        /// <returns>The name of the first valid sql driver name found.  String.Empty if none were found.</returns>
        /// <exception cref="OdbcManagerException" />
        private static string FindValidSqlDriverName(WorldShipOdbcManager worldShipOdbcMgr)
        {
            // Build a list of driver names we support
            string[] driverNames = new[] { "SQL Server", "SQL Server Native Client 10.0", "SQL Native Client" };

            // Iterate through each driver name, asking the odbc manager if it is a valid driver
            foreach (string driverName in driverNames)
            {
                if (worldShipOdbcMgr.IsValidDriverName(driverName))
                {
                    // Found it!  return it.
                    return driverName;
                }
            }

            // If we got here, we didn't find any valid sql driver names
            return string.Empty;
        }

        /// <summary>
        /// Saves an embedded resource to a file on disk
        /// </summary>
        /// <param name="resourcePath">Path to the resource in the assembly</param>
        /// <param name="outputPathAndFilename">File into which the resource content will be written</param>
        /// <param name="overwrite">Flag whether the file should be overwritten</param>
        /// <exception cref="ResourceUtilityException" />
        private static void SaveMapToDisk(string resourcePath, string outputPathAndFilename, bool overwrite)
        {
            bool fileExists = File.Exists(outputPathAndFilename);

            if (!fileExists || overwrite)
            {
                try
                {
                    ResourceUtility.SaveManifestResourceStreamToFile(resourcePath, outputPathAndFilename, overwrite);
                }
                catch (ResourceUtilityException resourceUtilityException)
                {
                    log.ErrorFormat("ShipWorks was unable to retrieve/save the embeded WolrdShip map resource '{0}'", resourcePath);
                    throw new ResourceUtilityException("ShipWorks was unable to save the WorldShip map .dat file to your computer.", resourceUtilityException);
                }
            }
        }
    }
}
