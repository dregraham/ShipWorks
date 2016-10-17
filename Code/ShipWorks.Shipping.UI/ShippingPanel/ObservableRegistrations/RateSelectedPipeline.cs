using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Loading;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle the store changing
    /// </summary>
    public class RateSelectedPipeline : IShippingPanelTransientPipeline
    {
        readonly IObservable<IShipWorksMessage> messages;
        private IDisposable subscription;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messages"></param>
        public RateSelectedPipeline(IObservable<IShipWorksMessage> messages, ISchedulerProvider schedulerProvider)
        {
            this.messages = messages;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messages.OfType<SelectedRateChangedMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Where(x => IsSenderOutsideShippingPanel(x.Sender, viewModel) &&
                       viewModel.LoadedShipmentResult == ShippingPanelLoadedShipmentResult.Success &&
                       IsValidShipmentType(x.RateResult.ShipmentType))
                .Subscribe(x => SelectRate(viewModel, x.RateResult));
        }

        /// <summary>
        /// Is the sender something other than the shipping panel
        /// </summary>
        private bool IsSenderOutsideShippingPanel(object sender, ShippingPanelViewModel viewModel)
        {
            return sender != viewModel && sender != viewModel.ShipmentViewModel;
        }

        /// <summary>
        /// Is the shipment type valid for rating in the panel
        /// </summary>
        private bool IsValidShipmentType(ShipmentTypeCode shipmentTypeCode)
        {
            return shipmentTypeCode != ShipmentTypeCode.Amazon;
        }

        /// <summary>
        /// Select the specified rate
        /// </summary>
        private void SelectRate(ShippingPanelViewModel viewModel, RateResult selectedRate)
        {
            viewModel.ShipmentViewModel.SelectRate(selectedRate);
            viewModel.SaveToDatabase();
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
