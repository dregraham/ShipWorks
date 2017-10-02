using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.OnlineUpdating
{
    /// <summary>
    /// Factory for creating the correct Magento online updater
    /// </summary>
    [Component]
    public class MagentoOnlineUpdaterFactory : IMagentoOnlineUpdaterFactory
    {
        private readonly Func<MagentoStoreEntity, MagentoTwoRestOnlineUpdater> createRestUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoOnlineUpdaterFactory(Func<MagentoStoreEntity, MagentoTwoRestOnlineUpdater> createRestUpdater)
        {
            this.createRestUpdater = createRestUpdater;
        }

        /// <summary>
        /// Create the correct updater
        /// </summary>
        public IMagentoOnlineUpdater Create(MagentoStoreEntity store)
        {
            return store.MagentoVersion == (int) MagentoVersion.MagentoTwoREST ?
                (IMagentoOnlineUpdater) createRestUpdater(store) :
                new MagentoOnlineUpdater(store);
        }
    }
}
