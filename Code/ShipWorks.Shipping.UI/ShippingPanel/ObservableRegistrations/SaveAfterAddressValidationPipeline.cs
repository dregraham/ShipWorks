using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Save the shipment after the destination has been validated
    /// </summary>
    public class SaveAfterAddressValidationPipeline : IShippingPanelTransientPipeline
    {
        private IDisposable subscription;
        private readonly ISchedulerProvider schedulerProvider;

        public SaveAfterAddressValidationPipeline(ISchedulerProvider schedulerProvider)
        {
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = viewModel.Destination
                .PropertyChangeStream
                .ObserveOn(schedulerProvider.Dispatcher)
                .Where(x => x == nameof(viewModel.Destination.ValidationStatus))
                .Subscribe(_ => viewModel.SaveToDatabase());
        }

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose() => subscription?.Dispose();
    }
}
