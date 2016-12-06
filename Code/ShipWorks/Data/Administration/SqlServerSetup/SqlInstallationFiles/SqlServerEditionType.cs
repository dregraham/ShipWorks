using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles
{
    /// <summary>
    /// Enum representing the SQL Server Editions supported
    /// </summary>
    public enum SqlServerEditionType
    {
        // SQL Server 2008 R2 SP2 Express 
        Express2008R2Sp2 = 0,

        // SQL Server 2014 Express
        Express2014 = 1,

        // SQL Server 2014 LocalDB
        LocalDb2014 = 2,

        // SQL Server 2016 Express
        Express2016 = 3,

        // SQL Server 2016 LocalDB
        LocalDb2016 = 4,
    }
}
