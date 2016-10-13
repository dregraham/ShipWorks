using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle when the shipping dialog opens
    /// </summary>
    public class OpenShippingDialogPipeline : IRatingPanelGlobalPipeline
    {
        private readonly IObservable<IShipWorksMessage> messages;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenShippingDialogPipeline(IObservable<IShipWorksMessage> messages, ISchedulerProvider schedulerProvider)
        {
            this.messages = messages;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline for the view model
        /// </summary>
        public IDisposable Register(RatingPanelViewModel viewModel)
        {
            return messages.OfType<OpenShippingDialogMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(_ =>
                {
                    viewModel.SetRateResults(Enumerable.Empty<RateResult>(), string.Empty, Enumerable.Empty<object>());
                    viewModel.IsLoading = false;
                });
        }
    }
}
