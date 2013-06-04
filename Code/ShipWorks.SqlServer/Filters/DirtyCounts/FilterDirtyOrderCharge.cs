using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Filters.DirtyCounts;


public partial class Triggers
{
    [SqlTrigger(Name = "FilterDirtyOrderCharge", Target = "OrderCharge", Event = "AFTER UPDATE, INSERT, DELETE")]
    public static void FilterDirtyOrderCharge()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            if (UtilityFunctions.GetLastRowCount(con) == 0)
            {
                return;
            }

            FilterNodeContentDirtyUtility.InsertTriggeredOrderChild(con, FilterNodeColumnMaskTable.OrderCharge);
        }
    }
}
