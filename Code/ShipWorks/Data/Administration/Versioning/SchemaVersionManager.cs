using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using Interapptive.Shared.Data;
using Newtonsoft.Json;

namespace ShipWorks.Data.Administration.Versioning
{
    public class SchemaVersionManager : ISchemaVersionManager
    {
        // Used for executing scripts
        private static SqlScriptLoader sqlLoader = new SqlScriptLoader("ShipWorks.Data.Administration.Scripts.Update");

        private static string serializedSchemaVersionInfo;

        private List<UpgradePath> allVersions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaVersionManager"/> class.
        /// </summary>
        public SchemaVersionManager()
            : this(GetSerializedSchemaUpdateInformation())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaVersionManager"/> class.
        /// </summary>
        /// <param name="serializedVersions">The serialized versions.</param>
        public SchemaVersionManager(string serializedVersions)
        {
            allVersions = GetAllVersions(serializedVersions);
        }


        /// <summary>
        /// Gets the required schema version for the running version of the software.
        /// </summary>
        /// <returns></returns>
        public SchemaVersion GetRequiredSchemaVersion()
        {
            return new SchemaVersion(allVersions.Last().To);
        }

        /// <summary>
        /// Gets the serialized versions.
        /// </summary>
        public static string GetSerializedSchemaUpdateInformation()
        {
            Stream shipWorksVersions = Assembly.GetCallingAssembly().GetManifestResourceStream("ShipWorks.Data.Administration.Scripts.Update.ShipWorksVersions.json");
            using (StreamReader reader = new StreamReader(shipWorksVersions))
            {
                serializedSchemaVersionInfo = reader.ReadToEnd();
            }
            return serializedSchemaVersionInfo;
        }
        
        /// <summary>
        /// Get a list of all the update scripts in ShipWorks, in the order they should be applied.
        /// </summary>
        public List<SqlUpdateScript> GetUpdateScripts(SchemaVersion fromVersion, SchemaVersion toVersion)
        {
            VersionGraph versionGraph = new VersionGraph();

            List<VersionUpgradeStep> upgradePath = versionGraph.GetUpgradePath(fromVersion, toVersion, allVersions);

            IEnumerable<string> allScripts = GetAllSqlScriptsFromAssembly();

            List<SqlUpdateScript> sqlUpdateScripts = new List<SqlUpdateScript>();

            foreach (var versionUpgradeStep in upgradePath)
            {
                string upgradeScript = allScripts.SingleOrDefault(
                    script =>
                        script.EndsWith(string.Format("{0}.sql", versionUpgradeStep.Script),
                            StringComparison.OrdinalIgnoreCase));
                
                if (string.IsNullOrEmpty(upgradeScript))
                {
                    throw new InvalidOperationException(string.Format("Script {0} not found.", versionUpgradeStep.Script));
                }

                sqlUpdateScripts.Add(new SqlUpdateScript(upgradeScript, versionUpgradeStep.Script, versionUpgradeStep.Process));
            }

            return sqlUpdateScripts;
        }

        /// <summary>
        /// Gets the scripts from the script files in the assembly.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> GetAllSqlScriptsFromAssembly()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Where(r => r.StartsWith(sqlLoader.ResourcePath, StringComparison.OrdinalIgnoreCase) && r.EndsWith("sql", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Deserailize serializedVersions
        /// </summary>
        private List<UpgradePath> GetAllVersions(string serializedVersions)
        {
            List<UpgradePath> versions =
                JsonConvert.DeserializeObject<List<UpgradePath>>(serializedVersions);

            return versions;
        }
    }
}