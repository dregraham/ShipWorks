﻿using Autofac;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;

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
        }
    }
}
