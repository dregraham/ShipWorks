using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.SqlServer.General;

public partial class Triggers
{
    [SqlTrigger(Target = "PrintResult", Event = "FOR DELETE")]
    public static void PrintResultDeleteTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Delete all references that the print results hold.  This is for resource usage, and whatever else.
            foreach (long printResultID in UtilityFunctions.GetTriggerPrimaryKeys(con))
            {
                ObjectReferenceManager.ClearReferences(printResultID, con);
            }
        }
    }
}
