using Interapptive.Shared.Utility;
using ShipWorks.Products;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Create an outline for a Bundled Product
    /// </summary>
    public class OrderItemProductBundleOutline : ElementOutline
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderItemProductBundleOutline(TemplateTranslationContext context)
            : base(context)
        {
        }

        /// <summary>
        /// The OrderItemEntity represented by the bound outline
        /// </summary>
        private IProductVariant product { get; set; }

        /// <summary>
        /// Create a new cloned outline bound to a given order item and product variant
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            MethodConditions.EnsureArgumentIsNotNull(data, nameof(data));

            IProductVariant product = (IProductVariant) data;

            var boundClone = new OrderItemProductBundleOutline(Context) { product = product };
            product.WriteXml(boundClone, null);
            return boundClone;
        }
    }
}