using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Hashing;

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
        /// Calculate the order total of the order.  The FK rows must be present and referenced
        /// by the order object.
        /// </summary>
        public static decimal CalculateTotal(OrderEntity order, bool includeCharges)
        {
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));

            PopulateOrderDetails(order);

            // If includeCharges was true, send the order's OrderCharges collection.  Otherwise, if charges should not be included,
            // send an empty collection.
            EntityCollection<OrderChargeEntity> orderCharges = includeCharges ? order.OrderCharges : new EntityCollection<OrderChargeEntity>();

            return CalculateTotal(order.OrderItems, orderCharges);
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

        /// <summary>
        /// Copies any note entities from one order to another.
        /// </summary>
        public static async Task CopyNotes(long fromOrderID, OrderEntity toOrder, ISqlAdapter sqlAdapter)
        {
            var factory = new QueryFactory();
            var query = factory.Note.Where(NoteFields.EntityID == fromOrderID);
            var newNotes = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);

            foreach (NoteEntity note in newNotes)
            {
                EntityUtility.MarkAsNew(note);
                note.Order = toOrder;

                NoteManager.SaveNote(note);
            }
        }

        /// <summary>
        /// Copies any shipment entities from one order to another
        /// </summary>
        public static async Task CopyShipments(long fromOrderID, OrderEntity toOrder, ISqlAdapter sqlAdapter)
        {
            var factory = new QueryFactory();
            var query = factory.Shipment.Where(ShipmentFields.OrderID == fromOrderID);
            var shipments = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);

            // Copy any existing shipments
            foreach (ShipmentEntity shipment in shipments)
            {
                // load all carrier and customs data
                ShippingManager.EnsureShipmentLoaded(shipment);

                // this is now a new shipment to be inserted
                EntityUtility.MarkAsNew(shipment);
                shipment.Order = toOrder;

                // Mark all the carrier-specific stuff as new
                foreach (IEntityCore entity in ((IEntityCore) shipment).GetDependingRelatedEntities())
                {
                    EntityUtility.MarkAsNew(entity);
                }

                // And all the customers stuff as new
                foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
                {
                    EntityUtility.MarkAsNew(customsItem);
                }

                ShippingManager.SaveShipment(shipment);
            }
        }

        /// <summary>
        /// Populates the order, order items, and order item attribute for the given shipment.
        /// </summary>
        public static OrderEntity FetchOrder(long orderID)
        {
            OrderEntity order = DataProvider.GetEntity(orderID) as OrderEntity;
            if (order != null)
            {
                PopulateOrderDetails(order);
            }
            return order;
        }

        /// <summary>
        /// Populates the order, order items, and order item attribute for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public static void PopulateOrderDetails(ShipmentEntity shipment)
        {
            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                PopulateOrderDetails(shipment, adapter);
            }
        }

        /// <summary>
        /// Populates the order, order items, and order item attribute for the given shipment.
        /// </summary>
        public static void PopulateOrderDetails(ShipmentEntity shipment, SqlAdapter adapter)
        {
            if (shipment.Order == null)
            {
                shipment.Order = (OrderEntity) DataProvider.GetEntity(shipment.OrderID);
            }

            PopulateOrderDetails(shipment.Order, adapter);
        }

        /// <summary>
        /// Populates the order, order items, order charges, and order item attribute for the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        public static void PopulateOrderDetails(OrderEntity order)
        {
            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                PopulateOrderDetails(order, adapter);
            }
        }

        /// <summary>
        /// Populates the order, order items, and order item attribute for the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="adapter">The adapter.</param>
        public static void PopulateOrderDetails(OrderEntity order, ISqlAdapter adapter)
        {
            if (order.Store == null)
            {
                order.Store = StoreManager.GetStore(order.StoreID);
            }

            if (order.OrderItems.None())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            if (order.OrderCharges.None())
            {
                adapter.FetchEntityCollection(order.OrderCharges, new RelationPredicateBucket(OrderChargeFields.OrderID == order.OrderID));
            }

            foreach (OrderItemEntity orderItemEntity in order.OrderItems)
            {
                if (orderItemEntity.OrderItemAttributes.None())
                {
                    adapter.FetchEntityCollection(orderItemEntity.OrderItemAttributes, new RelationPredicateBucket(OrderItemAttributeFields.OrderItemID == orderItemEntity.OrderItemID));
                }
            }
        }

        /// <summary>
        /// Updates the ShipSense hash key of an order if any of the properties/attributes of the
        /// order items are dirty.
        /// </summary>
        /// <param name="order">The order.</param>
        public static void UpdateShipSenseHashKey(OrderEntity order)
        {
            // Use the knowledge base to determine the hash key as well, so the values sync up with
            // what actually is used by the knowledge base
            Knowledgebase knowledgebase = new Knowledgebase();

            KnowledgebaseHashResult hash = knowledgebase.GetHashResult(order);
            order.ShipSenseHashKey = hash.IsValid ? hash.HashValue : string.Empty;
        }
    }
}
