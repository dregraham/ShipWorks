using Autofac;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Stores.Platforms.LemonStand.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.LemonStand
{
    public class LemonStandStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LemonStandStoreType>()
                .Keyed<StoreType>(StoreTypeCode.LemonStand)
                .ExternallyOwned();

            builder.RegisterType<LemonStandAccountSettingsControl>()
                .Keyed<AccountSettingsControlBase>(StoreTypeCode.LemonStand)
                .ExternallyOwned();

            builder.RegisterType<LemonStandAccountPage>()
                .Keyed<WizardPage>(StoreTypeCode.LemonStand)
                .ExternallyOwned();
        }
    }
}
