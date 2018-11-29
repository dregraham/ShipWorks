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
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductVariant(string sku, IProductVariantEntity variant, ILog log)
        {
            this.sku = sku?.Trim() ?? string.Empty;
            this.variant = variant;
            this.log = log;
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
                item.OrderItemID, sku);

            ApplyDecimalValue(variant.Weight, () => item.Weight = (double) variant.Weight.Value);
            ApplyDecimalValue(variant.Length, () => item.Length = variant.Length.Value);
            ApplyDecimalValue(variant.Width, () => item.Width = variant.Width.Value);
            ApplyDecimalValue(variant.Height, () => item.Height = variant.Height.Value);
        }

        /// <summary>
        /// Apply the product data to the customs item
        /// </summary>
        public void Apply(ShipmentCustomsItemEntity customsItem)
        {
            if (variant == null || !variant.IsActive)
            {
                return;
            }

            log.InfoFormat($"Attempting to apply product information to customs item for sku {sku}");

            ApplyStringValue(variant.Name, () => customsItem.Description = variant.Name);
            ApplyDecimalValue(variant.Weight, () => customsItem.Weight = (double) variant.Weight);
            ApplyDecimalValue(variant.DeclaredValue, () => customsItem.UnitValue = variant.DeclaredValue.Value);
            ApplyDecimalValue(variant.DeclaredValue, () => customsItem.UnitPriceAmount = variant.DeclaredValue.Value);
            ApplyStringValue(variant.CountryOfOrigin, () => customsItem.CountryOfOrigin = variant.CountryOfOrigin);
            ApplyStringValue(variant.HarmonizedCode, () => customsItem.HarmonizedCode = variant.HarmonizedCode);
        }

        /// <summary>
        /// Apply a decimal value when appropriate
        /// </summary>
        private void ApplyDecimalValue(decimal? toApply, Action apply)
        {
            if (toApply.HasValue && toApply.Value > 0)
            {
                apply();
            }
        }

        /// <summary>
        /// Apply a string value when appropriate
        /// </summary>
        private void ApplyStringValue(string toApply, Action apply)
        {
            if (!string.IsNullOrWhiteSpace(toApply))
            {
                apply();
            }
        }
    }
}
