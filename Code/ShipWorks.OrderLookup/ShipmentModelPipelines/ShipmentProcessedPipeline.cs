using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using log4net;
//using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Settings;

namespace ShipWorks.OrderLookup.ShipmentModelPipelines
{
    /// <summary>
    /// Pipeline for handling post shipment processing actions
    /// </summary>
    public class ShipmentProcessedPipeline : IOrderLookupShipmentModelPipeline
    {
        private readonly IMessageHelper messageHelper;
        private readonly IMainForm mainForm;
        private readonly IMessenger messenger;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentProcessedPipeline(
            IMessenger messenger,
            IMessageHelper messageHelper,
            IMainForm mainForm,
            Func<Type, ILog> createLog)
        {
            this.messageHelper = messageHelper;
            this.mainForm = mainForm;
            this.messenger = messenger;
            log = createLog(GetType());
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public IDisposable Register(IOrderLookupShipmentModel model) => new CompositeDisposable(
            messenger.OfType<ShipmentsProcessedMessage>()
                .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                .Where(x => x.Shipments.All(s => s.Shipment.Processed))
                .Do(x => HandleSuccessfulShipment(model, x))
                .CatchAndContinue((Exception ex) => log.Error("Error handling successful label creation", ex))
                .Subscribe(),
            messenger.OfType<ShipmentsProcessedMessage>()
                .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                .Where(x => x.Shipments.Any(y => !y.IsSuccessful))
                .Do(x => model.LoadOrder(x.Shipments.FirstOrDefault().Shipment?.Order))
                .CatchAndContinue((Exception ex) => log.Error("Error handling failed label creation", ex))
                .Subscribe()
        );

        /// <summary>
        /// Handle a successful ShipmentsProcessedMessage
        /// </summary>
        private void HandleSuccessfulShipment(IOrderLookupShipmentModel model, ShipmentsProcessedMessage message)
        {
            model.LoadOrder(message.Shipments.First().Shipment.Order);

            messageHelper.ShowPopup(
                "Processed Successfully\n" +
                $"Order: {message.Shipments.FirstOrDefault().Shipment?.Order.OrderNumberComplete}",
                TimeSpan.FromSeconds(1.5));
        }
    }
}