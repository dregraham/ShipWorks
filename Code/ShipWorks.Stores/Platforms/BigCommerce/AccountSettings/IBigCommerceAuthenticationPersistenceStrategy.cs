using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce.AccountSettings
{
    /// <summary>
    /// Strategy for persisting authentication data
    /// </summary>
    public interface IBigCommerceAuthenticationPersistenceStrategy
    {
        /// <summary>
        /// Test whether connection verification is needed
        /// </summary>
        bool ConnectionVerificationNeeded(BigCommerceStoreEntity store);

        /// <summary>
        /// Load store data into view model
        /// </summary>
        void LoadStoreIntoViewModel(IBigCommerceStoreEntity store, IBigCommerceAccountSettingsViewModel viewModel);

        /// <summary>
        /// Save view model data into store
        /// </summary>
        GenericResult<BigCommerceStoreEntity> SaveDataToStoreFromViewModel(BigCommerceStoreEntity store, IBigCommerceAccountSettingsViewModel viewModel);
    }
}