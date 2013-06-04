using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using System.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using log4net;
using ShipWorks.Templates.Tokens;
using ShipWorks.Shipping;
using ShipWorks.Data.Adapter.Custom;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Utility functions for working with orders
    /// </summary>
    public static class OrderUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(OrderUtility));

        /// <summary>
        /// Returns the most recent, non-voided, processed shipment for the provided order
        /// </summary>
        public static ShipmentEntity GetLatestActiveShipment(long orderID)
        {
            ShipmentEntity shipment = 
                DataProvider.GetRelatedEntities(orderID, EntityType.ShipmentEntity)
                    .Cast<ShipmentEntity>()
                    .Where(s => s.Processed && !s.Voided)
                    .OrderBy(s => s.ProcessedDate)
                    .LastOrDefault();

            if (shipment != null)
            {
                // Shipments should always have their order attached.  Lots of parts of ShipWorks expect this
                OrderEntity order = (OrderEntity) DataProvider.GetEntity(shipment.OrderID);
                if (order == null)
                {
                    return null;
                }

                shipment.Order = order;

                return shipment;
            }

            return null;
        }

        /// <summary>
        /// Calculate the order total of the given order
        /// </summary>
        public static decimal CalculateTotal(long orderID, bool includeCharges)
        {
            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.OrderEntity);

            // Grab items and their attributes
            prefetch.Add(OrderEntity.PrefetchPathOrderItems).SubPath.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);

            // Grab charges
            if (includeCharges)
            {
                prefetch.Add(OrderEntity.PrefetchPathOrderCharges);
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                OrderEntity order = new OrderEntity(orderID);
                adapter.FetchEntity(order, prefetch);

                return CalculateTotal(order);
            }
        }

        /// <summary>
        /// Calculate the order total of the order.  The FK rows must be present and referenced
        /// by the order object.
        /// </summary>
        public static decimal CalculateTotal(OrderEntity order)
        {
            return CalculateTotal(order.OrderItems, order.OrderCharges);
        }

        /// <summary>
        /// Calculate the order total given the list of items and charges. Attributes for each item must already be attached
        /// to the item entity.
        /// </summary>
        public static decimal CalculateTotal(ICollection<OrderItemEntity> items, ICollection<OrderChargeEntity> charges)
        {
            decimal totalItems = 0;
            decimal totalCharges = 0;

            // Go through each item
            foreach (OrderItemEntity item in items)
            {
                // Add in the price of the item
                totalItems += ((decimal) item.Quantity * item.UnitPrice);

                // Go through each option
                foreach (OrderItemAttributeEntity option in item.OrderItemAttributes)
                {
                    // Add in the price of the option
                    totalItems += ((decimal) item.Quantity * option.UnitPrice);
                }
            }

            // Go through each charge
            foreach (OrderChargeEntity charge in charges)
            {
                totalCharges += charge.Amount;
            }

            decimal orderTotal = totalItems + totalCharges;

            return orderTotal;
        }

        /// <summary>
        /// Update the order total for the given orderID based on its items and attributes
        /// </summary>
        public static void UpdateOrderTotal(long orderID)
        {
            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.OrderEntity);

            // Grab charges
            prefetch.Add(OrderEntity.PrefetchPathOrderCharges);

            // Grab items and their attributes
            prefetch.Add(OrderEntity.PrefetchPathOrderItems).SubPath.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                OrderEntity order = new OrderEntity(orderID);
                adapter.FetchEntity(order, prefetch);

                order.OrderTotal = CalculateTotal(order);

                adapter.SaveEntity(order);
            }
        }

        /// <summary>
        /// Delete the charge with the given ID. This ensures the charge's order's total gets updated.
        /// </summary>
        public static void DeleteCharge(long chargeID)
        {
            OrderChargeEntity charge = (OrderChargeEntity) DataProvider.GetEntity(chargeID);

            // If it's already deleted I think we're safe to let it go
            if (charge == null)
            {
                log.WarnFormat("Charge {0} appears to be deleted already.", chargeID);
                return;
            }

            long orderID = charge.OrderID;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.DeleteEntity(charge);

                UpdateOrderTotal(orderID);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Save changes to the given charge.  The charge could be new or edited. This ensures the charge's order's total gets updated.
        /// </summary>
        public static void SaveCharge(OrderChargeEntity charge)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.SaveAndRefetch(charge);

                UpdateOrderTotal(charge.OrderID);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Delete the item with the given ID. This ensures the charge's order's total gets updated.
        /// </summary>
        public static void DeleteItem(long itemID)
        {
            OrderItemEntity item = (OrderItemEntity) DataProvider.GetEntity(itemID);

            if (item.OrderItemAttributes.Count == 0)
            {
                // load the attributes
                item.OrderItemAttributes.AddRange(DataProvider.GetRelatedEntities(itemID, EntityType.OrderItemAttributeEntity).Cast<OrderItemAttributeEntity>());
            }

            // If its already deleted i think we are safe to ignore
            if (item == null)
            {
                log.WarnFormat("Item {0} appears to be deleted already.", itemID);
                return;
            }

            long orderID = item.OrderID;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // delete each attribute entity so derived entities are also deleted
                foreach (OrderItemAttributeEntity attrib in item.OrderItemAttributes)
                {
                    adapter.DeleteEntity(attrib);
                }

                adapter.DeleteEntity(item);

                UpdateOrderTotal(orderID);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Delete the payment detail with the given id
        /// </summary>
        public static void DeletePaymentDetail(long detailID)
        {
            OrderPaymentDetailEntity detail = (OrderPaymentDetailEntity) DataProvider.GetEntity(detailID);

            // If it's already deleted I think we're safe to let it go
            if (detail == null)
            {
                log.WarnFormat("Payment detail {0} appears to be deleted already.", detailID);
                return;
            }

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.DeleteEntity(detail);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Save the given payment detail to the database
        /// </summary>
        public static void SavePaymentDetail(OrderPaymentDetailEntity detail)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.SaveAndRefetch(detail);

                adapter.Commit();
            }
        }
    }
}
