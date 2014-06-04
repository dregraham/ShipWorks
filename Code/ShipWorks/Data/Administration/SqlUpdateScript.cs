using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Represents a single update script found in ShipWorks embedded resources
    /// </summary>
    public class SqlUpdateScript
    {
        Version schemaVersion;
        string scriptName;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlUpdateScript(string resourcePath)
        {
            // We have to extract out the path part
            int updateIndex = resourcePath.IndexOf("Update");
            if (updateIndex < 0)
            {
                throw new InvalidOperationException("Unexpected sql update script path.");
            }

            // Set the script name
            scriptName = resourcePath.Substring(updateIndex + "Update.".Length);

            Match versionMatch = Regex.Match(resourcePath, @"\d+\.\d+\.\d+\.\d+\.sql$");
            if (!versionMatch.Success)
            {
                throw new InvalidOperationException("Could not extract script name from path " + resourcePath);
            }

            // Create the version from that
            schemaVersion = new Version(versionMatch.Value.Replace(".sql", ""));
        }

        /// <summary>
        /// The ShipWorks database schema version number
        /// </summary>
        public Version SchemaVersion
        {
            get { return schemaVersion; }
        }

        /// <summary>
        /// The name of the script without the path or .sql extension
        /// </summary>
        public string ScriptName
        {
            get { return scriptName; }
        }
    }
}
