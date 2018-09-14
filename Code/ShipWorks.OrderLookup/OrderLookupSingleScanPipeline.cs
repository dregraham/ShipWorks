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
                .Where(x => !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup)
                .Do(x => onDemandDownloaderFactory.CreateOnDemandDownloader().Download(x.ScannedText))
                .SelectMany(x => AutoPrint(x))
                .SelectMany(x => orderRepository.GetOrder(x.OrderID))
                .Subscribe(SendOrderMessage),

                messenger.OfType<OrderLookupSearchMessage>()
                .Where(x => !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup)
                .Do(x => onDemandDownloaderFactory.CreateOnDemandDownloader().Download(x.SearchText))
                .SelectMany(x => orderRepository.GetOrderID(x.SearchText).ToObservable())
                .SelectMany(x => orderRepository.GetOrder(x))
                .Subscribe(SendOrderMessage)

                );
        }

        /// <summary>
        /// Do AutoPrint for the order
        /// </summary>
        private async Task<(long OrderID, SingleScanMessage Message)> AutoPrint(SingleScanMessage message)
        {
            long orderId = await orderRepository.GetOrderID(message.ScannedText);
            AutoPrintCompletionResult result = await orderLookupAutoPrintService.AutoPrintShipment(orderId, message);

            return (orderId, message);
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