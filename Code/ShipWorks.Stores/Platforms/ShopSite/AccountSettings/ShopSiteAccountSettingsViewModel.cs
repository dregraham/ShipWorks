using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Logging;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
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
        private readonly PropertyChangedHandler handler;

        private string apiUrl;
        private string legacyMerchantID;
        private string legacyPassword;
        private string oAuthClientID;
        private string oAuthSecretKey;
        private string oAuthAuthorizationCode;
        private IShopSiteAuthenticationPersistenceStrategy persistenceStrategy;
        private bool legacyUseUnsecureHttp;
        private ShopSiteAuthenticationType authenticationType;


        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountSettingsViewModel(IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            AuthenticationType = ShopSiteAuthenticationType.Oauth;
            MigrateToOauth = new RelayCommand(MigrateToOauthAction);
        }

        /// <summary>
        /// The property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Authentication type to use for BigCommerce requests
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
        public string ApiUrl //rename to ApiUrl
        {
            get { return apiUrl; }
            set { handler.Set(nameof(ApiUrl), ref apiUrl, value); }
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
        /// Username for OAuth accounts
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OAuthClientID
        {
            get { return oAuthClientID; }
            set { handler.Set(nameof(OAuthClientID), ref oAuthClientID, value); }
        }

        /// <summary>
        /// Password for the OAuth accounts
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OAuthSecretKey
        {
            get { return oAuthSecretKey; }
            set { handler.Set(nameof(OAuthSecretKey), ref oAuthSecretKey, value); }
        }

        /// <summary>
        /// Authorization code for the OAuth accounts
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OAuthAuthorizationCode
        {
            get { return oAuthAuthorizationCode; }
            set { handler.Set(nameof(OAuthAuthorizationCode), ref oAuthAuthorizationCode, value); }
        }

        /// <summary>
        /// Get the current persistence strategy
        /// </summary>
        private IShopSiteAuthenticationPersistenceStrategy PersistenceStrategy
        {
            get
            {
                return null; //persistenceStrategy ?? ()
            }
        }

        /// <summary>
        /// Migrate from Legacy to OAuth
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand MigrateToOauth { get; }

        /// <summary>
        /// Load the account settings UI from the given store
        /// </summary>
        public void LoadStore(StoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            ShopSiteStoreEntity shopSiteStore = store as ShopSiteStoreEntity;
            if (shopSiteStore == null)
            {
                throw new ArgumentException("A non ShopSite store was passed to osc account settings.");
            }

            LegacyMerchantID = shopSiteStore.Username;
            LegacyPassword = SecureText.Decrypt(shopSiteStore.Password, shopSiteStore.Username);
            ApiUrl = shopSiteStore.ApiUrl;

            AuthenticationType = shopSiteStore.Authentication; 

            LegacyUseUnsecureHttp = !shopSiteStore.RequireSSL;

            OAuthClientID = shopSiteStore.OauthClientID;
            OAuthSecretKey = SecureText.Decrypt(shopSiteStore.OauthSecretKey, shopSiteStore.OauthClientID);
            OAuthAuthorizationCode = shopSiteStore.AuthorizationCode;


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
            string url = ApiUrl?.Trim();

            // Check empty
            if (url.IsNullOrWhiteSpace() && AuthenticationType == ShopSiteAuthenticationType.Basic)
            {
                messageHelper.ShowMessage("Enter the URL of the CGI script.");
                return false;
            }

            else if (url.IsNullOrWhiteSpace() && AuthenticationType == ShopSiteAuthenticationType.Oauth)
            {
                messageHelper.ShowMessage("Please enter an OAuth URL");
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
           /* if (!url.EndsWith("db_xml.cgi") && AuthenticationType == ShopSiteAuthenticationType.Basic)
            {
                messageHelper.ShowInformation("A valid URL to the CGI script should end with 'db_xml.cgi'.");
                return false;
            }

            else if (!url.EndsWith(".cgi") && AuthenticationType == ShopSiteAuthenticationType.Oauth)
            {
                messageHelper.ShowInformation("A valid URL to the CGI script should end with '.cgi'.");
                return false;
            }*/



            // Has to point to the CGI
            if (AuthenticationType == ShopSiteAuthenticationType.Basic)
            {
                if (!url.EndsWith("db_xml.cgi"))
                {
                    messageHelper.ShowInformation("A valid URL to the CGI script should end with 'db_xml.cgi'.");
                    return false;
                }

                shopSiteStore.Username = LegacyMerchantID;
                shopSiteStore.Password = SecureText.Encrypt(LegacyPassword, LegacyMerchantID);
            }

            else if (AuthenticationType == ShopSiteAuthenticationType.Oauth)
            {
                if (!url.EndsWith(".cgi"))
                {
                    messageHelper.ShowInformation("A valid URL to the CGI script should end with '.cgi'.");
                    return false;
                }

                shopSiteStore.OauthClientID = OAuthClientID;
                shopSiteStore.OauthSecretKey = SecureText.Encrypt(OAuthSecretKey, OAuthClientID);
                shopSiteStore.AuthorizationCode = OAuthAuthorizationCode;
            }

            shopSiteStore.ApiUrl = url;
            shopSiteStore.RequireSSL = !LegacyUseUnsecureHttp;

            if (shopSiteStore.Fields[(int)ShopSiteStoreFieldIndex.Username].IsChanged ||
                shopSiteStore.Fields[(int)ShopSiteStoreFieldIndex.Password].IsChanged ||
                shopSiteStore.Fields[(int)ShopSiteStoreFieldIndex.ApiUrl].IsChanged)
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
                    messageHelper.ShowError(ex.Message);

                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Validate and format the API url
        /// </summary>
        private GenericResult<string> SaveApiUrlToStore(ShopSiteStoreEntity store, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return GenericResult.FromError<string>("Please enter the API path of your ShopSite store");
            }

            //Trim and convert to v2 url
            string storeUrlToCheck = url.Trim().Replace("/v3", "/v2");

            // Check for the url scheme and add https if not present
            if (storeUrlToCheck.IndexOf(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase) == -1)
            {
                storeUrlToCheck = string.Format("https://{0}", storeUrlToCheck);
            }

            // Now check the url to see if it is a valid address
            if (!Uri.IsWellFormedUriString(storeUrlToCheck, UriKind.Absolute))
            {
                return GenericResult.FromError<string>("The specified API path is not a valid address");
            }

            store.ApiUrl = storeUrlToCheck;

            if (string.IsNullOrWhiteSpace(store.Identifier))
            {
                store.Identifier = store.ApiUrl;
            }

            return GenericResult.FromSuccess(storeUrlToCheck);
        }

        private void MigrateToOauthAction()
        {
            AuthenticationType = ShopSiteAuthenticationType.Oauth;
            persistenceStrategy = null;

            ApiUrl = TranslateApiUrl(ApiUrl);
        }

        /// <summary>
        /// Translate the Api URl from legacy to OAuth
        /// </summary>
        /// <remarks>
        /// Old style:
        /// New style:
        /// </remarks>
        private static string TranslateApiUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            // First find the store identifier
            int start = url.IndexOf("", StringComparison.InvariantCultureIgnoreCase) + 6;
            int end = url.IndexOf("", StringComparison.InvariantCultureIgnoreCase);

            if (start < 0 || end < 0 || (end - start) <= 0)
            {
                return string.Empty;
            }

            string storeIdentifier = url.Substring(start, end - start);

            // Now format it correctly    
            return $"http://";
           
        }
    }
}
