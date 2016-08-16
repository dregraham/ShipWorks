using System;
using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using System.Security.Cryptography;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;
using log4net;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Common user control for configuring account settings from the Store Management window
    /// and the setup wizard.
    /// </summary>
    public partial class AmazonAccountSettingsControl : AccountSettingsControlBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonAccountSettingsControl));

        ClientCertificate amazonCertificate;
        string accessKeyID = "";

        DateTime? cookieExpires = null;

        AmazonMwsAccountSettingsControl mwsAccountSettingsControl = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Whether or not the certificate button is displayed
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        bool showExtendedPanel = true;
        public bool ShowExtendedPanel
        {
            get { return showExtendedPanel; }
            set
            {
                if (showExtendedPanel != value)
                {
                    showExtendedPanel = value;
                    UpdateUI();
                }
            }
        }

        /// <summary>
        /// Load options from the entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            AmazonStoreEntity amazonStore = store as AmazonStoreEntity;

            if (amazonStore == null)
            {
                throw new InvalidOperationException("A non Amazon store was passed to the Amazon Account Setting control.");
            }

            if (!String.IsNullOrEmpty(amazonStore.Cookie))
            {
                cookieExpires = amazonStore.CookieExpires;
            }

            // seller central credentials
            username.Text = amazonStore.SellerCentralUsername;
            password.Text = SecureText.Decrypt(amazonStore.SellerCentralPassword, amazonStore.SellerCentralUsername);

            // merchant information
            merchant.Text = amazonStore.MerchantName;
            merchantToken.Text = amazonStore.MerchantToken;

            try
            {
                amazonCertificate = new ClientCertificate();
                amazonCertificate.Import(amazonStore.Certificate);
                certificateTextBox.Text = amazonCertificate.Subject;
            }
            catch (CryptographicException ex)
            {
                log.Error("Could not import amazon certificate", ex);

                amazonCertificate = null;
            }

            accessKeyID = amazonStore.AccessKeyID;
            accessKeyTextBox.Text = accessKeyID;
        }

        /// <summary>
        /// Save selections to the entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            // validate that we have an amazon entity
            AmazonStoreEntity amazonStore = store as AmazonStoreEntity;
            if (amazonStore == null)
            {
                throw new InvalidOperationException("A non Amazon store was passed to the Amazon Account Setting control.");
            }

            if (mwsAccountSettingsControl == null)
            {
                // require all fields
                if (username.Text.Trim().Length == 0 || password.Text.Trim().Length == 0
                    || merchantToken.Text.Trim().Length == 0 || merchant.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowError(this, "Please enter all your information for each field.");

                    return false;
                }

                // save to the entity
                amazonStore.SellerCentralUsername = username.Text.Trim();
                amazonStore.SellerCentralPassword = SecureText.Encrypt(password.Text.Trim(), username.Text.Trim());
                amazonStore.MerchantName = merchant.Text.Trim();
                amazonStore.MerchantToken = merchantToken.Text.Trim();

                // certificate
                amazonStore.AccessKeyID = accessKeyID;

                // Only set it if we have one successfully loaded
                if (amazonCertificate != null)
                {
                    amazonStore.Certificate = amazonCertificate.Export();
                }

                // verify these first
                try
                {
                    AmazonFeedClient client = new AmazonFeedClient(amazonStore);
                    client.TestConnection();
                }
                catch (AmazonException ex)
                {
                    MessageHelper.ShowError(this, String.Format("Unable to validate these credentials with Amazon:\r\n{0}", ex.Message));
                    return false;
                }

                return true;
            }
            else
            {
                // Upgraded to MWS
                amazonStore.AmazonApi = (int)AmazonApi.MarketplaceWebService;

                // save the MWS settings
                return mwsAccountSettingsControl.SaveToEntity(store);
            }
        }

        /// <summary>
        /// Loading the UI
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// Updates the Visible/Hidden certificate button
        /// </summary>
        private void UpdateUI()
        {
            extendedPanel.Visible = showExtendedPanel;

            if (!extendedPanel.Visible)
            {
                Height = extendedPanel.Top;
            }
            else
            {
                Height = extendedPanel.Top + extendedPanel.Height;
            }
        }

        /// <summary>
        /// Brings up the window for the user to load in new AWS credentials.
        /// </summary>
        private void OnImportClick(object sender, EventArgs e)
        {
            // create and display the import form
            using (AmazonCertificateImportForm form = new AmazonCertificateImportForm(accessKeyID))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    // keep track of the newly imported certificate
                    accessKeyID = form.AccessKeyID;
                    amazonCertificate = form.ClientCertificate;

                    accessKeyTextBox.Text = accessKeyID;
                    certificateTextBox.Text = amazonCertificate.Subject;
                }
            }
        }

        /// <summary>
        /// Open the seller central
        /// </summary>
        private void OnLinkSellerSentral(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://sellercentral.amazon.com/gp/seller/configuration/account-info-page.html", this);
        }

        /// <summary>
        /// Enable Marketplace Web Services
        /// </summary>
        private void OnEnableMWSClicked(object sender, EventArgs e)
        {
            //string message = "Are you sure you want to convert to Amazon MWS?  This cannot be undone.";

            //// show a confirmation and/or warnings if they aren't absolutely required to upgrade since some functionality will be lost
            //if (MessageHelper.ShowQuestion(this, message) != DialogResult.OK)
            //{
            //    return;
            //}

            //using (AuthorizationInstructionsDlg dlg = new AuthorizationInstructionsDlg("US"))
            //{
            //    if (dlg.ShowDialog(this) == DialogResult.OK)
            //    {
            //        AmazonStoreEntity tempStore = new AmazonStoreEntity();
            //        tempStore.MerchantID = dlg.MerchantID;

            //        mwsAccountSettingsControl = new AmazonMwsAccountSettingsControl();
            //        mwsAccountSettingsControl.LoadStore(tempStore);

            //        // clear all child controls
            //        Controls.Clear();

            //        // add in the new MWS Account Settings Control
            //        Controls.Add(mwsAccountSettingsControl);
            //    }
            //}

            throw new AmazonException("Legacy Code");
        }
    }
}
