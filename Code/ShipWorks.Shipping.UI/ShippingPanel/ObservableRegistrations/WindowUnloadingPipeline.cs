using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle the window unloading message
    /// </summary>
    public class WindowUnloadingPipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private IDisposable subscription;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowUnloadingPipeline(IObservable<IShipWorksMessage> messageStream, ISchedulerProvider schedulerProvider)
        {
            this.messageStream = messageStream;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<WindowResettingMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(x =>
                {
                    viewModel?.SaveToDatabase();
                    viewModel?.UnloadShipment();
                });
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
