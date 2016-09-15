using System;
using ShipWorks.Filters;

namespace ShipWorks.SqlServer.Filters
{
    /// <summary>
    /// Update filter counts for quick filters
    /// </summary>
    public class QuickFilterCountUpdater : FilterCountUpdater
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        public QuickFilterCountUpdater()
        {
            EnforceTimeLimit = false;
            acquireLockTimeLimit = TimeSpan.FromMilliseconds(5);
            acquiringCountsLockName = ActiveCalculationUtility.QuickFilterLockName;

            filterNodeContentDirtyTableName = "QuickFilterNodeContentDirty";
            filterNodeUpdateCheckpointTableName = "QuickFilterNodeUpdateCheckpoint";
            filterNodeUpdatePendingTableName = "QuickFilterNodeUpdatePending";
            filterNodeUpdateCustomerTableName = "QuickFilterNodeUpdateCustomer";
            filterNodeUpdateOrderTableName = "QuickFilterNodeUpdateOrder";
            filterNodeUpdateItemTableName = "QuickFilterNodeUpdateItem";
            filterNodeUpdateShipmentTableName = "QuickFilterNodeUpdateShipment";

            purposeInParam = $"{ (int) FilterNodePurpose.Quick }";
        }
    }
}