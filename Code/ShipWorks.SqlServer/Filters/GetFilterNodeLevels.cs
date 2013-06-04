using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess=DataAccessKind.Read)]
    public static SqlInt32 GetFilterNodeLevels(SqlInt64 filterLayoutID)
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.Parameters.AddWithValue("@FilterLayoutID", filterLayoutID);
            cmd.CommandText = @"
	            WITH Levels AS
	            (
		            SELECT FilterNodeID, 1 as 'Level'
		            FROM FilterLayout
                    WHERE FilterLayoutID = @FilterLayoutID

		            UNION ALL

		            SELECT n.FilterNodeID, p.Level + 1 as 'Level'
		            FROM FilterNode n INNER JOIN Levels p ON n.ParentFilterNodeID = p.FilterNodeID
	            )
	            SELECT MAX(Level) From Levels
            ";

            return new SqlInt32(Convert.ToInt32(cmd.ExecuteScalar()));
        }
    }
};

