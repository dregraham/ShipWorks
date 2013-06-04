using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.WebServices.Oms;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Window shown to the user when the upgrade from legacy to OMS
    /// </summary>
    public partial class MarketplaceAdvisorOmsUpgradeDlg : Form
    {
        OMFlags customFlags;

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOmsUpgradeDlg(OMFlags customFlags)
        {
            InitializeComponent();

            this.customFlags = customFlags;
        }

        /// <summary>
        /// Initialize the control
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            flagsControl.LoadFlags(customFlags, MarketplaceAdvisorOmsFlagTypes.None);
        }

        /// <summary>
        /// The selected flags.  Only valid when DialogResult == OK
        /// </summary>
        public MarketplaceAdvisorOmsFlagTypes SelectedFlags
        {
            get { return flagsControl.ReadFlags(); }
        }
    }
}
