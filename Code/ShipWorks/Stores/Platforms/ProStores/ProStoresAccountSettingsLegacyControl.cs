using System;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// Account settings control for stores that logon to ProStores the legacy way
    /// </summary>
    [ToolboxItem(true)]
    public partial class ProStoresAccountSettingsLegacyControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresAccountSettingsLegacyControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data for the given store
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            ProStoresStoreEntity proStore = (ProStoresStoreEntity) store;

            username.Text = proStore.Username;
            password.Text = (proStore.LegacyPassword.Length > 0) ? SecureText.Decrypt(proStore.LegacyPassword, proStore.Username) : "";

            shortName.Text = proStore.ShortName;
            adminUrl.Text = proStore.LegacyAdminUrl;
            xteProcessUrl.Text = proStore.LegacyXtePath;
            prefix.Text = proStore.LegacyPrefix;
        }

        /// <summary>
        /// Save the data to the given store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            if (username.Text.Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your ProStores username.");

                return false;
            }

            if (shortName.Text.Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter the unique name of your ProStores store.");

                return false;
            }

            string urlAdmin = adminUrl.Text.Trim();
            string urlXte = xteProcessUrl.Text.Trim();

            if (urlXte.Length == 0 || urlAdmin.Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter the Admin URL and XTE Process URL of your ProStores store.");

                return false;
            }

            if (urlAdmin.IndexOf(Uri.SchemeDelimiter) == -1)
            {
                urlAdmin = "https://" + urlAdmin;
            }

            if (!urlAdmin.ToLower().StartsWith("https://"))
            {
                MessageHelper.ShowInformation(this, "The ProStores Admin URL must use a secure https connection.");

                return false;
            }

            ProStoresStoreEntity proStore = (ProStoresStoreEntity) store;

            proStore.LoginMethod = (int) ProStoresLoginMethod.LegacyUserPass;

            proStore.Username = username.Text;
            proStore.LegacyPassword = SecureText.Encrypt(password.Text, username.Text);

            proStore.ShortName = shortName.Text.Trim();
            proStore.LegacyAdminUrl = urlAdmin;
            proStore.LegacyXtePath = urlXte;
            proStore.LegacyPrefix = prefix.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ProStoresWebClient.TestXteConnection(proStore);

                return true;
            }
            catch (ProStoresException ex)
            {
                MessageHelper.ShowError(this, "An error occurred while connecting to your ProStores account:\n\n" + ex.Message);

                return false;
            }
        }
    }
}
