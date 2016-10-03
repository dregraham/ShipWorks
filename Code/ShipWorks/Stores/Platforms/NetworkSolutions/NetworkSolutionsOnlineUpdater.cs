using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Handles uploading data to NetworkSolutions
    /// </summary>
    public class NetworkSolutionsOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(NetworkSolutionsOnlineUpdater));

        // store for which this updater is to operate
        private readonly NetworkSolutionsStoreEntity store;

        // status code provider
        private NetworkSolutionsStatusCodeProvider statusCodeProvider;

        /// <summary>
        /// Gets the status code provider
        /// </summary>
        protected NetworkSolutionsStatusCodeProvider StatusCodes =>
            statusCodeProvider ?? (statusCodeProvider = new NetworkSolutionsStatusCodeProvider(store));

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsOnlineUpdater(NetworkSolutionsStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Changes the status of an NetworkSolutions order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, long statusCode, string comments)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UpdateOrderStatus(orderID, statusCode, comments, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an NetworkSolutions order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, long statusCode, string comments, UnitOfWork2 unitOfWork)
        {
            NetworkSolutionsOrderEntity order = DataProvider.GetEntity(orderID) as NetworkSolutionsOrderEntity;
            if (order != null)
            {
                if (!order.IsManual)
                {
                    string processedComment = TemplateTokenProcessor.ProcessTokens(comments, orderID);

                    NetworkSolutionsWebClient client = new NetworkSolutionsWebClient(store);
                    client.UpdateOrderStatus(order.NetworkSolutionsOrderID, (long) order.OnlineStatusCode, statusCode, processedComment);

                    // Update the local database with the new status
                    OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
                    basePrototype.OnlineStatusCode = statusCode;
                    basePrototype.OnlineStatus = StatusCodes.GetCodeName(statusCode);

                    unitOfWork.AddForSave(basePrototype);
                }
                else
                {
                    log.WarnFormat("Not uploading order status since order {0} is manual.", order.OrderID);
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
                NetworkSolutionsWebClient client = new NetworkSolutionsWebClient(store);

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
