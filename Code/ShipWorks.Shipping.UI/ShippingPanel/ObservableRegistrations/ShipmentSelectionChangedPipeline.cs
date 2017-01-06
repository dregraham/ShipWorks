using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging.TrackedObservable;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// The shipment selection has changed
    /// </summary>
    public class ShipmentSelectionChangedPipeline : IShippingPanelGlobalPipeline
    {
        readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentSelectionChangedPipeline(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        /// <summary>
        /// Register this pipeline
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messenger.OfType<ShipmentSelectionChangedMessage>()
                .Trackable()
                .Do(this, x => HandleShipmentSelectionChanged(x.SelectedShipmentIDs, viewModel))
                .Subscribe();
        }

        /// <summary>
        /// Handle the shipment selection changed message
        /// </summary>
        private void HandleShipmentSelectionChanged(IEnumerable<long> shipmentIDs, ShippingPanelViewModel viewModel)
        {
            if (shipmentIDs.CompareCountTo(1) != ComparisonResult.Equal)
            {
                viewModel.UnloadShipment();
                viewModel.SelectedShipments = shipmentIDs.ToReadOnly();
                messenger.Send(new RatesNotSupportedMessage(viewModel, "Unable to get rates for multiple shipments."));
                viewModel.LoadedShipmentResult = ShippingPanelLoadedShipmentResult.Multiple;
                return;
            }

            long shipmentID = shipmentIDs.Single();
            ICarrierShipmentAdapter shipment = viewModel.GetShipmentAdapterWithID(shipmentID);

            if (shipment != null)
            {
                viewModel.LoadShipment(shipment);
            }
            else
            {
                viewModel.LoadShipmentWhenAvailable(shipmentID);
            }
        }
    }
}
