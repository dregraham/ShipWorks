using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations
{
    /// <summary>
    /// Pipeline to handle retrieving rates
    /// </summary>
    public class RatesRetrievedPipeline : IRatingPanelGlobalPipeline
    {
        readonly IObservable<IShipWorksMessage> messenger;
        readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatesRetrievedPipeline(IObservable<IShipWorksMessage> messenger, ISchedulerProvider schedulerProvider)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline for the view model
        /// </summary>
        public IDisposable Register(RatingPanelViewModel viewModel)
        {
            return new CompositeDisposable(
                messenger.OfType<RatesRetrievingMessage>()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .IgnoreBetweenMessages(
                        messenger.OfType<OpenShippingDialogMessage>(),
                        messenger.OfType<OrderSelectionChangingMessage>())
                    .Subscribe(_ => viewModel.ShowSpinner()),
                messenger.OfType<RatesRetrievingMessage>()
                    .Select(GetMatchingRatesRetrievedMessage)
                    .Switch()
                    .Throttle(TimeSpan.FromMilliseconds(250), schedulerProvider.Default)
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .IgnoreBetweenMessages(
                        messenger.OfType<OpenShippingDialogMessage>().Select(x => (object) x).Merge(messenger.OfType<RatesNotSupportedMessage>()),
                        messenger.OfType<OrderSelectionChangingMessage>())
                    .Subscribe(viewModel.LoadRates));
        }

        /// <summary>
        /// Get rates retrieved messages that match the rates retrieving message
        /// </summary>
        private IObservable<RatesRetrievedMessage> GetMatchingRatesRetrievedMessage(RatesRetrievingMessage rateRetrievingMsg)
        {
            return messenger.OfType<RatesRetrievedMessage>()
               .Where(rateRetrivedMsg => rateRetrievingMsg.RatingHash == rateRetrivedMsg.RatingHash);
        }
    }
}
