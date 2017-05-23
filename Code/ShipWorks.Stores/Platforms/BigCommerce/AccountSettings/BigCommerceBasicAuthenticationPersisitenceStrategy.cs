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
    [KeyedComponent(typeof(IBigCommerceAuthenticationPersistenceStrategy), BigCommerceAuthenticationType.Basic)]
    public class BigCommerceBasicAuthenticationPersisitenceStrategy : IBigCommerceAuthenticationPersistenceStrategy
    {
        /// <summary>
        /// Test whether connection verification is needed
        /// </summary>
        public bool ConnectionVerificationNeeded(BigCommerceStoreEntity store)
        {
            return store.Fields[(int) BigCommerceStoreFieldIndex.ApiUserName].IsChanged ||
                store.Fields[(int) BigCommerceStoreFieldIndex.ApiToken].IsChanged;
        }

        /// <summary>
        /// Load store data into view model
        /// </summary>
        public void LoadStoreIntoViewModel(IBigCommerceStoreEntity store, IBigCommerceAccountSettingsViewModel viewModel)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(viewModel, nameof(viewModel));

            viewModel.BasicUsername = store.ApiUserName;
            viewModel.BasicToken = store.ApiToken;
            viewModel.OauthClientID = string.Empty;
            viewModel.OauthToken = string.Empty;
        }

        /// <summary>
        /// Save view model data into store
        /// </summary>
        public GenericResult<BigCommerceStoreEntity> SaveDataToStoreFromViewModel(BigCommerceStoreEntity store, IBigCommerceAccountSettingsViewModel viewModel)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(viewModel, nameof(viewModel));

            // To make a call to the store, we need a valid api user name, so check that next.
            if (string.IsNullOrWhiteSpace(viewModel.BasicUsername))
            {
                return GenericResult.FromError<BigCommerceStoreEntity>("Please enter the API Username for your BigCommerce store.");
            }

            // Check the api token
            if (string.IsNullOrWhiteSpace(viewModel.BasicToken))
            {
                return GenericResult.FromError<BigCommerceStoreEntity>("Please enter an API Token.");
            }

            store.ApiToken = viewModel.BasicToken.Trim();
            store.ApiUserName = viewModel.BasicUsername.Trim();
            store.OauthClientId = string.Empty;
            store.OauthToken = string.Empty;
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Basic;

            return GenericResult.FromSuccess(store);
        }
    }
}
