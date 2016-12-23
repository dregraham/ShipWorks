using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Messaging.TrackedObservable;
using Interapptive.Shared.Threading;
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
                    .Trackable()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .IgnoreBetweenMessages(
                        messenger.OfType<OpenShippingDialogMessage>(),
                        messenger.OfType<OrderSelectionChangingMessage>())
                    .Subscribe(this, _ => viewModel.ShowSpinner()),
                messenger.OfType<RatesRetrievingMessage>()
                    .Trackable()
                    .Select(this, GetMatchingRatesRetrievedMessage)
                    .Switch(this)
                    .Throttle(TimeSpan.FromMilliseconds(250), schedulerProvider.Default)
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Dump(this)
                    .IgnoreBetweenMessages(
                        messenger.OfType<OpenShippingDialogMessage>().Trackable().Select(this, x => "Window closing"),
                        messenger.OfType<OrderSelectionChangingMessage>().Trackable().Select(this, x => "Window opening"))
                    .Subscribe(this, viewModel.LoadRates));
        }

        /// <summary>
        /// Get rates retrieved messages that match the rates retrieving message
        /// </summary>
        private IObservable<IMessageTracker<RatesRetrievedMessage>> GetMatchingRatesRetrievedMessage(RatesRetrievingMessage rateRetrievingMsg)
        {
            return messenger.OfType<RatesRetrievedMessage>().Trackable()
               .Where(this, rateRetrivedMsg => rateRetrievingMsg.RatingHash == rateRetrivedMsg.RatingHash);
        }
    }
}
