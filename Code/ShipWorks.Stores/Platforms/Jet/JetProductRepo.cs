using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Repository for jet products
    /// </summary>
    [Component(SingleInstance = true)]
    public class JetProductRepo : IJetProductRepo
    {
        private readonly IJetWebClient webClient;
        private readonly LruCache<string, JetProduct> productCache;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="webClient"></param>
        public JetProductRepo(IJetWebClient webClient)
        {
            this.webClient = webClient;
            productCache = new LruCache<string, JetProduct>(1000);
        }

        /// <summary>
        /// Get a product for the given item
        /// </summary>
        public JetProduct GetProduct(JetOrderItem item)
        {
            if (productCache.Contains(item.MerchantSku))
            {
                return productCache[item.MerchantSku];
            }

            GenericResult<JetProduct> result = webClient.GetProduct(item);
            if (result.Success)
            {
                AddProduct(item.MerchantSku, result.Value);
                return result.Value;
            }

            throw new JetException($"Error retrieving product information for {item.MerchantSku}, {result.Message}.");
        }

        /// <summary>
        /// Add the given product to the cache
        /// </summary>
        public void AddProduct(string sku, JetProduct product)
        {
            productCache[sku] = product;
        }
    }
}