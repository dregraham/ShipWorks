using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ShipWorks.SqlServer.General;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
using System.Collections;

namespace ShipWorks.SqlServer.Filters.DirtyCounts
{
    /// <summary>
    /// Utility class for inserting data into the FilterNodeContentDirty table
    /// </summary>
    public static class FilterNodeContentDirtyUtility
    {
        /// <summary>
        /// Insert all customers affected by the current trigger that havnt yet been marked dirty in this session
        /// </summary>
        public static void InsertTriggeredCustomers(SqlConnection con)
        {
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(UtilityFunctions.GetTriggerTableName(), "CustomerID", "NULL", 12, GetUpdatedColumns(FilterNodeColumnMaskTable.Customer), con);
        }

        /// <summary>
        /// Insert all orders affected by the current trigger
        /// </summary>
        public static void InsertTriggeredOrders(SqlConnection con)
        {
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(UtilityFunctions.GetTriggerTableName(), "OrderID", "CustomerID", 6, GetUpdatedColumns(FilterNodeColumnMaskTable.Order), con);
        }

        /// <summary>
        /// Insert dirty order items from the current trigger
        /// </summary>
        public static void InsertTriggeredItems(SqlConnection con)
        {
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(UtilityFunctions.GetTriggerTableName(), "OrderItemID", "OrderID", 13, GetUpdatedColumns(FilterNodeColumnMaskTable.OrderItem), con);
        }

        /// <summary>
        /// Insert dirty shpments from the current trigger
        /// </summary>
        public static void InsertTriggeredShipments(SqlConnection con)
        {
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(UtilityFunctions.GetTriggerTableName(), "ShipmentID", "OrderID", 31, GetUpdatedColumns(FilterNodeColumnMaskTable.Shipment), con);
        }

        /// <summary>
        /// Insert dirty derived shipment records from whatever the current trigger is
        /// </summary>
        public static void InsertTriggeredDerivedShipmentType(SqlConnection con, FilterNodeColumnMaskTable table)
        {
            // We don't have the OrderID for the derivieed shipment tables
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(UtilityFunctions.GetTriggerTableName(), "ShipmentID", "NULL", 31, GetUpdatedColumns(table), con);
        }

        /// <summary>
        /// Insert dirty order child from the current trigger
        /// </summary>
        public static void InsertTriggeredOrderChild(SqlConnection con, FilterNodeColumnMaskTable table)
        {
            // Children are't directly filterable, so we just insert there orders
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(UtilityFunctions.GetTriggerTableName(), "OrderID", "NULL", 6, GetUpdatedColumns(table), con);
        }

        /// <summary>
        /// Insert dirty item of a derived OrderItem, that has to do a query up to the order child, that has access to the OrderID
        /// </summary>
        public static void InsertTriggeredDerivedOrderItem(SqlConnection con, FilterNodeColumnMaskTable table)
        {
            InsertTriggeredDerivedOrderItem(con, "(SELECT OrderID FROM [OrderItem] WHERE OrderItemID = inserted.OrderItemID) as Objects", table);
        }

        /// <summary>
        /// Insert dirty item of a derived OrderItem, that has to do a query up to the order child, that has access to the OrderID
        /// </summary>
        public static void InsertTriggeredDerivedOrderItem(SqlConnection con, string orderIDQuery, FilterNodeColumnMaskTable table)
        {
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(UtilityFunctions.GetTriggerTableName(), "OrderItemID", orderIDQuery, 13, GetUpdatedColumns(table), con);
        }

        /// <summary>
        /// Insert dirty records required when download detail is inserted
        /// </summary>
        public static void InsertTriggeredDownloadDetail(SqlConnection con)
        {
            // Only record this when it is a subsequent download.  For the initial downlaod, it will already be recorded, since the order itself will have been inserted.
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty("(SELECT OrderID FROM inserted WHERE InitialDownload = 0) as Objects", "OrderID", "NULL", 6, GetUpdatedColumns(FilterNodeColumnMaskTable.DownloadDetail), con);
        }

        /// <summary>
        /// Insert dirty records related to updated\changed outbound email
        /// </summary>
        public static void InsertTriggeredEmailOutboundRelation(SqlConnection con)
        {
            string fromTable = UtilityFunctions.GetTriggerTableName();

            string customersClause = string.Format(@"(SELECT ObjectID as CustomerID FROM {0} WHERE RelationType = 1 AND dbo.GetEntityTableNumber(ObjectID) = 12) as Objects", fromTable);
            string ordersClause = string.Format(@"(SELECT ObjectID as OrderID FROM {0} WHERE RelationType = 1 AND dbo.GetEntityTableNumber(ObjectID) = 6) as Objects", fromTable);

            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(customersClause, "CustomerID", "NULL", 12, GetUpdatedColumns(FilterNodeColumnMaskTable.EmailOutboundRelation), con);
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(ordersClause, "OrderID", "NULL", 6, GetUpdatedColumns(FilterNodeColumnMaskTable.EmailOutboundRelation), con);
        }

        /// <summary>
        /// Insert dirty records related to updated\changed outbound email
        /// </summary>
        public static void InsertTriggeredEmailOutbound(SqlConnection con)
        {
            string fromTable = UtilityFunctions.GetTriggerTableName();

            string customersClause = string.Format(
                @"(SELECT eor.ObjectID as CustomerID " +
                @"    FROM {0} eo INNER JOIN [EmailOutboundRelation] eor ON eo.EmailOutboundID = eor.EmailOutboundID " +
                @"    WHERE eor.RelationType = 1 AND dbo.GetEntityTableNumber(eor.ObjectID) = 12) " +
                @" as Objects", fromTable);

            string ordersClause = string.Format(
                @"(SELECT eor.ObjectID as OrderID " +
                @"    FROM {0} eo INNER JOIN [EmailOutboundRelation] eor ON eo.EmailOutboundID = eor.EmailOutboundID " +
                @"    WHERE eor.RelationType = 1 AND dbo.GetEntityTableNumber(eor.ObjectID) = 6) " +
                @" as Objects", fromTable);

            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(customersClause, "CustomerID", "NULL", 12, GetUpdatedColumns(FilterNodeColumnMaskTable.EmailOutbound), con);
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(ordersClause, "OrderID", "NULL", 6, GetUpdatedColumns(FilterNodeColumnMaskTable.EmailOutbound), con);
        }

        /// <summary>
        /// Insert dirty records related to updated\changed print results
        /// </summary>
        public static void InsertTriggeredPrintResult(SqlConnection con)
        {
            string fromTable = UtilityFunctions.GetTriggerTableName();

            string customersClause = string.Format(@"(SELECT RelatedObjectID as CustomerID FROM {0} WHERE dbo.GetEntityTableNumber(RelatedObjectID) = 12) as Objects", fromTable);
            string ordersClause = string.Format(@"(SELECT RelatedObjectID as OrderID FROM {0} WHERE dbo.GetEntityTableNumber(RelatedObjectID) = 6) as Objects", fromTable);

            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(customersClause, "CustomerID", "NULL", 12, GetUpdatedColumns(FilterNodeColumnMaskTable.PrintResult), con);
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(ordersClause, "OrderID", "NULL", 6, GetUpdatedColumns(FilterNodeColumnMaskTable.PrintResult), con);
        }

        /// <summary>
        /// Insert dirty records related to updated\changed notes
        /// </summary>
        public static void InsertTriggeredNote(SqlConnection con)
        {
            string fromTable = UtilityFunctions.GetTriggerTableName();

            string customersClause = string.Format(@"(SELECT ObjectID as CustomerID FROM {0} WHERE dbo.GetEntityTableNumber(ObjectID) = 12) as Objects", fromTable);
            string ordersClause = string.Format(@"(SELECT ObjectID as OrderID FROM {0} WHERE dbo.GetEntityTableNumber(ObjectID) = 6) as Objects", fromTable);

            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(customersClause, "CustomerID", "NULL", 12, GetUpdatedColumns(FilterNodeColumnMaskTable.Note), con);
            FilterNodeContentDirtyUtility.MergeIntoFilterDirty(ordersClause, "OrderID", "NULL", 6, GetUpdatedColumns(FilterNodeColumnMaskTable.Note), con);
        }

        /// <summary>
        /// Insert dirty object IDs from the given table and column into the dirty table
        /// </summary>
        private static void MergeIntoFilterDirty(string table, string primaryKey, string parentID, int type, byte[] columnsUpdated, SqlConnection con)
        {
            long computerID = UtilityFunctions.GetComputerID(con);

            // Insert everything into the dirty table.  Dupes will be dealt with when we update the counts.
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = string.Format(@"
                    INSERT INTO FilterNodeContentDirty (ObjectID, ParentID, ObjectType, ComputerID, ColumnsUpdated)
                      SELECT {0}, {1}, @type, @computerID, @columns FROM {2}

                    INSERT INTO QuickFilterNodeContentDirty (ObjectID, ParentID, ObjectType, ComputerID, ColumnsUpdated)
                      SELECT {0}, {1}, @type, @computerID, @columns FROM {2}
                    ",
                    primaryKey, parentID, table);

                cmd.Parameters.AddWithValue("@computerID", computerID);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@columns", columnsUpdated);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get the bit mask that represents the affected columns of the current operation
        /// </summary>
        private static byte[] GetUpdatedColumns(FilterNodeColumnMaskTable table)
        {
            BitArray mask = new BitArray(FilterNodeColumnMaskUtility.TotalBytes * 8);

            for (int i = 0; i < SqlContext.TriggerContext.ColumnCount; i++)
            {
                bool affected = (SqlContext.TriggerContext.TriggerAction != TriggerAction.Update) || SqlContext.TriggerContext.IsUpdatedColumn(i);
                int bitPosition = FilterNodeColumnMaskUtility.GetBitPosition(table, i);

                // A non-update (delete or insert) is effectively a change to every column
                mask[bitPosition] = affected;

                if (affected)
                {
                    // FilterCountUpdater.DebugMessage(string.Format("Bit:{0} is affected", bitPosition));
                }
            }

            return FilterNodeColumnMaskUtility.ConvertBitArrayToBitmask(mask);
        }
    }
}