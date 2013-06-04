using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;
using System.Data;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Handles uploading tracking information and status code updates back to the online store.
    /// </summary>
    public class GenericStoreOnlineUpdater
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GenericStoreOnlineUpdater));

        // the store this instances is working for
        GenericModuleStoreEntity store;

        // the status codes the store supports
        GenericStoreStatusCodeProvider statusCodeProvider;

        // storetype reference
        GenericModuleStoreType genericStoreType;

        /// <summary>
        /// The store this instance is executing for
        /// </summary>
        protected GenericModuleStoreEntity Store
        {
            get { return store; }
        }

        /// <summary>
        /// Status Codes for the store
        /// </summary>
        protected GenericStoreStatusCodeProvider StatusCodes
        {
            get
            {
                if (statusCodeProvider == null)
                {
                    statusCodeProvider = genericStoreType.CreateStatusCodeProvider();
                }

                return statusCodeProvider;
            }
        }

        /// <summary>
        /// StoreType reference
        /// </summary>
        protected GenericModuleStoreType GenericStoreType
        {
            get { return genericStoreType; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreOnlineUpdater(GenericModuleStoreEntity store)
        {
            this.store = store;

            // get the implementing storetype
            genericStoreType = (GenericModuleStoreType)StoreTypeManager.GetType(store);
        }

        /// <summary>
        /// Posts the tracking number for the identified shipment to the store
        /// </summary>
        public void UploadTrackingNumber(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            UploadTrackingNumber(shipment);
        }

        /// <summary>
        /// Posts the tracking number for the identified shipment to the store
        /// </summary>
        public void UploadTrackingNumber(ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;

            if (!order.IsManual)
            {
                // Upload tracking number
                GenericStoreWebClient webClient = genericStoreType.CreateWebClient();
                webClient.UploadShipmentDetails(order, shipment);
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }

        /// <summary>
        /// Update the online status of the order with id orderID.
        /// </summary>
        public void UpdateOrderStatus(long orderID, object code, string comment)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UpdateOrderStatus(orderID, code, comment, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Update the online status of the order with id orderID.
        /// </summary>
        public void UpdateOrderStatus(long orderID, object code, string comment, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (!order.IsManual)
                {
                    string processedComment = (comment == null) ? "" : TemplateTokenProcessor.ProcessTokens(comment, orderID);

                    GenericStoreWebClient webClient = genericStoreType.CreateWebClient();
                    webClient.UpdateOrderStatus(order, code, processedComment);

                    // Update the database to match, status code display
                    OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
                    basePrototype.OnlineStatusCode = code;
                    basePrototype.OnlineStatus = StatusCodes.GetCodeName(code);

                    unitOfWork.AddForSave(basePrototype);
                }
                else
                {
                    log.InfoFormat("Not uploading order status since order {0} is manual.", order.OrderID);
                }
            }
            else
            {
                // log it and continue
                log.WarnFormat("Unable to update online status for order {0}: cannot find online identifier.", orderID);
            }
        }
    }
}
