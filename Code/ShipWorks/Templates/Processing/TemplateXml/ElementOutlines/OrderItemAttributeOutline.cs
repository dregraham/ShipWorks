using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Data;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for attributes
    /// </summary>
    public class OrderItemAttributeOutline : ElementOutline
    {
        OrderItemAttributeEntity attribute;
        long storeID;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderItemAttributeOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => attribute.OrderItemAttributeID);

            AddElement("Name", () => attribute.Name);
            AddElement("Description", () => attribute.Description);
            AddElement("UnitPrice", () => attribute.UnitPrice);

            // Add an outline entry for each unique store type that could potentially be used
            foreach (StoreType storeType in StoreManager.GetUniqueStoreTypes())
            {
                // Let the StoreType generate its elements into a stand-in container
                ElementOutline container = new ElementOutline(context);
                storeType.GenerateTemplateOrderItemAttributeElements(container, () => attribute);

                // We need to "hoist" this as its own variable - otherwise the same storeType variable intance would get captured for each iteration.
                StoreTypeCode storeCode = storeType.TypeCode;

                // Copy the elements from the stand-in to ourself, adding on the StoreType specific condition
                AddElements(container, If(() => !attribute.IsManual && StoreManager.GetStore(storeID).TypeCode == (int)storeCode));
            }

            // Template backwards compatibility
            AddElementLegacy2x("Code", () => attribute.Name);
        }

        /// <summary>
        /// Bind a new instance of the outline to the given data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            Tuple<OrderItemAttributeEntity, long> tuple = (Tuple<OrderItemAttributeEntity, long>) data;

            return new OrderItemAttributeOutline(Context) { attribute = tuple.Item1, storeID = tuple.Item2  };
        }

    }
}
