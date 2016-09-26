using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Data.Rollups;

public partial class Triggers
{
    [SqlTrigger (Target="OrderItem", Event="FOR UPDATE, INSERT, DELETE")]
    public static void OrderItemRollupTrigger()
    {
        // If we are deleting the store, then the parent orders are going to be deleted anyway, so we don't have to mess with the rollups
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            if (UtilityFunctions.GetUserContext(con).DeletingStore)
            {
                return;
            }
        }

        string parentTable = "Order";
        string parentKey = "OrderID";
        string childCountColumn = "RollupItemCount";
        string childTable = "OrderItem";

        List<RollupColumn> rollupColumns = new List<RollupColumn>();
        rollupColumns.Add(new RollupColumn { SourceColumn = "Name",        TargetColumn = "RollupItemName",        Method = RollupMethod.SingleOrNull });
        rollupColumns.Add(new RollupColumn { SourceColumn = "Code",        TargetColumn = "RollupItemCode",        Method = RollupMethod.SingleOrNull });
        rollupColumns.Add(new RollupColumn { SourceColumn = "SKU",         TargetColumn = "RollupItemSKU",         Method = RollupMethod.SingleOrNull });
        rollupColumns.Add(new RollupColumn { SourceColumn = "Location",    TargetColumn = "RollupItemLocation",    Method = RollupMethod.SingleOrNull });
        rollupColumns.Add(new RollupColumn { SourceColumn = "Quantity",    TargetColumn = "RollupItemQuantity",    Method = RollupMethod.Sum});
        rollupColumns.Add(new RollupColumn { SourceColumn = "TotalWeight", TargetColumn = "RollupItemTotalWeight", Method = RollupMethod.Sum,  SourceDependencies = new List<string> { "Weight", "Quantity" } });

        RollupUtility.UpdateRollups(parentTable, parentKey, childCountColumn, childTable, rollupColumns);
    }
}
