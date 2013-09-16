using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.Filters;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Filters.DirtyCounts;

public partial class Triggers
{
    [SqlTrigger(Name = "FilterDirtyOrder", Target = "Order", Event = "AFTER UPDATE, INSERT, DELETE")]
    public static void FilterDirtyOrder()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            if (UtilityFunctions.GetLastRowCount(con) == 0)
            {
                return;
            }

            FilterNodeContentDirtyUtility.InsertTriggeredOrders(con);

            // Add any deleted orders to the actionqueue for the root order filter.  Filter calculations normally 
            // do this, but the root does not have a filter calculation.  It is handled manually here.
            if (SqlContext.TriggerContext.TriggerAction == TriggerAction.Delete)
            {
                SqlCommand actionCmd = con.CreateCommand();
                actionCmd.CommandText = @"
                    INSERT INTO ActionQueue(ActionID, ActionName, TriggerComputerID, ComputerLimitedList, ObjectID, Status, NextStep)
                       SELECT t.ActionID, '', @computerID, CASE t.ComputerLimitedType WHEN 0 THEN '' WHEN 1 THEN CAST(@computerID as varchar(150)) ELSE t.ComputerLimitedList END, d.OrderID, 0, 0
                       FROM ActionFilterTrigger t, deleted d
                       WHERE t.FilterNodeID = @filterNodeID AND
                             t.Direction = 0";
                actionCmd.Parameters.AddWithValue("@computerID", UtilityFunctions.GetComputerID(con));
                actionCmd.Parameters.AddWithValue("@filterNodeID", BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));
                actionCmd.ExecuteNonQuery();
            }
            else if (SqlContext.TriggerContext.TriggerAction == TriggerAction.Insert)
            {
                SqlCommand actionCmd = con.CreateCommand();
                actionCmd.CommandText = @"
                    INSERT INTO ActionQueue(ActionID, ActionName, TriggerComputerID, ComputerLimitedList, ObjectID, Status, NextStep)
                       SELECT t.ActionID, '', @computerID,  CASE t.ComputerLimitedType WHEN 0 THEN '' WHEN 1 THEN CAST(@computerID as varchar(150)) ELSE t.ComputerLimitedList END, i.OrderID, 0, 0
                       FROM ActionFilterTrigger t, inserted i
                       WHERE t.FilterNodeID = @filterNodeID AND
                             t.Direction = 1";
                actionCmd.Parameters.AddWithValue("@computerID", UtilityFunctions.GetComputerID(con));
                actionCmd.Parameters.AddWithValue("@filterNodeID", BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));
                actionCmd.ExecuteNonQuery();
            }

            // Update the root order count
            if (SqlContext.TriggerContext.TriggerAction == TriggerAction.Insert || SqlContext.TriggerContext.TriggerAction == TriggerAction.Delete)
            {
                SqlCommand rootCmd = con.CreateCommand();
                rootCmd.CommandText = @"

               DECLARE @change int
               SET @change = (SELECT COUNT(*) FROM inserted) - (SELECT COUNT(*) FROM deleted)

               INSERT INTO FilterNodeRootDirty (FilterNodeContentID, Change)
                 VALUES (@FilterNodeContentID, @change)";

                rootCmd.Parameters.AddWithValue("@FilterNodeContentID", BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));
                rootCmd.ExecuteNonQuery();
            }
        }
    }
}
