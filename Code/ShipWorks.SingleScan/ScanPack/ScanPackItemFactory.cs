using System.Collections.Generic;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products;

namespace ShipWorks.SingleScan.ScanPack
{
    /// <summary>
    /// ScanPackItem Factory
    /// </summary>
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
        public List<ScanPackItem> Create(OrderEntity order)
        {
            List<ScanPackItem> result = new List<ScanPackItem>();

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                foreach (OrderItemEntity item in order.OrderItems)
                {
                    ProductVariantEntity product = productCatalog.FetchProductVariantEntity(adapter, item.SKU);
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
            string imageUrl = string.IsNullOrWhiteSpace(product.ImageUrl) ? item.Image : product.ImageUrl;
            string name = string.IsNullOrWhiteSpace(product.Name) ? item.Name : product.Name;

            return new ScanPackItem(name, imageUrl, item.Quantity);
        }
    }
}
