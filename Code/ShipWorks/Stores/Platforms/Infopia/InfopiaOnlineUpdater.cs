using System;
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

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Handles uploading data to Infopia
    /// </summary>
    public class InfopiaOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(InfopiaOnlineUpdater));

        // store for which this updater is to operate
        private readonly InfopiaStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaOnlineUpdater(InfopiaStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Posts the tracking number for hte identified shipment to the infopia store
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
        /// Posts the tracking number for hte identified shipment to the infopia store
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
                InfopiaWebClient client = new InfopiaWebClient(store);

                string trackingNumber = "";
                string shipper = "";
                decimal charge = shipment.ShipmentCost;

                InfopiaUtility.GetShipmentUploadValues(shipment, out shipper, out trackingNumber);

                // upload the details
                client.UploadShipmentDetails(order.OrderNumber, shipper, trackingNumber, charge);
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }

        /// <summary>
        /// Changes the status of an Infopia order to that specified.
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
        /// Changes the status of an Infopia order to that specified.
        /// </summary>
        public void UpdateOrderStatus(long orderID, string status, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (!order.IsManual)
                {
                    InfopiaWebClient client = new InfopiaWebClient(store);
                    client.UpdateOrderStatus(order.OrderNumber, status);

                    // Update the local database with the new status
                    OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
                    basePrototype.OnlineStatus = status;

                    unitOfWork.AddForSave(basePrototype);
                }
                else
                {
                    log.WarnFormat("Not uploading shipment details since order {0} is manual.", order.OrderID);
                }
            }
            else
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
            }
        }
    }
}
