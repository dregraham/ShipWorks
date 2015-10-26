using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using log4net;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    public partial class ThreeDCartAccountSettingsControl : AccountSettingsControlBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartAccountSettingsControl));

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
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        /// <param name="store"></param>
        /// <returns>True if the entered settings can successfully connect to the store.</returns>
        public override bool SaveToEntity(StoreEntity store)
        {
            // Check the store url for validity
            // First check for blank
            string storeUrlToCheck = storeUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(storeUrlToCheck))
            {
                MessageHelper.ShowError(this, "Please enter the URL of your 3D Cart store.");
                return false;
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
                return false;
            }

            // To make a call to the store, we need a valid api user key, so check that next.
            string apiUserKeyToCheck = apiUserKey.Text.Trim();
            if (string.IsNullOrWhiteSpace(apiUserKeyToCheck))
            {
                MessageHelper.ShowError(this, "Please enter the Api User Key for your 3D Cart store.");
                return false;
            }

            // As per the api documentation, the api user key must be 32 characters
            if (apiUserKeyToCheck.Length != 32)
            {
                MessageHelper.ShowError(this, "The specified Api User Key is not valid, it must be 32 characters in length.");
                return false;
            }

            ThreeDCartStoreEntity threeDCartStore = GetThreeDCartStore(store);
            threeDCartStore.StoreUrl = storeUrlToCheck;
            threeDCartStore.ApiUserKey = apiUserKeyToCheck;

            // Now try to make a call to the store with the user's entered settings
            if (ConnectionVerificationNeeded(threeDCartStore))
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    ThreeDCartWebClient webClient = new ThreeDCartWebClient(threeDCartStore, null);
                    webClient.TestConnection();

                    ThreeDCartStatusCodeProvider statusProvider = new ThreeDCartStatusCodeProvider(threeDCartStore);
                    statusProvider.UpdateFromOnlineStore();
                }
                catch (ThreeDCartException ex)
                {
                    log.Error("ShipWorks encountered an error while attempting to contact 3D Cart.", ex);
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
                throw new ArgumentException("A non ThreeDCart store was passed to 3D Cart account settings.");
            }

            return threeDCartStore;
        }
    }
}
