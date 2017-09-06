using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Templates.Tokens;
using Autofac;
using ShipWorks.ApplicationCore;
using System.Collections.Generic;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using Interapptive.Shared.Collections;
using System.Linq;
using System;
using Interapptive.Shared.Utility;

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

        private ICombineOrderSearchProvider<string> combinedOrderSearchProvider;

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

            if (order.CombineSplitStatus == CombineSplitStatusType.None && order.IsManual)
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
                return;
            }

            // Upload tracking number
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IGenericStoreWebClientFactory webClientFactory = scope.Resolve<IGenericStoreWebClientFactory>();
                IGenericStoreWebClient webClient = webClientFactory.CreateWebClient(order.StoreID);

                IEnumerable<string> identifiers = await GetCombinedOrderIdentifiers(order).ConfigureAwait(false);

                IResult[] results = null;
                foreach (var chunk in identifiers.SplitIntoChunksOf(4))
                {
                    results = await Task.WhenAll(chunk.Select(x => webClient.UploadShipmentDetails(order, x, shipment))).ConfigureAwait(false);
                }

                if (results?.Any(r => r.Failure) == true)
                {
                    throw new GenericStoreException($"An error occurred uploading shipment information for order { order.OrderNumber }");
                }
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

            IResult[] results = null;
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IGenericStoreWebClientFactory webClientFactory = scope.Resolve<IGenericStoreWebClientFactory>();
                IGenericStoreWebClient webClient = webClientFactory.CreateWebClient(order.StoreID);

                IEnumerable<string> identifiers = await GetCombinedOrderIdentifiers(order).ConfigureAwait(false);

                foreach (var chunk in identifiers.SplitIntoChunksOf(4))
                {
                    results = await Task.WhenAll(chunk.Select(x => webClient.UpdateOrderStatus(order, x, code, processedComment))).ConfigureAwait(false);
                }
            }

            if (results?.Any(r => r.Failure) == true)
            {
                throw new GenericStoreException($"An error occurred uploading order information for order { order.OrderNumber }");
            }

            // Update the database to match, status code display
            OrderEntity basePrototype = new OrderEntity(orderID) { IsNew = false };
            basePrototype.OnlineStatusCode = code;
            basePrototype.OnlineStatus = StatusCodes.GetCodeName(code);

            unitOfWork.AddForSave(basePrototype);
        }

        /// <summary>
        /// Get the combined order identifiers for the given order.
        /// </summary>
        private async Task<IEnumerable<string>> GetCombinedOrderIdentifiers(OrderEntity order)
        {
            IEnumerable<string> identifiers;
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                // See if there is a store specific implementation of ICombineOrderSearchProvider, and if so, use it
                if (scope.IsRegisteredWithKey(Store.StoreTypeCode, typeof(ICombineOrderSearchProvider<string>)))
                {
                    combinedOrderSearchProvider = scope.ResolveKeyed<ICombineOrderSearchProvider<string>>(Store.StoreTypeCode);
                }
                else
                {
                    combinedOrderSearchProvider = scope.Resolve<ICombineOrderNumberCompleteSearchProvider>();
                }

                identifiers = await combinedOrderSearchProvider.GetOrderIdentifiers(order);
            }
            return identifiers;
        }
    }
}
