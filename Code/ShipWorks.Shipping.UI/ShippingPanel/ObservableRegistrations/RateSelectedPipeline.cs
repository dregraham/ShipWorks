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
                       viewModel.LoadedShipmentResult == ShippingPanelLoadedShipmentResult.Success)
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
        /// Select the specified rate
        /// </summary>
        private void SelectRate(ShippingPanelViewModel viewModel, RateResult selectedRate)
        {
            if (!viewModel.ShipmentAdapter.DoesRateMatchSelectedService(selectedRate))
            {
                viewModel.ShipmentViewModel.SelectRate(selectedRate);
                viewModel.SaveToDatabase();
            }
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
