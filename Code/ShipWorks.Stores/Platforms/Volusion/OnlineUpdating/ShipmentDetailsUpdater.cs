using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Stores.Platforms.Volusion.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Handles uploading shipment details to Volusion
    /// </summary>
    [Component]
    public class ShipmentDetailsUpdater : IShipmentDetailsUpdater
    {
        private readonly IVolusionWebClient webClient;
        private readonly ICombineOrderNumberSearchProvider orderNumberProvider;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDetailsUpdater(IVolusionWebClient webClient, ICombineOrderNumberSearchProvider orderNumberProvider, Func<Type, ILog> createLogger)
        {
            this.webClient = webClient;
            this.orderNumberProvider = orderNumberProvider;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        public async Task UploadShipmentDetails(IVolusionStoreEntity store, ShipmentEntity shipment, bool sendEmail)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UploadShipmentDetails(store, shipment, sendEmail, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        public async Task UploadShipmentDetails(IVolusionStoreEntity store, ShipmentEntity shipment, bool sendEmail, UnitOfWork2 unitOfWork)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;
            if (order.IsManual)
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
                return;
            }

            var orderNumbers = await orderNumberProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            foreach (var orderNumber in orderNumbers)
            {
                try
                {
                    webClient.UploadShipmentDetails(store, orderNumber, shipment, sendEmail);
                }
                catch (VolusionException ex)
                {
                    // re-submitting the same tracking number causes a primary key constraint error in Volusion so we ignore it
                    if (!ex.Message.Contains("PRIMARY KEY constraint"))
                    {
                        throw;
                    }
                }
            }

            // clear out the Volusion online status locally, so that it can fall out of any local by-status filters
            order.OnlineStatus = "";

            unitOfWork.AddForSave(order);
        }
    }
}
