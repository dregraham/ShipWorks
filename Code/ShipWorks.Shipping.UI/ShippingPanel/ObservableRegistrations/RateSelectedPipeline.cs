using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Loading;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle the store changing
    /// </summary>
    public class RateSelectedPipeline : IShippingPanelObservableRegistration
    {
        readonly IObservable<IShipWorksMessage> messages;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messages"></param>
        public RateSelectedPipeline(IObservable<IShipWorksMessage> messages)
        {
            this.messages = messages;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messages.OfType<SelectedRateChangedMessage>()
                .Where(x => x.Sender != viewModel &&
                       x.Sender != viewModel.ShipmentViewModel &&
                       viewModel.LoadedShipmentResult == ShippingPanelLoadedShipmentResult.Success &&
                       x.RateResult.ShipmentType != ShipmentTypeCode.Amazon &&
                       x.RateResult.ShipmentType != ShipmentTypeCode.BestRate)
                .Subscribe(x => SelectRate(viewModel, x.RateResult));
        }

        /// <summary>
        /// Select the specified rate
        /// </summary>
        private void SelectRate(ShippingPanelViewModel viewModel, RateResult selectedRate)
        {
            viewModel.ShipmentViewModel.SelectRate(selectedRate);
        }
    }
}
