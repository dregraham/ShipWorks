using Autofac;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;
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
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcDataSourcePage>()
                .Keyed<WizardPage>(StoreTypeCode.Odbc)
                .ExternallyOwned();

            builder.RegisterType<OdbcDataSourceRepository>()
                .AsImplementedInterfaces();

            builder.RegisterType<DsnProvider>()
                .AsImplementedInterfaces();
        }
    }
}
