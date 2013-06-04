using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Ebay.WizardPages
{
    /// <summary>
    /// Setup Wizard page to configure PayPal interaction for Ebay.
    /// </summary>
    public partial class EBayPayPalPage : WizardPage
    {
        /// <summary>
        /// Gets the Store being configured, as an eBay entity.
        /// </summary>
        private EbayStoreEntity EbayStore
        {
            get
            {
                return (EbayStoreEntity)((AddStoreWizard)Wizard).Store;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EBayPayPalPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// User is moving into this wizard page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            payPalCredentials.LoadCredentials(new PayPalAccountAdapter(EbayStore, "PayPal"));
        }

        /// <summary>
        /// User is navigating to the next page in the wizard.
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            // paypal setting
            EbayStore.DownloadPayPalDetails = usePayPal.Checked;

            if (EbayStore.DownloadPayPalDetails)
            {
                // must have credentials, so validate them now
                if (!payPalCredentials.SaveCredentials(new PayPalAccountAdapter(EbayStore, "PayPal")))
                {
                    // any error messages would have been displayed in the credentials control, so just handle staying here
                    e.NextPage = this;
                    return;
                }
            }
        }

        /// <summary>
        /// User toggled to download paypal information.
        /// </summary>
        private void OnUsePayPalCheckedChanged(object sender, EventArgs e)
        {
            payPalCredentials.Enabled = usePayPal.Checked;
        }
    }
}
