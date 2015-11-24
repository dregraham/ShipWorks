using Autofac;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Yahoo
{
    public class YahooStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<YahooApiAccountPageViewModel>();
            builder.RegisterType<YahooApiAccountSettingsViewModel>();

            builder.RegisterType<YahooApiAccountPageHost>()
                .Keyed<WizardPage>(StoreTypeCode.Yahoo)
                .ExternallyOwned();

            builder.RegisterType<YahooApiWebClient>().AsImplementedInterfaces();
        }
    }
}