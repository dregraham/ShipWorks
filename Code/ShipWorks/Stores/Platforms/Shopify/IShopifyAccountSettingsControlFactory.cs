using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Shopify
{
	public interface IShopifyAccountSettingsControlFactory
	{
		AccountSettingsControlBase Create(IStoreEntity store);
	}
}