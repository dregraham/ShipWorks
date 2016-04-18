using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    ///     Uploads shipment information to LemonStand
    /// </summary>
    public class LemonStandOnlineUpdater
    {
        // the store this instance is for
        private readonly ILemonStandWebClient client;
        // Logger
        private readonly ILog log;
        private readonly LemonStandStoreEntity store;
        private LemonStandStatusCodeProvider statusCodeProvider;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LemonStandOnlineUpdater(LemonStandStoreEntity store)
            : this(LogManager.GetLogger(typeof (LemonStandOnlineUpdater)), new LemonStandWebClient(store))
        {
            this.store = store;
        }

        public LemonStandOnlineUpdater(ILog log, ILemonStandWebClient client)
        {
            this.log = log;
            this.client = client;
        }

        /// <summary>
        /// Gets the status code provider
        /// </summary>
        protected LemonStandStatusCodeProvider StatusCodeProvider
        {
            get
            {
                if (statusCodeProvider == null)
                {
                    statusCodeProvider = new LemonStandStatusCodeProvider(store);
                }

                return statusCodeProvider;
            }
        }

        /// <summary>
        /// Changes the status of an LemonStand order to that specified
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
        /// Changes the status of an LemonStand order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = DataProvider.GetEntity(orderID) as OrderEntity;

            if (order != null)
            {
                if (order.IsManual)
                {
                    log.Warn($"Not uploading order status since order {orderID} is manual");
                    return;
                }

                LemonStandOrderEntity lemonStandOrder = (LemonStandOrderEntity)order;

                client.UpdateOrderStatus(lemonStandOrder.LemonStandOrderID, StatusCodeProvider.GetCodeName(statusCode));

                // Update the local database with the new status
                OrderEntity basePrototype = new OrderEntity(orderID)
                {
                    IsNew = false,
                    OnlineStatusCode = statusCode,
                    OnlineStatus = StatusCodeProvider.GetCodeName(statusCode)
                };

                unitOfWork.AddForSave(basePrototype);
            }
            else
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
            }
        }

        /// <summary>
        ///     Push the shipment details to the store.
        /// </summary>
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

                if (!shipment.Order.IsManual)
                {
                    string shipmentID = GetShipmentID(shipment);

                    client.UploadShipmentDetails(shipment.TrackingNumber, shipmentID);
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
                throw new ArgumentNullException(nameof(shipment));
            }

            if (!shipment.Order.IsManual)
            {
                string shipmentID = GetShipmentID(shipment);

                client.UploadShipmentDetails(shipment.TrackingNumber, shipmentID);
            }
        }

        /// <summary>
        ///     Gets the LemonStand shipment ID as it is required to upload tracking information
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>LemonStand API Shipment ID</returns>
        private string GetShipmentID(ShipmentEntity shipmentEntity)
        {
            LemonStandOrderEntity order = (LemonStandOrderEntity) shipmentEntity.Order;

            string orderID = order.OrderNumber.ToString();
            // use order id to get invoice
            JToken invoice = client.GetOrderInvoice(orderID);
            string invoiceID = invoice.SelectToken("data.invoices.data").Children().First().SelectToken("id").ToString();

            // use invoice id to get shipment
            JToken shipment = client.GetShipment(invoiceID);
            string shipmentID =
                shipment.SelectToken("data.shipments.data").Children().First().SelectToken("id").ToString();

            return shipmentID;
        }
    }
}