using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.ShopSite.AccountSettings
{
    /// <summary>
    /// Strategy for persisting authentication data
    /// </summary>
    [KeyedComponent(typeof(IShopSiteAuthenticationPersistenceStrategy), ShopSiteAuthenticationType.Oauth)]
    public class ShopSiteOauthAuthenticationPersisitenceStrategy : IShopSiteAuthenticationPersistenceStrategy
    {
        private readonly IDictionary<Func<IShopSiteAccountSettingsViewModel, string>, string> viewModelValidations =
            new Dictionary<Func<IShopSiteAccountSettingsViewModel, string>, string>
            {
                { x => x.OAuthClientID, "Please enter the Client ID for your ShopSite store." },
                { x => x.OAuthSecretKey, "Please enter an OAuth Token."},
                { x => x.OAuthAuthorizationCode, "Please enter an Authorization Code."}
            };
        private readonly IDatabaseSpecificEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteOauthAuthenticationPersisitenceStrategy(IDatabaseSpecificEncryptionProvider encryptionProvider)
        {
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Test whether connection verification is needed
        /// </summary>
        public bool ConnectionVerificationNeeded(ShopSiteStoreEntity store)
        {
            return store.Fields[(int) ShopSiteStoreFieldIndex.OauthClientID].IsChanged ||
                store.Fields[(int) ShopSiteStoreFieldIndex.OauthSecretKey].IsChanged;
        }

        /// <summary>
        /// Load store data into view model
        /// </summary>
        public void LoadStoreIntoViewModel(IShopSiteStoreEntity store, IShopSiteAccountSettingsViewModel viewModel)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(viewModel, nameof(viewModel));

            viewModel.LegacyMerchantID = string.Empty;
            viewModel.LegacyPassword = string.Empty;

            viewModel.OAuthClientID = store.OauthClientID;
            viewModel.OAuthSecretKey = encryptionProvider.Decrypt(store.OauthSecretKey);
            viewModel.OAuthAuthorizationCode = store.OauthAuthorizationCode;
        }

        /// <summary>
        /// Save view model data into store
        /// </summary>
        public GenericResult<ShopSiteStoreEntity> SaveDataToStoreFromViewModel(ShopSiteStoreEntity store, IShopSiteAccountSettingsViewModel viewModel)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(viewModel, nameof(viewModel));

            foreach (KeyValuePair<Func<IShopSiteAccountSettingsViewModel, string>, string> validation in viewModelValidations)
            {
                if (string.IsNullOrWhiteSpace(validation.Key(viewModel)))
                {
                    return GenericResult.FromError<ShopSiteStoreEntity>(validation.Value);
                }
            }

            store.Username = string.Empty;
            store.Password = string.Empty;

            store.OauthClientID = viewModel.OAuthClientID.Trim();
            store.OauthSecretKey = encryptionProvider.Encrypt(viewModel.OAuthSecretKey.Trim());
            store.OauthAuthorizationCode = viewModel.OAuthAuthorizationCode.Trim();

            store.ShopSiteAuthentication = ShopSiteAuthenticationType.Oauth;

            return GenericResult.FromSuccess(store);
        }

        /// <summary>
        /// Validate the api url
        /// </summary>
        public IResult ValidateApiUrl(string apiUrl)
        {
            if (apiUrl.EndsWith("/authorize.cgi"))
            {
                return Result.FromSuccess();
            }

            return Result.FromError("A valid URL to the CGI script should end with '/authorize.cgi'.");
        }
    }
}
