using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.SingleScan;
using ShipWorks.Stores.Communication;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Listens for scan. Sends OrderFound message when the scan corresponds to an order
    /// </summary>
    public class OrderLookupSingleScanPipeline : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly IMainForm mainForm;
        private readonly IOrderLookupOrderRepository orderRepository;
        private readonly IOnDemandDownloaderFactory onDemandDownloaderFactory;
        private readonly IOrderLookupAutoPrintService orderLookupAutoPrintService;
        private readonly IAutoWeighService autoWeighService;
        private readonly IOrderLookupShipmentModel shipmentModel;
        private readonly IOrderLookupConfirmationService orderLookupConfirmationService;
        private readonly ILog log;
        private IDisposable subscriptions;
        private bool processingScan = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupSingleScanPipeline(
            IMessenger messenger,
            IMainForm mainForm,
            IOrderLookupOrderRepository orderRepository,
            IOnDemandDownloaderFactory onDemandDownloaderFactory,
            IOrderLookupAutoPrintService orderLookupAutoPrintService,
            IAutoWeighService autoWeighService,
            IOrderLookupShipmentModel shipmentModel,
            Func<Type, ILog> createLogger,
            IOrderLookupConfirmationService orderLookupConfirmationService)
        {
            this.messenger = messenger;
            this.mainForm = mainForm;
            this.orderRepository = orderRepository;
            this.onDemandDownloaderFactory = onDemandDownloaderFactory;
            this.orderLookupAutoPrintService = orderLookupAutoPrintService;
            this.autoWeighService = autoWeighService;
            this.shipmentModel = shipmentModel;
            this.orderLookupConfirmationService = orderLookupConfirmationService;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Initialize pipeline
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();

            subscriptions = new CompositeDisposable(
                messenger.OfType<SingleScanMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && !mainForm.IsShipmentHistoryActive())
                .Do(_ => processingScan = true)
                .Subscribe(x => OnSingleScanMessage(x).Forget()),

                messenger.OfType<OrderLookupSearchMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && !mainForm.IsShipmentHistoryActive())
                .Do(_ => processingScan = true)
                .Subscribe(x => OnOrderLookupSearchMessage(x).Forget())
            );
        }

        /// <summary>
        /// Download order, auto print if needed, send order message
        /// </summary>
        private async Task OnSingleScanMessage(SingleScanMessage message)
        {
            try
            {
                shipmentModel.SaveToDatabase();

                await onDemandDownloaderFactory.CreateOnDemandDownloader().Download(message.ScannedText).ConfigureAwait(true);
                long? orderId = await orderLookupConfirmationService.ConfirmOrder(orderRepository.GetOrderIDs(message.ScannedText));
                OrderEntity order = null;

                if (orderId.HasValue)
                {
                    AutoPrintCompletionResult result = await orderLookupAutoPrintService.AutoPrintShipment(orderId.Value, message).ConfigureAwait(true);
                    order = result.ProcessShipmentResults?.Select(x => x.Shipment.Order).FirstOrDefault();
                    if (order == null)
                    {
                        order = await orderRepository.GetOrder(orderId.Value).ConfigureAwait(true);
                    }

                    if (order != null &&
                        order.Shipments.Any() &&
                        !order.Shipments.Last().Processed)
                    {
                        using (ITrackedEvent telemetry = new TrackedEvent("OrderLookup.Search.AutoWeigh"))
                        {
                            autoWeighService.ApplyWeight(new[] { order.Shipments.Last() }, telemetry);
                        }
                    }
                }

                shipmentModel.LoadOrder(order);
            }
            catch (Exception ex)
            {
                log.Error("Error while loading an order", ex);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// Download order, send order message
        /// </summary>
        private async Task OnOrderLookupSearchMessage(OrderLookupSearchMessage message)
        {
            try
            {
                shipmentModel.SaveToDatabase();

                await onDemandDownloaderFactory.CreateOnDemandDownloader().Download(message.SearchText).ConfigureAwait(true);
                long? orderId = await orderLookupConfirmationService.ConfirmOrder(orderRepository.GetOrderIDs(message.SearchText));

                OrderEntity order = null;
                if (orderId.HasValue)
                {
                    order = await orderRepository.GetOrder(orderId.Value).ConfigureAwait(true);
                }

                shipmentModel.LoadOrder(order);
            }
            catch (Exception ex)
            {
                log.Error("Error while loading an order", ex);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// End the session
        /// </summary>
        public void Dispose() => EndSession();

        /// <summary>
        /// End the session
        /// </summary>
        public void EndSession() => subscriptions?.Dispose();
    }
}