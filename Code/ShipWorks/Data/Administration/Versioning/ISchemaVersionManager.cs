using System.Collections.Generic;

namespace ShipWorks.Data.Administration.Versioning
{
    public interface ISchemaVersionManager
    {
        /// <summary>
        /// Get a list of all the update scripts in ShipWorks, in the order they should be applied.
        /// </summary>
        List<SqlUpdateScript> GetUpdateScripts(SchemaVersion fromVersion, SchemaVersion toVersion);
    }
}