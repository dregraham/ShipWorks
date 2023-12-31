﻿using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle shipment change messages
    /// </summary>
    public class ShipmentChangedPipeline : IRatingPanelGlobalPipeline
    {
        readonly IObservable<IShipWorksMessage> messages;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentChangedPipeline(IObservable<IShipWorksMessage> messages, ISchedulerProvider schedulerProvider)
        {
            this.messages = messages;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline for the view model
        /// </summary>
        public IDisposable Register(RatingPanelViewModel viewModel)
        {
            return messages.OfType<ShipmentChangedMessage>()
                .Where(x => x.ChangedField == "ServiceType")
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(x => viewModel.SelectRate(x.ShipmentAdapter));
        }
    }
}
