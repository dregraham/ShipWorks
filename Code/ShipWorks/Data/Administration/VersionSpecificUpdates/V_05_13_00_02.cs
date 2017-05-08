using System;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.ShopSite;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// Update the License URL (Identifier) for ShopSite so that we don't have to calculate it each time
    /// </summary>
    public class V_05_13_00_02 : IVersionSpecificUpdate
    {
        readonly ISqlAdapterFactory sqlAdapterFactory;
        readonly IShopSiteIdentifier identifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public V_05_13_00_02(ISqlAdapterFactory sqlAdapterFactory, IShopSiteIdentifier identifier)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.identifier = identifier;
        }

        /// <summary>
        /// Which version does this update apply to
        /// </summary>
        public Version AppliesTo => new Version(5, 13, 0, 1);

        /// <summary>
        /// Should the update always be run
        /// </summary>
        public bool AlwaysRun => false;

        /// <summary>
        /// Perform the actual update
        /// </summary>
        public void Update()
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                EntityQuery<ShopSiteStoreEntity> query = new QueryFactory().ShopSiteStore
                     .Where(ShopSiteStoreFields.Identifier == string.Empty);

                IEntityCollection2 stores = sqlAdapter.FetchQueryAsync<ShopSiteStoreEntity>(query).Result;
                foreach (ShopSiteStoreEntity store in stores.OfType<ShopSiteStoreEntity>())
                {
                    identifier.Set(store, store.ApiUrl);
                }

                sqlAdapter.SaveEntityCollection(stores, refetchSavedEntitiesAfterSave: true, recurse: false);
            }
        }
    }
}
