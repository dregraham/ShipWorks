using System;
using System.Windows.Forms;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ShopSite;

namespace ShipWorks.Stores.UI.Platforms.ShopSite
{
    /// <summary>
    /// Account settings for ShopSite stores
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.ShopSite, ExternallyOwned = true)]
    public partial class ShopSiteAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the account settings UI from the given store
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            ShopSiteStoreEntity shopSiteStore = store as ShopSiteStoreEntity;
            if (shopSiteStore == null)
            {
                throw new ArgumentException("A non ShopSite store was passed to osc account settings.");
            }

            username.Text = shopSiteStore.Username;
            password.Text = SecureText.Decrypt(shopSiteStore.Password, shopSiteStore.Username);
            apiUrl.Text = shopSiteStore.ApiUrl;
            connectUnsecure.Checked = !shopSiteStore.RequireSSL;
        }

        /// <summary>
        /// Save the UI values to the given store.  Nothing is saved to the database.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            ShopSiteStoreEntity shopSiteStore = store as ShopSiteStoreEntity;
            if (shopSiteStore == null)
            {
                throw new ArgumentException("A non ShopSite store was passed to ShopSite account settings.");
            }

            // Url to the module
            string url = apiUrl.Text.Trim();

            // Check empty
            if (url.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter the URL of the CGI script.");
                return false;
            }

            // Default to https if not specified
            if (url.IndexOf(Uri.SchemeDelimiter) == -1)
            {
                url = "https://" + url;
            }

            // Check valid
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                MessageHelper.ShowError(this, "The specified URL is not a valid web address.");
                return false;
            }

            // Has to point to the CGI
            if (!url.EndsWith("db_xml.cgi"))
            {
                MessageHelper.ShowInformation(this, "A valid URL to the CGI script should end with 'db_xml.cgi'.");
                return false;
            }

            shopSiteStore.Username = username.Text;
            shopSiteStore.Password = SecureText.Encrypt(password.Text, username.Text);
            shopSiteStore.ApiUrl = url;
            shopSiteStore.RequireSSL = !connectUnsecure.Checked;

            if (shopSiteStore.Fields[(int) ShopSiteStoreFieldIndex.Username].IsChanged ||
                shopSiteStore.Fields[(int) ShopSiteStoreFieldIndex.Password].IsChanged ||
                shopSiteStore.Fields[(int) ShopSiteStoreFieldIndex.ApiUrl].IsChanged)
            {

                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    // Create the client for connecting to the module
                    ShopSiteWebClient webClient = new ShopSiteWebClient(shopSiteStore);
                    webClient.TestConnection();

                    store.StoreName = "ShopSite Store";
                    store.Website = new Uri(url).Host;

                    return true;
                }
                catch (ShopSiteException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);

                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
