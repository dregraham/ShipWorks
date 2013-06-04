using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Returns the FilterLayoutID that contains the given node.  Or null
    /// if the node is not apart of a layout.
    /// </summary>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlInt64 GetFilterNodeLayoutID(long filterNodeID)
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.Parameters.AddWithValue("@nodeID", filterNodeID);
            cmd.CommandText = @"
                WITH AncestorChain AS
                (
                     SELECT @nodeID as 'FilterNodeID'

                     UNION ALL

                     SELECT n.ParentFilterNodeID as 'FilterNodeID'
                       FROM FilterNode n INNER JOIN AncestorChain c ON n.FilterNodeID = c.FilterNodeID
                       WHERE c.FilterNodeID IS NOT NULL
                )
                SELECT l.FilterLayoutID 
                  FROM AncestorChain c INNER JOIN FilterLayout l ON c.FilterNodeID = l.FilterNodeID
            ";

            object result = cmd.ExecuteScalar();

            if (result is DBNull || result == null)
            {
                return SqlInt64.Null;
            }
            else
            {
                return new SqlInt64(Convert.ToInt64(result));
            }
        }
    }
};

