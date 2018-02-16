using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles
{
    /// <summary>
    /// Enum representing the SQL Server Editions supported
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum SqlServerEditionType
    {
        // SQL Server 2014 Express
        [Description("SQL Server 2014 Express")]
        Express2014 = 1,

        // SQL Server 2014 LocalDB
        [Description("SQL Server 2014 LocalDB")]
        LocalDb2014 = 2,

        // SQL Server 2017 Express
        [Description("SQL Server 2017 Express")]
        Express2017 = 3,

        // SQL Server 2017 LocalDB
        [Description("SQL Server 2017 LocalDB")]
        LocalDb2017 = 4,
    }
}
