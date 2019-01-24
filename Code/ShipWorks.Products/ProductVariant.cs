using System;
using System.Linq;
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
        protected readonly string sku;
        protected readonly IProductVariantEntity variant;
        protected readonly ILog log;
        private readonly int? quantity;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductVariant(string sku, IProductVariantEntity variant, ILog log) : this(sku, variant, null, log)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductVariant(string sku, IProductVariantEntity variant, int? quantity, ILog log)
        {
            this.sku = sku?.Trim() ?? string.Empty;
            this.variant = variant;
            this.quantity = quantity;
            this.log = log;
        }

        /// <summary>
        /// Return true if product can write XML
        /// </summary>
        public bool CanWriteXml => variant?.IsActive == true;

        /// <summary>
        /// Write product XML
        /// </summary>
        public virtual void WriteXml(ElementOutline outline, Func<OrderItemProductBundleOutline> createOrderItemProductBundleOutline)
        {
            outline.AddAttribute("ProductID", () => variant.ProductID);
            outline.AddAttribute("ProductVariantID", () => variant.ProductVariantID);
            outline.AddElement("SKU", () => variant.DefaultSku);
            outline.AddElement("Weight", () => (double?) variant.Weight);
            outline.AddElement("Length", () => (double?) variant.Length);
            outline.AddElement("Width", () => (double?) variant.Width);
            outline.AddElement("Height", () => (double?) variant.Height);
            outline.AddElement("Name", () => variant.Name);
            outline.AddElement("ImageUrl", () => variant.ImageUrl);
            outline.AddElement("Location", () => variant.BinLocation);
            outline.AddElement("DateCreated", () => variant.CreatedDate);
            outline.AddElement("HarmonizedCode", () => variant.HarmonizedCode);
            outline.AddElement("CountryOfOrigin", () => variant.CountryOfOrigin);
            outline.AddElement("DeclaredValue", () => variant.DeclaredValue);
            outline.AddElement("Quantity", () => quantity, ElementOutline.If(() => quantity.HasValue));

            if (variant.AttributeValues.Any())
            {
                foreach (IProductVariantAttributeValueEntity attribute in variant.AttributeValues)
                {
                    ElementOutline attributeOutline = new ElementOutline(outline.Context);
                    attributeOutline.AddElement("Name", () => attribute.ProductAttribute.AttributeName);
                    attributeOutline.AddElement("Value", () => attribute.AttributeValue);

                    outline.AddElement("Attribute", attributeOutline);
                }
            }
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

            // "Attempting to apply product dimensions to item 00001 for SKU ABCD"
            log.InfoFormat("Attempting to apply product dimensions to item {0} for SKU {1}",
                item.OrderItemID, sku);

            ApplyValue(variant.Weight, () => item.Weight = (double) variant.Weight.Value);
            ApplyValue(variant.Length, () => item.Length = variant.Length.Value);
            ApplyValue(variant.Width, () => item.Width = variant.Width.Value);
            ApplyValue(variant.Height, () => item.Height = variant.Height.Value);
        }

        /// <summary>
        /// Adds customs data to a shipment for a line item
        /// </summary>
        public virtual void ApplyCustoms(OrderItemEntity item, ShipmentEntity shipment)
        {
            log.InfoFormat("Applying product information to customs item for SKU {0}", sku);

            ApplyCustoms(item, shipment, variant);
        }

        /// <summary>
        /// Applies customs item for a variant
        /// </summary>
        protected ShipmentCustomsItemEntity ApplyCustoms(OrderItemEntity item, ShipmentEntity shipment, IProductVariantEntity variantEntity)
        {
            ShipmentCustomsItemEntity customsItem = new ShipmentCustomsItemEntity
            {
                Shipment = shipment,
                Description = item.Name,
                Quantity = item.Quantity,
                Weight = item.Weight,
                UnitValue = item.UnitPrice,
                CountryOfOrigin = "US",
                HarmonizedCode = item.HarmonizedCode,
                NumberOfPieces = 0,
                UnitPriceAmount = item.UnitPrice
            };

            if (variantEntity != null && variantEntity.IsActive)
            {
                ApplyValue(variantEntity.Name, () => customsItem.Description = variantEntity.Name);
                ApplyValue(variantEntity.Weight, () => customsItem.Weight = (double) variantEntity.Weight);
                ApplyValue(variantEntity.DeclaredValue, () => customsItem.UnitValue = variantEntity.DeclaredValue.Value);
                ApplyValue(variantEntity.DeclaredValue, () => customsItem.UnitPriceAmount = variantEntity.DeclaredValue.Value);
                ApplyValue(variantEntity.CountryOfOrigin, () => customsItem.CountryOfOrigin = variantEntity.CountryOfOrigin);
                ApplyValue(variantEntity.HarmonizedCode, () => customsItem.HarmonizedCode = variantEntity.HarmonizedCode);
            }

            customsItem.UnitValue += item.OrderItemAttributes.Sum(oia => oia.UnitPrice);
            customsItem.UnitPriceAmount += item.OrderItemAttributes.Sum(oia => oia.UnitPrice);

            return customsItem;
        }

        /// <summary>
        /// Apply a decimal value when appropriate
        /// </summary>
        private void ApplyValue(decimal? toApply, Action apply)
        {
            if (toApply.HasValue && toApply.Value > 0)
            {
                apply();
            }
        }

        /// <summary>
        /// Apply a string value when appropriate
        /// </summary>
        private void ApplyValue(string toApply, Action apply)
        {
            if (!string.IsNullOrWhiteSpace(toApply))
            {
                apply();
            }
        }
    }
}
