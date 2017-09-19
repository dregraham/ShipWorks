using System;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions
{
    /// <summary>
    /// Editor for changing MarketplaceAdvisor flags for an order or parcel
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public partial class MarketplaceAdvisorChangeFlagsEditor : ActionTaskEditor
    {
        MarketplaceAdvisorChangeFlagsTaskBase task;

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorChangeFlagsEditor(MarketplaceAdvisorChangeFlagsTaskBase task)
        {
            InitializeComponent();

            this.task = task;
        }

        /// <summary>
        /// Loading
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            MarketplaceAdvisorStoreEntity store = (MarketplaceAdvisorStoreEntity) StoreManager.GetStore(task.StoreID);
            if (store != null)
            {
                try
                {
                    MarketplaceAdvisorOmsFlagManager.LoadFlagGrid(store, task.FlagsOn, gridFlagsOn, false);
                    MarketplaceAdvisorOmsFlagManager.LoadFlagGrid(store, task.FlagsOff, gridFlagsOff, false);
                }
                catch (MarketplaceAdvisorException ex)
                {
                    MessageHelper.ShowError(this, "ShipWorks could not retrieve the custom flags for your account:\n\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// The checked state of the grid has changed
        /// </summary>
        private void OnCheckChanged(object sender, GridRowCheckEventArgs e)
        {
            task.FlagsOn = MarketplaceAdvisorOmsFlagManager.ReadFlagGrid(gridFlagsOn);
            task.FlagsOff = MarketplaceAdvisorOmsFlagManager.ReadFlagGrid(gridFlagsOff);
        }
    }
}
