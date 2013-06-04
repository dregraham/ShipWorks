using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    /// <summary>
    /// The action taken, if any, of migration of ShipWorks 2x data to ShipWorks 3x
    /// </summary>
    public enum ConfigurationMigrationAction
    {
        /// <summary>
        /// No migration has taken place
        /// </summary>
        None = 0,

        /// <summary>
        /// No valid instances of ShipWorks2 were found installed to migrate from
        /// </summary>
        NotNeeded = 1,

        /// <summary>
        /// ShipWorks 3x was installed on top of ShipWorks 2x, and data was migrated from that same directory.
        /// </summary>
        InPlace = 2,

        /// <summary>
        /// ShipWorks 2x was found installed in another folder, and the user migrated from that
        /// </summary>
        SelectedFolder = 3,

        /// <summary>
        /// ShipWorks 2x was found installed in another folder, but migration was declined
        /// </summary>
        SideBySide = 4,
    }
}
