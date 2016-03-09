using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle when a label should be created
    /// </summary>
    public class CreateLabelPipeline : IShippingPanelObservableRegistration
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelPipeline(IObservable<IShipWorksMessage> messageStream, Func<Type, ILog> logManager)
        {
            this.messageStream = messageStream;
            log = logManager(typeof(CreateLabelPipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messageStream.OfType<CreateLabelMessage>()
                .Where(x => x.ShipmentID == viewModel.Shipment?.ShipmentID)
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while handling processed shipment", ex))
                .Subscribe(_ => viewModel.CreateLabel());
        }
    }
}
