using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            ThreeDCartOrderEntity order = (ThreeDCartOrderEntity) DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (order.IsManual)
                {
                    log.InfoFormat($"Not uploading order status since order {order.OrderID} is manual.");
                    return;
                }
                string status = EnumHelper.GetDescription((Enums.ThreeDCartOrderStatus) statusID);

                order.OnlineStatus = status;
                order.OnlineStatusCode = statusID;

                // Fetch the order items
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
                }

                ThreeDCartOrderItemEntity item = order.OrderItems?.FirstOrDefault() as ThreeDCartOrderItemEntity;

                if (item == null)
                {
                    throw new ThreeDCartException("No items were found on the order. ShipWorks cannot upload order status information without items from 3D Cart.");
                }

                ThreeDCartShipment shipment = new ThreeDCartShipment()
                {
                    OrderID = order.ThreeDCartOrderID,
                    ShipmentID = item.ThreeDCartShipmentID,
                    ShipmentOrderStatus = (int)EnumHelper.GetEnumByApiValue<Enums.ThreeDCartOrderStatus>(order.OnlineStatus),
                    ShipmentPhone = order.ShipPhone,
                    ShipmentFirstName = order.ShipFirstName,
                    ShipmentLastName = order.ShipLastName,
                    ShipmentAddress = order.ShipStreet1,
                    ShipmentAddress2 = order.ShipStreet2,
                    ShipmentCity = order.ShipCity,
                    ShipmentState = order.ShipStateProvCode,
                    ShipmentZipCode = order.ShipPostalCode,
                    ShipmentCountry = order.ShipCountryCode,
                    ShipmentCompany = order.ShipCompany,
                    ShipmentEmail = order.ShipEmail,
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
                log.WarnFormat($"Unable to update online status for order {orderID}: cannot find order");
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
        /// Push the shipment details to the store.
        /// </summary>
        private void UpdateShipmentDetails(ShipmentEntity shipmentEntity)
        {
            ThreeDCartOrderEntity order = (ThreeDCartOrderEntity) shipmentEntity.Order;
            if (order.IsManual)
            {
                log.WarnFormat("Not updating order {0} since it is manual.", shipmentEntity.Order.OrderNumberComplete);
                return;
            }

            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            ThreeDCartOrderItemEntity threeDCartOrderItem = order.OrderItems?.FirstOrDefault(oi => oi is ThreeDCartOrderItemEntity) as ThreeDCartOrderItemEntity;

            // Get the 3D Cart shipment id from the first 3D Cart order item
            if (threeDCartOrderItem == null)
            {
                throw new ThreeDCartException("No items were found on the order. ShipWorks cannot upload tracking information without items from 3D Cart.");
            }

            ThreeDCartShipment shipment = new ThreeDCartShipment()
            {
                OrderID = order.ThreeDCartOrderID,
                ShipmentID = threeDCartOrderItem.ThreeDCartShipmentID,
                ShipmentOrderStatus = (int) EnumHelper.GetEnumByApiValue<Enums.ThreeDCartOrderStatus>(order.OnlineStatus),
                ShipmentMethodName = GetShipmentMethodName(shipmentEntity),
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
        private string GetShipmentMethodName(ShipmentEntity shipmentEntity)
        {
            ShipmentTypeCode typeCode = ((ShipmentTypeCode) shipmentEntity.ShipmentType);

            List<string> methods = new List<string>
            {
                CheckForUsps(shipmentEntity, typeCode),
                CheckForFedex(shipmentEntity, typeCode),
                CheckForUps(shipmentEntity, typeCode),
                CheckForOthers(shipmentEntity, typeCode)
            };

            foreach (string method in methods.Where(method => !string.IsNullOrWhiteSpace(method)))
            {
                return method;
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks for usps shipping method
        /// </summary>
        private string CheckForUsps(ShipmentEntity shipmentEntity, ShipmentTypeCode typeCode)
        {
            if (typeCode != ShipmentTypeCode.Express1Endicia &&
                typeCode != ShipmentTypeCode.Express1Usps &&
                typeCode != ShipmentTypeCode.PostalWebTools &&
                typeCode != ShipmentTypeCode.Usps &&
                typeCode != ShipmentTypeCode.Endicia)
            {
                return string.Empty;
            }

            if (shipmentEntity.Postal != null && ShipmentTypeManager.IsDhl((PostalServiceType)shipmentEntity.Postal.Service))
            {
                return $"DHL - {shipmentEntity.Postal.Service}";
            }

            if (shipmentEntity.Postal != null && ShipmentTypeManager.IsConsolidator((PostalServiceType)shipmentEntity.Postal.Service))
            {
                return $"Consolidator - {shipmentEntity.Postal.Service}";
            }

            return $"USPS - {shipmentEntity.Postal?.Service}";
        }

        /// <summary>
        /// Checks for fedex shipping method
        /// </summary>
        private string CheckForFedex(ShipmentEntity shipmentEntity, ShipmentTypeCode typeCode)
        {
            return typeCode == ShipmentTypeCode.FedEx ? $"FEDEX - {shipmentEntity.FedEx.Service}" : string.Empty;
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
            // Adjust tracking details per Mail Innovations and others
            if (UpsUtility.IsUpsMiService((UpsServiceType)shipmentEntity.Ups.Service))
            {
                if (shipmentEntity.Ups.UspsTrackingNumber.Length > 0)
                {
                    return $"UPS MI - {shipmentEntity.Ups.Service}";
                }
            }

            return $"UPS - {shipmentEntity.Ups.Service}";
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
                case ShipmentTypeCode.OnTrac:
                    return $"OnTrac - {shipmentEntity.OnTrac.Service}";
                case ShipmentTypeCode.iParcel:
                    return $"iParcel - {shipmentEntity.IParcel.Service}";
                case ShipmentTypeCode.Other:
                    return $"{shipmentEntity.Other.Carrier} - {shipmentEntity.Other.Service}";
                default:
                    return string.Empty;
            }
        }
    }
}