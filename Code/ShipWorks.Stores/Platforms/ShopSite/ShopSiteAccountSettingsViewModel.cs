using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// View model for ShopSite account settings
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ShopSiteAccountSettingsViewModel : INotifyPropertyChanged
    {
        private readonly IMessageHelper messageHelper;
        private string legacyCgiUrl;
        private readonly PropertyChangedHandler handler;
        private string legacyMerchantID;
        private string legacyPassword;
        private bool legacyUseUnsecureHttp;
        private readonly IShopSiteIdentifier identifier;
        private ShopSiteAuthenticationType authenticationType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountSettingsViewModel(IMessageHelper messageHelper, IShopSiteIdentifier identifier)
        {
            this.messageHelper = messageHelper;
            this.identifier = identifier;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Authentication type to use for ShopSite requests
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShopSiteAuthenticationType AuthenticationType
        {
            get { return authenticationType; }
            private set { handler.Set(nameof(AuthenticationType), ref authenticationType, value); }
        }

        /// <summary>
        /// Url for the API
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string LegacyCgiUrl
        {
            get { return legacyCgiUrl; }
            set { handler.Set(nameof(LegacyCgiUrl), ref legacyCgiUrl, value); }
        }

        /// <summary>
        /// MerchantID for the legacy accounts
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string LegacyMerchantID
        {
            get { return legacyMerchantID; }
            set { handler.Set(nameof(LegacyMerchantID), ref legacyMerchantID, value); }
        }

        /// <summary>
        /// Password for the legacy accounts
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string LegacyPassword
        {
            get { return legacyPassword; }
            set { handler.Set(nameof(LegacyPassword), ref legacyPassword, value); }
        }

        /// <summary>
        /// Using unsecure connections for legacy accounts
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool LegacyUseUnsecureHttp
        {
            get { return legacyUseUnsecureHttp; }
            set { handler.Set(nameof(LegacyUseUnsecureHttp), ref legacyUseUnsecureHttp, value); }
        }

        /// <summary>
        /// Load the account settings UI from the given store
        /// </summary>
        public void LoadStore(StoreEntity store)
        {
            ShopSiteStoreEntity shopSiteStore = store as ShopSiteStoreEntity;
            if (shopSiteStore == null)
            {
                throw new ArgumentException("A non ShopSite store was passed to osc account settings.");
            }

            LegacyMerchantID = shopSiteStore.Username;
            LegacyPassword = SecureText.Decrypt(shopSiteStore.Password, shopSiteStore.Username);
            LegacyCgiUrl = shopSiteStore.ApiUrl;
            LegacyUseUnsecureHttp = !shopSiteStore.RequireSSL;
            AuthenticationType = shopSiteStore.Authentication;
        }

        /// <summary>
        /// Save the UI values to the given store.  Nothing is saved to the database.
        /// </summary>
        public bool SaveToEntity(StoreEntity store)
        {
            ShopSiteStoreEntity shopSiteStore = store as ShopSiteStoreEntity;
            if (shopSiteStore == null)
            {
                throw new ArgumentException("A non ShopSite store was passed to ShopSite account settings.");
            }

            // Url to the module
            string url = LegacyCgiUrl.Trim();

            // Check empty
            if (url.Length == 0)
            {
                messageHelper.ShowMessage("Enter the URL of the CGI script.");
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
                messageHelper.ShowError("The specified URL is not a valid web address.");
                return false;
            }

            // Has to point to the CGI
            if (!url.EndsWith("db_xml.cgi"))
            {
                messageHelper.ShowInformation("A valid URL to the CGI script should end with 'db_xml.cgi'.");
                return false;
            }

            shopSiteStore.Username = LegacyMerchantID;
            shopSiteStore.Password = SecureText.Encrypt(LegacyPassword, LegacyMerchantID);

            shopSiteStore.ApiUrl = url;
            identifier.Set(shopSiteStore, url);

            shopSiteStore.RequireSSL = !LegacyUseUnsecureHttp;

            if (shopSiteStore.Fields[(int)ShopSiteStoreFieldIndex.Username].IsChanged ||
                shopSiteStore.Fields[(int)ShopSiteStoreFieldIndex.Password].IsChanged ||
                shopSiteStore.Fields[(int)ShopSiteStoreFieldIndex.ApiUrl].IsChanged)
            {

                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    // Create the client for connecting to the module
                    using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                    {
                        IShopSiteWebClient webClient = lifetimeScope.ResolveKeyed<IShopSiteWebClient>(shopSiteStore.Authentication, TypedParameter.From(shopSiteStore as IShopSiteStoreEntity));

                        webClient.TestConnection();
                    }

                    store.StoreName = "ShopSite Store";
                    store.Website = new Uri(url).Host;

                    return true;
                }
                catch (ShopSiteException ex)
                {
                    messageHelper.ShowError(ex.Message);

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
