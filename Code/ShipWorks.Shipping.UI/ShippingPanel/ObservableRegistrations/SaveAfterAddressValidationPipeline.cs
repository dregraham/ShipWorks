using System;
using System.Linq;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Save the shipment after the destination has been validated
    /// </summary>
    public class SaveAfterAddressValidationPipeline : IShippingPanelTransientPipeline
    {
        private IDisposable subscription;

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = viewModel.Destination
                .PropertyChangeStream
                .Where(x => x == nameof(viewModel.Destination.ValidationStatus))
                .Subscribe(_ => viewModel.SaveToDatabase());
        }

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose() => subscription?.Dispose();
    }
}
