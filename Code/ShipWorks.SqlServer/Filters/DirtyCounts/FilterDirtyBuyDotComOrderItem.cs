using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Filters.DirtyCounts;


public partial class Triggers
{
    [SqlTrigger(Name = "FilterDirtyBuyDotComOrderItem", Target = "BuyDotComOrderItem", Event = "AFTER UPDATE")]
    public static void FilterDirtyBuyDotComOrderItem()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            if (UtilityFunctions.GetLastRowCount(con) == 0)
            {
                return;
            }

            FilterNodeContentDirtyUtility.InsertTriggeredDerivedOrderItem(con, FilterNodeColumnMaskTable.BuyDotComOrderItem);
        }
    }
}
