using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Gets the Ticks for a DateTime
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    [SqlFunction(IsDeterministic = true, DataAccess = DataAccessKind.None)]
    public static long GetTicksFromDateTime(SqlDateTime dateTime)
    {
        return dateTime.Value.Ticks;
    }
};

