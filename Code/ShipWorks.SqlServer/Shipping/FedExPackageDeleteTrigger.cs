using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Common.Data;

public partial class Triggers
{
    [SqlTrigger(Target = "FedExPackage", Event = "FOR DELETE")]
    public static void FedExPackageDeleteTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            foreach (long packageID in UtilityFunctions.GetTriggerPrimaryKeys(con))
            {
                ObjectReferenceManager.ClearReferences(packageID, con);
            }
        }
    }
}
