using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using System.Collections.Generic;

public partial class Triggers
{
    [SqlTrigger (Target="TemplateFolder", Event="FOR UPDATE")]
    public static void TemplateFolderLabelTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // To determine what columns were updated we need to get the mapping of names to ordinals
            Dictionary<string, int> ordinalMap = UtilityFunctions.GetColumnNameOrdinalMap(con, "inserted");

            // We don't have to do anything if name hasn't changed
            if (!SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap["Name"]) &&
                !SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap["ParentFolderID"]))
            {
                return;
            }

            // Update the reference names of all affected templates
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText =
                @"UPDATE ObjectLabel
                      SET Label = LEFT(dbo.GetTemplateFullName(d.TemplateID), 100)
                      FROM inserted u CROSS APPLY dbo.GetTemplateDescendantsOfFolder(u.TemplateFolderID) d
                      WHERE ObjectLabel.ObjectID = d.TemplateID";
            cmd.ExecuteNonQuery();
        }
    }
}
