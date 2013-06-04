using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using System.Collections.Generic;

public partial class Triggers
{
    [SqlTrigger(Target = "OrderItem", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void OrderItemLabelTrigger()
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
                            i.OrderItemID,
                            LEFT(i.Name, 100),
                            13,
                            i.OrderID,
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
                if (!SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap["Name"]) && !SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap["OrderID"]))
                {
                    return;
                }

                // Update the reference names of all affected templates
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText =
                    @"UPDATE ObjectLabel
                      SET Label = LEFT(u.Name, 100), ParentID = u.OrderID
                      FROM inserted u
                      WHERE ObjectLabel.ObjectID = u.OrderItemID";
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
                  WHERE ObjectLabel.ObjectID = d.OrderItemID";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
