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

        GenericResult<ShopSiteStoreEntity> SaveDataToStoreFromViewModel(ShopSiteStoreEntity store,
            IShopSiteAccountSettingsViewModel viewModel);
    }
}
