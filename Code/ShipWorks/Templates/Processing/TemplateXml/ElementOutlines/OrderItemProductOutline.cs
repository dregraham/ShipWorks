using System;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Products;

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
        public OrderItemProductOutline(TemplateTranslationContext context)
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
            IProductVariant product = (IProductVariant) data;

            var boundClone = new OrderItemProductOutline(Context) { product = product };
            product.WriteXml(boundClone);
            return boundClone;
        }
    }
}
