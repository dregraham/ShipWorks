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
    public class WindowUnloadingPipeline : IShippingPanelObservableRegistration
    {
        private readonly IObservable<IShipWorksMessage> messageStream;

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
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messageStream.OfType<WindowResettingMessage>()
                .Subscribe(x =>
                {
                    viewModel?.SaveToDatabase();
                    viewModel?.UnloadShipment();
                });
        }
    }
}
