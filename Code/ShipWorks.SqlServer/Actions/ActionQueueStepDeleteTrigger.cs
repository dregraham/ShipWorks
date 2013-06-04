using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;

public partial class Triggers
{
    [SqlTrigger(Target = "ActionQueueStep", Event = "FOR DELETE")]
    public static void ActionQueueStepDeleteTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            UtilityFunctions.IncreaseDbts(con);
        }
    }
}
