using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ShipWorks.SqlServer.General;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
using ShipWorks.Filters;
using System.Data;
using ShipWorks.SqlServer.Filters.DirtyCounts;
using System.Threading;
using System.Runtime.CompilerServices;

namespace ShipWorks.SqlServer.Filters
{
    /// <summary>
    /// Put this in a seperate class instead of inline in the stored proc so we can have scoped member variables
    /// </summary>
    [CompilerGenerated]
    public class FilterCountUpdater
    {
        // Get out as soon after this as possible
        TimeSpan timeLimit = TimeSpan.FromSeconds(15);

        // Ordered list of states
        List<FilterCountCheckpointState> states = new List<FilterCountCheckpointState>();

        // Maps states to required data
        Dictionary<FilterCountCheckpointState, FilterTarget> updateFilterTargetMap = new Dictionary<FilterCountCheckpointState, FilterTarget>();
        Dictionary<FilterCountCheckpointState, string> updateDirtyTableMap = new Dictionary<FilterCountCheckpointState, string>();

        // Indicates if we've updated the root counts so far this session
        bool rootCountsUpdated = false;

        // Indicates if we've discovered we have to truncate with DELETE due to missing permissions
        static bool truncateWithDelete = false;

        class FilterTargetNodeData
        {
            public List<byte[]> ColumnMasks { get; set; }
            public int JoinMask { get; set; }
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        public FilterCountUpdater()
        {
            states.AddRange(Enum.GetValues(typeof(FilterCountCheckpointState)).OfType<FilterCountCheckpointState>().OrderBy(v => (int) v));

            updateFilterTargetMap[FilterCountCheckpointState.UpdateCustomers] = FilterTarget.Customers;
            updateFilterTargetMap[FilterCountCheckpointState.UpdateOrders] = FilterTarget.Orders;
            updateFilterTargetMap[FilterCountCheckpointState.UpdateItems] = FilterTarget.Items;
            updateFilterTargetMap[FilterCountCheckpointState.UpdateShipments] = FilterTarget.Shipments;

            updateDirtyTableMap[FilterCountCheckpointState.UpdateCustomers] = "FilterNodeUpdateCustomer";
            updateDirtyTableMap[FilterCountCheckpointState.UpdateOrders] = "FilterNodeUpdateOrder";
            updateDirtyTableMap[FilterCountCheckpointState.UpdateItems] = "FilterNodeUpdateItem";
            updateDirtyTableMap[FilterCountCheckpointState.UpdateShipments] = "FilterNodeUpdateShipment";
        }

        /// <summary>
        /// Entry point for calculating update filter counts
        /// </summary>
        public void CalculateUpdateFilterCounts()
        {
            // Attach to the connection
            using (SqlConnection con = new SqlConnection("Context connection = true"))
            {
                con.Open();

                // Can't have an open transaction going in
                UtilityFunctions.EnsureNotTransacted(con);

                // Start the timer
                Stopwatch timer = Stopwatch.StartNew();

                // Do out state processing loop
                while (true)
                {
                    // We'll need to have the lock to do the next stop of the checkpoint
                    if (!ActiveCalculationUtility.AcquireCalculatingLock(con, TimeSpan.FromSeconds(5)))
                    {
                        DebugMessage("UpdateCounts: Could not get lock.");
                        return;
                    }

                    try
                    {
                        if (timer.Elapsed >= timeLimit)
                        {
                            DebugMessage("UpdateCounts: Ran out of time.");
                            return;
                        }

                        // We have to reget the checkpoint each time through the loop, b\c another it could have been updated by another
                        // process while we were waiting to acquire the lock for this loop iteration.
                        FilterCountCheckpoint checkpoint = GetCheckpoint(con);
                        
                        // If null it means there are no dirty records to deal with
                        if (checkpoint == null)
                        {
                            DebugMessage("UpdateCounts: Nothing dirty to calculate");
                            return;
                        }

                        // Always update the roots - even if we're in the middle of a checkpoint.  We want the global overall count
                        // to be as updated and accurate as possible.
                        if (!rootCountsUpdated)
                        {
                            UpdateRootNodeCounts(FilterTarget.Orders, con);
                            UpdateRootNodeCounts(FilterTarget.Customers, con);

                            rootCountsUpdated = true;
                        }

                        Stopwatch iterationTimer = Stopwatch.StartNew();

                        bool readyForNextState = true;

                        switch (checkpoint.State)
                        {
                            // Capture the filter nodes that will be updated and prepare the dirty tables
                            case FilterCountCheckpointState.Prepare:
                            {
                                var targetMasks = CaptureNodesForUpdate(checkpoint.MaxDirtyID, con);

                                // Shortcut - if there are no nodes to update (we'd have a mask for each node) then just to straight to being done
                                if (!targetMasks.Any(tm => tm.Value.ColumnMasks.Count > 0))
                                {
                                    DebugMessage("No nodes captured to cleanup");

                                    // Nothing to update - we can go straight to cleanup.  My only use of the 'goto' keyword EVER in my life :)
                                    goto case FilterCountCheckpointState.Cleanup;
                                }

                                // If any of the masks require going getting customerID's for each orderID... we need to make sure all orderID's are filled in
                                if (targetMasks.Values.Any(data => (data.JoinMask & (int) FilterNodeJoinType.CustomerToOrder) != 0))
                                {
                                    SqlCommand fillParentKeyCmd = con.CreateCommand();
                                    fillParentKeyCmd.CommandText = @"
                                        UPDATE FilterNodeContentDirty
                                            SET ParentID = o.CustomerID
                                            FROM [Order] o 
                                            WHERE ObjectType = 6 AND ParentID IS NULL AND o.OrderID = ObjectID;";
                                    int updated = fillParentKeyCmd.ExecuteNonQuery();

                                    DebugMessage(string.Format("Updated {0} order ID's with their customer ID's", updated));
                                }

                                // If any masks require getting the OrderID's for each ShipmentID... we need to make sure all the OrderID's are filled in
                                if (targetMasks.Values.Any(data => ((data.JoinMask & (int) FilterNodeJoinType.CustomerToShipment) != 0) || ((data.JoinMask & (int) FilterNodeJoinType.OrderToShipment) != 0)))
                                {
                                    SqlCommand fillParentKeyCmd = con.CreateCommand();
                                    fillParentKeyCmd.CommandText = @"
                                        UPDATE FilterNodeContentDirty
                                            SET ParentID = s.OrderID
                                            FROM [Shipment] s 
                                            WHERE ObjectType = 31 AND ParentID IS NULL AND s.ShipmentID = ObjectID;";
                                    int updated = fillParentKeyCmd.ExecuteNonQuery();

                                    DebugMessage(string.Format("Updated {0} shipment ID's with their order ID's", updated));
                                }


                                PrepareDirtyCustomers(targetMasks[FilterTarget.Customers], checkpoint.MaxDirtyID, con);
                                PrepareDirtyOrders(targetMasks[FilterTarget.Orders], checkpoint.MaxDirtyID, con);
                                PrepareDirtyItems(targetMasks[FilterTarget.Items], checkpoint.MaxDirtyID, con);
                                PrepareDirtyShipments(targetMasks[FilterTarget.Shipments], checkpoint.MaxDirtyID, con);

                                break;
                            }


                            // Do the updates
                            case FilterCountCheckpointState.UpdateCustomers:
                            case FilterCountCheckpointState.UpdateOrders:
                            case FilterCountCheckpointState.UpdateItems:
                            case FilterCountCheckpointState.UpdateShipments:
                            {
                                readyForNextState = !PerformNextCalculation(updateFilterTargetMap[checkpoint.State], updateDirtyTableMap[checkpoint.State], con);
                                break;
                            }

                            // Do all the cleanup work
                            case FilterCountCheckpointState.Cleanup:
                            {
                                TruncateTable("FilterNodeUpdatePending", con);
                                TruncateTable("FilterNodeUpdateCustomer", con);
                                TruncateTable("FilterNodeUpdateShipment", con);
                                TruncateTable("FilterNodeUpdateItem", con);
                                TruncateTable("FilterNodeUpdateOrder", con);

                                // Delete all the dirty records we processed from the dirty table.  We add the READPAST for 2 reasons:
                                //  1) Its not a big deal if we leave a record in there.  It will get re-looked at the next time we calculate the filter updates,
                                //     but it will not change the counts if it was already used.
                                //  2) Without it, we see deadlocks.  What can happen is this.
                                //       a) ShipWorks starts a TRAN, adds a Customer, which updates (and locks) rows in FilterNodeContentDirty
                                //       b) This DELETE taks a lock on IX_FilterNodeContentDirty, then needs the update lock on the rows to do the delete.
                                //            - it can't get the update lock, sinc the ShipWorks TRAN is holding it.
                                //       c) ShipWorks adds an Order, and needs to update FilterNodeContentDirty again.  So it needs a lock on IX_FilterNodeContentDiryt, but
                                //          it can't, b\c the delete already took it.
                                SqlCommand clearDirtyCmd = con.CreateCommand();
                                clearDirtyCmd.CommandText = "DELETE FilterNodeContentDirty WITH (READPAST) WHERE FilterNodeContentDirtyID <= @maxDirtyID; SELECT @@ROWCOUNT";
                                clearDirtyCmd.Parameters.AddWithValue("@maxDirtyID", checkpoint.MaxDirtyID);
                                int deletedCount = (int) clearDirtyCmd.ExecuteScalar();

                                // And finally, clear our checkpoint
                                SqlCommand deleteCheckpointCmd = con.CreateCommand();
                                deleteCheckpointCmd.CommandText = "DELETE FilterNodeUpdateCheckpoint";
                                deleteCheckpointCmd.ExecuteNonQuery();

                                DebugMessage(string.Format("Finished update counts for checkpoint {0}.  Dirty: {1}, Cleaned {2}, Time {3}", 
                                    checkpoint.MaxDirtyID,
                                    checkpoint.DirtyCount,
                                    deletedCount,
                                    (checkpoint.Duration + iterationTimer.ElapsedMilliseconds) / 1000.0));

                                // Don't break - return.  We are done!
                                return;
                            }

                            default:
                            {
                                throw new InvalidOperationException("Invalid state value: " + checkpoint.State);
                            }
                        }

                        // Update the checkpoint for the next state
                        UpdateCheckpoint(checkpoint, readyForNextState, iterationTimer, con);
                    }
                    finally
                    {
                        ActiveCalculationUtility.ReleaseCalculatingLock(con);
                    }
                }
            }
        }

        /// <summary>
        /// Get the current checkpoint.  If there are no current dirty objects and no current checkpoint, this will return null.
        /// </summary>
        private FilterCountCheckpoint GetCheckpoint(SqlConnection con)
        {
            SqlCommand selectCmd = con.CreateCommand();
            selectCmd.CommandText = "SELECT MaxDirtyID, DirtyCount, State, Duration FROM FilterNodeUpdateCheckpoint";

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    FilterCountCheckpoint checkpoint = new FilterCountCheckpoint();
                    checkpoint.MaxDirtyID = (long) reader.GetSqlInt64(0);
                    checkpoint.DirtyCount = (int) reader.GetInt32(1);
                    checkpoint.State = (FilterCountCheckpointState) reader.GetInt32(2);
                    checkpoint.Duration = (int) reader.GetInt32(3);

                    return checkpoint;
                }
            }

            SqlCommand anyDirty = con.CreateCommand();
            anyDirty.CommandText = "SELECT MAX(FilterNodeContentDirtyID) FROM FilterNodeContentDirty WITH (NOLOCK)";

            object maxID = anyDirty.ExecuteScalar();

            // If none are dirty, no need for a checkpoint
            if (maxID == null || maxID is DBNull)
            {
                return null;
            }

            SqlCommand insertCmd = con.CreateCommand();
            insertCmd.CommandText = string.Format(
                @"INSERT INTO FilterNodeUpdateCheckpoint (MaxDirtyID, DirtyCount, State, Duration) 
                    VALUES (@maxDirtyID, (SELECT COUNT(ObjectID) FROM FilterNodeContentDirty WITH (NOLOCK) WHERE FilterNodeContentDirtyID <= @maxDirtyID), {0}, 0)", 
                (int) states[0]);
            insertCmd.Parameters.AddWithValue("@maxDirtyID", (long) maxID);
            insertCmd.ExecuteNonQuery();

            // It will now be there
            return GetCheckpoint(con);
        }

        /// <summary>
        /// Move the checkpoint state to the next in sequence
        /// </summary>
        private void UpdateCheckpoint(FilterCountCheckpoint checkpoint, bool moveToNextState, Stopwatch iterationTimer, SqlConnection con)
        {
            if (moveToNextState)
            {
                int index = states.IndexOf(checkpoint.State);

                DebugMessage(string.Format("UpdateCounts: moving from {0} to {1}", checkpoint.State, states[index + 1]));

                checkpoint.State = states[index + 1];
            }

            DebugMessage(string.Format("  [Iteration took {0}]", iterationTimer.Elapsed.TotalSeconds));

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE FilterNodeUpdateCheckpoint SET State = @state, Duration = Duration + @duration";
            cmd.Parameters.AddWithValue("@state", (int) checkpoint.State);
            cmd.Parameters.AddWithValue("@duration", iterationTimer.ElapsedMilliseconds);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Root nodes are treated specially for performance reasons.
        /// </summary>
        private static void UpdateRootNodeCounts(FilterTarget target, SqlConnection con)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"

                SET XACT_ABORT ON;

                BEGIN TRAN;

                CREATE TABLE #Change (Change int);

                DELETE FilterNodeRootDirty WITH (READPAST)
                  OUTPUT deleted.Change INTO #Change
                  WHERE FilterNodeContentID = @FilterNodeContentID;

                IF (@@ROWCOUNT > 0)
                BEGIN

                    DECLARE @change int
                    SELECT @change = COALESCE(SUM(Change), 0) FROM #Change;

                    UPDATE FilterNodeContent
                         SET [Count] = [Count] + @change,
                             [CountVersion] = [CountVersion] + 1
                      WHERE FilterNodeContentID = @FilterNodeContentID;

                END

                DROP TABLE #Change;

                COMMIT;

                SET XACT_ABORT OFF;
            ";
             
            cmd.Parameters.AddWithValue("@FilterNodeContentID", BuiltinFilter.GetTopLevelKey(target));
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Capture all the nodes that are ready for updating. Returns false if there are no needs that need calculating.
        /// </summary>
        private static Dictionary<FilterTarget, FilterTargetNodeData> CaptureNodesForUpdate(long maxDirtyID, SqlConnection con)
        {
            // In case we partially finished last time
            TruncateTable("FilterNodeUpdatePending", con);

            // Select all counts, filters before folders, from least to greatest cost, where initial counts have already completed.
            // We know that no new rows where the initial counts are completed will happen during this procedure due to the lock
            // we take with ActiveCalculationUtility.
            SqlCommand readyToCountCmd = con.CreateCommand();
            readyToCountCmd.CommandText = string.Format(@"
                    INSERT INTO FilterNodeUpdatePending (FilterNodeContentID, FilterTarget, ColumnMask, JoinMask, Position)
		                OUTPUT inserted.FilterTarget, inserted.ColumnMask, inserted.JoinMask
	                    SELECT c.FilterNodeContentID, f.FilterTarget, c.ColumnMask, c.JoinMask, ROW_NUMBER() OVER(ORDER BY f.IsFolder ASC, c.Cost ASC)
	                      FROM FilterNode n INNER JOIN FilterSequence s ON n.FilterSequenceID = s.FilterSequenceID 
						                    INNER JOIN Filter f ON s.FilterID = f.FilterID 
						                    INNER JOIN FilterNodeContent c ON n.FilterNodeContentID = c.FilterNodeContentID
	                      WHERE f.[State] = 1 AND c.UpdateCalculation != '' AND (c.Status != {0} AND c.Status != {1})
                                AND (SELECT COUNT(*) FROM FilterNodeContentDirty WITH (NOLOCK) WHERE dbo.BitwiseAnd(ColumnsUpdated, c.ColumnMask) != 0x0) > 0",
                (int) FilterCountStatus.NeedsInitialCount,
                (int) FilterCountStatus.RunningInitialCount);

            // We want to know what masks each target uses, so we know what the dependencies are
            Dictionary<FilterTarget, FilterTargetNodeData> targetNodeData = new Dictionary<FilterTarget, FilterTargetNodeData>();
            targetNodeData[FilterTarget.Customers] = new FilterTargetNodeData { ColumnMasks = new List<byte[]>() };
            targetNodeData[FilterTarget.Orders] = new FilterTargetNodeData { ColumnMasks = new List<byte[]>() };
            targetNodeData[FilterTarget.Items] = new FilterTargetNodeData { ColumnMasks = new List<byte[]>() };
            targetNodeData[FilterTarget.Shipments] = new FilterTargetNodeData { ColumnMasks = new List<byte[]>() };

            // Load all the masks, sorted by what FilterTarget they apply to
            using (SqlDataReader reader = readyToCountCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    FilterTarget target = (FilterTarget) reader[0];
                    byte[] columnMask = (byte[]) reader[1];
                    int joinMask = (int) reader[2];

                    targetNodeData[target].ColumnMasks.Add(columnMask);
                    targetNodeData[target].JoinMask |= joinMask;
                }
            }

            List<byte[]> allMasks = targetNodeData.SelectMany(t => t.Value.ColumnMasks).ToList();

            // We don't need to trim if there arent any, b\c we won't be acting on them anyway
            if (allMasks.Count > 0)
            {
                // We don't even need to look at dirty rows that we have no filters that care about them
                SqlCommand unneededCmd = con.CreateCommand();
                unneededCmd.CommandText = "DELETE FilterNodeContentDirty WHERE FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(ColumnsUpdated, @columnMask) = 0x0";
                unneededCmd.Parameters.AddWithValue("@columnMask", FilterNodeColumnMaskUtility.CreateUnionedBitmask(allMasks));
                unneededCmd.Parameters.AddWithValue("@maxDirtyID", maxDirtyID);

                int removed = unneededCmd.ExecuteNonQuery();

                DebugMessage(string.Format("Removed {0} unneeded dirty counts", removed));
            }

            return targetNodeData;
        }

        /// <summary>
        /// Create the dirty customers table and returns how many were dirty.
        /// </summary>
        private static void PrepareDirtyCustomers(FilterTargetNodeData nodeData, long maxDirtyID, SqlConnection con)
        {
            // Incase we partically finished last time
            TruncateTable("FilterNodeUpdateCustomer", con);

            // There are no nodes of this type to even use this data anyway, get out
            if (nodeData.ColumnMasks.Count == 0)
            {
                return;
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.Parameters.AddWithValue("@maxDirtyID", maxDirtyID);
            cmd.Parameters.AddWithValue("@columnMask", FilterNodeColumnMaskUtility.CreateUnionedBitmask(nodeData.ColumnMasks));

            // Of course we need the actual dirty customers
            cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateCustomer (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT ObjectID, ComputerID, ColumnsUpdated 
                   FROM FilterNodeContentDirty WITH (NOLOCK)
                   WHERE ObjectType = 12 AND FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(ColumnsUpdated, @columnMask) != 0x0;";
            cmd.ExecuteNonQuery();

            // If a customer filter joins to orders, we need the customer ID of each edited order
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToOrder) != 0)
            {
                DebugMessage("Rolling orders up to customers");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateCustomer (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT ParentID, ComputerID, ColumnsUpdated
                   FROM FilterNodeContentDirty WITH (NOLOCK)
                   WHERE ObjectType = 6 AND ParentID IS NOT NULL AND FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            // If a customer filter joins to items, we need the customer ID of each edited item
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToItem) != 0)
            {
                DebugMessage("Rolling items up to customers");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateCustomer (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT o.CustomerID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o WITH (NOLOCK) ON o.OrderID = d.ParentID 
                       WHERE ObjectType = 13 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            // If a customer filter joins to shipments, we need the customer ID of each edited shipment
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToShipment) != 0)
            {
                DebugMessage("Rolling shipments up to customers");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateCustomer (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT o.CustomerID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o WITH (NOLOCK) ON o.OrderID = d.ParentID 
                   WHERE ObjectType = 31 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create a table that contains all dirty orders
        /// </summary>
        private static void PrepareDirtyOrders(FilterTargetNodeData nodeData, long maxDirtyID, SqlConnection con)
        {
            // Incase we partically finished last time
            TruncateTable("FilterNodeUpdateOrder", con);

            // There are no nodes of this type to even use this data anyway, get out
            if (nodeData.ColumnMasks.Count == 0)
            {
                return;
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.Parameters.AddWithValue("@maxDirtyID", maxDirtyID);
            cmd.Parameters.AddWithValue("@columnMask", FilterNodeColumnMaskUtility.CreateUnionedBitmask(nodeData.ColumnMasks));

            // Of course we need the actual dirty orders
            cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateOrder (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT ObjectID, ComputerID, ColumnsUpdated
                   FROM FilterNodeContentDirty WITH (NOLOCK) 
                   WHERE ObjectType = 6 AND FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(ColumnsUpdated, @columnMask) != 0x0;";
            cmd.ExecuteNonQuery();

            // If we join to a customer that joins to orders, we need the customer ID of each edited order
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToOrder) != 0)
            {
                DebugMessage("Rolling orders -> Customers -> orders");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateOrder (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT o.OrderID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o WITH (NOLOCK) ON o.CustomerID = d.ParentID
                   WHERE d.ObjectType = 6 AND d.ParentID IS NOT NULL AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to a customer that joins to items, we need the orders of each customer with an edited item
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToItem) != 0)
            {
                DebugMessage("Rolling items -> Customers -> orders");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateOrder (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT o2.OrderID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o1 WITH (NOLOCK) ON o1.OrderID = d.ParentID INNER JOIN [Order] o2 WITH (NOLOCK) ON o1.CustomerID = o2.CustomerID
                       WHERE d.ObjectType = 13 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to a customer that joins to shipments, we need the customer ID of each edited shipment
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToShipment) != 0)
            {
                DebugMessage("Rolling shipments -> Customers -> orders");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateOrder (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT o2.OrderID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o1 WITH (NOLOCK) ON o1.OrderID = d.ParentID INNER JOIN [Order] o2 WITH (NOLOCK) ON o1.CustomerID = o2.CustomerID
                       WHERE d.ObjectType = 31 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            // If the order refers to customers, we need any dirty customers order's
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.OrderToCustomer) != 0)
            {
                DebugMessage("Rolling Customers -> orders");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateOrder (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT o.OrderID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o WITH (NOLOCK) ON o.CustomerID = d.ObjectID
                   WHERE d.ObjectType = 12 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            // If a order filter joins to items, we need the order ID of each edited item
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.OrderToItem) != 0 && (nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToItem) == 0)
            {
                DebugMessage("Rolling items -> orders");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateOrder (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT d.ParentID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK)
                       WHERE d.ObjectType = 13 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND d.ParentID IS NOT NULL AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            // If a order filter joins to shipments, we need the order ID of each edited shipment
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.OrderToShipment) != 0 && (nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToShipment) == 0)
            {
                DebugMessage("Rolling shipments -> orders");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateOrder (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT d.ParentID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK)
                   WHERE d.ObjectType = 31 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND d.ParentID IS NOT NULL AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create a table that contains all dirty items
        /// </summary>
        private static void PrepareDirtyItems(FilterTargetNodeData nodeData, long maxDirtyID, SqlConnection con)
        {
            // Incase we partically finished last time
            TruncateTable("FilterNodeUpdateItem", con);

            // There are no nodes of this type to even use this data anyway, get out
            if (nodeData.ColumnMasks.Count == 0)
            {
                return;
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.Parameters.AddWithValue("@maxDirtyID", maxDirtyID);
            cmd.Parameters.AddWithValue("@columnMask", FilterNodeColumnMaskUtility.CreateUnionedBitmask(nodeData.ColumnMasks));

            // Of course we need the actual dirty items
            cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateItem (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT ObjectID, ComputerID, ColumnsUpdated
                   FROM FilterNodeContentDirty WITH (NOLOCK) 
                   WHERE ObjectType = 13 AND FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(ColumnsUpdated, @columnMask) != 0x0;";
            cmd.ExecuteNonQuery();

            // If we join to a customer that joins to orders, we need the items of each customer with an edited order
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToOrder) != 0)
            {
                DebugMessage("Rolling orders -> Customers -> items");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateItem (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT i.OrderItemID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o WITH (NOLOCK) ON o.CustomerID = d.ParentID INNER JOIN [OrderItem] i WITH (NOLOCK) ON i.OrderID = o.OrderID
                   WHERE d.ObjectType = 6 AND d.ParentID IS NOT NULL AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to a customer that joins to items, we need the items of each customer with an edited item
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToItem) != 0)
            {
                DebugMessage("Rolling items -> Customers -> items");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateItem (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT i.OrderItemID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o1 WITH (NOLOCK) ON o1.OrderID = d.ParentID INNER JOIN [Order] o2 WITH (NOLOCK) ON o1.CustomerID = o2.CustomerID INNER JOIN [OrderItem] i WITH (NOLOCK) ON i.OrderID = o2.OrderID
                       WHERE d.ObjectType = 13 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to a customer that joins to shipments, we need all items of each customer with an edited shipment
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToShipment) != 0)
            {
                DebugMessage("Rolling shipments -> Customers -> items");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateItem (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT i.OrderItemID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o1 WITH (NOLOCK) ON o1.OrderID = d.ParentID INNER JOIN [Order] o2 WITH (NOLOCK) ON o1.CustomerID = o2.CustomerID INNER JOIN [OrderItem] i WITH (NOLOCK) ON i.OrderID = o2.OrderID
                       WHERE d.ObjectType = 31 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to orders that joins to items, we need the items of each order with an edited item
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.OrderToItem) != 0 && (nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToItem) == 0)
            {
                DebugMessage("Rolling items -> orders -> items");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateItem (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT i.OrderItemID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [OrderItem] i WITH (NOLOCK) ON i.OrderID = d.ParentID
                       WHERE d.ObjectType = 13 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to orders that joins to shipments, we need all items of each order with an edited shipment
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.OrderToShipment) != 0 && (nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToShipment) == 0)
            {
                DebugMessage("Rolling shipments -> orders -> items");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateItem (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT i.OrderItemID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [OrderItem] i WITH (NOLOCK) ON i.OrderID = d.ParentID
                       WHERE d.ObjectType = 31 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            // If we join to a order, we need the items of each edited order
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.ItemToOrder) != 0 && (nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToOrder) == 0)
            {
                DebugMessage("Rolling orders -> items");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateItem (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT i.OrderItemID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [OrderItem] i WITH (NOLOCK) ON i.OrderID = d.ObjectID
                   WHERE d.ObjectType = 6 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            // If we join to a customer, we need the items of each edited customer
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.ItemToCustomer) != 0)
            {
                DebugMessage("Rolling customers -> items");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateItem (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT i.OrderItemID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o WITH (NOLOCK) ON o.CustomerID = d.ObjectID INNER JOIN [OrderItem] i WITH (NOLOCK) ON i.OrderID = o.OrderID
                   WHERE d.ObjectType = 12 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create a table that contains all dirty shipments
        /// </summary>
        private static void PrepareDirtyShipments(FilterTargetNodeData nodeData, long maxDirtyID, SqlConnection con)
        {
            // Incase we partically finished last time
            TruncateTable("FilterNodeUpdateShipment", con);

            // There are no nodes of this type to even use this data anyway, get out
            if (nodeData.ColumnMasks.Count == 0)
            {
                return;
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.Parameters.AddWithValue("@maxDirtyID", maxDirtyID);
            cmd.Parameters.AddWithValue("@columnMask", FilterNodeColumnMaskUtility.CreateUnionedBitmask(nodeData.ColumnMasks));

            // Of course we need the actual dirty shipments
            cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateShipment (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT ObjectID, ComputerID, ColumnsUpdated
                   FROM FilterNodeContentDirty WITH (NOLOCK) 
                   WHERE ObjectType = 31 AND FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(ColumnsUpdated, @columnMask) != 0x0;";
            cmd.ExecuteNonQuery();

            // If we join to a customer that joins to orders, we need the shipments of each customer with an edited order
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToOrder) != 0)
            {
                DebugMessage("Rolling orders -> Customers -> shipments");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateShipment (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT s.ShipmentID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o WITH (NOLOCK) ON o.CustomerID = d.ParentID INNER JOIN [Shipment] s WITH (NOLOCK) ON s.OrderID = o.OrderID
                   WHERE d.ObjectType = 6 AND d.ParentID IS NOT NULL AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to a customer that joins to items, we need the shipments of each customer with an edited item
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToItem) != 0)
            {
                DebugMessage("Rolling items -> Customers -> shipments");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateShipment (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT s.ShipmentID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o1 WITH (NOLOCK) ON o1.OrderID = d.ParentID INNER JOIN [Order] o2 WITH (NOLOCK) ON o1.CustomerID = o2.CustomerID INNER JOIN [Shipment] s WITH (NOLOCK) ON s.OrderID = o2.OrderID
                       WHERE d.ObjectType = 13 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to a customer that joins to shipments, we need all shipments of each customer with an edited shipment
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToShipment) != 0)
            {
                DebugMessage("Rolling shipments -> Customers -> shipments");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateShipment (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT s.ShipmentID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o1 WITH (NOLOCK) ON o1.OrderID = d.ParentID INNER JOIN [Order] o2 WITH (NOLOCK) ON o1.CustomerID = o2.CustomerID INNER JOIN [Shipment] s WITH (NOLOCK) ON s.OrderID = o2.OrderID
                       WHERE d.ObjectType = 31 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to orders that joins to items, we need the shipments of each order with an edited item
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.OrderToItem) != 0 && (nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToItem) == 0)
            {
                DebugMessage("Rolling items -> orders -> shipments");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateShipment (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT s.ShipmentID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Shipment] s WITH (NOLOCK) ON s.OrderID = d.ParentID
                       WHERE d.ObjectType = 13 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            //  If we join to orders that joins to shipments, we need all shipments of each order with an edited shipment
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.OrderToShipment) != 0 && (nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToShipment) == 0)
            {
                DebugMessage("Rolling shipments -> orders -> shipments");

                cmd.CommandText = @"
                    INSERT INTO FilterNodeUpdateShipment (ObjectID, ComputerID, ColumnsUpdated)
                       SELECT s.ShipmentID, d.ComputerID, d.ColumnsUpdated
                       FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Shipment] s WITH (NOLOCK) ON s.OrderID = d.ParentID
                       WHERE d.ObjectType = 31 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            // If we join to a order, we need the shipments of each edited order
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.ShipmentToOrder) != 0 && (nodeData.JoinMask & (int) FilterNodeJoinType.CustomerToOrder) == 0)
            {
                DebugMessage("Rolling orders -> shipments");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateShipment (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT s.ShipmentID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Shipment] s WITH (NOLOCK) ON s.OrderID = d.ObjectID
                   WHERE d.ObjectType = 6 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }

            // If we join to a customer, we need the shipments of each edited customer
            if ((nodeData.JoinMask & (int) FilterNodeJoinType.ShipmentToCustomer) != 0)
            {
                DebugMessage("Rolling customers -> shipments");

                cmd.CommandText = @"
                INSERT INTO FilterNodeUpdateShipment (ObjectID, ComputerID, ColumnsUpdated)
                   SELECT s.ShipmentID, d.ComputerID, d.ColumnsUpdated
                   FROM FilterNodeContentDirty d WITH (NOLOCK) INNER JOIN [Order] o WITH (NOLOCK) ON o.CustomerID = d.ObjectID INNER JOIN [Shipment] s WITH (NOLOCK) ON s.OrderID = o.OrderID
                   WHERE d.ObjectType = 12 AND d.FilterNodeContentDirtyID <= @maxDirtyID AND dbo.BitwiseAnd(d.ColumnsUpdated, @columnMask) != 0x0;";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Perform the calculation on the record actively pointed to by the reader.  Returns false if there weren't anymore to process.
        /// </summary>
        private static bool PerformNextCalculation(FilterTarget filterTarget, string dirtyTable, SqlConnection con)
        {
            // Get the next count to do for the given target
            SqlCommand selectCmd = con.CreateCommand();
            selectCmd.CommandText = "SELECT TOP(1) FilterNodeContentID FROM FilterNodeUpdatePending WHERE FilterTarget = @target ORDER BY Position ASC";
            selectCmd.Parameters.AddWithValue("@target", (int) filterTarget);
            object nodeResult = selectCmd.ExecuteScalar();

            // If null, then there are no more to do for this filter target
            if (nodeResult == null)
            {
                DebugMessage("UpdateCalculations: No more nodes to calculate for " + filterTarget);
                return false;
            }

            long filterNodeContentID = (long) nodeResult;

            // Now we lookup the calculation SQL at the same time we verify this content still belongs in a filternode.  If it doesn't, there's no need
            // to calculate it.
            SqlCommand retrieveCmd = con.CreateCommand();
            retrieveCmd.CommandText = @"
                SELECT n.FilterNodeID, c.UpdateCalculation 
                    FROM FilterNode n INNER JOIN FilterNodeContent c ON n.FilterNodeContentID = c.FilterNodeContentID
                    WHERE c.FilterNodeContentID = @contentID";
            retrieveCmd.Parameters.AddWithValue("@contentID", filterNodeContentID);

            long filterNodeID = -1;
            string calculationSql = null;

            using (SqlDataReader reader = retrieveCmd.ExecuteReader())
            {
                // May not have any records if the content is no longer apart of a filter and\or get deleted
                if (reader.Read())
                {
                    filterNodeID = reader.GetInt64(0);
                    calculationSql = reader.GetString(1);
                }
            }

            try
            {
                // Make sure we got it
                if (filterNodeID != -1)
                {
                    // We want ourselves to be chosen as the deadlock victim, since we are prepared to handle it.
                    UtilityFunctions.SetDeadlockPriority(con, -5);

                    // Mark it as calculating so the UI can update
                    SqlCommand activeCmd = con.CreateCommand();
                    activeCmd.CommandText = string.Format(@"
                    UPDATE FilterNodeContent
                    SET Status = {0}
                    WHERE FilterNodeContentID = @contentID",
                        (int) FilterCountStatus.RunningUpdateCount);
                    activeCmd.Parameters.AddWithValue("@contentID", filterNodeContentID);
                    activeCmd.ExecuteNonQuery();

                    // The calculation string is designed to take a single replacement value of the FilterNodeID. Didn't use {0} notation though
                    // since a user could have that in their actual filter content
                    calculationSql = calculationSql.Replace("<SwFilterNodeID />", filterNodeID.ToString());
                    calculationSql = calculationSql.Replace("#DirtyObjects", dirtyTable);

                    // Run the actual calculation.  Any required transactions are taken care of within the SQL
                    SqlCommand calculateCmd = con.CreateCommand();
                    calculateCmd.CommandText = calculationSql;
                    calculateCmd.ExecuteNonQuery();

                    // Reset the deadlock priority
                    UtilityFunctions.SetDeadlockPriority(con, 0);

                    DebugMessage(string.Format("Calculated content {0} for node {1}", filterNodeContentID, filterNodeID));
                }
                else
                {
                    DebugMessage(string.Format("Node for content {0} went away", filterNodeContentID));
                }

                // We are now done processing this entry 
                SqlCommand deleteCmd = con.CreateCommand();
                deleteCmd.CommandText = "DELETE FilterNodeUpdatePending WHERE FilterNodeContentID = @contentID";
                deleteCmd.Parameters.AddWithValue("@contentID", filterNodeContentID);
                deleteCmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // If it's a deadlock, then we can just ignore it and try again on the next loop
                if (UtilityFunctions.IsDeadlockException(ex))
                {
                    DebugMessage("CalculateUpdateCounts: Deadlocked, will retry.");
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        /// <summary>
        /// Truncate the given table contents
        /// </summary>
        private static void TruncateTable(string table, SqlConnection con)
        {
            if (!truncateWithDelete)
            {
                // Try TRUNCATE for optimal performance
                try
                {
                    SqlCommand truncateCmd = con.CreateCommand();
                    truncateCmd.CommandText = "TRUNCATE TABLE " + table;
                    truncateCmd.ExecuteNonQuery();

                    return;
                }
                catch (SqlException)
                {

                }
            }

            // Fallback to DELETE in case user doesn't have permission
            SqlCommand deleteCmd = con.CreateCommand();
            deleteCmd.CommandText = "DELETE " + table;
            deleteCmd.ExecuteNonQuery();

            truncateWithDelete = true;
        }

        /// <summary>
        /// Send the given message to the SQL pipe
        /// </summary>
        [Conditional("DEBUG")]
        public static void DebugMessage(string message)
        {
            SqlContext.Pipe.Send(message);
        }
    }
}
