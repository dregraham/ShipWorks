using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle rate refresh requests
    /// </summary>
    public class RefreshRatesPipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messages;
        private readonly ILog log;
        private readonly IShippingManager shippingManager;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly IMessenger messenger;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public RefreshRatesPipeline(IObservable<IShipWorksMessage> messages, IShippingManager shippingManager,
            IMessenger messenger,
            ISchedulerProvider schedulerProvider, Func<Type, ILog> logFactory)
        {
            this.messenger = messenger;
            this.messages = messages;
            this.shippingManager = shippingManager;
            this.schedulerProvider = schedulerProvider;
            log = logFactory(typeof(RefreshRatesPipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messages.OfType<ShippingSettingsChangedMessage>()
                .Where(_ => viewModel.AllowEditing)
                .ObserveOn(schedulerProvider.TaskPool)
                .Select(_ => shippingManager.GetShipment(viewModel.Shipment.ShipmentID))
                .ObserveOn(schedulerProvider.Dispatcher)
                .Where(x => viewModel.Shipment.ShipmentID == x.Shipment.ShipmentID)
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while refreshing rates.", ex))
                .Subscribe(x => messenger.Send(new ShipmentChangedMessage(this, x)));
        }

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
