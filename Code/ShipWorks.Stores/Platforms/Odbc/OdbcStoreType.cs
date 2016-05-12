using System;
using System.Collections.Generic;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.UI.Wizard;
using ShipWorks.Data;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// The Odbc Store Type
    /// </summary>
    public class OdbcStoreType : StoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Returns the OdbcStoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Odbc;

        /// <summary>
        /// Creates the license identifier for the OdbcStoreType
        /// </summary>
        protected override string InternalLicenseIdentifier => $"{Store.StoreID} {SystemData.Fetch().DatabaseID.ToString("D")}";

        /// <summary>
        /// Create a downloader for the OdbcStoreType
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create an order identifier
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an empty OdbcStoreType
        /// </summary>
        /// <returns></returns>
        public override StoreEntity CreateStoreInstance()
        {
            OdbcStoreEntity store = new OdbcStoreEntity
            {
                ConnectionString = string.Empty
            };

            InitializeStoreDefaults(store);

            return store;
        }

        /// <summary>
        /// Creates the add store wizard pages.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            return new List<WizardPage>
                {
                    IoC.UnsafeGlobalLifetimeScope.ResolveNamed<WizardPage>("OdbcDataSourcePage"),
                    IoC.UnsafeGlobalLifetimeScope.ResolveNamed<WizardPage>("OdbcImportFieldMappingPage")
                };
        }
    }
}
