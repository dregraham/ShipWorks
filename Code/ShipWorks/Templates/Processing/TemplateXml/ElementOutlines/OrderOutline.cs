﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using log4net;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model;
using ShipWorks.Stores;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the 'Order' element
    /// </summary>
    public class OrderOutline : ElementOutline
    {
        static readonly ILog log = LogManager.GetLogger(typeof(OrderOutline));

        long orderID;
        OrderEntity order;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public OrderOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => orderID);
            AddAttribute("storeID", () => Order.StoreID);

            AddElement("Number", () => Order.OrderNumberComplete);
            AddElement("Date", () => Order.OrderDate);

            AddElement("Status", () => Order.LocalStatus);
            AddElement("OnlineStatus", () => Order.OnlineStatus);

            AddElement("OnlineStatusCode", () => Order.OnlineStatusCode, If(() => Order.OnlineStatusCode != null));
            AddElement("OnlineCustomerID", () => Order.OnlineCustomerID, If(() => Order.OnlineCustomerID != null));
            
            AddElement("IsManual", () => Order.IsManual);
            AddElement("RequestedShipping", () => Order.RequestedShipping);
            AddElement("Total", () => Order.OrderTotal);

            AddElement("ChannelOrderID", () => Order.ChannelOrderID);
            AddElement("ShipByDate", () => Order.ShipByDate);

            AddElement("CustomField1", () => Order.Custom1);
            AddElement("CustomField2", () => Order.Custom2);
            AddElement("CustomField3", () => Order.Custom3);
            AddElement("CustomField4", () => Order.Custom4);
            AddElement("CustomField5", () => Order.Custom5);
            AddElement("CustomField6", () => Order.Custom6);
            AddElement("CustomField7", () => Order.Custom7);
            AddElement("CustomField8", () => Order.Custom8);
            AddElement("CustomField9", () => Order.Custom9);
            AddElement("CustomField10", () => Order.Custom10);

            AddElement("Address", new AddressOutline(context, "ship", true), () => new PersonAdapter(Order, "Ship"));
            AddElement("Address", new AddressOutline(context, "bill", true), () => new PersonAdapter(Order, "Bill"));

            // Add an outline entry for each unique store type that could potentially be used
            foreach (StoreType storeType in StoreManager.GetUniqueStoreTypes())
            {
                // Let the StoreType generate its elements into a stand-in container
                ElementOutline container = new ElementOutline(context);
                storeType.GenerateTemplateOrderElements(container, () => Order);

                // We need to "hoist" this as its own variable - otherwise the same storeType variable intance would get captured for each iteration.
                StoreTypeCode storeCode = storeType.TypeCode;
                
                // Copy the elements from the stand-in to ourself, adding on the StoreType specific condition
                AddElements(container, If(() => !Order.IsManual && StoreManager.GetStore(Order.StoreID).TypeCode == (int) storeCode));
            }

            AddElement("Item", new OrderItemOutline(context), () => context.Input.GetOrderItemKeys(orderID).Select(itemID => new Tuple<OrderEntity, long>(Order, itemID)));
            AddElement("Charge", new OrderChargeOutline(context), () => DataProvider.GetRelatedKeys(orderID, EntityType.OrderChargeEntity));
            AddElement("Payment", new OrderPaymentOutline(context), () => orderID);
            AddElement("Shipment", new ShipmentOutline(context), () => context.Input.GetShipmentKeys(orderID));

            AddElement("Note", new NoteOutline(context), () => DataProvider.GetRelatedKeys(orderID, EntityType.NoteEntity));
            AddElement("Notes", NoteOutline.CreateLegacy2xNotesOutline(context, () => orderID));
        }

        /// <summary>
        /// Create a clone of the outline, bound to the given data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new OrderOutline(Context) { orderID = (long) data };
        }

        /// <summary>
        /// The OrderEntity represented by the bound outline
        /// </summary>
        private OrderEntity Order
        {
            get
            {
                if (order == null)
                {
                    order = (OrderEntity) DataProvider.GetEntity(orderID);
                    if (order == null)
                    {
                        log.WarnFormat("Order {0} was deleted and cannot be processed by template.", orderID);
                        throw new TemplateProcessException("An order has been deleted.");
                    }
                }

                return order;
            }
        }
    }
}
