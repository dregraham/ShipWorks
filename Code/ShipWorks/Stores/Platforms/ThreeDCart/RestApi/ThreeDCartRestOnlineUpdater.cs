using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public class ThreeDCartRestOnlineUpdater
    {
        private readonly ILog log;
        private readonly IThreeDCartRestWebClient webClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeDCartRestOnlineUpdater"/> class.
        /// </summary>
        public ThreeDCartRestOnlineUpdater(ThreeDCartStoreEntity store)
            : this(LogManager.GetLogger(typeof (ThreeDCartRestOnlineUpdater)), new ThreeDCartRestWebClient(store))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeDCartRestOnlineUpdater"/> class.
        /// </summary>
        public ThreeDCartRestOnlineUpdater(ILog log, IThreeDCartRestWebClient webClient)
        {
            this.log = log;
            this.webClient = webClient;
        }

        /// <summary>
        /// Changes the status of an order
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusID)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UpdateOrderStatus(orderID, statusID, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an order
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusID, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);

            if (order != null)
            {
                if (order.IsManual)
                {
                    log.InfoFormat($"Not uploading order status since order {order.OrderNumberComplete} is manual.");
                    return;
                }

                ThreeDCartOrderEntity threeDCartOrder = order as ThreeDCartOrderEntity;

                // Downloaded using the SOAP API
                if (threeDCartOrder == null || threeDCartOrder.ThreeDCartOrderID == -1)
                {
                    log.WarnFormat($"Unable to update online status for order {order.OrderNumberComplete}: cannot find order." +
                                   "This is most likely because the order was downloaded using the SOAP API and it is trying to" +
                                   "update using the REST API.");

                    throw new ThreeDCartException("3dcart orders downloaded using their SOAP API can not be updated online through ShipWorks after " +
                                                  "your store has been upgraded to use their REST API.");
                }

                string status = EnumHelper.GetDescription((Enums.ThreeDCartOrderStatus) statusID);

                threeDCartOrder.OnlineStatus = status;
                threeDCartOrder.OnlineStatusCode = statusID;

                // Fetch the order items
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(threeDCartOrder.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == threeDCartOrder.OrderID));
                }

                ThreeDCartOrderItemEntity item = threeDCartOrder.OrderItems?.FirstOrDefault() as ThreeDCartOrderItemEntity;

                if (item == null)
                {
                    throw new ThreeDCartException("No items were found on the order. ShipWorks cannot upload order status information without items from 3dcart.");
                }

                ThreeDCartShipment shipment = new ThreeDCartShipment()
                {
                    OrderID = threeDCartOrder.ThreeDCartOrderID,
                    ShipmentID = item.ThreeDCartShipmentID,
                    ShipmentOrderStatus = (int) EnumHelper.GetEnumByApiValue<Enums.ThreeDCartOrderStatus>(threeDCartOrder.OnlineStatus),
                    ShipmentPhone = threeDCartOrder.ShipPhone,
                    ShipmentFirstName = threeDCartOrder.ShipFirstName,
                    ShipmentLastName = threeDCartOrder.ShipLastName,
                    ShipmentAddress = threeDCartOrder.ShipStreet1,
                    ShipmentAddress2 = threeDCartOrder.ShipStreet2,
                    ShipmentCity = threeDCartOrder.ShipCity,
                    ShipmentState = threeDCartOrder.ShipStateProvCode,
                    ShipmentZipCode = threeDCartOrder.ShipPostalCode,
                    ShipmentCountry = threeDCartOrder.ShipCountryCode,
                    ShipmentCompany = threeDCartOrder.ShipCompany,
                    ShipmentEmail = threeDCartOrder.ShipEmail,
                };

                webClient.UpdateOrderStatus(shipment);

                // Update the local database with the new status
                OrderEntity basePrototype = new OrderEntity(orderID)
                {
                    IsNew = false,
                    OnlineStatusCode = statusID,
                    OnlineStatus = status
                };

                unitOfWork.AddForSave(basePrototype);
            }
            else
            {
                log.WarnFormat($"Unable to update online status for order {orderID}: Unable to find order");
            }
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(OrderEntity order)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
            if (shipment == null)
            {
                // log that there was no shipment, and return
                log.DebugFormat($"There was no shipment found for order Id: {order.OrderID}");
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
                log.WarnFormat($"Not updating status of shipment {shipmentID} as it has gone away.");
                return;
            }

            UpdateShipmentDetails(shipment);
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        private void UpdateShipmentDetails(ShipmentEntity shipmentEntity)
        {
            ShippingManager.EnsureShipmentLoaded(shipmentEntity);
            OrderEntity order = shipmentEntity.Order;
            if (order.IsManual)
            {
                log.WarnFormat($"Not updating order {shipmentEntity.Order.OrderNumberComplete} since it is manual.");
                return;
            }

            ThreeDCartOrderEntity threeDCartOrder = order as ThreeDCartOrderEntity;

            // Downloaded using the SOAP API
            if (threeDCartOrder == null || threeDCartOrder.ThreeDCartOrderID == -1)
            {
                log.WarnFormat($"Unable to update online status for order {order.OrderNumberComplete}: cannot find order." +
                                   "This is most likely because the order was downloaded using the SOAP API and it is trying to" +
                                   "update using the REST API.");

                throw new ThreeDCartException("3dcart orders downloaded using their SOAP API can not be updated online through ShipWorks after " +
                                                  "your store has been upgraded to use their REST API.");
            }

            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(threeDCartOrder.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == threeDCartOrder.OrderID));
            }

            ThreeDCartOrderItemEntity threeDCartOrderItem = threeDCartOrder.OrderItems?.FirstOrDefault(oi => oi is ThreeDCartOrderItemEntity) as ThreeDCartOrderItemEntity;

            // Get the 3dcart shipment id from the first 3dcart order item
            if (threeDCartOrderItem == null)
            {
                throw new ThreeDCartException("No items were found on the order. ShipWorks cannot upload tracking information without items from 3dcart.");
            }

            ThreeDCartShipment shipment = new ThreeDCartShipment
            {
                OrderID = threeDCartOrder.ThreeDCartOrderID,
                ShipmentID = threeDCartOrderItem.ThreeDCartShipmentID,
                ShipmentOrderStatus =
                    (int) EnumHelper.GetEnumByApiValue<Enums.ThreeDCartOrderStatus>(threeDCartOrder.OnlineStatus),
                ShipmentMethodName = GetShipmentMethod(shipmentEntity),
                ShipmentPhone = shipmentEntity.ShipPhone,
                ShipmentFirstName = shipmentEntity.ShipFirstName,
                ShipmentLastName = shipmentEntity.ShipLastName,
                ShipmentAddress = shipmentEntity.ShipStreet1,
                ShipmentAddress2 = shipmentEntity.ShipStreet2,
                ShipmentCity = shipmentEntity.ShipCity,
                ShipmentState = shipmentEntity.ShipStateProvCode,
                ShipmentZipCode = shipmentEntity.ShipPostalCode,
                ShipmentCountry = shipmentEntity.ShipCountryCode,
                ShipmentCompany = shipmentEntity.ShipCompany,
                ShipmentEmail = shipmentEntity.ShipEmail,
                ShipmentLastUpdate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                ShipmentShippedDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                ShipmentTrackingCode = shipmentEntity.TrackingNumber,
                ShipmentWeight = shipmentEntity.TotalWeight
            };

            webClient.UploadShipmentDetails(shipment);
        }

        /// <summary>
        /// Gets the name of the shipment method.
        /// </summary>
        public string GetShipmentMethod(ShipmentEntity shipmentEntity)
        {
            ShipmentTypeCode typeCode = ((ShipmentTypeCode) shipmentEntity.ShipmentType);

            List<string> methods = new List<string>
            {
                CheckForUsps(shipmentEntity, typeCode),
                CheckForUps(shipmentEntity, typeCode),
                CheckForOthers(shipmentEntity, typeCode)
            };

            return methods.FirstOrDefault(m => !string.IsNullOrWhiteSpace(m)) ?? string.Empty;
        }

        /// <summary>
        /// Checks for usps shipping method
        /// </summary>
        private string CheckForUsps(ShipmentEntity shipmentEntity, ShipmentTypeCode typeCode)
        {
            if (!PostalUtility.IsPostalShipmentType(typeCode))
            {
                return string.Empty;
            }

            PostalServiceType service = (PostalServiceType) shipmentEntity.Postal.Service;

            if (shipmentEntity.Postal != null && ShipmentTypeManager.IsDhl(service))
            {
                // Don't prefix with DHL since it is in the service name
                return $"{EnumHelper.GetDescription(service)}";
            }

            return $"USPS - {EnumHelper.GetDescription(service)}";
        }

        /// <summary>
        /// Checks for ups shipping method
        /// </summary>
        private string CheckForUps(ShipmentEntity shipmentEntity, ShipmentTypeCode typeCode)
        {
            if (typeCode != ShipmentTypeCode.UpsOnLineTools && typeCode != ShipmentTypeCode.UpsWorldShip)
            {
                return string.Empty;
            }

            UpsServiceType service = (UpsServiceType) shipmentEntity.Ups.Service;

            // Adjust tracking details per Mail Innovations and others
            if (UpsUtility.IsUpsMiService(service))
            {
                if (shipmentEntity.Ups.UspsTrackingNumber.Length > 0)
                {
                    return $"UPS MI - {EnumHelper.GetDescription(service)}";
                }
            }

            // Don't prefix with UPS since it is in the service name
            return $"{EnumHelper.GetDescription(service)}";
        }

        /// <summary>
        /// Checks for other shipping methods
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="typeCode">The type code.</param>
        /// <returns></returns>
        private string CheckForOthers(ShipmentEntity shipmentEntity, ShipmentTypeCode typeCode)
        {
            switch (typeCode)
            {
                // Don't prefix with FedEx since it is in the service name
                case ShipmentTypeCode.FedEx:
                    return $"{EnumHelper.GetDescription((FedExServiceType) shipmentEntity.FedEx.Service)}";
                case ShipmentTypeCode.OnTrac:
                    return $"OnTrac - {EnumHelper.GetDescription((OnTracServiceType) shipmentEntity.OnTrac.Service)}";
                case ShipmentTypeCode.iParcel:
                    return $"iParcel - {EnumHelper.GetDescription((iParcelServiceType) shipmentEntity.IParcel.Service)}";
                case ShipmentTypeCode.Other:
                    return $"{shipmentEntity.Other.Carrier} - {shipmentEntity.Other.Service}";
                default:
                    return string.Empty;
            }
        }
    }
}