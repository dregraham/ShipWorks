using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    /// <summary>
    /// Page for entering the miva credentials
    /// </summary>
    public partial class MivaModuleLoginPage : AddStoreWizardPage
    {
        List<MivaStoreHeader> stores = new List<MivaStoreHeader>();

        /// <summary>
        /// Constructor
        /// </summary>
        public MivaModuleLoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The list of avaliable stores returned from the web client
        /// </summary>
        public List<MivaStoreHeader> AvailableStores
        {
            get { return stores; }
        }

        /// <summary>
        /// Stepping next from the login page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            MivaStoreEntity store = GetStore<MivaStoreEntity>();

            store.ModuleUsername = username.Text;
            store.ModulePassword = SecureText.Encrypt(password.Text, username.Text);
            store.EncryptionPassphrase = encryptionPassphrase.Text.Length == 0 ? "" : SecureText.Encrypt(encryptionPassphrase.Text, username.Text);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                MivaStoreType storeType = (MivaStoreType) StoreTypeManager.GetType(store);

                MivaWebClient webClient = (MivaWebClient) storeType.CreateWebClient();
                stores = webClient.GetMivaStores();

                if (stores.Count == 0)
                {
                    MessageHelper.ShowError(this, "No stores were found on your Miva Merchant website.");

                    e.NextPage = this;
                    return;
                }
            }
            catch (GenericStoreException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                e.NextPage = this;
                return;
            }
        }
    }
}
