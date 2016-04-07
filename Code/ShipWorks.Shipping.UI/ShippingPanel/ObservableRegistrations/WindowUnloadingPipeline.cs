using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowUnloadingPipeline(IObservable<IShipWorksMessage> messageStream)
        {
            this.messageStream = messageStream;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<WindowResettingMessage>()
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
