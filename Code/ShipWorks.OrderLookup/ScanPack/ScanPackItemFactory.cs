using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip;

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
        private List<ScanPackItem> scanPackItems; 

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
            scanPackItems = new List<ScanPackItem>();

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                var skusInOrder = order.OrderItems.Select(i => i.SKU).Where(s => !string.IsNullOrEmpty(s));
                var products = (await productCatalog
                    .FetchProductVariantEntities(adapter, skusInOrder).ConfigureAwait(true)).ToList();
                
                foreach (OrderItemEntity item in order.OrderItems)
                {
                    ProductVariantEntity product = null;
                    if (!string.IsNullOrWhiteSpace(item.SKU))
                    {
                        product = products.FirstOrDefault(p => p.Aliases.Any(a => a.Sku.Equals(item.SKU, StringComparison.InvariantCulture)));
                    }

                    if (product?.Product.IsBundle ?? false)
                    {
                        AddBundle(item, product);
                    }
                    else
                    {
                        AddItem(item, product);
                    }
                }
            }

            return scanPackItems;
        }

        /// <summary>
        /// Adds bundle
        /// </summary>
        private void AddBundle(OrderItemEntity item, ProductVariantEntity product)
        {
            // keep a running total of quantity in case the quantity isn't an int
            var itemQuantity = item.Quantity;
            while(itemQuantity > 0)
            {
                double quantityToAdd = 1;
                if (itemQuantity < 1)
                {
                    quantityToAdd = itemQuantity;
                }

                var parentSortIdentifier = AddScanPackItem(product, item, null, quantityToAdd, true);
                
                itemQuantity -= quantityToAdd;

                foreach (var bundledItem in product.Product.Bundles)
                {
                    AddScanPackItem(bundledItem.ChildVariant, null, parentSortIdentifier, bundledItem.Quantity * quantityToAdd);
                }
            }
        }
        
        /// <summary>
        /// Adds an item that isn't a bundle
        /// </summary>
        private void AddItem(OrderItemEntity item, ProductVariantEntity product)
        {
            Debug.Assert(product == null || product.Product.IsBundle == false, "A bundle should never be sent to this method.");

            AddScanPackItem(product, item, null, item.Quantity);
        }

        /// <summary>
        /// Adds a ScanPackItem to the collection and returns the identifier
        /// </summary>
        private int AddScanPackItem(ProductVariantEntity product, OrderItemEntity item, int? parentSortIdentifier, double quantity, bool isBundle = false)
        {
            string imageUrl = product?.ImageUrl;
            if (imageUrl.IsNullOrWhiteSpace())
            {
                imageUrl = item?.Image;
            }
            if (imageUrl.IsNullOrWhiteSpace())
            {
                imageUrl = item?.Thumbnail ?? string.Empty;
            }

            string name = string.IsNullOrWhiteSpace(product?.Name) ? item?.Name : product.Name ?? string.Empty;
            string itemUpc = item?.UPC ?? string.Empty;
            string itemCode = item?.Code ?? string.Empty;
            string productUpc = product?.UPC ?? string.Empty;
            string sku = item?.SKU ?? string.Empty;

            int sortIdentifier = scanPackItems.Count;

            scanPackItems.Add(new ScanPackItem(sortIdentifier, name, imageUrl, quantity, isBundle, parentSortIdentifier, true, itemCode, itemUpc, productUpc, sku, product?.EAN, product?.FNSku));

            return sortIdentifier;
        }
    }
}
