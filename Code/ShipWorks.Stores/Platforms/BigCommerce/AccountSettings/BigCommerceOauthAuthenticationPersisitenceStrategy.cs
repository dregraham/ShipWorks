using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce.AccountSettings
{
    /// <summary>
    /// Strategy for persisting authentication data
    /// </summary>
    [KeyedComponent(typeof(IBigCommerceAuthenticationPersistenceStrategy), BigCommerceAuthenticationType.Oauth)]
    public class BigCommerceOauthAuthenticationPersisitenceStrategy : IBigCommerceAuthenticationPersistenceStrategy
    {
        /// <summary>
        /// Test whether connection verification is needed
        /// </summary>
        public bool ConnectionVerificationNeeded(BigCommerceStoreEntity store)
        {
            return store.Fields[(int) BigCommerceStoreFieldIndex.OauthClientId].IsChanged ||
                store.Fields[(int) BigCommerceStoreFieldIndex.OauthToken].IsChanged;
        }

        /// <summary>
        /// Load store data into view model
        /// </summary>
        public void LoadStoreIntoViewModel(IBigCommerceStoreEntity store, IBigCommerceAccountSettingsViewModel viewModel)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(viewModel, nameof(viewModel));

            viewModel.BasicUsername = string.Empty;
            viewModel.BasicToken = string.Empty;
            viewModel.OauthClientID = store.OauthClientId;
            viewModel.OauthToken = store.OauthToken;
        }

        /// <summary>
        /// Save view model data into store
        /// </summary>
        public GenericResult<BigCommerceStoreEntity> SaveDataToStoreFromViewModel(BigCommerceStoreEntity store, IBigCommerceAccountSettingsViewModel viewModel)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(viewModel, nameof(viewModel));

            // To make a call to the store, we need a valid api user name, so check that next.
            if (string.IsNullOrWhiteSpace(viewModel.OauthClientID))
            {
                return GenericResult.FromError<BigCommerceStoreEntity>("Please enter the Client ID for your BigCommerce store.");
            }

            // Check the api token
            if (string.IsNullOrWhiteSpace(viewModel.OauthToken))
            {
                return GenericResult.FromError<BigCommerceStoreEntity>("Please enter an OAuth Token.");
            }

            store.ApiUserName = string.Empty;
            store.ApiToken = string.Empty;
            store.OauthClientId = viewModel.OauthClientID.Trim();
            store.OauthToken = viewModel.OauthToken.Trim();
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;

            return GenericResult.FromSuccess(store);
        }
    }
}
