using System;

namespace ShipWorks.Data.Administration.Versioning
{
    /// <summary>
    /// Represents a single update script found in ShipWorks embedded resources
    /// </summary>
    public class SqlUpdateScript
    {
        string schemaVersion;
        string scriptName;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlUpdateScript(string resourcePath, string resourceName)
        {
            // We have to extract out the path part
            int updateIndex = resourcePath.IndexOf("Update");
            if (updateIndex < 0)
            {
                throw new InvalidOperationException("Unexpected sql update script path.");
            }

            // Set the script name
            scriptName = resourcePath.Substring(updateIndex + "Update.".Length);

            schemaVersion = resourceName;
        }

        /// <summary>
        /// Copies existing SqlUpdateScript into a new one. This helps with a linq join.
        /// </summary>
        public SqlUpdateScript(SqlUpdateScript script)
        {
            schemaVersion = script.schemaVersion;
            scriptName = script.scriptName;
        }

        /// <summary>
        /// The ShipWorks database schema version number
        /// </summary>
        public string SchemaVersion
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

        /// <summary>
        /// Gets the name of the update process.
        /// </summary>
        public string UpdateProcessName { get; set; }

    }
}
