﻿using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Messaging.TrackedObservable;
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
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatesRetrievedPipeline(IMessenger messenger, ISchedulerProvider schedulerProvider)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline for the view model
        /// </summary>
        public IDisposable Register(RatingPanelViewModel viewModel)
        {
            IDisposable registration = new CompositeDisposable(
                messenger.OfType<RatesRetrievingMessage>()
                    .Trackable()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .IgnoreBetweenMessages(
                        messenger.OfType<OpenShippingDialogMessage>(),
                        GetResumeObservable())
                    .Subscribe(this, _ => viewModel.ShowSpinner()),
                messenger.OfType<RatesRetrievingMessage>()
                    .Trackable()
                    .Select(this, GetMatchingRatesRetrievedMessage)
                    .Switch(this)
                    .Throttle(TimeSpan.FromMilliseconds(250), schedulerProvider.Default)
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Dump(this)
<<<<<<< Updated upstream
                    .IgnoreBetweenMessages(
                        messenger.OfType<OpenShippingDialogMessage>().Trackable().Select(this, x => "Window closing"),
                        GetResumeObservable().Trackable().Select(this, x => "Window opening"))
                    .Subscribe(this, viewModel.LoadRates));
=======
                    .Subscribe(this, viewModel.LoadRates),
               messenger.OfType<ShipmentsProcessedMessage>()
                    .Trackable()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Subscribe(this, _ => viewModel.ClearRates()),
               messenger.OfType<OpenShippingDialogWithOrdersMessage>()
                    .Trackable()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Subscribe(this, _ => viewModel.HideRates()));
>>>>>>> Stashed changes

            messenger.Send(new InitializeRatesRetrievedPipelineMessage());

            return registration;
        }

        /// <summary>
        /// Act when ORderSelectionChangingMessage or OrderLookupSingleScanMessage received.
        /// </summary>
        /// <returns></returns>
        private IObservable<IShipWorksMessage> GetResumeObservable()
        {
            return messenger.OfType<OrderSelectionChangingMessage>()
                .Select(x => x as IShipWorksMessage)
                .Merge(messenger.OfType<InitializeRatesRetrievedPipelineMessage>().Select(x => x as IShipWorksMessage));
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
