using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.SingleScan;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Subscription for AutoPrint
    /// </summary>
    [Component]
    public class OrderLookupAutoPrintService : IOrderLookupAutoPrintService
    {
        private const int ShipmentsProcessedMessageTimeoutInMinutes = 5;

        private readonly ILog log;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly IAutoPrintService autoPrintService;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintServicePipeline"/> class.
        /// </summary>
        public OrderLookupAutoPrintService(
            IAutoPrintService autoPrintService, 
            Func<Type, ILog> logFactory, 
            ISqlAdapterFactory sqlAdapterFactory,
            IMessenger messenger,
            ISchedulerProvider schedulerProvider)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.autoPrintService = autoPrintService;
            log = logFactory(typeof(AutoPrintServicePipeline));
        }

        /// <summary>
        /// Auto print shipments for the given orderid
        /// </summary>
        public async Task<AutoPrintCompletionResult> AutoPrintShipment(long orderId, SingleScanMessage message)
        {
            if (!autoPrintService.AllowAutoPrint(message))
            {
                return new AutoPrintCompletionResult(orderId);
            }

            GenericResult<AutoPrintResult> autoPrintResult = 
                await autoPrintService.Print(new AutoPrintServiceDto() { OrderID = orderId, MatchedOrderCount = 1 });

            AutoPrintCompletionResult result = await WaitForShipmentsProcessedMessage(autoPrintResult);
            SaveUnprocessedShipments(result);

            return result;
        }

        /// <summary>
        /// Waits for shipments processed message.
        /// </summary>
        private async Task<AutoPrintCompletionResult> WaitForShipmentsProcessedMessage(GenericResult<AutoPrintResult> autoPrintResult)
        {
            AutoPrintCompletionResult returnResult;
            long? orderId = autoPrintResult.Value.OrderId;

            if (autoPrintResult.Success)
            {
                log.Info("Waiting for ShipmentsProcessedMessage");
                // Listen for ShipmentsProcessedMessages, but timeout if processing takes
                // longer than ShipmentsProcessedMessageTimeoutInMinutes.
                // We don't get an observable to start from, but we need one to use ContinueAfter, so using
                // Observable.Return to get an observable to work with.
                returnResult = await Observable.Return(0)
                    .ContinueAfter(messenger.OfType<ShipmentsProcessedMessage>(),
                        TimeSpan.FromMinutes(ShipmentsProcessedMessageTimeoutInMinutes),
                        schedulerProvider.Default,
                        (i, shipmentsProcessedMessage) => shipmentsProcessedMessage)
                    .Select(shipmentsProcessedMessage =>
                    {
                        if (EqualityComparer<ShipmentsProcessedMessage>.Default.Equals(shipmentsProcessedMessage, default(ShipmentsProcessedMessage)))
                        {
                            log.Info("Timeout waiting for ShipmentsProcessedMessage");
                            return new AutoPrintCompletionResult(orderId);
                        }

                        log.Info($"ShipmentsProcessedMessage received from scan {autoPrintResult.Value.ScannedBarcode}");
                        return new AutoPrintCompletionResult(orderId, shipmentsProcessedMessage.Shipments);
                    }).FirstAsync().ToTask().ConfigureAwait(false);
            }
            else
            {
                log.Info("No Shipments, not waiting for ShipmentsProcessMessageScan");
                returnResult = new AutoPrintCompletionResult(orderId);
            }

            return returnResult;
        }


        /// <summary>
        /// Saves any changed weights for shipments that failed to process
        /// </summary>
        private void SaveUnprocessedShipments(AutoPrintCompletionResult shipmentsProcessedResult)
        {
            if (shipmentsProcessedResult.ProcessShipmentResults.Any())
            {
                List<ShipmentEntity> unprocessedShipments =
                    shipmentsProcessedResult.ProcessShipmentResults.Where(s => !s.IsSuccessful)
                        .Select(s => s.Shipment)
                        .ToList();

                if (unprocessedShipments.Any())
                {
                    log.Info($"Error processing {unprocessedShipments.Count}. Saving unprocessed shipments.");
                    using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
                    {
                        foreach (ShipmentEntity unprocessedShipment in unprocessedShipments)
                        {
                            sqlAdapter.SaveAndRefetch(unprocessedShipment);
                        }
                        sqlAdapter.Commit();
                    }
                }
                else
                {
                    log.Info("All shipments successfully processed.");
                }
            }
        }
    }
}
