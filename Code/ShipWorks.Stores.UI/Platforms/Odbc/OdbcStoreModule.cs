using Autofac;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;
using System.Reflection;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
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

            builder.RegisterType<OdbcDataSource>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcCipherKey>()
                .Keyed<ICipherKey>(CipherContext.Odbc);

            builder.RegisterType<OdbcTableFactory>()
                .AsSelf();

            builder.RegisterType<OdbcImportFieldMappingControl>()
                .AsSelf();

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
