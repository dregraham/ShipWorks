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
        Express2008R2Sp2 = 0,

        Express2014 = 1,

        LocalDb2014 = 2,

        Express2016 = 3,

        LocalDb2016 = 4,
    }
}
