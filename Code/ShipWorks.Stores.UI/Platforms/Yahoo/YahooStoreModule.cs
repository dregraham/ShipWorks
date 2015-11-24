using Autofac;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.WizardPages;

namespace ShipWorks.Stores.UI.Platforms.Yahoo
{
    public class YahooStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<YahooApiAccountPageViewModel>();
        }
    }
}