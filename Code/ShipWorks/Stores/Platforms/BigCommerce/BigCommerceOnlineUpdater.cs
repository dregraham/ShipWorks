using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;
using log4net;
using Quartz.Util;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Updates BigCommerce order status/shipments
    /// </summary>
    public class BigCommerceOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BigCommerceOnlineUpdater));
        readonly BigCommerceStoreEntity bigCommerceStore;

        // status code provider
        BigCommerceStatusCodeProvider statusCodeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceOnlineUpdater(BigCommerceStoreEntity store)
        {
            bigCommerceStore = store;
        }

        /// <summary>
        /// Gets the status code provider
        /// </summary>
        protected BigCommerceStatusCodeProvider StatusCodeProvider
        {
            get { return statusCodeProvider ?? (statusCodeProvider = new BigCommerceStatusCodeProvider(bigCommerceStore)); }
        }

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusCode)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UpdateOrderStatus(orderID, statusCode, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (!order.IsManual)
                {
                    BigCommerceWebClient client = new BigCommerceWebClient(bigCommerceStore.ApiUserName, bigCommerceStore.ApiUrl, bigCommerceStore.ApiToken);
                    client.UpdateOrderStatus(Convert.ToInt32(order.OrderNumber), statusCode);

                    // Update the local database with the new status
                    OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
                    basePrototype.OnlineStatusCode = statusCode;
                    basePrototype.OnlineStatus = StatusCodeProvider.GetCodeName(statusCode);

                    unitOfWork.AddForSave(basePrototype);
                }
                else
                {
                    log.InfoFormat("Not uploading order status since order {0} is manual or the order only has digital items.", order.OrderID);
                }
            }
            else
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
            }
        }

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        public void UpdateShipmentDetails(OrderEntity order)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
            if (shipment == null)
            {
                // log that there was no shipment, and return
                log.DebugFormat("There was no shipment found for order Id: {0}", order.OrderID);
                return;
            }

            UpdateShipmentDetails(shipment);
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.WarnFormat("Not updating status of shipment {0} as it has gone away.", shipmentID);
                return;
            }

            UpdateShipmentDetails(shipment);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            OrderEntity order = shipment.Order;
            if (order.IsManual)
            {
                log.WarnFormat("Not updating order {0} since it is manual.", shipment.Order.OrderNumberComplete);
                return;
            }

            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            // BigCommerce requires order items to create a shipment, so make sure we have some
            if (order.OrderItems == null || order.OrderItems.Count == 0)
            {
                throw new BigCommerceException(string.Format("Unable to upload shipment details because no order items were found for order number {0}", order.OrderNumber));
            }

            // If the order is only digital, update the order status to completed, then return
            // We don't want to actually "ship" the order because there is no order address id, and BigCommerce
            // will return an error.
            if (IsOrderAllDigital(order))
            {
                UpdateOrderStatus(order.OrderID, BigCommerceConstants.OrderStatusCompleted);
                return;
            }

            BigCommerceWebClient webClient = new BigCommerceWebClient(bigCommerceStore.ApiUserName, bigCommerceStore.ApiUrl, bigCommerceStore.ApiToken);
            List<BigCommerceProduct> orderProducts = null;
            long bigCommerceOrderAddressId = BigCommerceConstants.InvalidOrderAddressID;

            // If the order item doesn't have a valid OrderAddressID, we know it's a legacy order.  Set a flag noting this.
            // Note: Digital items have an order address id of 0 when downloaded
            bool hasBigCommerceRequiredShippingFields = order.OrderItems.Any(oi => (((BigCommerceOrderItemEntity)oi).OrderAddressID > 0));

            // If this is a legacy order AND it is a multi-ship to order, we can't determine which items go to which order.
            // So we'll skip uploading shipment info, and inform the user to use the website to finish.
            if (!hasBigCommerceRequiredShippingFields && order.OrderNumberComplete.IndexOf("-", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new BigCommerceException(string.Format("Order number {0} was downloaded prior to the new BigCommerce API upgrade.  Uploading shipment information is not supported for orders with multiple ship-to addresses that were downloaded before the upgrade. Please enter shipping information using the BigCommerce admin website.", order.OrderNumberComplete));
            }

            if (!hasBigCommerceRequiredShippingFields)
            {
                // We don't have a valid order item address id, so ask the cart for it
                // Note: Digital items have an order address id of 0 when downloaded
                orderProducts = webClient.GetOrderProducts(order.OrderNumber);
                bigCommerceOrderAddressId = orderProducts.FirstOrDefault(op => op.order_address_id > 0).order_address_id;
            }
            else
            {
                // This is an order downloaded via the API, we should be good to go with the downloaded data.
                BigCommerceOrderItemEntity orderItem = (BigCommerceOrderItemEntity)order.OrderItems.FirstOrDefault(oi => (((BigCommerceOrderItemEntity)oi).OrderAddressID > 0));
                bigCommerceOrderAddressId = orderItem.OrderAddressID;
            }

            // If we couldn't find an order address id, the order is not supposed to be shipped, so just return
            if (bigCommerceOrderAddressId == BigCommerceConstants.InvalidOrderAddressID)
            {
                log.WarnFormat("Not processing shipment for order {0} since it has no BigCommerce shipping order address.  The order probably has only digital items.", shipment.Order.OrderNumberComplete);
                return;
            }

            // Get the BigCommerceItems to pass to the web client
            List<BigCommerceItem> bigCommerceOrderItems = GetOrderItems(order, orderProducts);

            string shippingMethod = GetShippingMethod(shipment);

            webClient.UploadOrderShipmentDetails(order.OrderNumber, bigCommerceOrderAddressId, shipment.TrackingNumber, shippingMethod, bigCommerceOrderItems);
        }

        /// <summary>
        /// Gets the shipping method.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        private static string GetShippingMethod(ShipmentEntity shipment)
        {
            string carrier = ShipmentTypeManager.GetType(shipment).ToString();
            string service = ShippingManager.GetServiceUsed(shipment);

            // If the service starts with the carrier name, cut the carrier name off
            if (service.StartsWith(carrier))
            {
                service = service.Substring(carrier.Length + 1);
            }

            // Bigcommerce doesn't like it when you set shipping_method to an empty string
            if (carrier.IsNullOrWhiteSpace() && service.IsNullOrWhiteSpace())
            {
                return "No shipping method was entered";
            }

            return $"{carrier} ({service})";
        }

        /// <summary>
        /// Gets a list of BigCommerceItems for the order's items
        /// </summary>
        /// <param name="orderEntity">The order from which to populate the list</param>
        /// <param name="orderProducts">List of BigCommerceProducts.  If populated, will be used for creating the list of BigCommerceItems.  Otherwise, order.OrderItems will be used.</param>
        /// <returns>List of BigCommerceItems for the order's items</returns>
        private static List<BigCommerceItem> GetOrderItems(OrderEntity orderEntity, List<BigCommerceProduct> orderProducts)
        {
            List<BigCommerceItem> bigCommerceOrderItems = new List<BigCommerceItem>();

            // To support legacy order items that wouldn't have order address id or order product id, we may need to 
            // use the list of order products received from BigCommerce.  If the passed orderProducts is null, we use
            // order.OrderItems, otherwise we use orderProducts.  This assumes the check has already been done and the
            // appropriate orderProducts has been passed.
            if (orderProducts == null)
            {
                foreach (OrderItemEntity orderItem in orderEntity.OrderItems)
                {
                    // Check to make sure the order item is a BigCommerceOrderItemEntity
                    // If they added an item manually through ShipWorks, it will not be.
                    if (orderItem is BigCommerceOrderItemEntity)
                    {
                        BigCommerceOrderItemEntity bigCommerceOrderItem = (BigCommerceOrderItemEntity) orderItem;

                        // Create a list of BigCommerceItems to send to BigCommerce for this shipment
                        BigCommerceItem bigCommerceItem = new BigCommerceItem
                            {
                                order_product_id = (int) bigCommerceOrderItem.OrderProductID,
                                quantity = (int) bigCommerceOrderItem.Quantity
                            };

                        bigCommerceOrderItems.Add(bigCommerceItem);
                    }
                }
            }
            else
            {
                foreach (BigCommerceProduct orderProduct in orderProducts)
                {
                    // Create a list of BigCommerceItems to send to BigCommerce for this shipment
                    BigCommerceItem bigCommerceItem = new BigCommerceItem
                    {
                        order_product_id = orderProduct.id,
                        quantity = orderProduct.quantity
                    };

                    bigCommerceOrderItems.Add(bigCommerceItem);
                }
            }

            return bigCommerceOrderItems;
        }

        /// <summary>
        /// Determines if an orders' items are all digital
        /// </summary>
        /// <param name="order">The order to check</param>
        /// <returns>True if all items are digital, otherwise false</returns>
        private static bool IsOrderAllDigital(OrderEntity order)
        {
            IEnumerable<OrderItemEntity> items = null;

            if (!order.OrderItems.Any())
            {
                items = DataProvider.GetRelatedEntities(order.OrderID, EntityType.OrderItemEntity).Cast<OrderItemEntity>();
            }
            else
            {
                items = order.OrderItems;
            }

            return items.OfType<BigCommerceOrderItemEntity>().All(oi => oi.IsDigitalItem);
        }
    }
}
