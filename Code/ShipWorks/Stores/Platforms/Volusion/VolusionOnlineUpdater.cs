using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Handles uploading shipment details to Volusion
    /// </summary>
    public class VolusionOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(VolusionOnlineUpdater));

        // store for which this updater is to operate
        private readonly VolusionStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionOnlineUpdater(VolusionStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment, bool sendEmail)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UploadShipmentDetails(shipment, sendEmail, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment, bool sendEmail, UnitOfWork2 unitOfWork)
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
                VolusionWebClient client = new VolusionWebClient(store);

                // upload the details
                try
                {
                    client.UploadShipmentDetails(shipment, sendEmail);
                }
                catch (VolusionException ex)
                {
                    // re-submitting the same tracking number causes a primary key constraint error in Volusion so we ignore it
                    if (!ex.Message.Contains("PRIMARY KEY constraint"))
                    {
                        throw;
                    }
                }

                // clear out the volusion online status locally, so that it can fall out of any local by-status filters
                order.OnlineStatus = "";

                unitOfWork.AddForSave(order);
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }
    }
}
