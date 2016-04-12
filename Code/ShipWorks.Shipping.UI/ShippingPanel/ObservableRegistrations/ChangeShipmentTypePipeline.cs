using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Pipeline for changing shipment type
    /// </summary>
    public class ChangeShipmentTypePipeline : IShippingPanelTransientPipeline
    {
        private readonly ILog log;
        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeShipmentTypePipeline(IShippingManager shippingManager, IMessenger messenger, Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(ChangeShipmentTypePipeline));
            this.shippingManager = shippingManager;
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = viewModel.PropertyChangeStream
                .Where(x => x == nameof(viewModel.ShipmentType))
                .Where(x => viewModel.Shipment != null && viewModel.ShipmentStatus == ShipmentStatus.Unprocessed)
                .Select(_ => ChangeShipmentType(viewModel))
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while changing shipment types", ex))
                .Subscribe(x =>
                {
                    viewModel.LoadShipment(x, nameof(viewModel.ShipmentType));
                    viewModel.SaveToDatabase();
                    messenger.Send(new ShipmentChangedMessage(this, x, nameof(viewModel.ShipmentType)));
                });
        }

        /// <summary>
        /// Get a shipping adapter from the changed shipment type
        /// </summary>
        private ICarrierShipmentAdapter ChangeShipmentType(ShippingPanelViewModel viewModel) =>
            shippingManager.ChangeShipmentType(viewModel.ShipmentType, viewModel.Shipment);

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
