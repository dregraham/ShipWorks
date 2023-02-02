namespace ShipWorks.Stores.Platforms.Shopify
{
	public class ShopifyExtension
	{
		public string GetShopUrl(string shopifyShopUrlName)
			=> $"{shopifyShopUrlName}.myshopify.com";

	}
}
