using System;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for item product node
    /// </summary>
    public class OrderItemProductOutline : ElementOutline
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderItemProductOutline));

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public OrderItemProductOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("SKU", () => Item.SKU);
            AddElement("Weight", () => ProductVariant.Weight);
            AddElement("Length", () => ProductVariant.Length);
            AddElement("Width", () => ProductVariant.Width);
            AddElement("Height", () => ProductVariant.Height);
            AddElement("Name", () => ProductVariant.Name);
            AddElement("ImageUrl", () => ProductVariant.ImageUrl);
            AddElement("Location", () => ProductVariant.BinLocation);
            AddElement("DateCreated", () => ProductVariant.CreatedDate.ToString("d"));
            AddElement("IsActive", () => ProductVariant.IsActive ? "Yes" : "No");
            AddElement("HarmonizedCode", () => ProductVariant.HarmonizedCode);
            AddElement("CountryOfOrigin", () => ProductVariant.CountryOfOrigin);
            AddElement("DeclaredValue", () => ProductVariant.DeclaredValue);
        }

        /// <summary>
        /// The OrderItemEntity represented by the bound outline
        /// </summary>
        private OrderItemEntity Item { get; set; }

        /// <summary>
        /// The IProductVariantEntity represented by the bound outline
        /// </summary>
        private IProductVariantEntity ProductVariant { get; set; }

        /// <summary>
        /// Create a new cloned outline bound to a given order item and product variant
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            Tuple<IProductVariantEntity, OrderItemEntity> tuple = (Tuple<IProductVariantEntity, OrderItemEntity>) data;

            return new OrderItemProductOutline(Context) { ProductVariant = tuple.Item1, Item = tuple.Item2 };
        }
    }
}
