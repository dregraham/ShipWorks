using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using ShipWorks.SqlServer.General;

public partial class Triggers
{
    [SqlTrigger(Target = "Customer", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void CustomerLabelTrigger()
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
                            i.CustomerID,
                            i.BillFirstName + ' ' + i.BillLastName,
                            12,
                            NULL,
                            0
                      FROM inserted i";
                cmd.ExecuteNonQuery();
            }

            // Update
            if (SqlContext.TriggerContext.TriggerAction == TriggerAction.Update)
            {
                // To determine what columns were updated we need to get the mapping of names to ordinals
                Dictionary<string, int> ordinalMap = UtilityFunctions.GetColumnNameOrdinalMap(con, "inserted");

                // We don't have to do anything if name hasn't changed
                if (!SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap["BillFirstName"]) &&
                    !SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap["BillLastName"]))
                {
                    return;
                }

                // Update the reference names of all affected templates
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText =
                    @"UPDATE ObjectLabel
                      SET Label = u.BillFirstName + ' ' + u.BillLastName
                      FROM inserted u
                      WHERE ObjectLabel.ObjectID = u.CustomerID";
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
                  WHERE ObjectLabel.ObjectID = d.CustomerID";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
