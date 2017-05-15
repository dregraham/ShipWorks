using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite.AccountSettings;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Strategy for persisting authentication data
    /// </summary>
    [KeyedComponent(typeof(IShopSiteAuthenticationPersistenceStrategy), ShopSiteAuthenticationType.Basic)]
    public class ShopSiteLegacyAuthenticationPersistenceStrategy : IShopSiteAuthenticationPersistenceStrategy
    {
        readonly IEncryptionProviderFactory encryptionFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteLegacyAuthenticationPersistenceStrategy(IEncryptionProviderFactory encryptionFactory)
        {
            this.encryptionFactory = encryptionFactory;
        }

        /// <summary>
        /// Test whether connection verification is needed
        /// </summary>
        public bool ConnectionVerificationNeeded(ShopSiteStoreEntity store)
        {
            return store.Fields[(int) ShopSiteStoreFieldIndex.Username].IsChanged ||
                   store.Fields[(int) ShopSiteStoreFieldIndex.Password].IsChanged;
        }

        /// <summary>
        /// Load data into view model
        /// </summary>
        public void LoadStoreIntoViewModel(IShopSiteStoreEntity store, IShopSiteAccountSettingsViewModel viewModel)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(viewModel, nameof(viewModel));

            viewModel.LegacyMerchantID = store.Username;
            viewModel.LegacyPassword = encryptionFactory.CreateSecureTextEncryptionProvider(store.Username).Decrypt(store.Password);
            viewModel.OAuthClientID = string.Empty;
            viewModel.OAuthSecretKey = string.Empty;
            viewModel.OAuthAuthorizationCode = string.Empty;
        }

        /// <summary>
        /// Save view model data into store
        /// </summary>
        public GenericResult<ShopSiteStoreEntity> SaveDataToStoreFromViewModel(ShopSiteStoreEntity store, IShopSiteAccountSettingsViewModel viewModel)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(viewModel, nameof(viewModel));

            // To make a call to the store, we need to valid the username
            if (string.IsNullOrWhiteSpace(viewModel.LegacyMerchantID))
            {
                return GenericResult.FromError<ShopSiteStoreEntity>("Please enter a username for your ShopSite Store.");
            }

            // To make a call to the store, we need to validate the password
            if (string.IsNullOrWhiteSpace(viewModel.LegacyPassword))
            {
                return GenericResult.FromError<ShopSiteStoreEntity>("Please enter a password for your ShopSite Store.");
            }

            store.Username = viewModel.LegacyMerchantID.Trim();
            store.Password = encryptionFactory.CreateSecureTextEncryptionProvider(store.Username).Encrypt(viewModel.LegacyPassword.Trim());

            store.OauthClientID = string.Empty;
            store.OauthSecretKey = string.Empty;
            store.OauthAuthorizationCode = string.Empty;

            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Basic;

            return GenericResult.FromSuccess(store);
        }

        /// <summary>
        /// Validate the api url
        /// </summary>
        public IResult ValidateApiUrl(string apiUrl)
        {
            if (apiUrl.EndsWith("start.cgi"))
            {
                return Result.FromSuccess();
            }

            return GenericResult.FromError<string>("A valid URl to the CGI script should end with '/start.cgi'.");
        }
    }
}
