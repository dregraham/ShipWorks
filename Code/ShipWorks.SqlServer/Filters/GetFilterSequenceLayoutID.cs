using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Returns the FilterLayoutID that contains the given sequence.  Or null
    /// if the sequence is not apart of a layout.
    /// </summary>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlInt64 GetFilterSequenceLayoutID(long filterSequenceID)
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.Parameters.AddWithValue("@sequenceID", filterSequenceID);
            cmd.CommandText = @"
                WITH AncestorChain AS
                (
                     SELECT FilterNodeID as 'FilterNodeID'
                        FROM FilterNode
                        WHERE FilterSequenceID = @sequenceID

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

