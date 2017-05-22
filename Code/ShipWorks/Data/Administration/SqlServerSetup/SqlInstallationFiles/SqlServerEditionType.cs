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

        // SQL Server 2016 Express
        [Description("SQL Server 2016 Express")]
        Express2016 = 3,

        // SQL Server 2016 LocalDB
        [Description("SQL Server 2016 LocalDB")]
        LocalDb2016 = 4,
    }
}
