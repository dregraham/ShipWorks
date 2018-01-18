using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// The Odbc Store Type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Odbc)]
    [Component(RegistrationType.Self)]
    public class OdbcStoreType : StoreType
    {
        private readonly IIndex<StoreTypeCode, IDownloadSettingsControl> downloadSettingsFactory;
        private readonly IIndex<StoreTypeCode, IStoreWizardFinishPageControlFactory> storeWizardFinishPageControlFactory;
        private readonly OdbcStoreEntity odbcStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcStoreType(StoreEntity store,
            IIndex<StoreTypeCode, IDownloadSettingsControl> downloadSettingsFactory,
            IIndex<StoreTypeCode, IStoreWizardFinishPageControlFactory> storeWizardFinishPageControlFactory)
            : base(store)
        {
            this.downloadSettingsFactory = downloadSettingsFactory;
            this.storeWizardFinishPageControlFactory = storeWizardFinishPageControlFactory;
            odbcStore = (OdbcStoreEntity) store;
        }

        /// <summary>
        /// Returns the OdbcStoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Odbc;

        /// <summary>
        /// Creates the license identifier for the OdbcStoreType
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                StringHash stringHash = new StringHash();

                return $"{stringHash.Hash(odbcStore.ImportConnectionString, "ODBC")} {SystemData.Fetch().DatabaseID.ToString("D")}";
            }
        }

        /// <summary>
        /// Create an order identifier
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            // Put this here for now so that we can work on the downloader
            return CreateOrderIdentifier(order.OrderNumberComplete);
        }

        /// <summary>
        /// Create an order identifier
        /// </summary>
        public OrderIdentifier CreateOrderIdentifier(string orderNumber)
        {
            return new AlphaNumericOrderIdentifier(orderNumber);
        }

        /// <summary>
        /// Returns an empty OdbcStoreType
        /// </summary>
        /// <returns></returns>
        public override StoreEntity CreateStoreInstance()
        {
            OdbcStoreEntity store = new OdbcStoreEntity
            {
                ImportConnectionString = string.Empty,
                ImportMap = string.Empty,
                ImportStrategy = (int) OdbcImportStrategy.ByModifiedTime,
                ImportColumnSourceType = (int) OdbcColumnSourceType.Table,
                ImportColumnSource = string.Empty,
                ImportOrderItemStrategy = (int) OdbcImportOrderItemStrategy.SingleLine,
                UploadMap = string.Empty,
                UploadStrategy = (int) OdbcShipmentUploadStrategy.DoNotUpload,
                UploadColumnSourceType = (int) OdbcColumnSourceType.Table,
                UploadColumnSource = string.Empty,
                UploadConnectionString = string.Empty,
            };

            InitializeStoreDefaults(store);

            return store;
        }

        /// <summary>
        /// Creates the add store wizard pages.
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            IEnumerable<IOdbcWizardPage> wizardPages = scope.Resolve<IEnumerable<IOdbcWizardPage>>();
            return wizardPages.OrderBy(w => w.Position).Cast<WizardPage>().ToList();
        }

        /// <summary>
        /// Support for online columns
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Sets the stores initial download policy
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return ((OdbcStoreEntity) Store).ImportStrategy == (int) OdbcImportStrategy.ByModifiedTime
                    ? new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack) { DefaultDaysBack = 30, MaxDaysBack = 30 }
                    : new InitialDownloadPolicy(InitialDownloadRestrictionType.None);
            }
        }

        /// <summary>
        /// Creates the add store wizard online update action control.
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return ((OdbcStoreEntity) Store).UploadStrategy == (int) OdbcShipmentUploadStrategy.DoNotUpload ? null :
                new OnlineUpdateShipmentUpdateActionControl(typeof(OdbcShipmentUploadTask));
        }

        /// <summary>
        /// Create the download settings control
        /// </summary>
        /// <returns></returns>
        public override IDownloadSettingsControl CreateDownloadSettingsControl()
        {
            if (odbcStore.ImportStrategy == (int) OdbcImportStrategy.OnDemand)
            {
                return downloadSettingsFactory[StoreTypeCode.Odbc];
            }

            return base.CreateDownloadSettingsControl();
        }

        /// <summary>
        /// Show the settings wizard page only if the store has a download strategy or upload strategy
        /// </summary>
        /// <returns></returns>
        public override bool ShowTaskWizardPage()
        {
            if (odbcStore == null)
            {
                return false;
            }

            return odbcStore.ImportStrategy == (int) OdbcImportStrategy.ByModifiedTime ||
                   odbcStore.UploadStrategy != (int) OdbcShipmentUploadStrategy.DoNotUpload;
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        public virtual string GetOnlineOrderIdentifier(OrderEntity order) => order.OrderNumberComplete;

        /// <summary>
        /// Returns messaging to display on the AddStoreWizard finish page
        /// </summary>
        public override Control CreateWizardFinishPageControl()
        {
            return storeWizardFinishPageControlFactory[StoreTypeCode.Odbc].Create(odbcStore);
        }

        /// <summary>
        /// Should this store type auto download
        /// </summary>
        public override bool ShouldAutoDownload()
        {
            if(odbcStore.ImportStrategy == (int) OdbcImportStrategy.OnDemand)
            {
                return false;
            }

            return true;
        }
    }
}
