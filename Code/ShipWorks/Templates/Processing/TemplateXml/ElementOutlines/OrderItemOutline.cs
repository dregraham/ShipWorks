using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using log4net;
using ShipWorks.Data.Model;
using ShipWorks.Stores;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for 'Item' node
    /// </summary>
    public class OrderItemOutline : ElementOutline
    {
        static readonly ILog log = LogManager.GetLogger(typeof(OrderItemOutline));

        OrderEntity order;

        long itemID;
        OrderItemEntity item;

        List<OrderItemAttributeEntity> attributes;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public OrderItemOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => itemID);

            AddElement("Name", () => Item.Name);
            AddElement("Code", () => Item.Code);
            AddElement("SKU", () => Item.SKU);
            AddElement("Description", () => Item.Description);
            AddElement("Location", () => Item.Location);
            AddElement("ISBN", () => Item.ISBN);
            AddElement("UPC", () => Item.UPC);
            AddElement("Thumbnail", () => Item.Thumbnail);
            AddElement("Image", () => Item.Image);
            AddElement("UnitPrice", () => Item.UnitPrice);
            AddElement("UnitPriceWithOptions", () => GetUnitPriceWithOptions(Item, Attributes));
            AddElement("UnitCost", () => Item.UnitCost);
            AddElement("Weight", () => Item.Weight);
            AddElement("Quantity", () => Item.Quantity);
            AddElement("TotalPrice", () => Item.UnitPrice * (decimal) Item.Quantity);
            AddElement("TotalPriceWithOptions", () => GetUnitPriceWithOptions(Item, Attributes) * (decimal) Item.Quantity);
            AddElement("TotalCost", () => Item.UnitCost * (decimal) Item.Quantity);
            AddElement("TotalWeight", () => Item.Weight * Item.Quantity);
            AddElement("Status", () => Item.LocalStatus);
            AddElement("IsManual", () => Item.IsManual);

            // Add an outline entry for each unique store type that could potentially be used
            foreach (StoreType storeType in StoreManager.GetUniqueStoreTypes())
            {
                // Let the StoreType generate its elements into a stand-in container
                ElementOutline container = new ElementOutline(context);
                storeType.GenerateTemplateOrderItemElements(container, () => Item);

                // We need to "hoist" this as its own variable - otherwise the same storeType variable intance would get captured for each iteration.
                StoreTypeCode storeCode = storeType.TypeCode;

                // Copy the elements from the stand-in to ourself, adding on the StoreType specific condition
                AddElements(container, If(() => !Item.IsManual && StoreManager.GetStore(order.StoreID).TypeCode == (int) storeCode));
            }

            // Output the total for the item, including child attribute costs
            // Converted to legacy element on 05/07/2013 with the addition of the new "TotalXXX" elements
            AddElementLegacy2x("Total", () => GetUnitPriceWithOptions(Item, Attributes) * (decimal) Item.Quantity);

            // Add in each attribute
            AddElement("Option", new OrderItemAttributeOutline(context), () => Attributes.Select(a => new Tuple<OrderItemAttributeEntity, long>(a, order.StoreID)));
        }

        /// <summary>
        /// Get the total cost of the item including attributes
        /// </summary>
        private static decimal GetUnitPriceWithOptions(OrderItemEntity item, List<OrderItemAttributeEntity> attributes)
        {
            decimal unitTotal = item.UnitPrice;

            foreach (OrderItemAttributeEntity attribute in attributes)
            {
                unitTotal += attribute.UnitPrice;
            }

            return unitTotal;
        }

        /// <summary>
        /// Create a new cloned outline bound to a given order item ID
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            Tuple<OrderEntity, long> tuple = (Tuple<OrderEntity, long>) data;

            return new OrderItemOutline(Context) { order = EntityUtility.CloneEntity(tuple.Item1), itemID = tuple.Item2 };
        }

        /// <summary>
        /// The OrderItemEntity represented by the bound outline
        /// </summary>
        private OrderItemEntity Item
        {
            get
            {
                if (item == null)
                {
                    item = (OrderItemEntity) DataProvider.GetEntity(itemID);
                    if (item == null)
                    {
                        log.WarnFormat("Order charge {0} was deleted and cannot be processed by template.", itemID);
                        throw new TemplateProcessException("An order  charge has been deleted.");
                    }

                    item.Order = order;
                }

                return item;
            }
        }

        /// <summary>
        /// The list of child attributes
        /// </summary>
        private List<OrderItemAttributeEntity> Attributes
        {
            get
            {
                if (attributes == null)
                {
                    attributes = DataProvider.GetRelatedEntities(itemID, EntityType.OrderItemAttributeEntity).Cast<OrderItemAttributeEntity>().ToList();
                }

                return attributes;
            }
        }
    }
}
