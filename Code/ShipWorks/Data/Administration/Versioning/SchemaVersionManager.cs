using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            return new SchemaVersion(allVersions.Last().ToVersion);
        }
        
        /// <summary>
        /// Get a list of all the update scripts in ShipWorks, in the order they should be applied.
        /// </summary>
        public List<SqlUpdateScript> GetUpdateScripts(SchemaVersion fromVersion, SchemaVersion toVersion)
        {
            VersionGraph versionGraph = new VersionGraph();

            List<string> upgradePath = versionGraph.GetUpgradePath(fromVersion, toVersion, allVersions);

            List<SqlUpdateScript> scripts = GetAllScripts();

			// select s
			// from upgradPath up
			// inner join scripts s on up.scriptname = s.schemaversion 
			return upgradePath.Join(scripts, up => up, s => s.SchemaVersion, (s, script) => script).ToList();			
        }

        /// <summary>
        /// Gets the serialized versions.
        /// </summary>
        public static string GetSerializedSchemaUpdateInformation()
        {
            Stream shipWorksVersions = Assembly.GetCallingAssembly().GetManifestResourceStream("ShipWorks.Data.Administration.Scripts.Update.ShipWorksVersions.json");
            StreamReader reader = new StreamReader(shipWorksVersions);

            serializedSchemaVersionInfo = reader.ReadToEnd();
            return serializedSchemaVersionInfo;
        }

        /// <summary>
        /// Gets the scripts from the script files in the assembly.
        /// </summary>
        /// <returns></returns>
        private List<SqlUpdateScript> GetAllScripts()
        {
            HashSet<string> resourceNames = GetScriptNames();

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
        private string GetShortResourceName(string resource, HashSet<string> scriptNames)
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
                if (scriptNames.Contains(checkName))
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
        private HashSet<string> GetScriptNames()
        {
            HashSet<string> versions = new HashSet<string>();

            foreach (var versionPair in allVersions)
            {
                versions.Add(versionPair.ToVersion);
            }

            return versions;
        }

        /// <summary>
        /// Deserailize serializedVersions
        /// </summary>
        public List<UpgradePath> GetAllVersions(string serializedVersions)
        {
            List<UpgradePath> versions =
                JsonConvert.DeserializeObject<List<UpgradePath>>(serializedVersions);

            return versions;
        }
    }
}