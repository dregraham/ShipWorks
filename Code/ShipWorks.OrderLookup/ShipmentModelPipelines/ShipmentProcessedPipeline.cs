using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.UI;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.OrderLookup.ShipmentModelPipelines
{
    /// <summary>
    /// Pipeline for handling post shipment processing actions
    /// </summary>
    public class ShipmentProcessedPipeline : IOrderLookupShipmentModelPipeline
    {
        private readonly IMessageHelper messageHelper;
        private readonly IMessenger messenger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="messageHelper"></param>
        public ShipmentProcessedPipeline(IMessenger messenger, IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public IDisposable Register(IOrderLookupShipmentModel model) => new CompositeDisposable(
            messenger.OfType<ShipmentsProcessedMessage>()
                .Where(x => x.Shipments.All(y => y.IsSuccessful))
                .Subscribe(x => model.Unload()),
            messenger.OfType<ShipmentsProcessedMessage>()
                .Where(x => x.Shipments.Any(y => !y.IsSuccessful))
                .Subscribe(x => model.LoadOrder(x.Shipments.FirstOrDefault().Shipment?.Order)),
            messenger.OfType<ShipmentsProcessedMessage>()
                .Where(m => m.Shipments.All(y => y.IsSuccessful))
                .Subscribe(x => messageHelper.ShowPopup($"Successfully processed order {x.Shipments.FirstOrDefault().Shipment?.Order.OrderNumberComplete}")));
    }
}