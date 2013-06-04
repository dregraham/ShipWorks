using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Common.Data;

public partial class Triggers
{
    [SqlTrigger(Target = "Shipment", Event = "FOR DELETE")]
    public static void ShipmentDeleteTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            foreach (long shipmentID in UtilityFunctions.GetTriggerPrimaryKeys(con))
            {
                ObjectReferenceManager.ClearReferences(shipmentID, con);
            }

            UtilityFunctions.IncreaseDbts(con);
        }
    }
}
