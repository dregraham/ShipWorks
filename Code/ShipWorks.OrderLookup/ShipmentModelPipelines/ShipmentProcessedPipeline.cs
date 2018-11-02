using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
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

        /// <summary>
        /// ctor
        /// </summary>
        public ShipmentProcessedPipeline(IMessenger messenger, IMessageHelper messageHelper, IMainForm mainForm)
        {
            this.messageHelper = messageHelper;
            this.mainForm = mainForm;
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public IDisposable Register(IOrderLookupShipmentModel model) => new CompositeDisposable(
            messenger.OfType<ShipmentsProcessedMessage>()
                .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                .Where(x => x.Shipments.All(s => s.Shipment.Processed))
                .Subscribe(x => HandleSuccessfulShipment(model, x)),
            messenger.OfType<ShipmentsProcessedMessage>()
                .Where(x => x.Shipments.Any(y => !y.IsSuccessful))
                .Subscribe(x => model.LoadOrder(x.Shipments.FirstOrDefault().Shipment?.Order))
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