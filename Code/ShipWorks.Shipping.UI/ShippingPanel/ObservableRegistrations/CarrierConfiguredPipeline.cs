﻿using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle configuration of a carrier
    /// </summary>
    public class CarrierConfiguredPipeline : IShippingPanelGlobalPipeline
    {
        private readonly IObservable<IShipWorksMessage> messages;
        private readonly ILog log;
        private readonly IShippingManager shippingManager;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierConfiguredPipeline(IObservable<IShipWorksMessage> messages, IShippingManager shippingManager,
            ISchedulerProvider schedulerProvider, Func<Type, ILog> logFactory)
        {
            this.messages = messages;
            this.shippingManager = shippingManager;
            this.schedulerProvider = schedulerProvider;
            log = logFactory(typeof(CarrierConfiguredPipeline));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messages.OfType<CarrierConfiguredMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Where(x => x.ShipmentTypeCode == viewModel.ShipmentType)
                .Do(_ => viewModel.AllowEditing = false)
                .ObserveOn(schedulerProvider.TaskPool)
                .Where(_ => viewModel.Shipment != null)
                .Select(_ => shippingManager.GetShipment(viewModel.Shipment.ShipmentID))
                .ObserveOn(schedulerProvider.Dispatcher)
                .Where(x => viewModel.Shipment.ShipmentID == x.Shipment.ShipmentID)
                .Do(viewModel.LoadShipment)
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while changing shipment types", ex))
                .Subscribe();
        }
    }
}
