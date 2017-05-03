using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using Autofac.Features.OwnedInstances;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce.AccountSettings
{
    /// <summary>
    /// View model for the BigCommerce account settings control
    /// </summary>
    [Component(RegistrationType.Self)]
    public class BigCommerceAccountSettingsViewModel : IBigCommerceAccountSettingsViewModel, INotifyPropertyChanged
    {
        readonly ILog log;
        readonly IMessageHelper messageHelper;
        readonly IIndex<BigCommerceAuthenticationType, IBigCommerceAuthenticationPersistenceStrategy> persistenceStrategyFactory;
        readonly IBigCommerceConnectionVerifier connectionVerifier;
        readonly PropertyChangedHandler handler;

        BigCommerceAuthenticationType authenticationType;
        string apiUrl;
        string basicUsername;
        string basicToken;
        string oauthClientID;
        string oauthToken;
        private IBigCommerceAuthenticationPersistenceStrategy persistenceStrategy;
        readonly IBigCommerceIdentifier identifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceAccountSettingsViewModel(IMessageHelper messageHelper,
            IIndex<BigCommerceAuthenticationType, IBigCommerceAuthenticationPersistenceStrategy> persistenceStrategyFactory,
            IBigCommerceConnectionVerifier connectionVerifier,
            IBigCommerceIdentifier identifier,
            Func<Type, ILog> createLogger)
        {
            this.connectionVerifier = connectionVerifier;
            this.persistenceStrategyFactory = persistenceStrategyFactory;
            this.messageHelper = messageHelper;
            this.identifier = identifier;
            log = createLogger(GetType());
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            AuthenticationType = BigCommerceAuthenticationType.Oauth;

            MigrateToOauth = new RelayCommand(MigrateToOauthAction);
        }

        /// <summary>
        /// Has a property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Authentication type to use for BigCommerce requests
        /// </summary>
        [Obfuscation(Exclude = true)]
        public BigCommerceAuthenticationType AuthenticationType
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
        /// User name for legacy API access
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string BasicUsername
        {
            get { return basicUsername; }
            set { handler.Set(nameof(BasicUsername), ref basicUsername, value); }
        }

        /// <summary>
        /// Token for legacy API access
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string BasicToken
        {
            get { return basicToken; }
            set { handler.Set(nameof(BasicToken), ref basicToken, value); }
        }

        /// <summary>
        /// Client ID for OAuth access
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OauthClientID
        {
            get { return oauthClientID; }
            set { handler.Set(nameof(OauthClientID), ref oauthClientID, value); }
        }

        /// <summary>
        /// Access Token for OAuth access
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OauthToken
        {
            get { return oauthToken; }
            set { handler.Set(nameof(OauthToken), ref oauthToken, value); }
        }

        /// <summary>
        /// Get the current persistence strategy
        /// </summary>
        private IBigCommerceAuthenticationPersistenceStrategy PersistenceStrategy
        {
            get
            {
                return persistenceStrategy ?? (persistenceStrategy = persistenceStrategyFactory[AuthenticationType]);
            }
        }

        /// <summary>
        /// Migrate from Basic to OAuth
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand MigrateToOauth { get; }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        /// <param name="store"></param>
        public void LoadStore(IBigCommerceStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            ApiUrl = store.ApiUrl;
            AuthenticationType = store.BigCommerceAuthentication;

            PersistenceStrategy.LoadStoreIntoViewModel(store, this);
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        /// <param name="store"></param>
        /// <returns>True if the entered settings can successfully connect to the store.</returns>
        public bool SaveToEntity(BigCommerceStoreEntity store)
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
        private IResult PerformSave(BigCommerceStoreEntity store)
        {
            GenericResult<string> storeUrlToCheck = SaveApiUrlToStore(store, ApiUrl);
            if (storeUrlToCheck.Failure)
            {
                return storeUrlToCheck;
            }

            IResult persistenceResult = PersistenceStrategy.SaveDataToStoreFromViewModel(store, this);
            if (persistenceResult.Failure)
            {
                return persistenceResult;
            }

            using (messageHelper.SetCursor(Cursors.WaitCursor))
            {
                return connectionVerifier.Verify(store, PersistenceStrategy);
            }
        }

        /// <summary>
        /// Validate and format the API url
        /// </summary>
        private GenericResult<string> SaveApiUrlToStore(BigCommerceStoreEntity store, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return GenericResult.FromError<string>("Please enter the API Path of your BigCommerce store.");
            }

            // Trim and convert to v2 URL
            string storeUrlToCheck = url.Trim().Replace("/v3", "/v2");

            // Check for the url scheme, and add https if not present
            if (storeUrlToCheck.IndexOf(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase) == -1)
            {
                storeUrlToCheck = "https://" + storeUrlToCheck;
            }

            // Now check the url to see if it's a valid address
            if (!Uri.IsWellFormedUriString(storeUrlToCheck, UriKind.Absolute))
            {
                return GenericResult.FromError<string>("The specified API Path is not a valid address.");
            }

            store.ApiUrl = storeUrlToCheck;
            identifier.Set(store, storeUrlToCheck);

            return GenericResult.FromSuccess(storeUrlToCheck);
        }

        /// <summary>
        /// Migrate from basic to OAuth
        /// </summary>
        private void MigrateToOauthAction()
        {
            AuthenticationType = BigCommerceAuthenticationType.Oauth;
            persistenceStrategy = null;

            ApiUrl = TranslateApiUrl(ApiUrl);
        }

        /// <summary>
        /// Translate the Api URL from legacy to OAuth
        /// </summary>
        /// <remarks>
        /// Old style:  https://store-vplh1lw.mybigcommerce.com/api/v2/
        /// New style:  https://api.bigcommerce.com/stores/vplh1lw/v2/
        /// </remarks>
        private static string TranslateApiUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            // First find the BC store identifier
            int start = url.IndexOf("store-", StringComparison.InvariantCultureIgnoreCase) + 6;
            int end = url.IndexOf(".mybigcommerce.com", StringComparison.InvariantCultureIgnoreCase);

            if (start < 0 || end < 0 || (end - start) <= 0)
            {
                return string.Empty;
            }

            string storeIdentifier = url.Substring(start, end - start);

            // Now format it correctly
            return $"https://api.bigcommerce.com/stores/{storeIdentifier}/v2/";
        }
    }
}
