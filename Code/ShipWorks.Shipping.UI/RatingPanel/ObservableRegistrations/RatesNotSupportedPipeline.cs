﻿using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public RatesNotSupportedPipeline(IObservable<IShipWorksMessage> messages)
        {
            this.messages = messages;
        }

        /// <summary>
        /// Register the pipeline for the view model
        /// </summary>
        public IDisposable Register(RatingPanelViewModel viewModel)
        {
            return messages.OfType<RatesNotSupportedMessage>()
                .Do(_ => viewModel.IsLoading = false)
                .Subscribe(x => viewModel.SetRateResults(Enumerable.Empty<RateResult>(), x.Message, Enumerable.Empty<object>()));
        }
    }
}
