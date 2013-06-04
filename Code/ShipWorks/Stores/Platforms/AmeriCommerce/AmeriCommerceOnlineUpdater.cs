﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Handles uploading data to AmeriCommerce
    /// </summary>
    public class AmeriCommerceOnlineUpdater
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(AmeriCommerceOnlineUpdater));
						  
        // store for which this updater is to operate
        AmeriCommerceStoreEntity store;

        // status code provider
        AmeriCommerceStatusCodeProvider statusCodeProvider;

        /// <summary>
        /// Gets the status code provider
        /// </summary>
        protected AmeriCommerceStatusCodeProvider StatusCodes
        {
            get
            {
                if (statusCodeProvider == null)
                {
                    statusCodeProvider = new AmeriCommerceStatusCodeProvider(store);
                }

                return statusCodeProvider;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceOnlineUpdater(AmeriCommerceStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Changes the status of an AmeriCommerce order to that specified
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
        /// Changes the status of an AmeriCommerce order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (!order.IsManual)
                {
                    AmeriCommerceWebClient client = new AmeriCommerceWebClient(store);
                    client.UpdateOrderStatus(Convert.ToInt32(order.OrderNumber), statusCode);

                    // Update the local database with the new status
                    OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
                    basePrototype.OnlineStatusCode = statusCode;
                    basePrototype.OnlineStatus = StatusCodes.GetCodeName(statusCode);

                    unitOfWork.AddForSave(basePrototype);
                }
                else
                {
                    log.InfoFormat("Not uploading order status since order {0} is manual.", order.OrderID);
                }
            }
            else
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
            }
        }

        /// <summary>
        /// Uploads shipment details for the given shipment Id
        /// </summary>
        public void UploadShipmentDetails(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipoment {0}, shipment was deleted.", shipmentID);
                return;
            }

            UploadShipmentDetails(shipment);
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;
            if (!order.IsManual)
            {
                // Upload tracking number 
                AmeriCommerceWebClient client = new AmeriCommerceWebClient(store);

                // upload the details
                client.UploadShipmentDetails(shipment);
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }
    }
}
