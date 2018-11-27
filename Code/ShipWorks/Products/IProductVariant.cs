using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        void WriteXml(OrderItemProductOutline outline);

        /// <summary>
        /// Apply product data to an order item
        /// </summary>
        void Apply(OrderItemEntity item);
    }
}
