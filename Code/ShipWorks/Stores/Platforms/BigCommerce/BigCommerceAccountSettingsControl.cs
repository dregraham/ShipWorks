using System;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using log4net;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Control for editing BigCommerce account details
    /// </summary>
    public partial class BigCommerceAccountSettingsControl : AccountSettingsControlBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BigCommerceAccountSettingsControl));

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceAccountSettingsControl()
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

            BigCommerceStoreEntity bigCommerceStore = GetBigCommerceStore(store);

            apiUrl.Text = bigCommerceStore.ApiUrl;
            apiUsername.Text = bigCommerceStore.ApiUserName;
            apiToken.Text = bigCommerceStore.ApiToken;
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        /// <param name="store"></param>
        /// <returns>True if the entered settings can successfully connect to the store.</returns>
        [NDependIgnoreLongMethod]
        public override bool SaveToEntity(StoreEntity store)
        {
            // Check the api url for validity
            // First check for blank
            string storeUrlToCheck = apiUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(storeUrlToCheck))
            {
                MessageHelper.ShowError(this, "Please enter the API Path of your BigCommerce store.");
                return false;
            }

            // Check for the url scheme, and add https if not present
            if (storeUrlToCheck.IndexOf(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase) == -1)
            {
                storeUrlToCheck = string.Format("https://{0}", storeUrlToCheck);
            }

            // Now check the url to see if it's a valid address
            if (!Uri.IsWellFormedUriString(storeUrlToCheck, UriKind.Absolute))
            {
                MessageHelper.ShowError(this, "The specified API Path is not a valid address.");
                return false;
            }

            // To make a call to the store, we need a valid api user name, so check that next.
            string apiUsernameToCheck = apiUsername.Text.Trim();
            if (string.IsNullOrWhiteSpace(apiUsernameToCheck))
            {
                MessageHelper.ShowError(this, "Please enter the API Username for your BigCommerce store.");
                return false;
            }

            // Check the api token
            string apiTokenToCheck = apiToken.Text.Trim();
            if (string.IsNullOrWhiteSpace(apiTokenToCheck))
            {
                MessageHelper.ShowError(this, "Please enter an API Token.");
                return false;
            }

            BigCommerceStoreEntity bigCommerceStore = GetBigCommerceStore(store);
            bigCommerceStore.ApiUrl = storeUrlToCheck;
            bigCommerceStore.ApiToken = apiTokenToCheck;
            bigCommerceStore.ApiUserName = apiUsernameToCheck;

            // Now try to make a call to the store with the user's entered settings
            if (ConnectionVerificationNeeded(bigCommerceStore))
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    BigCommerceWebClient webClient = new BigCommerceWebClient(bigCommerceStore.ApiUserName, bigCommerceStore.ApiUrl, bigCommerceStore.ApiToken);
                    webClient.TestConnection();

                    BigCommerceStatusCodeProvider statusProvider = new BigCommerceStatusCodeProvider(bigCommerceStore);
                    statusProvider.UpdateFromOnlineStore();
                }
                catch (BigCommerceException ex)
                {
                    log.Error(ex.Message, ex);
                    MessageHelper.ShowError(this, ex.Message);

                    return false;
                }
            }

            // Everything succeeded, so return true
            return true;
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        protected static bool ConnectionVerificationNeeded(BigCommerceStoreEntity store)
        {
            return (store.Fields[(int)BigCommerceStoreFieldIndex.ApiUrl].IsChanged ||
                    store.Fields[(int)BigCommerceStoreFieldIndex.ApiUserName].IsChanged ||
                    store.Fields[(int)BigCommerceStoreFieldIndex.ApiToken].IsChanged);
        }

        /// <summary>
        /// Helper method that that does type checking and casts the generic StoreEntity
        /// to a BigCommerceStoreEntity.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns>A BigCommerceStoreEntity.</returns>
        /// <exception cref="ArgumentException" />
        private static BigCommerceStoreEntity GetBigCommerceStore(StoreEntity store)
        {
            BigCommerceStoreEntity bigCommerceStore = store as BigCommerceStoreEntity;
            if (bigCommerceStore == null)
            {
                throw new ArgumentException("A non BigCommerce store was passed to BigCommerce account settings.");
            }

            return bigCommerceStore;
        }
    }
}
