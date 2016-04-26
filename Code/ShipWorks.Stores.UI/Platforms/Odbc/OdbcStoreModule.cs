using Autofac;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Odbc
{
    public class OdbcStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OdbcStoreType>()
                .Keyed<StoreType>(StoreTypeCode.Odbc)
                .ExternallyOwned();

            builder.RegisterType<ShipWorksOdbcProvider>()
                .AsImplementedInterfaces();

            builder.RegisterType<OdbcDataSourcePage>()
                .Keyed<WizardPage>(StoreTypeCode.Odbc)
                .ExternallyOwned();
        }
    }
}
