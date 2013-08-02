using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Returns the server Time Zone name
    /// </summary>
    /// <returns></returns>
    [SqlFunction(IsDeterministic = true, DataAccess = DataAccessKind.None)]
    public static string GetLocalTimezoneName()
    {
        return TimeZone.CurrentTimeZone.StandardName;
    }
};

