using Autofac;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;
using System.Reflection;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using Module = Autofac.Module;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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

            builder.RegisterType<OdbcColumnSource>()
                .AsImplementedInterfaces();

            builder.RegisterType<WizardPages.OdbcImportFieldMappingControl>()
                .AsSelf();

            builder.RegisterType<OdbcMapSettingsControlViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcImportFieldMappingControlViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcFieldMapFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcFieldMap>()
                .As<IOdbcFieldMap>();

            builder.RegisterType<JsonOdbcFieldMapIOFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcDataSourcePage>()
                .As<IOdbcWizardPage>()
                .ExternallyOwned();

            builder.RegisterType<OdbcImportMapSettingsPage>()
                .As<IOdbcWizardPage>()
                .ExternallyOwned();

            builder.RegisterType<OdbcImportFieldMappingPage>()
                .As<IOdbcWizardPage>()
                .ExternallyOwned();

            builder.RegisterType<JsonOdbcFieldMapReader>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<JsonOdbcFieldMapWriter>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<OdbcSchema>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcDownloadCommand>();

            builder.RegisterType<OdbcStoreDownloader>();

            builder.RegisterType<OdbcCommandFactory>();

            builder.RegisterType<OdbcSampleDataCommand>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcCustomQueryWarningDlg>()
                .Named<IDialog>("OdbcCustomQueryWarningDlg");

            RegisterOrderLoadingTypes(builder);
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
