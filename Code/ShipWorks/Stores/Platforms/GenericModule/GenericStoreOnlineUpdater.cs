using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
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

        // the status codes the store supports
        private GenericStoreStatusCodeProvider statusCodeProvider;

        /// <summary>
        /// The store this instance is executing for
        /// </summary>
        protected GenericModuleStoreEntity Store { get; }

        /// <summary>
        /// Status Codes for the store
        /// </summary>
        protected GenericStoreStatusCodeProvider StatusCodes =>
            statusCodeProvider ?? (statusCodeProvider = GenericStoreType.CreateStatusCodeProvider());

        /// <summary>
        /// StoreType reference
        /// </summary>
        protected GenericModuleStoreType GenericStoreType { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreOnlineUpdater(GenericModuleStoreEntity store)
        {
            this.Store = store;

            // get the implementing storetype
            GenericStoreType = (GenericModuleStoreType) StoreTypeManager.GetType(store);
        }

        /// <summary>
        /// Posts the tracking number for the identified shipment to the store
        /// </summary>
        public async Task UploadTrackingNumber(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            await UploadTrackingNumber(shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Posts the tracking number for the identified shipment to the store
        /// </summary>
        public async Task UploadTrackingNumber(ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order ?? (OrderEntity) DataProvider.GetEntity(shipment.OrderID);

            if (!order.IsManual)
            {
                // Upload tracking number
                GenericStoreWebClient webClient = GenericStoreType.CreateWebClient();
                await webClient.UploadShipmentDetails(order, shipment).ConfigureAwait(false);
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }

        /// <summary>
        /// Update the online status of the order with id orderID.
        /// </summary>
        public async Task UpdateOrderStatus(long orderID, object code, string comment)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UpdateOrderStatus(orderID, code, comment, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                await unitOfWork.CommitAsync(adapter).ConfigureAwait(false);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Update the online status of the order with id orderID.
        /// </summary>
        public async Task UpdateOrderStatus(long orderID, object code, string comment, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);
            if (order == null)
            {
                // log it and continue
                log.WarnFormat("Unable to update online status for order {0}: cannot find online identifier.", orderID);
                return;
            }

            if (order.IsManual && order.CombineSplitStatus != CombineSplitStatusType.Combined)
            {
                log.InfoFormat("Not uploading order status since order {0} is manual.", order.OrderID);
                return;
            }

            string processedComment = (comment == null) ? "" : TemplateTokenProcessor.ProcessTokens(comment, orderID);

            GenericStoreWebClient webClient = GenericStoreType.CreateWebClient();
            await webClient.UpdateOrderStatus(order, code, processedComment).ConfigureAwait(false);

            // Update the database to match, status code display
            OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
            basePrototype.OnlineStatusCode = code;
            basePrototype.OnlineStatus = StatusCodes.GetCodeName(code);

            unitOfWork.AddForSave(basePrototype);
        }
    }
}
