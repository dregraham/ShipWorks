using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using ShipWorks.SqlServer.General;

public partial class Triggers
{
    [SqlTrigger (Target = "Template", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void TemplateLabelTrigger()
    {
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // New template
            if (SqlContext.TriggerContext.TriggerAction == TriggerAction.Insert)
            {
                // Update the reference names of all affected templates
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText =
                    @"INSERT INTO ObjectLabel
                        (
                            ObjectID, Label, ObjectType, ParentID, IsDeleted
                        )
                      SELECT
                            u.TemplateID,
                            LEFT(dbo.GetTemplateFullName(u.TemplateID), 100),
                            25,
                            NULL,
                            0
                      FROM inserted u";
                cmd.ExecuteNonQuery();
            }

            // Update
            if (SqlContext.TriggerContext.TriggerAction == TriggerAction.Update)
            {
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
                      SET Label = LEFT(dbo.GetTemplateFullName(u.TemplateID), 100)
                      FROM inserted u
                      WHERE ObjectLabel.ObjectID = u.TemplateID";
                cmd.ExecuteNonQuery();
            }

            // Delete
            if (SqlContext.TriggerContext.TriggerAction == TriggerAction.Delete)
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = @"
                UPDATE ObjectLabel
                  SET IsDeleted = 1
                  FROM deleted d
                  WHERE ObjectLabel.ObjectID = d.TemplateID";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
