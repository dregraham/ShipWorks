using System;

namespace ShipWorks.Stores.Platforms.Shopify
{
    public static class ShopifyHelper
    {
        public static string GetShopUrl(string shopifyShopUrlName)
            => $"{shopifyShopUrlName}.myshopify.com";

        public static string GetCleanNoteText(string text)
        {
            const string breakNode = "<br/>";
            if (text.Contains(breakNode))
            {
                var values = text.Split(new[] { breakNode }, StringSplitOptions.None);
                if (string.IsNullOrWhiteSpace(values[0]) || values[0] == "null")
                {
                    return string.Empty;
                }
                text = values[0];
            }

            return text;
        }

}
}
