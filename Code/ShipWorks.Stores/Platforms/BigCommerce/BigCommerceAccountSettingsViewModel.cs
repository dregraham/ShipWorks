using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// View model for the BigCommerce account settings control
    /// </summary>
    [Component(RegistrationType.Self)]
    public class BigCommerceAccountSettingsViewModel : INotifyPropertyChanged
    {
        readonly IBigCommerceWebClientFactory webClientFactory;
        readonly Func<BigCommerceStoreEntity, Owned<BigCommerceStatusCodeProvider>> createStatusCodeProvider;
        readonly ILog log;
        readonly IMessageHelper messageHelper;
        readonly PropertyChangedHandler handler;

        string apiUrl;
        string apiUsername;
        string apiToken;
        string clientID;
        string accessToken;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceAccountSettingsViewModel(IBigCommerceWebClientFactory webClientFactory,
            IMessageHelper messageHelper,
            Func<BigCommerceStoreEntity, Owned<BigCommerceStatusCodeProvider>> createStatusCodeProvider,
            Func<Type, ILog> createLogger)
        {
            this.messageHelper = messageHelper;
            this.createStatusCodeProvider = createStatusCodeProvider;
            this.webClientFactory = webClientFactory;
            log = createLogger(GetType());
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
        public string ApiUsername
        {
            get { return apiUsername; }
            set { handler.Set(nameof(ApiUsername), ref apiUsername, value); }
        }

        /// <summary>
        /// Token for legacy API access
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ApiToken
        {
            get { return apiToken; }
            set { handler.Set(nameof(ApiToken), ref apiToken, value); }
        }

        /// <summary>
        /// Client ID for OAuth access
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ClientID
        {
            get { return clientID; }
            set { handler.Set(nameof(ClientID), ref clientID, value); }
        }

        /// <summary>
        /// Access Token for OAuth access
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AccessToken
        {
            get { return accessToken; }
            set { handler.Set(nameof(AccessToken), ref accessToken, value); }
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        /// <param name="store"></param>
        public void LoadStore(StoreEntity store)
        {
            BigCommerceStoreEntity bigCommerceStore = GetBigCommerceStore(store);

            ApiUrl = bigCommerceStore.ApiUrl;
            ApiUsername = bigCommerceStore.ApiUserName;
            ApiToken = bigCommerceStore.ApiToken;
        }

        /// <summary>
        /// Save the data into the StoreEntity.  Nothing is saved to the database.
        /// </summary>
        /// <param name="store"></param>
        /// <returns>True if the entered settings can successfully connect to the store.</returns>
        public bool SaveToEntity(StoreEntity store)
        {
            // To make a call to the store, we need a valid api user name, so check that next.
            string apiUsernameToCheck = apiUsername.Trim();
            if (string.IsNullOrWhiteSpace(apiUsernameToCheck))
            {
                messageHelper.ShowError("Please enter the API Username for your BigCommerce store.");
                return false;
            }

            // Check the api token
            string apiTokenToCheck = apiToken.Trim();
            if (string.IsNullOrWhiteSpace(apiTokenToCheck))
            {
                messageHelper.ShowError("Please enter an API Token.");
                return false;
            }

            GenericResult<string> storeUrlToCheck = ValidateAndFormatApiUrl(ApiUrl);
            if (storeUrlToCheck.Failure)
            {
                messageHelper.ShowError(storeUrlToCheck.Message);
                return false;
            }

            BigCommerceStoreEntity bigCommerceStore = GetBigCommerceStore(store);
            bigCommerceStore.ApiUrl = storeUrlToCheck.Value;
            bigCommerceStore.ApiToken = apiTokenToCheck;
            bigCommerceStore.ApiUserName = apiUsernameToCheck;

            return ConnectionVerificationNeeded(bigCommerceStore) ?
                UpdateConnection(bigCommerceStore) :
                true;
        }

        private bool UpdateConnection(BigCommerceStoreEntity store)
        {
            using (messageHelper.SetCursor(Cursors.WaitCursor))
            {
                try
                {
                    IBigCommerceWebClient webClient = webClientFactory.Create(store);
                    webClient.TestConnection();

                    using (Owned<BigCommerceStatusCodeProvider> statusProvider = createStatusCodeProvider(store))
                    {
                        statusProvider.Value.UpdateFromOnlineStore();
                    }

                    return true;
                }
                catch (BigCommerceException ex)
                {
                    log.Error(ex.Message, ex);
                    messageHelper.ShowError(ex.Message);

                    return false;
                }
            }
        }

        /// <summary>
        /// Validate and format the API url
        /// </summary>
        private GenericResult<string> ValidateAndFormatApiUrl(string url)
        {
            string storeUrlToCheck = url.Trim();
            if (string.IsNullOrWhiteSpace(storeUrlToCheck))
            {
                return GenericResult.FromError("Please enter the API Path of your BigCommerce store.", string.Empty);
            }

            // Check for the url scheme, and add https if not present
            if (storeUrlToCheck.IndexOf(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase) == -1)
            {
                storeUrlToCheck = string.Format("https://{0}", storeUrlToCheck);
            }

            // Now check the url to see if it's a valid address
            if (!Uri.IsWellFormedUriString(storeUrlToCheck, UriKind.Absolute))
            {
                return GenericResult.FromError("The specified API Path is not a valid address.", string.Empty);
            }

            return GenericResult.FromSuccess(storeUrlToCheck);
        }

        /// <summary>
        /// For determining if the connection needs to be tested
        /// </summary>
        protected static bool ConnectionVerificationNeeded(BigCommerceStoreEntity store)
        {
            return (store.Fields[(int) BigCommerceStoreFieldIndex.ApiUrl].IsChanged ||
                    store.Fields[(int) BigCommerceStoreFieldIndex.ApiUserName].IsChanged ||
                    store.Fields[(int) BigCommerceStoreFieldIndex.ApiToken].IsChanged);
        }

        /// <summary>
        /// Helper method that does type checking and casts the generic StoreEntity
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
