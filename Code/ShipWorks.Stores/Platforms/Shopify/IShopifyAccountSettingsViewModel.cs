using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Shopify
{
	/// <summary>
	/// Interface for an ShopifyAccountSettingsViewModel
	/// </summary>
	public interface IShopifyAccountSettingsViewModel
	{
		/// <summary>
		/// Load the viewmodel from a store
		/// </summary>
		void Load(IShopifyStoreEntity etsyStoreEntity);
	}
}