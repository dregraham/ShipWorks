using Autofac;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
using ShipWorks.Stores.UI.Platforms.Odbc.Controls;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Upload;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Upload;
using System.Reflection;
using Module = Autofac.Module;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcStoreModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            RegisterOrderLoadingTypes(builder);
            RegisterFieldMapClasses(builder);
            RegisterFieldValueResolvers(builder);

            builder.RegisterType<OdbcStoreType>()
                .Keyed<StoreType>(StoreTypeCode.Odbc)
                .ExternallyOwned();

            builder.RegisterType<OdbcShipWorksDbProviderFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<OdbcDataSourceRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DsnProvider>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcControlPanel>()
                .AsImplementedInterfaces();

            builder.RegisterType<EncryptedOdbcDataSource>()
                .As<IOdbcDataSource>();

            builder.RegisterType<OdbcCipherKey>()
                .Keyed<ICipherKey>(CipherContext.Odbc);

            builder.RegisterType<OdbcImportMapSettingsControlViewModel>()
                .Keyed<IOdbcMapSettingsControlViewModel>("Import");

            builder.RegisterType<OdbcUploadMapSettingsControlViewModel>()
                .Keyed<IOdbcMapSettingsControlViewModel>("Upload");

            builder.RegisterType<OdbcImportMappingControlViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcUploadMappingControlViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcSchema>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcDownloadCommand>();

            builder.RegisterType<OdbcStoreDownloader>();

            builder.RegisterType<OdbcSampleDataCommand>()
                .AsImplementedInterfaces();

            builder.RegisterType<Controls.OdbcCustomQueryWarningDlg>()
                .Named<IDialog>("OdbcCustomQueryWarningDlg");

            builder.RegisterType<OdbcColumnSource>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcDataSourceService>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcDownloadCommandFactory>()
                .As<IOdbcDownloadCommandFactory>();

            builder.RegisterType<OdbcUploadCommandFactory>()
                .As<IOdbcUploadCommandFactory>();

            builder.RegisterType<OdbcUploader>()
                .As<IOdbcUploader>();

            builder.RegisterType<OdbcUploadMenuCommand>();

            builder.RegisterType<ApiLogEntry>()
                .As<IApiLogEntry>();
        }

        /// <summary>
        /// Registers the field value resolvers
        /// </summary>
        private static void RegisterFieldValueResolvers(ContainerBuilder builder)
        {
            builder.RegisterType<OdbcShippingServiceFieldValueResolver>()
                .Keyed<IOdbcFieldValueResolver>(OdbcFieldValueResolutionStrategy.ShippingService);

            builder.RegisterType<OdbcShippingCarrierFieldValueResolver>()
                .Keyed<IOdbcFieldValueResolver>(OdbcFieldValueResolutionStrategy.ShippingCarrier);

            builder.RegisterType<OdbcDefaultFieldValueResolver>()
                .Keyed<IOdbcFieldValueResolver>(OdbcFieldValueResolutionStrategy.Default);
        }

        /// <summary>
        /// Registers the field map classes.
        /// </summary>
        private static void RegisterFieldMapClasses(ContainerBuilder builder)
        {
            builder.RegisterType<OdbcFieldMapFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcFieldMap>()
                .As<IOdbcFieldMap>();

            builder.RegisterType<JsonOdbcFieldMapIOFactory>()
                .AsImplementedInterfaces();

            RegisterWizardPages(builder);

            builder.RegisterType<JsonOdbcFieldMapReader>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<JsonOdbcFieldMapWriter>()
                .AsImplementedInterfaces()
                .AsSelf();
        }

        /// <summary>
        /// Registers the wizard pages.
        /// </summary>
        private static void RegisterWizardPages(ContainerBuilder builder)
        {
            builder.RegisterType<OdbcImportDataSourcePage>()
                .As<IOdbcWizardPage>()
                .AsSelf();

            builder.RegisterType<OdbcImportMapSettingsPage>()
                .As<IOdbcWizardPage>()
                .AsSelf();

            builder.RegisterType<OdbcImportMappingPage>()
                .As<IOdbcWizardPage>()
                .AsSelf();

            builder.RegisterType<OdbcUploadShipmentStrategyPage>()
                .As<IOdbcWizardPage>()
                .AsSelf();

            builder.RegisterType<OdbcUploadDataSourcePage>()
                .As<IOdbcWizardPage>()
                .AsSelf();

            builder.RegisterType<OdbcUploadMapSettingsPage>()
                .As<IOdbcWizardPage>()
                .AsSelf();

            builder.RegisterType<OdbcUploadMappingPage>()
                .As<IOdbcWizardPage>()
                .AsSelf();

            builder.RegisterType<OdbcSetupFinishPage>()
                .AsSelf();

            builder.RegisterType<OdbcConnectionSettingsControl>()
                .Keyed<AccountSettingsControlBase>(StoreTypeCode.Odbc)
                .ExternallyOwned();
        }

        /// <summary>
        /// Registers the order loading types.
        /// </summary>
        private static void RegisterOrderLoadingTypes(ContainerBuilder builder)
        {
            builder.RegisterType<OdbcOrderLoader>()
                .As<IOdbcOrderLoader>();

            builder.RegisterType<OdbcOrderItemLoader>()
                .As<IOdbcOrderItemLoader>();

            builder.RegisterType<OdbcItemAttributeLoader>()
                .As<IOdbcItemAttributeLoader>();

            builder
                .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IOdbcOrderDetailLoader)))
                .Where(t => typeof(IOdbcOrderDetailLoader).IsAssignableFrom(t))
                .As<IOdbcOrderDetailLoader>();
        }
    }
}
