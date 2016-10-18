using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle when a label should be created
    /// </summary>
    public class CreateLabelPipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private readonly ILog log;
        private IDisposable subscription;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelPipeline(IObservable<IShipWorksMessage> messageStream,
            ISchedulerProvider schedulerProvider,
            Func<Type, ILog> logManager)
        {
            this.messageStream = messageStream;
            this.schedulerProvider = schedulerProvider;
            log = logManager(typeof(CreateLabelPipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<CreateLabelMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Where(x => x.ShipmentID == viewModel.Shipment?.ShipmentID)
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while handling processed shipment", ex))
                .Subscribe(_ => viewModel.CreateLabel());
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
