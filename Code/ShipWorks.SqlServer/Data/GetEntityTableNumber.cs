using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.None)]
    public static int GetEntityTableNumber(SqlInt64 entityID)
    {
        if (entityID.IsNull)
        {
            return 0;
        }

        decimal num = (decimal) entityID / 100m;
        int seedBase = (int) (100m * Math.Round(num - Math.Truncate(num), 2));

        return seedBase;
    }
};

