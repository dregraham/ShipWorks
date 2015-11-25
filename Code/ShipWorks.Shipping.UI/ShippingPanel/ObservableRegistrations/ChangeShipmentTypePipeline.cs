using ShipWorks.Shipping.Services;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Pipeline for changing shipment type
    /// </summary>
    public class ChangeShipmentTypePipeline : IShippingPanelObservableRegistration
    {
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChangeShipmentTypePipeline(IShippingManager shippingManager)
        {
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return viewModel.PropertyChangeStream
                .Where(x => x == nameof(viewModel.ShipmentType))
                .Where(x => !(viewModel.ShipmentAdapter.Shipment?.Processed ?? true))
                .Subscribe(_ => viewModel.Populate(GetAdapter(viewModel)));
        }

        /// <summary>
        /// Get a shipping adapter from the changed shipment type
        /// </summary>
        private ICarrierShipmentAdapter GetAdapter(ShippingPanelViewModel viewModel) =>
            shippingManager.ChangeShipmentType(viewModel.ShipmentType, viewModel.ShipmentAdapter.Shipment);
    }
}
