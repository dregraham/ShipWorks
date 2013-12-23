using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Data;
using Newtonsoft.Json;

namespace ShipWorks.Data.Administration
{
    public class UpdateScriptManager
    {
        // Used for executing scripts
        private static SqlScriptLoader sqlLoader = new SqlScriptLoader("ShipWorks.Data.Administration.Scripts.Update");

        private List<KeyValuePair<string, List<String>>> allVersions;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateScriptManager"/> class.
        /// </summary>
        public UpdateScriptManager()
            : this(GetSerializedSchemaUpdateInformation())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateScriptManager"/> class.
        /// </summary>
        /// <param name="serializedVersions">The serialized versions.</param>
        public UpdateScriptManager(string serializedVersions)
        {
            allVersions = GetAllVersions(serializedVersions);
        }


        /// <summary>
        /// Gets the required schema version for the running version of the software.
        /// </summary>
        /// <returns></returns>
        public string GetRequiredSchemaVersion()
        {
            return allVersions.Last().Key;
        }

        /// <summary>
        /// Doeses ShipWorks need to be upgraded - This is ran by the installer.
        /// </summary>
        /// <param name="schemaVersion">The schema version.</param>
        /// <returns></returns>
        public bool DoesShipWorksNeedToBeUpgraded(string schemaVersion)
        {
            if (GetRequiredSchemaVersion() == schemaVersion)
            {
                return false;
            }

            Version parsedVersion;
            if (Version.TryParse(schemaVersion, out parsedVersion))
            {
                if (parsedVersion.Major == 2)
                {
                    return false;
                }
            }

            try
            {
                // If InvalidOperationException, no path from installed version to DB version. 
                // This either means the DB version is newer or ShipWorks doesn't recognize it for some other weird reason.
                //   The vast majority of the time, it is the former, so we return true on Exception.
                GetUpdateScripts(schemaVersion);
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }

        /// <summary>
        /// Does the database need to be upgraded - using the ShipWorksVersions file, see if DB needs an upgrade.
        /// </summary>
        /// <param name="schemaVersion">The schema version of the database being evaluated.</param>
        /// <param name="isInstalling">if set to <c>true</c> [is installing].</param>
        /// <returns></returns>
        public bool DoesDBNeedToBeUpgraded(string schemaVersion)
        {
            Version parsedVersion;
            if (Version.TryParse(schemaVersion, out parsedVersion))
            {
                if (parsedVersion.Major == 2)
                {
                    return true;
                }
            }

            try
            {
                // There are scripts that can update the DB, so true
                if (GetUpdateScripts(schemaVersion).Count > 0)
                {
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
                // No upgrade path, the likeliest reason is that the software is unaware of the DB version and 
                // the software needs to be upgraded. 
                return false;
            }

            return false;
        }

        /// <summary>
        /// Doeses the installing version require database to be upgraded.
        /// </summary>
        /// <param name="installingSchemaVersion">The installing schema version.</param>
        /// <param name="installedSchemaVersion">The installed schema version.</param>
        /// <returns></returns>
        public bool DoesInstallingVersionRequireDBToBeUpgraded(string installingSchemaVersion, string installedSchemaVersion)
        {
            if (installedSchemaVersion == installingSchemaVersion)
            {
                return false;
            }

            Version parsedVersion;
            if (Version.TryParse(installedSchemaVersion, out parsedVersion))
            {
                if (parsedVersion.Major == 2)
                {
                    return true;
                }
            }

            try
            {
                // There are scripts that can update the DB, so true.
                return GetUpdateScripts(installedSchemaVersion).Count > 0;
            }
            catch (InvalidOperationException)
            {
                // No upgrade path, the likeliest reason is that the installed software is unaware of the DB version
                // of the software being installed and the DB will need to be upgraded.
                return false;
            }
        }

        /// <summary>
        /// Get a list of all the update scripts in ShipWorks, ordered based on upgradePath.
        /// </summary>
        public List<SqlUpdateScript> GetUpdateScripts(string schemaVersion)
        {
            VersionGraph versionGraph = new VersionGraph();

            List<string> upgradePath = versionGraph.GetUpgradePath(schemaVersion, allVersions);

            List<SqlUpdateScript> scripts = GetAllScripts();

            return upgradePath.Join(scripts, up => up, s => s.SchemaVersion, (s, script) => script).ToList();
        }

        /// <summary>
        /// Gets the serialized versions.
        /// </summary>
        private static string GetSerializedSchemaUpdateInformation()
        {
            Stream shipWorksVersions = Assembly.GetCallingAssembly().GetManifestResourceStream("ShipWorks.Data.Administration.Scripts.Update.ShipWorksVersions.json");
            StreamReader reader = new StreamReader(shipWorksVersions);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Gets the scripts from the script files in the assembly.
        /// </summary>
        /// <returns></returns>
        private List<SqlUpdateScript> GetAllScripts()
        {
            HashSet<string> resourceNames = getScriptNames();

            List<SqlUpdateScript> scripts = new List<SqlUpdateScript>();
            foreach (string fullPathResourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames()
                                                .Where(r => r.StartsWith(sqlLoader.ResourcePath, StringComparison.OrdinalIgnoreCase)
                                                            && r.EndsWith("sql", StringComparison.OrdinalIgnoreCase)))
            {
                scripts.Add(new SqlUpdateScript(fullPathResourceName,
                                                GetShortResourceName(fullPathResourceName, resourceNames)));
            }
            return scripts;
        }

        /// <summary>
        /// Gets the short name of the resource.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Could not extract script name from path  + resource</exception>
        private string GetShortResourceName(string resource, HashSet<string> resourceNames)
        {
            // Try to find the last part of the resource in resourceNames. If not found, add the last part plus another letter.
            // For example, if the name of the resource is blah.abc.sql and there is a string in resourceNames called abc:
            // Step 1: strip out .sql (resource is now blah.abc)
            // Step 2: search for c in resourceNames
            // Step 3: search for bc in resourceNames
            // Step 4: find abc in resouceNames.
            // Step 5: return abc.
            resource = resource.Replace(".sql", string.Empty);

            for (int i = 1; i <= resource.Length; i++)
            {
                string checkName = resource.Substring(resource.Length - i);
                if (resourceNames.Contains(checkName))
                {
                    return checkName;
                }
            }

            throw new InvalidOperationException("Could not extract script name from path " + resource);
        }

        /// <summary>
        /// Gets the script names from the version file.
        /// </summary>
        /// <returns></returns>
        private HashSet<string> getScriptNames()
        {
            HashSet<string> versions = new HashSet<string>();

            foreach (var versionPair in allVersions)
            {
                versions.Add(versionPair.Key);
            }

            return versions;
        }

        /// <summary>
        /// Deserailize serializedVersions
        /// </summary>
        private List<KeyValuePair<string, List<String>>> GetAllVersions(string serializedVersions)
        {
            List<KeyValuePair<string, List<string>>> versions =
                JsonConvert.DeserializeObject<List<KeyValuePair<string, List<String>>>>(serializedVersions);

            return versions;
        }
    }
}