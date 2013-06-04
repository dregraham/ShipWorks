using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.SqlServer.General;

public partial class Triggers
{
    [SqlTrigger(Target="EmailOutbound", Event="FOR DELETE")]
    public static void EmailOutboundDeleteTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Delete all references that the emails hold.  This is for resource usage, and whatever else.
            foreach (long emailOutboundID in UtilityFunctions.GetTriggerPrimaryKeys(con))
            {
                ObjectReferenceManager.ClearReferences(emailOutboundID, con);
            }

            UtilityFunctions.IncreaseDbts(con);
        }
    }
}
