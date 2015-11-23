﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    public class YahooApiOnlineUpdater
    {
        private readonly ILog log;
        private readonly YahooApiWebClient client;
        private readonly YahooStoreEntity store;

        public YahooApiOnlineUpdater(YahooStoreEntity store) : this(LogManager.GetLogger(typeof (YahooApiOnlineUpdater)), new YahooApiWebClient(store))
        {
            this.store = store;
        }

        public YahooApiOnlineUpdater(ILog log, YahooApiWebClient client)
        {
            this.log = log;
            this.client = client;
        }

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, string status)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UpdateOrderStatus(orderID, status, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, string status, UnitOfWork2 unitOfWork)
        {
            YahooOrderEntity order = (YahooOrderEntity)DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (order.IsManual)
                {
                    return;
                }

                client.UploadOrderStatus(order.YahooOrderID, status);

                // Update the local database with the new status
                OrderEntity basePrototype = new OrderEntity(orderID)
                {
                    IsNew = false,
                    OnlineStatusCode = status,
                    OnlineStatus = status
                };

                unitOfWork.AddForSave(basePrototype);
            }
            else
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
            }
        }




        public void UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            foreach (long orderKey in orderKeys)
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);

                // Check to see if shipment exists
                if (shipment == null)
                {
                    log.InfoFormat("Not uploading orderid {0} has no items.", orderKey);
                    continue;
                }

                YahooOrderEntity order = (YahooOrderEntity)shipment.Order;
                if (!order.IsManual)
                {
                    client.UploadShipmentDetails(order.OrderNumber.ToString(), shipment.TrackingNumber, GetCarrierCode(shipment),
                        order.OnlineStatus);
                }
            }
        }

        /// <summary>
        ///     Push the online status for an shipment.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            YahooOrderEntity order = (YahooOrderEntity)shipment.Order;

            if (!order.IsManual)
            {
                client.UploadShipmentDetails(order.OrderNumber.ToString(), shipment.TrackingNumber, GetCarrierCode(shipment),
                        order.OnlineStatus);
            }
        }

        private string GetCarrierCode(ShipmentEntity shipmentEntity)
        {
            string carrierCode;

            switch (((ShipmentTypeCode)shipmentEntity.ShipmentType))
            {
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    carrierCode = "usps";
                    break;

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    // The shipment is an Endicia shipment, check to see if it's DHL
                    if (shipmentEntity.Postal != null && ShipmentTypeManager.IsDhl((PostalServiceType)shipmentEntity.Postal.Service))
                    {
                        // The DHL carrier for Endicia is:
                        carrierCode = "dhl";
                    }
                    else if (shipmentEntity.Postal != null && ShipmentTypeManager.IsConsolidator((PostalServiceType)shipmentEntity.Postal.Service))
                    {
                        carrierCode = "usps";
                    }
                    else
                    {
                        // Use the default carrier for other Endicia types
                        carrierCode = "usps";
                    }
                    break;

                case ShipmentTypeCode.FedEx:
                    carrierCode = "fedex";
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    ShippingManager.EnsureShipmentLoaded(shipmentEntity);
                    //The shipment is a UPS shipment, check to see if it is UPS-MI
                    carrierCode = "ups";
                    break;
                    
                default:
                    Debug.Fail("Unhandled ShipmentTypeCode in Groupon.GetCarrierCode");
                    carrierCode = "Other";
                    break;
            }

            return carrierCode;
        }
    }
}

