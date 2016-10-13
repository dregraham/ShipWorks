using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle when rates are not supported
    /// </summary>
    public class RatesNotSupportedPipeline : IRatingPanelGlobalPipeline
    {
        readonly IObservable<IShipWorksMessage> messages;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatesNotSupportedPipeline(IObservable<IShipWorksMessage> messages, ISchedulerProvider schedulerProvider)
        {
            this.messages = messages;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline for the view model
        /// </summary>
        public IDisposable Register(RatingPanelViewModel viewModel)
        {
            return messages.OfType<RatesNotSupportedMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Do(_ => viewModel.IsLoading = false)
                .Subscribe(x => viewModel.SetRateResults(Enumerable.Empty<RateResult>(), x.Message, Enumerable.Empty<object>()));
        }
    }
}
