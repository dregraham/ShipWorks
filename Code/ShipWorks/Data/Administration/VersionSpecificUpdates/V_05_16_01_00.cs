using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates 
{
    /// <summary>
    /// Updates ODBC store maps - Points order number to order number complete
    /// </summary>
    /// <seealso cref="ShipWorks.Data.Administration.VersionSpecificUpdates.IVersionSpecificUpdate" />
    public class V_05_16_01_00 : IVersionSpecificUpdate
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IStoreUpgrade storeUpgrade;

        /// <summary>
        /// Constructor
        /// </summary>
        public V_05_16_01_00(ISqlAdapterFactory sqlAdapterFactory, IStoreUpgrade storeUpgrade)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.storeUpgrade = storeUpgrade;
        }

        /// <summary>
        /// To which version does this update apply
        /// </summary>
        public Version AppliesTo => new Version(5, 16, 1, 0);

        /// <summary>
        /// Always run just in case it has never been run before.
        /// </summary>
        public bool AlwaysRun => true;

        /// <summary>
        /// Updates ODBC store maps - Points order number to order number complete
        /// </summary>
        public void Update()
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                EntityQuery<OdbcStoreEntity> query = new QueryFactory().OdbcStore;

                IEntityCollection2 stores = sqlAdapter.FetchQueryAsync(query).Result;
                foreach (OdbcStoreEntity store in stores.OfType<OdbcStoreEntity>())
                {
                    storeUpgrade.Upgrade(store);
                }
                sqlAdapter.SaveEntityCollection(stores, refetchSavedEntitiesAfterSave: true, recurse: false);
            }
        }
    }
}
