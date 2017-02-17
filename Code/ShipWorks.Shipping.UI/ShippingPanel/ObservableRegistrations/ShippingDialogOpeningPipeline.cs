using System;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.Messaging.Messages.Dialogs;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle the open shipping dialog message
    /// </summary>
    public class ShippingDialogOpeningPipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private readonly ISchedulerProvider schedulerProvider;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDialogOpeningPipeline(IObservable<IShipWorksMessage> messageStream,
            ISchedulerProvider schedulerProvider)
        {
            this.messageStream = messageStream;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<ShippingDialogOpeningMessage>()
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Subscribe(_ => viewModel.UnloadOrder());
        }

        /// <summary>
        /// Dispose the pipeline
        /// </summary>
        public void Dispose() => subscription?.Dispose();
    }
}
