using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.Reflection;

public partial class UserDefinedFunctions
{
    [SqlFunction(IsDeterministic = true, DataAccess = DataAccessKind.None)]
    public static SqlBoolean RegexMatch(SqlString input, SqlString regex)
    {
        if (input.IsNull || regex.IsNull)
        {
            return SqlBoolean.Null;
        }
        else
        {
            return (SqlBoolean) Regex.IsMatch(input.Value, regex.Value, 
                RegexOptions.IgnoreCase | 
                RegexOptions.CultureInvariant);
        }
    }
};

