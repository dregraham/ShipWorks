using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Interapptive.Shared.Security;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.BigCommerce;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// Update the LicenseUrl for BigCommerce so that we don't have to calculate it each time
    /// </summary>
    public class V_05_13_00_00 : IVersionSpecificUpdate
    {
        readonly ISqlAdapterFactory sqlAdapterFactory;
        readonly IBigCommerceIdentifier identifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public V_05_13_00_00(ISqlAdapterFactory sqlAdapterFactory, IBigCommerceIdentifier identifier)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.identifier = identifier;
        }

        /// <summary>
        /// Which version does this update apply to
        /// </summary>
        public Version AppliesTo => new Version(5, 13, 0, 0);

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
                EntityQuery<BigCommerceStoreEntity> query = new QueryFactory().BigCommerceStore
                    .Where(BigCommerceStoreFields.Identifier == string.Empty);

                IEntityCollection2 stores = sqlAdapter.FetchQueryAsync<BigCommerceStoreEntity>(query).Result;
                foreach (BigCommerceStoreEntity store in stores.OfType<BigCommerceStoreEntity>())
                {
                    identifier.Set(store, store.ApiUrl);
                }

                sqlAdapter.SaveEntityCollection(stores);
            }
        }
    }
}
