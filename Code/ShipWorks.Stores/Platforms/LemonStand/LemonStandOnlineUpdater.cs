using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
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
            : this(LogManager.GetLogger(typeof(LemonStandOnlineUpdater)), new LemonStandWebClient(store))
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
        public async Task UpdateOrderStatus(long orderID, int statusCode)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UpdateOrderStatus(orderID, statusCode, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an LemonStand order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = DataProvider.GetEntity(orderID) as OrderEntity;

            if (order != null)
            {
                if (order.IsManual && order.CombineSplitStatus == CombineSplitStatusType.None)
                {
                    return;
                }

                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    var combinedOrderSearchProvider = scope.Resolve<LemonStandCombineOrderIdSearchProvider>();
                    IEnumerable<string> identifiers = await combinedOrderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                    foreach (string lemonStandOrderID in identifiers)
                    {
                        client.UpdateOrderStatus(lemonStandOrderID, StatusCodeProvider.GetCodeName(statusCode));
                    }
                }

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
                log.WarnFormat($"Unable to update online status for order {orderID}: cannot find order");
            }
        }

        /// <summary>
        ///     Push the shipment details to the store.
        /// </summary>
        public async Task UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            foreach (long orderKey in orderKeys)
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);
                if (shipment != null)
                {
                    await UpdateShipmentDetails(shipment).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        ///     Push the online status for an shipment.
        /// </summary>
        public async Task UpdateShipmentDetails(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                var combinedOrderSearchProvider = scope.Resolve<LemonStandCombineOrderIdSearchProvider>();
                IEnumerable<string> identifiers = await combinedOrderSearchProvider.GetOrderIdentifiers(shipment.Order).ConfigureAwait(false);

                foreach (string orderNumber in identifiers)
                {
                    string shipmentID = GetShipmentID(orderNumber);

                    client.UploadShipmentDetails(shipment.TrackingNumber, shipmentID);
                }
            }
        }

        /// <summary>
        /// Gets the LemonStand shipment ID as it is required to upload tracking information
        /// </summary>
        /// <param name="orderNumber">The Order.OrderNumber.</param>
        /// <returns>LemonStand API Shipment ID</returns>
        private string GetShipmentID(string orderID)
        {
            // use order id to get invoice
            JToken invoice = client.GetOrderInvoice(orderID);
            string invoiceID = invoice.SelectToken("data.invoices.data").Children().First().SelectToken("id").ToString();

            // use invoice id to get shipment
            JToken shipment = client.GetShipment(invoiceID);
            string shipmentID = shipment.SelectToken("data.shipments.data").Children().First().SelectToken("id").ToString();

            return shipmentID;
        }
    }
}