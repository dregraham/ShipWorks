using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite.AccountSettings;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Authentication persistence strategy
    /// </summary>
    public interface IShopSiteAuthenticationPersistenceStrategy
    {
        /// <summary>
        /// Test whether connection verification is needed
        /// </summary>
        bool ConnectionVerificationNeeded(ShopSiteStoreEntity store);

        /// <summary>
        /// Load store data into view model
        /// </summary>
        void LoadStoreIntoViewModel(IShopSiteStoreEntity store, IShopSiteAccountSettingsViewModel viewModel);

        /// <summary>
        /// Save the view model data into the given store
        /// </summary>
        GenericResult<ShopSiteStoreEntity> SaveDataToStoreFromViewModel(ShopSiteStoreEntity store,
            IShopSiteAccountSettingsViewModel viewModel);

        /// <summary>
        /// Validate the api url
        /// </summary>
        IResult ValidateApiUrl(string apiUrl);
    }
}
