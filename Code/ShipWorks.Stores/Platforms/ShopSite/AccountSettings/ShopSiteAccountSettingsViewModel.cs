using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite.AccountSettings;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// View model for ShopSite account settings
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ShopSiteAccountSettingsViewModel : IShopSiteAccountSettingsViewModel, INotifyPropertyChanged
    {
        private readonly IMessageHelper messageHelper;
        private readonly PropertyChangedHandler handler;
        private readonly IShopSiteIdentifier identifier;
        private readonly IShopSiteConnectionVerifier connectionVerifier;
        private readonly IIndex<ShopSiteAuthenticationType, IShopSiteAuthenticationPersistenceStrategy> loadPersistenceStrategy;

        private string apiUrl;
        private string legacyMerchantID;
        private string legacyPassword;
        private string oAuthClientID;
        private string oAuthSecretKey;
        private string oAuthAuthorizationCode;
        
        private bool legacyUseUnsecureHttp;

        readonly IIndex<ShopSiteAuthenticationType, IShopSiteAuthenticationPersistenceStrategy> persistenceStrategyFactory;
        private IShopSiteAuthenticationPersistenceStrategy persistenceStrategy;
        private ShopSiteAuthenticationType authenticationType;


        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountSettingsViewModel(IMessageHelper messageHelper, IShopSiteIdentifier identifier, IShopSiteConnectionVerifier connectionVerifier, 
            IIndex<ShopSiteAuthenticationType, IShopSiteAuthenticationPersistenceStrategy> loadPersistenceStrategy)
        {
            this.messageHelper = messageHelper;
            this.identifier = identifier;
            this.connectionVerifier = connectionVerifier;
            this.loadPersistenceStrategy = loadPersistenceStrategy;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            AuthenticationType = ShopSiteAuthenticationType.Oauth;
            MigrateToOauth = new RelayCommand(MigrateToOauthAction);
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
        public string ApiUrl 
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
                return persistenceStrategy ?? (persistenceStrategy = loadPersistenceStrategy[AuthenticationType]);
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
        public void LoadStore(IShopSiteStoreEntity store)
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

            AuthenticationType = shopSiteStore.ShopSiteAuthentication; 
            LegacyUseUnsecureHttp = !shopSiteStore.RequireSSL;

            OAuthClientID = shopSiteStore.OauthClientID;
            OAuthSecretKey = SecureText.Decrypt(shopSiteStore.OauthSecretKey, shopSiteStore.OauthClientID);
            OAuthAuthorizationCode = shopSiteStore.OauthAuthorizationCode;
        }

        /// <summary>
        /// Save the UI values to the given store.  Nothing is saved to the database.
        /// </summary>
        public bool SaveToEntity(ShopSiteStoreEntity store)
        {
            IResult result = PerformSave(store);

            if (result.Failure)
            {
                messageHelper.ShowError(result.Message);
            }

            return result.Success;
        }

        /// <summary>
        /// Perform the actual save
        /// </summary>
        private IResult PerformSave(ShopSiteStoreEntity store)
        {
            GenericResult<string> storeUrlToCheck = SaveApiUrlToStore(store, ApiUrl);
            //ShopSiteStoreEntity shopSiteStore = store;
            if (storeUrlToCheck.Failure)
            {
                return storeUrlToCheck;
            }

            IResult persistenceResult = PersistenceStrategy.SaveDataToStoreFromViewModel(store, this);
            if (persistenceResult.Failure)
            {
                return persistenceResult;
            }

            store.RequireSSL = !LegacyUseUnsecureHttp;

            //Move to calling method
            if (string.IsNullOrEmpty(store.StoreName))
            {
                store.StoreName = "ShopSite Store";
            }

            // Move to calling method
            if (string.IsNullOrEmpty(store.Website))
            {
                store.Website = new Uri(ApiUrl).Host;
            }

            using (messageHelper.SetCursor(Cursors.WaitCursor))
            {
                return connectionVerifier.Verify(store, PersistenceStrategy);
            }
        }

        /// <summary>
        /// Validate and format the API url
        /// </summary>
        private GenericResult<string> SaveApiUrlToStore(ShopSiteStoreEntity store, string url)
        {
            ShopSiteStoreEntity shopSiteStore = store;
            if (shopSiteStore == null)
            {
                throw new ArgumentException("Please enter the API path of your ShopSite Store.");
            }

            //Trim URL
            string storeUrlToCheck = url?.Trim();

            if (AuthenticationType == ShopSiteAuthenticationType.Basic)
            {
                if (string.IsNullOrWhiteSpace(storeUrlToCheck))
                {
                    return GenericResult.FromError<string>("Please enter the path of your ShopSite store");
                }

                if (!storeUrlToCheck.EndsWith("start.cgi"))
                {
                    return GenericResult.FromError<string>(
                        "A valid URl to the CGI script should end with '/start.cgi'.");
                } 
            }

            else if (AuthenticationType == ShopSiteAuthenticationType.Oauth)
            {
                if (string.IsNullOrWhiteSpace(storeUrlToCheck))
                {
                    return GenericResult.FromError<string>("Please enter the OAuth URL");
                }

                if (!storeUrlToCheck.EndsWith("/authorize.cgi"))
                {
                    return GenericResult.FromError<string>("A valid URL to the CGI script should end with '/authorize.cgi'.");
                } 
            }

            // Check for the url scheme and add https if not present
            if (storeUrlToCheck.IndexOf(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase) == -1)
            {
                storeUrlToCheck = string.Format("https://{0}", storeUrlToCheck);
            }

            shopSiteStore.ApiUrl = storeUrlToCheck;
            identifier.Set(shopSiteStore, storeUrlToCheck);

            // Now check the url to see if it is a valid address
            if (!Uri.IsWellFormedUriString(storeUrlToCheck, UriKind.Absolute))
            {
                return GenericResult.FromError<string>("The specified URL is not a valid address");
            }

            store.ApiUrl = storeUrlToCheck;

            // Move to calling method
            if (string.IsNullOrWhiteSpace(store.Identifier))
            {
                store.Identifier = store.ApiUrl;
            }

            return GenericResult.FromSuccess(storeUrlToCheck);
        }

        /// <summary>
        /// Migrate from basic to OAuth
        /// </summary>
        private void MigrateToOauthAction()
        {
            AuthenticationType = ShopSiteAuthenticationType.Oauth;
            persistenceStrategy = null;
        }       
    }
}
