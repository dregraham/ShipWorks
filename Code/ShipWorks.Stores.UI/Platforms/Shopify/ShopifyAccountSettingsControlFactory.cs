using Autofac;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Shopify;

namespace ShipWorks.Stores.UI.Platforms.Shopify
{
	[Component]
	public class ShopifyAccountSettingsControlFactory : IShopifyAccountSettingsControlFactory
	{
		public AccountSettingsControlBase Create(IStoreEntity store)
		{
			if (string.IsNullOrEmpty(store.OrderSourceID))
			{
				var createOrderSourceViewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<IShopifyCreateOrderSourceViewModel>();
				return new ShopifyAccountSettingsMigrationControl(createOrderSourceViewModel);
			}

			var accountSettingsViewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<IShopifyAccountSettingsViewModel>();
			return new ShopifyAccountSettingsControl(accountSettingsViewModel);
		}
	}
}