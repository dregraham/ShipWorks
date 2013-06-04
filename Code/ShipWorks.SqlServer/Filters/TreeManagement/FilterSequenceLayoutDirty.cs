using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Text;

public partial class Triggers
{
    /// <summary>
    /// Marks the layout, if any, for the given line as dirty
    /// </summary>
    [SqlTrigger(Target = "FilterSequence", Event = "AFTER UPDATE, INSERT, DELETE")]
    public static void FilterSequenceLayoutDirty()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            SqlCommand layoutsCmd = con.CreateCommand();
            layoutsCmd.CommandText = @"
                SELECT dbo.GetFilterSequenceLayoutID(FilterSequenceID)
                    FROM (SELECT FilterSequenceID FROM deleted
                            UNION
                          SELECT FilterSequenceID FROM inserted) as Changed
            ";

            StringBuilder inList = new StringBuilder();

            // Build the list of layout ID's to update
            using (SqlDataReader reader = layoutsCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        if (inList.Length > 0)
                        {
                            inList.Append(",");
                        }

                        inList.Append(reader.GetValue(0));
                    }
                }
            }

            // Make each of the layout's dirty, if any
            if (inList.Length > 0)
            {
                SqlCommand makeDirtyCmd = con.CreateCommand();
                makeDirtyCmd.CommandText = @"
                UPDATE FilterLayout
                    SET FilterNodeID = FilterNodeID
                    WHERE FilterLayoutID IN (" + inList + ")";

                makeDirtyCmd.ExecuteNonQuery();
            }
        }
    }
}
