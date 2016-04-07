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
    public class ApplyProfilePipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly ILog log;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplyProfilePipeline(IObservable<IShipWorksMessage> messageStream,
            IShipmentTypeManager shipmentTypeManager,
            Func<Type, ILog> logManager)
        {
            this.messageStream = messageStream;
            this.shipmentTypeManager = shipmentTypeManager;
            log = logManager(typeof(ApplyProfilePipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<ApplyProfileMessage>()
                .Where(x => x.ShipmentID == viewModel.Shipment?.ShipmentID)
                .Select(x =>
                {
                    ShipmentType shipmentType = shipmentTypeManager.Get(viewModel.Shipment);
                    shipmentType.ApplyProfile(viewModel.Shipment, x.Profile);
                    return viewModel.ShipmentAdapter;
                })
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while applying profile to shipment", ex))
                .Subscribe(x => viewModel.LoadShipment(x));
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
