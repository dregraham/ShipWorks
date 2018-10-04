using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.Stores.Communication;
using System.Threading.Tasks;
using ShipWorks.SingleScan;
using System.Linq;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages;

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
        private IDisposable subscriptions;

        bool processingScan = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupSingleScanPipeline(
            IMessenger messenger,
            IMainForm mainForm,
            IOrderLookupOrderRepository orderRepository,
            IOnDemandDownloaderFactory onDemandDownloaderFactory,
            IOrderLookupAutoPrintService orderLookupAutoPrintService)
        {
            this.messenger = messenger;
            this.mainForm = mainForm;
            this.orderRepository = orderRepository;
            this.onDemandDownloaderFactory = onDemandDownloaderFactory;
            this.orderLookupAutoPrintService = orderLookupAutoPrintService;
            
        }

        /// <summary>
        /// Initialize pipeline
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();

            subscriptions = new CompositeDisposable(
                messenger.OfType<SingleScanMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup)
                .Do(_ => processingScan = true)
                .Subscribe(x => OnSingleScanMessage(x).ToObservable()),

                messenger.OfType<OrderLookupSearchMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup)
                .Do(_ => processingScan = true)
                .Subscribe(x => OnOrderLookupSearchMessage(x).ToObservable())
            );
        }

        /// <summary>
        /// Download order, auto print if needed, send order message
        /// </summary>
        private async Task OnSingleScanMessage(SingleScanMessage message)
        {
            try
            {
                await onDemandDownloaderFactory.CreateOnDemandDownloader().Download(message.ScannedText).ConfigureAwait(true);
                long? orderId = orderRepository.GetOrderID(message.ScannedText);
                OrderEntity order = null;

                if (orderId.HasValue)
                {
                    AutoPrintCompletionResult result = await orderLookupAutoPrintService.AutoPrintShipment(orderId.Value, message).ConfigureAwait(true);
                    order = result.ProcessShipmentResults?.Cast<ProcessShipmentResult?>().FirstOrDefault()?.Shipment.Order;
                    if (order == null)
                    {
                        order = await orderRepository.GetOrder(orderId.Value).ConfigureAwait(true);
                    }
                }

                SendOrderMessage(order);
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
                await onDemandDownloaderFactory.CreateOnDemandDownloader().Download(message.SearchText).ConfigureAwait(true);
                long? orderId = orderRepository.GetOrderID(message.SearchText);

                OrderEntity order = null;
                if (orderId.HasValue)
                {
                    order = await orderRepository.GetOrder(orderId.Value).ConfigureAwait(true);
                }

                SendOrderMessage(order);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// SendOrderMessage
        /// </summary>
        private void SendOrderMessage(OrderEntity order)
        {
            messenger.Send(new OrderLookupSingleScanMessage(this, order));
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