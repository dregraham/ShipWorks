using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public partial class Triggers
{
    /// <summary>
    /// Trigger that raised after the column settings for a filter node are deleted
    /// </summary>
    [SqlTrigger(Target = "FilterNodeColumnSettings", Event = "AFTER DELETE")]
    public static void FilterNodeColumnSettingsDeleted()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Cascade the delete to the layout columns we point to
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"
                DELETE GridColumnLayout
                  WHERE GridColumnLayoutID IN (SELECT GridColumnLayoutID FROM deleted)";

            cmd.ExecuteNonQuery();
        }
    }
}
