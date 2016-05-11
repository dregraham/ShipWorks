using Autofac;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;
using ShipWorks.UI.Controls.WebBrowser;
using ShipWorks.UI.Wizard;

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

            builder.RegisterType<OdbcDataSourcePage>()
                .Keyed<WizardPage>(StoreTypeCode.Odbc)
                .ExternallyOwned();

            builder.RegisterType<OdbcDataSourceRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DsnProvider>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcControlPanel>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcDataSource>();

            builder.RegisterType<OdbcCipherKey>()
                .Keyed<ICipherKey>(CipherContext.Odbc);

            builder.RegisterType<OdbcTableFactory>()
                .AsSelf();

            builder.RegisterType<OdbcImportFieldMappingDlg>()
                .Named<IDialog>("OdbcImportFieldMappingDlg");

            builder.RegisterType<OdbcImportFieldMappingDlgViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcFieldMapFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<JsonOdbcFieldMapIOFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcImportFieldMappingPage>()
                .Keyed<WizardPage>(StoreTypeCode.Odbc)
                .ExternallyOwned();

            builder.RegisterType<OdbcImportFieldMappingDlgFactory>()
                .AsSelf();
        }
    }
}
