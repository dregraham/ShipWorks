using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Filters.DirtyCounts;

public partial class Triggers
{
    [SqlTrigger(Name = "FilterDirtyFedExShipment", Target = "FedExShipment", Event = "AFTER UPDATE")]
    public static void FilterDirtyFedExShipment()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            if (UtilityFunctions.GetLastRowCount(con) == 0)
            {
                return;
            }

            FilterNodeContentDirtyUtility.InsertTriggeredDerivedShipmentType(con, FilterNodeColumnMaskTable.FedExShipment);
        }
    }
}
