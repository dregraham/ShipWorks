using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.WebServices.Oms;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// UserControl for selecting which MW flags affect the download
    /// </summary>
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
