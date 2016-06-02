using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.UI.Wizard;
using ShipWorks.Data;
using ShipWorks.Stores.Platforms.GenericFile;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// The Odbc Store Type
    /// </summary>
    public class OdbcStoreType : StoreType
    {
        private readonly Func<StoreEntity, OdbcStoreDownloader> downloadFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcStoreType(StoreEntity store, Func<StoreEntity, OdbcStoreDownloader> downloadFactory)
            : base(store)
        {
            this.downloadFactory = downloadFactory;
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
            OdbcStoreEntity odbcStore = Store as OdbcStoreEntity;
            MethodConditions.EnsureArgumentIsNotNull(odbcStore);

            return downloadFactory(odbcStore);
        }

        /// <summary>
        /// Create an order identifier
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            // Put this here for now so that we can work on the downloader
            return new GenericFileOrderIdentifier(order.OrderNumber, order.OrderNumberComplete);
        }

        /// <summary>
        /// Returns an empty OdbcStoreType
        /// </summary>
        /// <returns></returns>
        public override StoreEntity CreateStoreInstance()
        {
            OdbcStoreEntity store = new OdbcStoreEntity
            {
                ConnectionString = string.Empty,
                Map = string.Empty
            };

            InitializeStoreDefaults(store);

            return store;
        }

        /// <summary>
        /// Creates the add store wizard pages.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IEnumerable<IOdbcWizardPage> wizardPages = scope.Resolve<IEnumerable<IOdbcWizardPage>>();

                return wizardPages.OrderBy(w => w.Position).Cast<WizardPage>().ToList();
            }
        }
    }
}
