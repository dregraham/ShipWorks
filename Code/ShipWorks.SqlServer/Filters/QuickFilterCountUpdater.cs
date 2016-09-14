using System;

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
            enforceTimeLimit = false;
            acquireLockTimeLimit = TimeSpan.FromMilliseconds(5);
            acquiringCountsLockName = ActiveCalculationUtility.QuickFilterLockName;

            filterNodeContentDirtyTableName = "QuickFilterNodeContentDirty";
            filterNodeUpdateCheckpointTableName = "QuickFilterNodeUpdateCheckpoint";
            filterNodeUpdatePendingTableName = "QuickFilterNodeUpdatePending";
            filterNodeUpdateCustomerTableName = "QuickFilterNodeUpdateCustomer";
            filterNodeUpdateOrderTableName = "QuickFilterNodeUpdateOrder";
            filterNodeUpdateItemTableName = "QuickFilterNodeUpdateItem";
            filterNodeUpdateShipmentTableName = "QuickFilterNodeUpdateShipment";

            purposeInParam = "2";
        }
    }
}