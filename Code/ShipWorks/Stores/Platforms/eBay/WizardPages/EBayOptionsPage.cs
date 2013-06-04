using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.UI;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Ebay.WizardPages
{
    /// <summary>
    /// Setup wizard page allowing the user to configure eBay options
    /// </summary>
    public partial class EBayOptionsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EBayOptionsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// User moving to the next page in the wizard
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            EbayStoreEntity ebayStore = GetStore<EbayStoreEntity>();
            ebayStore.DownloadItemDetails = downloadDetails.Checked;
        }
    }
}
