using System;
using System.Linq;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Pipeline for changing Origin details
    /// </summary>
    public class ChangeOriginAddressFromTypePipeline : IShippingPanelObservableRegistration
    {
        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return viewModel.PropertyChangeStream
                .Where(x => x == nameof(viewModel.OriginAddressType))
                .Subscribe(_ => UpdateOriginAddress(viewModel));
        }

        /// <summary>
        /// Update the origin address details on the view model
        /// </summary>
        private void UpdateOriginAddress(ShippingPanelViewModel viewModel)
        {
            viewModel.Origin.SetAddressFromOrigin(viewModel.OriginAddressType,
                viewModel.ShipmentAdapter.Shipment?.OrderID ?? 0,
                viewModel.AccountId,
                viewModel.ShipmentType);
        }
    }
}
