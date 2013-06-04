using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Window for changing the flags set on a MarketplaceAdvisor order
    /// </summary>
    public partial class MarketplaceAdvisorOmsChangeFlagsDlg : Form
    {
        MarketplaceAdvisorStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOmsChangeFlagsDlg(MarketplaceAdvisorStoreEntity store)
        {
            InitializeComponent();

            this.store = store;
        }

        /// <summary>
        /// Window is being shown
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            Refresh();
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                MarketplaceAdvisorOmsFlagManager.LoadFlagGrid(store, MarketplaceAdvisorOmsFlagTypes.None, gridFlagsOn, false);
                MarketplaceAdvisorOmsFlagManager.LoadFlagGrid(store, MarketplaceAdvisorOmsFlagTypes.None, gridFlagsOff, false);
            }
            catch (MarketplaceAdvisorException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks could not retrieve the custom flags for your account:\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// The flags to be enabled.  Only valid when DialogResult is OK
        /// </summary>
        public MarketplaceAdvisorOmsFlagTypes FlagsOn
        {
            get { return MarketplaceAdvisorOmsFlagManager.ReadFlagGrid(gridFlagsOn); }
        }

        /// <summary>
        /// The flags to be disabled.  Only valid when DialogResult is OK
        /// </summary>
        public MarketplaceAdvisorOmsFlagTypes FlagsOff
        {
            get { return MarketplaceAdvisorOmsFlagManager.ReadFlagGrid(gridFlagsOff); }
        }
    }
}
