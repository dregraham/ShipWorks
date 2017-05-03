using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Interapptive.Shared.Security;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// Update the LicenseUrl for BigCommerce so that we don't have to calculate it each time
    /// </summary>
    public class V_05_13_00_00 : IVersionSpecificUpdate
    {
        readonly ISqlAdapterFactory sqlAdapterFactory;
        readonly IEncryptionProviderFactory encryptionFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public V_05_13_00_00(ISqlAdapterFactory sqlAdapterFactory, IEncryptionProviderFactory encryptionFactory)
        {
            this.encryptionFactory = encryptionFactory;
            this.sqlAdapterFactory = sqlAdapterFactory;
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
            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                EntityQuery<BigCommerceStoreEntity> query = new QueryFactory().BigCommerceStore
                    .Where(BigCommerceStoreFields.ApiUrl == BigCommerceStoreFields.Identifier);

                IEntityCollection2 stores = sqlAdapter.FetchQueryAsync<BigCommerceStoreEntity>(query).Result;
                foreach (BigCommerceStoreEntity store in stores.OfType<BigCommerceStoreEntity>())
                {
                    store.Identifier = CleanseIdentifier(store.Identifier);
                }

                sqlAdapter.SaveEntityCollection(stores);
            }
        }

        /// <summary>
        /// Translate the license URL
        /// </summary>
        private string CleanseIdentifier(string originalIdentifier)
        {
            string identifier = originalIdentifier;

            int indexOfApiPath = identifier.IndexOf("api/v2/", 0, StringComparison.OrdinalIgnoreCase);
            if (indexOfApiPath > 0)
            {
                identifier = identifier.Substring(0, indexOfApiPath);
            }

            identifier = Regex.Replace(identifier, @"(admin/)?[^/]*(\.)?[^/]+$", "", RegexOptions.IgnoreCase);

            // The regex above will return just the scheme if there's no ending /, so check for that and
            // reset to the StoreUrl
            if (identifier.EndsWith(Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase))
            {
                identifier = originalIdentifier;
            }

            return identifier;
        }
    }
}
