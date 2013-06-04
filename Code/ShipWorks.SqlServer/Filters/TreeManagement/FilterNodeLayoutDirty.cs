using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Text;

public partial class Triggers
{
    /// <summary>
    /// Marks the layout, if any, for the given line as dirty
    /// </summary>
    [SqlTrigger(Target = "FilterNode", Event = "AFTER UPDATE, INSERT, DELETE")]
    public static void FilterNodeLayoutDirty()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // We ask for the layout of the ParentFilterNodeID instead of the FilterNodeID.  We can't ask for the FilterNodeID
            // because that breaks if its here due to being deleted.  The GetFilterNodeLayoutID won't be able to join the deleted
            // ID to find the row to find the parent, since its no longer there.  So we just start it out at the parent.
            SqlCommand layoutsCmd = con.CreateCommand();
            layoutsCmd.CommandText = @"
                SELECT dbo.GetFilterNodeLayoutID(FilterNodeID)
                    FROM (SELECT ParentFilterNodeID as FilterNodeID FROM deleted
                            UNION
                          SELECT FilterNodeID FROM inserted) as Changed
                    WHERE FilterNodeID IS NOT NULL
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
