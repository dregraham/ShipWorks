using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.Stores;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Listens for scan. Sends OrderFound message when the scan corresponds to an order
    /// </summary>
    public class OrderLookupSingleScanPipeline : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly IMainForm mainForm;
        private readonly ICurrentUserSettings userSettings;
        private readonly IOrderRepository orderRepository;

        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupSingleScanPipeline(IMessenger messenger,
            IMainForm mainForm,
            ICurrentUserSettings userSettings,
            IOrderRepository orderRepository)
        {
            this.messenger = messenger;
            this.mainForm = mainForm;
            this.userSettings = userSettings;
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Initialize pipeline
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();

            subscription = messenger.OfType<SingleScanMessage>()
                .Where(x => !mainForm.AdditionalFormsOpen() && userSettings.GetUIMode() == UIMode.OrderLookup)
                .SelectMany(scanMsg => orderRepository.FindOrder(scanMsg.ScannedText).ToObservable())
                .Subscribe(SendOrderMessage);
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
        public void EndSession() => subscription?.Dispose();
    }
}
