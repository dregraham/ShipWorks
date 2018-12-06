using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Products
{
    /// <summary>
    /// Bundle from Catalog
    /// </summary>
    public class ProductBundle : ProductVariant
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductBundle(string sku, IProductVariantEntity variant, ILog log)
            : base(sku, variant, log)
        {
        }

        /// <summary>
        /// Apply the product data to the customs item
        /// </summary>
        public override void ApplyCustoms(OrderItemEntity item, ShipmentEntity shipment)
        {
            log.InfoFormat("Applying bundle information to customs item for sku {0}", sku);

            foreach (IProductBundleEntity bundleItem in variant.Product.Bundles)
            {
                ShipmentCustomsItemEntity customsItem = ApplyCustoms(item, shipment, bundleItem.ChildVariant);
                customsItem.Quantity = customsItem.Quantity * bundleItem.Quantity;
            }
        }

        /// <summary>
        /// Write the bundle's xml
        /// </summary>
        public override void WriteXml(ElementOutline outline, Func<OrderItemProductBundleOutline> createOrderItemProductBundleOutline)
        {
            base.WriteXml(outline, createOrderItemProductBundleOutline);

            if (createOrderItemProductBundleOutline != null)
            {
                outline.AddElement("BundledProduct", createOrderItemProductBundleOutline(),
                () => FetchBundledVariants(),
                ElementOutline.If(() => createOrderItemProductBundleOutline != null && variant.Product.IsBundle));
            }
        }

        /// <summary>
        /// Fetch bundled Variants
        /// </summary>
        private IEnumerable<ProductVariant> FetchBundledVariants()
        {
            return variant.Product.Bundles.Select(b => new ProductVariant(b.ChildVariant.DefaultSku, b.ChildVariant, b.Quantity, log));
        }
    }
}
