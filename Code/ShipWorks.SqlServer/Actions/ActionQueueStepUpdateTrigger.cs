using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using System.Collections.Generic;

public partial class Triggers
{
    [SqlTrigger(Target = "ActionQueueStep", Event = "FOR UPDATE")]
    public static void ActionQueueStepUpdateTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Map the column names to ordinals
            Dictionary<string, int> ordinalMap = UtilityFunctions.GetColumnNameOrdinalMap(con, "ActionQueueStep");

            // If its an error that's been reattempted, we need to make sure the parent timestamp changes, so grid's can know
            // the cache for the queue is invalid
            if (SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap["AttemptCount"]))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "UPDATE ActionQueue SET ActionID = ActionID WHERE ActionQueueID IN (SELECT ActionQueueID FROM inserted)";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
