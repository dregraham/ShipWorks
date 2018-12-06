using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Products
{
    /// <summary>
    /// Interface to a product
    /// </summary>
    public interface IProductVariant
    {
        /// <summary>
        /// Return true if product can write XML
        /// </summary>
        bool CanWriteXml { get; }

        /// <summary>
        /// Write product XML
        /// </summary>
        void WriteXml(ElementOutline outline, System.Func<OrderItemProductBundleOutline> createOrderItemProductBundleOutline);

        /// <summary>
        /// Apply product data to an order item
        /// </summary>
        void Apply(OrderItemEntity item);

        /// <summary>
        /// Apply the product data to the customs item
        /// </summary>
        void ApplyCustoms(OrderItemEntity item, ShipmentEntity shipment);

    }
}
