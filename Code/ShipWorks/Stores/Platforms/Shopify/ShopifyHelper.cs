using System;

namespace ShipWorks.Stores.Platforms.Shopify
{
    public static class ShopifyHelper
    {
        public static string GetShopUrl(string shopifyShopUrlName)
            => $"{shopifyShopUrlName}.myshopify.com";

        public static string[] GetSplitNotes(string text)
        {
            const string breakNode = "<br/>";
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (text.Contains(breakNode))
                {
                    var values = text.Split(new[] { breakNode }, StringSplitOptions.None);
                    return values;
                }

                return new[] { text };
            }

            return new []{ string.Empty };
        }
    }
}
