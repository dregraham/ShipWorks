using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using System.Collections.Generic;
using System.Text;
using ShipWorks.SqlServer.Data.Rollups;
using System.Diagnostics;

public partial class Triggers
{
    [SqlTrigger(Target = "Order", Event = "FOR UPDATE, INSERT, DELETE")]
    public static void OrderRollupTrigger()
    {
        string parentTable = "Customer";
        string parentKey = "CustomerID";
        string childCountColumn = "RollupOrderCount";
        string childTable = "Order";

        List<RollupColumn> rollupColumns = new List<RollupColumn>();
        rollupColumns.Add(new RollupColumn { SourceColumn = "OrderTotal", TargetColumn = "RollupOrderTotal", Method = RollupMethod.Sum });

        RollupUtility.UpdateRollups(parentTable, parentKey, childCountColumn, childTable, rollupColumns);
    }
}
