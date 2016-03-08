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
    /// Handle when a label should be voided
    /// </summary>
    public class VoidLabelPipeline : IShippingPanelObservableRegistration
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public VoidLabelPipeline(IObservable<IShipWorksMessage> messageStream, Func<Type, ILog> logManager)
        {
            this.messageStream = messageStream;
            log = logManager(typeof(VoidLabelPipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messageStream.OfType<VoidLabelMessage>()
                .Where(x => x.ShipmentID == viewModel.Shipment.ShipmentID)
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while handling processed shipment", ex))
                .Subscribe(viewModel.VoidLabel);
        }
    }
}
