using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// ScanPackItem Factory
    /// </summary>
    [Component]
    public class ScanPackItemFactory : IScanPackItemFactory
    {
        private readonly IProductCatalog productCatalog;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackItemFactory(IProductCatalog productCatalog, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.productCatalog = productCatalog;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Create ScanPackItems for the given order
        /// </summary>
        public async Task<List<ScanPackItem>> Create(OrderEntity order)
        {
            List<ScanPackItem> result = new List<ScanPackItem>();

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                IEnumerable<ProductVariantEntity> products = await productCatalog
                    .FetchProductVariantEntities(adapter, order.OrderItems.Select(i => i.SKU)).ConfigureAwait(true);

                foreach (OrderItemEntity item in order.OrderItems)
                {
                    ProductVariantEntity product = products.FirstOrDefault(p => p.Aliases.Any(a => a.Sku.Equals(item.SKU, StringComparison.InvariantCultureIgnoreCase)));
                    result.Add(CreateItem(product, item));
                }
            }

            return result;
        }

        /// <summary>
        /// Create a ScanPackItem based on the product and item
        /// </summary>
        private ScanPackItem CreateItem(ProductVariantEntity product, OrderItemEntity item)
        {
            string imageUrl = string.IsNullOrWhiteSpace(product?.ImageUrl) ? item.Image : product.ImageUrl;

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                imageUrl = item.Thumbnail;
            }

            string name = string.IsNullOrWhiteSpace(product?.Name) ? item.Name : product.Name;

            string sku = string.IsNullOrWhiteSpace(product?.DefaultSku) ? item.SKU : product.DefaultSku;

            return new ScanPackItem(name, imageUrl, item.Quantity, sku);
        }
    }
}
