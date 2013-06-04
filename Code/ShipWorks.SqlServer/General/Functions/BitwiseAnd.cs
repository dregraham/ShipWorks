using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(IsDeterministic = true, DataAccess = DataAccessKind.None)]
    public static SqlBinary BitwiseAnd(SqlBinary data, SqlBinary test)
    {
        byte[] result = new byte[Math.Min(data.Length, test.Length)];

        for (int i = 1; i <= Math.Min(data.Length, test.Length); i++)
        {
            result[result.Length - i] = (byte) (data[data.Length - i] & test[test.Length - i]);
        }

        return new SqlBinary(result);
    }
};

