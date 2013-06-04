using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Filters.DirtyCounts;


public partial class Triggers
{
    [SqlTrigger(Name = "FilterDirtyShopifyOrder", Target = "ShopifyOrder", Event = "AFTER UPDATE")]
    public static void FilterDirtyShopifyOrder()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            if (UtilityFunctions.GetLastRowCount(con) == 0)
            {
                return;
            }

            FilterNodeContentDirtyUtility.InsertTriggeredOrderChild(con, FilterNodeColumnMaskTable.ShopifyOrder);
        }
    }
}
