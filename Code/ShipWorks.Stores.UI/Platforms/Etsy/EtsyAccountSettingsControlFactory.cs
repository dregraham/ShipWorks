using Autofac;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Etsy;

namespace ShipWorks.Stores.UI.Platforms.Etsy
{
    [Component]
    public class EtsyAccountSettingsControlFactory : IEtsyAccountSettingsControlFactory
    {
        public AccountSettingsControlBase Create(IStoreEntity store)
        {
            if (string.IsNullOrEmpty(store.OrderSourceID))
            {
                var createOrderSourceViewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<IEtsyCreateOrderSourceViewModel>();
                return new EtsyAccountSettingsMigrationControl(createOrderSourceViewModel);
            }

            var accountSettingsViewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<IEtsyAccountSettingsViewModel>();
            return new EtsyAccountSettingsControl(accountSettingsViewModel);
        }
    }
}
