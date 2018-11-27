using System;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Products
{
    /// <summary>
    /// Product from Catalog
    /// </summary>
    public class ProductVariant : IProductVariant
    {
        private readonly string sku;
        private readonly IProductVariantEntity variant;
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductVariant));

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductVariant(string sku, IProductVariantEntity variant)
        {
            this.sku = sku;
            this.variant = variant;
        }

        /// <summary>
        /// Return true if product can write XML
        /// </summary>
        public bool CanWriteXml => variant != null;

        /// <summary>
        /// Write product XML
        /// </summary>
        public void WriteXml(OrderItemProductOutline outline)
        {
            outline.AddElement("SKU", () => sku);
            outline.AddElement("Weight", () => variant.Weight);
            outline.AddElement("Length", () => variant.Length);
            outline.AddElement("Width", () => variant.Width);
            outline.AddElement("Height", () => variant.Height);
            outline.AddElement("Name", () => variant.Name);
            outline.AddElement("ImageUrl", () => variant.ImageUrl);
            outline.AddElement("Location", () => variant.BinLocation);
            outline.AddElement("DateCreated", () => variant.CreatedDate.ToString("d"));
            outline.AddElement("IsActive", () => variant.IsActive ? "Yes" : "No");
            outline.AddElement("HarmonizedCode", () => variant.HarmonizedCode);
            outline.AddElement("CountryOfOrigin", () => variant.CountryOfOrigin);
            outline.AddElement("DeclaredValue", () => variant.DeclaredValue);
        }

        /// <summary>
        /// Applies product data to an order item
        /// </summary>
        public void Apply(OrderItemEntity item)
        {
            if (variant == null || !variant.IsActive)
            {
                return;
            }

            // "Attempting to apply product dimensions to item 00001 for sku ABCD"
            log.InfoFormat("Attempting to apply product dimensions to item {0} for sku {1}",
                item.OrderItemID, item.SKU.Trim());

            ApplyDim(variant.Weight, () => item.Weight = (double) variant.Weight.Value);
            ApplyDim(variant.Length, () => item.Length = variant.Length.Value);
            ApplyDim(variant.Width, () => item.Width = variant.Width.Value);
            ApplyDim(variant.Height, () => item.Height = variant.Height.Value);
        }

        /// <summary>
        /// Apply a dimension when appropriate 
        /// </summary>
        private void ApplyDim(decimal? toApply, Action apply)
        {
            if (toApply.HasValue && toApply.Value > 0)
            {
                apply();
            }
        }
    }
}
