﻿using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
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
        /// <summary>
        /// Test whether connection verification is needed
        /// </summary>
        public bool ConnectionVerificationNeeded(ShopSiteStoreEntity store)
        {
            return store.Fields[(int) ShopSiteStoreFieldIndex.OauthClientID].IsChanged ||
                store.Fields[(int)ShopSiteStoreFieldIndex.OauthSecretKey].IsChanged;
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
            viewModel.OAuthSecretKey = store.OauthSecretKey;
            viewModel.OAuthAuthorizationCode = store.AuthorizationCode; 
        }

        /// <summary>
        /// Save view model data into store
        /// </summary>
        public GenericResult<ShopSiteStoreEntity> SaveDataToStoreFromViewModel(ShopSiteStoreEntity store, IShopSiteAccountSettingsViewModel viewModel)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(viewModel, nameof(viewModel));

            // To make a call to the store, we need a valid api user name, so check that next.
            if (string.IsNullOrWhiteSpace(viewModel.OAuthClientID))
            {
                return GenericResult.FromError<ShopSiteStoreEntity>("Please enter the Client ID for your BigCommerce store.");
            }

            // Check the api token
            if (string.IsNullOrWhiteSpace(viewModel.OAuthSecretKey))
            {
                return GenericResult.FromError<ShopSiteStoreEntity>("Please enter an OAuth Token.");
            }

            store.Username = string.Empty;
            store.Password = string.Empty;
            store.OauthClientID = viewModel.OAuthClientID.Trim();
            store.OauthSecretKey = viewModel.OAuthSecretKey.Trim();
            store.Authentication = ShopSiteAuthenticationType.Oauth;
            store.AuthorizationCode = viewModel.OAuthAuthorizationCode;

            return GenericResult.FromSuccess(store);
        }
    }
}
