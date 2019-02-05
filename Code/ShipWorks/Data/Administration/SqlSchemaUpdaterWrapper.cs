using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Wrapper for the static SqlSchemaUpdater
    /// </summary>
    public class SqlSchemaUpdaterWrapper : ISqlSchemaUpdater
    {
        /// <summary>
        /// Get the schema version of the ShipWorks database
        /// </summary>
        public Version GetInstalledSchemaVersion() =>
            SqlSchemaUpdater.GetInstalledSchemaVersion();
    }
}
