using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.UI;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Control for setting miscellaneous options relating to the ebay integration
    /// </summary>
    public partial class EbayStoreSettingsControl : StoreSettingsControlBase
    {
        // temporary copy of the store entity being configured
        EbayStoreEntity storeCopy;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads store configuration from the provided entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            EbayStoreEntity ebayStore = store as EbayStoreEntity;
            if (ebayStore == null)
            {
                throw new InvalidOperationException("A non eBay store was passed to the EbayStoreSettingsControl");
            }

            downloadDetailsCheckBox.Checked = ebayStore.DownloadItemDetails;
            downloadPayPalCheckBox.Checked = ebayStore.DownloadPayPalDetails;
            downloadOlderOrders.Checked = ebayStore.DownloadOlderOrders;

            // keep a copy of the store around
            storeCopy = EntityUtility.CloneEntity<EbayStoreEntity>(ebayStore);

            // enable/disable paypal credentials
            credentialsPanel.Enabled = downloadPayPalCheckBox.Checked;
            UpdatePayPalConfig();

            // Load Accepted Payments UI
            acceptedPaymentsControl.LoadStore(ebayStore);

            LoadShippingServices(ebayStore);
        }

        /// <summary>
        /// Populate the shipping service comboboxes
        /// </summary>
        private void LoadShippingServices(EbayStoreEntity ebayStore)
        {
            domesticComboBox.ValueMember = "Key";
            domesticComboBox.DisplayMember = "Value";
            domesticComboBox.DataSource = EbayUtility.ShippingMethods;

            internationalComboBox.ValueMember = "Key";
            internationalComboBox.DisplayMember = "Value";
            internationalComboBox.DataSource = EbayUtility.ShippingMethods;

            // defaults
            if (ebayStore.DomesticShippingService.Length == 0)
            {
                domesticComboBox.SelectedIndex = 0;
            }
            else
            {
                domesticComboBox.SelectedValue = ebayStore.DomesticShippingService;
            }

            // default 
            if (ebayStore.InternationalShippingService.Length == 0)
            {
                internationalComboBox.SelectedIndex = 0;
            }
            else
            {
                internationalComboBox.SelectedValue = ebayStore.InternationalShippingService;
            }
        }

        /// <summary>
        /// Updaet the paypal configuration
        /// </summary>
        private void UpdatePayPalConfig()
        {
            PayPalCredentialType credentialType = (PayPalCredentialType)storeCopy.PayPalApiCredentialType;
            switch (credentialType)
            {
                case PayPalCredentialType.Certificate:
                    credentialTypeTextBox.Text = "Certificate";
                    break;
                case PayPalCredentialType.Signature:
                    credentialTypeTextBox.Text = "Signature";
                    break;
                default:
                    credentialTypeTextBox.Text = "";
                    break;
            }

            // just display the PayPalUserName field in the details
            credentialDetailsTextBox.Text = storeCopy.PayPalApiUserName;
        }

        /// <summary>
        /// Saves store configuration to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            EbayStoreEntity ebayStore = store as EbayStoreEntity;
            if (ebayStore == null)
            {
                throw new InvalidOperationException("A non eBay store was passed to the EbayStoreSettingsControl");
            }

            if (downloadPayPalCheckBox.Checked && storeCopy.PayPalApiUserName.Length == 0)
            {
                MessageHelper.ShowError(this, "PayPal Credentials must be configured.");

                // don't continue
                return false;
            }

            // copy the paypal fields from the temporary entity
            ebayStore.PayPalApiCertificate = storeCopy.PayPalApiCertificate;
            ebayStore.PayPalApiCredentialType = storeCopy.PayPalApiCredentialType;
            ebayStore.PayPalApiPassword = storeCopy.PayPalApiPassword;
            ebayStore.PayPalApiSignature = storeCopy.PayPalApiSignature;
            ebayStore.PayPalApiUserName = storeCopy.PayPalApiUserName;

            // download details
            ebayStore.DownloadItemDetails = downloadDetailsCheckBox.Checked;
            ebayStore.DownloadPayPalDetails = downloadPayPalCheckBox.Checked;
            ebayStore.DownloadOlderOrders = downloadOlderOrders.Checked;

            // accepted payments
            acceptedPaymentsControl.SaveToEntity(ebayStore);

            // default shipping servives when creating invoices
            ebayStore.DomesticShippingService = domesticComboBox.SelectedValue == null ? "" : (string)domesticComboBox.SelectedValue;
            ebayStore.InternationalShippingService = internationalComboBox.SelectedValue == null ? "" : (string)internationalComboBox.SelectedValue;

            return true;
        }

        /// <summary>
        /// User toggled whether or not to download PayPal details
        /// </summary>
        private void OnPayPalDetailsCheckedChanged(object sender, EventArgs e)
        {
            // enable/disable the panel with credential information
            credentialsPanel.Enabled = downloadPayPalCheckBox.Checked;
        }

        /// <summary>
        /// User clicked to configure PayPal credentials.
        /// </summary>
        private void OnConfigureClick(object sender, EventArgs e)
        {
            // show the credential editing dialog
            using (PayPalCredentialDlg dlg = new PayPalCredentialDlg(new PayPalAccountAdapter(storeCopy, "PayPal")))
            {
                dlg.ShowDialog(this);

                UpdatePayPalConfig();
            }
        }

        /// <summary>
        /// Control is loaded
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // resize the InvoicesTitle
            sectionInvoices.Width = Width - sectionInvoices.Left - 30;
            sectionInvoices.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        }
    }
}
