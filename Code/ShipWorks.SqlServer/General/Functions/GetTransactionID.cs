using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;

public partial class UserDefinedFunctions
{
    [SqlFunction(IsDeterministic = true, DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read)]
    public static SqlInt64 GetTransactionID()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            return UtilityFunctions.GetTransactionID(con);
        }
    }
};

