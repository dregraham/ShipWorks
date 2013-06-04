using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Reflection;

public partial class UserDefinedFunctions
{
    [SqlFunction(IsDeterministic = true, DataAccess = DataAccessKind.None)]
    public static SqlDateTime DateToUniversalTime(SqlDateTime dateTime)
    {
        return new SqlDateTime(dateTime.Value.ToUniversalTime());
    }
};

