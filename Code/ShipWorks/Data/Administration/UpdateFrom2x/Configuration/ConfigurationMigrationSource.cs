using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    /// <summary>
    /// Specifies to the MigrateConfigurationWizard what type of migration should take place
    /// </summary>
    public enum ConfigurationMigrationSource
    {
        InPlace,
        SelectFolder
    }
}
