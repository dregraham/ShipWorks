using System;
using System.Windows.Forms;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.WebServices.Oms;
using ShipWorks.Data.Model;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// UserControl for the account settings for MarketplaceAdvisor
    /// </summary>
    public partial class MarketplaceAdvisorAccountSettingsControl : AccountSettingsControlBase
    {
        bool isOms;

        // Will be set if we upgraded to OMS and the user had to pick which flags to use.
        MarketplaceAdvisorOmsFlagTypes? downloadFlags = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the store into the ui
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            MarketplaceAdvisorStoreEntity mwStore = (MarketplaceAdvisorStoreEntity) store;

            username.Text = mwStore.Username;
            password.Text = SecureText.Decrypt(mwStore.Password, mwStore.Username);

            isOms = (mwStore.AccountType == (int) MarketplaceAdvisorAccountType.OMS);

            if (isOms)
            {
                panelLegacy.Visible = false;
            }
            else
            {
                radioCorporate.Checked = mwStore.AccountType == (int) MarketplaceAdvisorAccountType.LegacyCorporate;
                radioStandard.Checked = mwStore.AccountType == (int) MarketplaceAdvisorAccountType.LegacyStandard;
            }
        }

        /// <summary>
        /// Save the data from the UI into the store
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            MarketplaceAdvisorStoreEntity mwStore = (MarketplaceAdvisorStoreEntity) store;

            mwStore.Username = username.Text.Trim();
            mwStore.Password = SecureText.Encrypt(password.Text, mwStore.Username);

            if (isOms)
            {
                mwStore.AccountType = (int) MarketplaceAdvisorAccountType.OMS;

                if (downloadFlags != null)
                {
                    mwStore.DownloadFlags = (int) downloadFlags.Value;
                }
            }
            else
            {
                mwStore.AccountType = radioCorporate.Checked ?
                    (int) MarketplaceAdvisorAccountType.LegacyCorporate :
                    (int) MarketplaceAdvisorAccountType.LegacyStandard;
            }

            Cursor.Current = Cursors.WaitCursor;

            if (mwStore.Fields[(int) MarketplaceAdvisorStoreFieldIndex.Username].IsChanged ||
                mwStore.Fields[(int) MarketplaceAdvisorStoreFieldIndex.Password].IsChanged ||
                mwStore.Fields[(int) MarketplaceAdvisorStoreFieldIndex.AccountType].IsChanged)
            {
                try
                {
                    // Create the processing client
                    MarketplaceAdvisorLegacyClient client = MarketplaceAdvisorLegacyClient.Create(mwStore);

                    // Process the request
                    client.GetUser();
                }
                catch (MarketplaceAdvisorException ex)
                {
                    MessageHelper.ShowError(this,
                        "ShipWorks could not connect to your MarketplaceAdvisor account using the " +
                        "username and password you entered.\n\n" +
                        "Error: " + ex.Message + "\n\n" +
                        "Please check that what you entered is correct.");

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Change the account type to be OMS
        /// </summary>
        private void OnChangeToOms(object sender, EventArgs e)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning,
                "Updating ShipWorks to use OMS for your MarketplaceAdvisor account cannot be undone.\n\nContinue?");

            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    OMFlags customFlags = MarketplaceAdvisorOmsFlagManager.GetCustomFlags(new MarketplaceAdvisorStoreEntity
                        {
                            Username = username.Text.Trim(),
                            Password = SecureText.Encrypt(password.Text, username.Text.Trim())
                        });

                    using (MarketplaceAdvisorOmsUpgradeDlg dlg = new MarketplaceAdvisorOmsUpgradeDlg(customFlags))
                    {
                        if (dlg.ShowDialog(this) == DialogResult.OK)
                        {
                            panelLegacy.Visible = false;
                            isOms = true;

                            downloadFlags = dlg.SelectedFlags;
                        }
                    }
                }
                catch (MarketplaceAdvisorException ex)
                {
                    MessageHelper.ShowError(this,
                        "ShipWorks could not retrieve the custom flags from your OMS account. Please ensure MarketplaceAdvisor has already upgraded your account.\n\nError: " + ex.Message);
                }
            }
        }
    }
}
