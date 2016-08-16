using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using log4net;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    public partial class ThreeDCartAccountSettingsControl : AccountSettingsControlBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartAccountSettingsControl));
        private ThreeDCartStoreEntity threeDCartStoreEntity;
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        /// <param name="store"></param>
        public override void LoadStore(StoreEntity store)
        {
            base.LoadStore(store);

            ThreeDCartStoreEntity threeDCartStore = GetThreeDCartStore(store);

            storeUrl.Text = threeDCartStore.StoreUrl;
            apiUserKey.Text = threeDCartStore.ApiUserKey;

            ThreeDCartStoreType storeType = new ThreeDCartStoreType(threeDCartStore);
            helpLink.Url = storeType.AccountSettingsHelpUrl;

            threeDCartStoreEntity = threeDCartStore;

            // Don't need the add store help link in settings page, they've clearly already added the store
            labelHelpText.Visible = false;
            helpLink.Visible = false;

            if (!threeDCartStore.RestUser)
            {
                panelUpgrade.Visible = true;
            }
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        /// <param name="store"></param>
        /// <returns>True if the entered settings can successfully connect to the store.</returns>
        public override bool SaveToEntity(StoreEntity store)
        {
            string storeUrlToSave = CheckStoreUrlForErrors();
            string apiUserKeyToSave = CheckTokenForErrors(apiUserKey.Text);
            if (storeUrlToSave == string.Empty || apiUserKeyToSave == string.Empty)
            {
                return false;
            }

            ThreeDCartStoreEntity threeDCartStore = GetThreeDCartStore(store);
            threeDCartStore.StoreUrl = storeUrlToSave;
            threeDCartStore.ApiUserKey = apiUserKeyToSave;

            // Now try to make a call to the store with the user's entered settings
            if (ConnectionVerificationNeeded(threeDCartStore))
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    if (threeDCartStore.RestUser)
                    {
                        ThreeDCartRestWebClient webClient = new ThreeDCartRestWebClient(threeDCartStore);
                        webClient.TestConnection();
                    }
                    else
                    {
                        ThreeDCartWebClient webClient = new ThreeDCartWebClient(threeDCartStore, null);
                        webClient.TestConnection();

                        ThreeDCartStatusCodeProvider statusProvider = new ThreeDCartStatusCodeProvider(threeDCartStore);
                        statusProvider.UpdateFromOnlineStore();
                    }
                }
                catch (ThreeDCartException ex)
                {
                    log.Error("ShipWorks encountered an error while attempting to contact 3dcart.", ex);
                    MessageHelper.ShowError(this, "Error connecting to 3dcart. Please check that the store URL and API token are correct.");

                    return false;
                }
            }

            // Everything succeeded, so return true
            return true;
        }

        /// <summary>
        /// Checks the store URL for errors.
        /// </summary>
        private string CheckStoreUrlForErrors()
        {
            // Check the store url for validity
            // First check for blank
            string storeUrlToCheck = storeUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(storeUrlToCheck))
            {
                MessageHelper.ShowError(this, "Please enter the URL of your 3dcart store.");
                return string.Empty;
            }

            // Check for the url scheme, and add https if not present
            if (storeUrlToCheck.IndexOf(Uri.SchemeDelimiter) == -1)
            {
                storeUrlToCheck = string.Format("https://{0}", storeUrlToCheck);
            }

            // Now check the url to see if it's a valid address
            if (!Uri.IsWellFormedUriString(storeUrlToCheck, UriKind.Absolute))
            {
                MessageHelper.ShowError(this, "The specified URL is not a valid address.");
                return string.Empty;
            }

            return storeUrlToCheck;
        }

        /// <summary>
        /// Checks the token for errors.
        /// </summary>
        private string CheckTokenForErrors(string token)
        {
            // To make a call to the store, we need a valid api user key, so check that next.
            string apiUserKeyToCheck = token.Trim();
            if (string.IsNullOrWhiteSpace(apiUserKeyToCheck))
            {
                MessageHelper.ShowError(this, "Please enter the API token for your 3dcart store.");
                return string.Empty;
            }

            // As per the api documentation, the api user key must be 32 characters
            if (apiUserKeyToCheck.Length != 32)
            {
                MessageHelper.ShowError(this, "The API token is not valid, it must be 32 characters in length.");
                return string.Empty;
            }
            return apiUserKeyToCheck;
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        protected bool ConnectionVerificationNeeded(ThreeDCartStoreEntity store)
        {
            return (store.Fields[(int)ThreeDCartStoreFieldIndex.StoreUrl].IsChanged ||
                    store.Fields[(int)ThreeDCartStoreFieldIndex.ApiUserKey].IsChanged);
        }

        /// <summary>
        /// Helper method that that does type checking and casts the generic StoreEntity
        /// to a ThreeDCartStoreEntity.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns>A ThreeDCartStoreEntity.</returns>
        /// <exception cref="ArgumentException" />
        private static ThreeDCartStoreEntity GetThreeDCartStore(StoreEntity store)
        {
            ThreeDCartStoreEntity threeDCartStore = store as ThreeDCartStoreEntity;
            if (threeDCartStore == null)
            {
                throw new ArgumentException("A non 3dcart store was passed to 3dcart account settings.");
            }

            return threeDCartStore;
        }

        /// <summary>
        /// Called when [click upgrade to rest].
        /// </summary>
        private void OnClickUpgradeToRest(object sender, EventArgs e)
        {
            string token = CheckTokenForErrors(textBoxUpgradeToken.Text);

            if (token == string.Empty)
            {
                return;
            }

            string message = "Before you upgrade..." + Environment.NewLine + Environment.NewLine +
                             "You will no longer be able to update orders in ShipWorks that were downloaded using 3dcart's legacy API." +
                             Environment.NewLine + Environment.NewLine +
                             "3dcart's new API does not support retrieving the names of customized order statuses. " +
                             "You will need to update any filters in ShipWorks that are checking for custom status names to " +
                             "check for the corresponding default 3dcart order status instead." +
                             Environment.NewLine + Environment.NewLine +
                             "Would you like to proceed with the upgrade?";

            DialogResult answer = MessageHelper.ShowQuestion(this,MessageBoxIcon.Warning, MessageBoxButtons.YesNo, message);

            if (answer == DialogResult.Yes)
            {
                threeDCartStoreEntity.RestUser = true;
                threeDCartStoreEntity.ApiUserKey = token;

                try
                {
                    ThreeDCartRestWebClient client = new ThreeDCartRestWebClient(threeDCartStoreEntity);
                    client.TestConnection();
                }
                catch (Exception ex)
                {
                    log.Error("ShipWorks encountered an error while attempting to contact 3dcart.", ex);
                    MessageHelper.ShowError(this, "Error connecting to 3dcart. Please check that the store URL and new API token are correct.");

                    return;
                }

                apiUserKey.Text = textBoxUpgradeToken.Text;
                SaveToEntity(threeDCartStoreEntity);

                panelUpgrade.Visible = false;
                StoreManager.CreateStoreStatusFilters(this, threeDCartStoreEntity);
            }
        }
    }
}
