using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Filters;
using System.Diagnostics;
using ShipWorks.Filters;
using System.Threading;
using ShipWorks.SqlServer.General;

public partial class StoredProcedures
{
    [SqlProcedure]
    public static void CalculateUpdateFilterCounts()
    {
        FilterCountUpdater updater = new FilterCountUpdater();
        updater.CalculateUpdateAllFilterCounts();
    }

    [SqlProcedure]
    public static void CalculateUpdateQuickFilterCounts()
    {
        QuickFilterCountUpdater updater = new QuickFilterCountUpdater();
        updater.CalculateUpdateQuickFilterCounts();
    }
}
