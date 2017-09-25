using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.WebServices.Oms;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// UserControl for selecting which MW flags affect the download
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public partial class MarketplaceAdvisorOmsFlagsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOmsFlagsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the flags of the given store into the UI
        /// </summary>
        public void LoadFlags(MarketplaceAdvisorStoreEntity store)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                MarketplaceAdvisorOmsFlagManager.LoadFlagGrid(store, (MarketplaceAdvisorOmsFlagTypes) store.DownloadFlags, sandGrid, true);
            }
            catch (MarketplaceAdvisorException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks could not retrieve the custom flags for your account:\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Load the given flags into the UI
        /// </summary>
        public void LoadFlags(OMFlags customFlags, MarketplaceAdvisorOmsFlagTypes currentFlags)
        {
            MarketplaceAdvisorOmsFlagManager.LoadFlagGrid(customFlags, currentFlags, sandGrid, true);
        }

        /// <summary>
        /// Read the selected flags from the UI
        /// </summary>
        public MarketplaceAdvisorOmsFlagTypes ReadFlags()
        {
            return MarketplaceAdvisorOmsFlagManager.ReadFlagGrid(sandGrid);
        }
    }
}
