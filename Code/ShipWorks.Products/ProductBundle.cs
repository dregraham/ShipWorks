using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Products
{
    public class ProductBundle : ProductVariant
    {
        public ProductBundle(string sku, IProductVariantEntity variant, ILog log) : base(sku, variant, log)
        {

        }

        public override void ApplyCustoms(OrderItemEntity item, ShipmentEntity shipment)
        {
            Debug.Assert(variant.IsActive, "Variant should be active");
            Debug.Assert(variant.Product.IsBundle, "Variant should be a bundle");

            log.InfoFormat($"Applying bundle information to customs item for sku {sku}");

            foreach (IProductBundleEntity bundleItem in variant.Product.Bundles)
            {
                ShipmentCustomsItemEntity customsItem = ApplyCustoms(item, shipment, bundleItem.ChildVariant);
                customsItem.Quantity = customsItem.Quantity * bundleItem.Quantity;                
            }
        }

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
            return variant.Product.Bundles.Select(b => new ProductVariant(b.ChildVariant.Aliases.FirstOrDefault(a => a.IsDefault).Sku, b.ChildVariant, b.Quantity, log));
        }
    }
}
