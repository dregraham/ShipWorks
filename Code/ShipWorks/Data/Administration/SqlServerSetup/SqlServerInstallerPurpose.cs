using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Indicates the purpose of how the SQL Server Installer will be used
    /// </summary>
    public enum SqlServerInstallerPurpose
    {
        Install,
        Upgrade,
        LocalDb
    }
}
