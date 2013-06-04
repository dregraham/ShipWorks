using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using System.Collections.Generic;
using ShipWorks.SqlServer.Data.Rollups;


public partial class Triggers
{
    [SqlTrigger (Target="EbayOrderItem", Event="FOR UPDATE, INSERT, DELETE")]
    public static void EbayOrderItemRollupTrigger()
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

        string parentTable = "EbayOrder";
        string parentKey = "OrderID";
        string childCountColumn = "RollupEbayItemCount";
        string childTable = "EbayOrderItem";

        List<RollupColumn> rollupColumns = new List<RollupColumn>();
        rollupColumns.Add(new RollupColumn { SourceColumn = "EffectivePaymentMethod",   TargetColumn = "RollupEffectivePaymentMethod",   Method = RollupMethod.SameOrNull });
        rollupColumns.Add(new RollupColumn { SourceColumn = "EffectiveCheckoutStatus",  TargetColumn = "RollupEffectiveCheckoutStatus",  Method = RollupMethod.SameOrNull });
       
        rollupColumns.Add(new RollupColumn { SourceColumn = "PayPalAddressStatus",      TargetColumn = "RollupPayPalAddressStatus",      Method = RollupMethod.SameOrNull });
        rollupColumns.Add(new RollupColumn { SourceColumn = "SellingManagerRecord",     TargetColumn = "RollupSellingManagerRecord",     Method = RollupMethod.SameOrNull });

        rollupColumns.Add(new RollupColumn { SourceColumn = "FeedbackLeftType",         TargetColumn = "RollupFeedbackLeftType",         Method = RollupMethod.SameOrNull });
        rollupColumns.Add(new RollupColumn { SourceColumn = "FeedbackLeftComments",     TargetColumn = "RollupFeedbackLeftComments",     Method = RollupMethod.SameOrNull });
        rollupColumns.Add(new RollupColumn { SourceColumn = "FeedbackReceivedType",     TargetColumn = "RollupFeedbackReceivedType",     Method = RollupMethod.SameOrNull });
        rollupColumns.Add(new RollupColumn { SourceColumn = "FeedbackReceivedComments", TargetColumn = "RollupFeedbackReceivedComments", Method = RollupMethod.SameOrNull });

        RollupUtility.UpdateRollups(parentTable, parentKey, childCountColumn, childTable, rollupColumns);
    }
}
