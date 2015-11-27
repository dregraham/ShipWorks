using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle a deleted shipment
    /// </summary>
    public class ShipmentDeletedPipeline : IShippingPanelObservableRegistration
    {
        readonly IObservable<IShipWorksMessage> messages;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDeletedPipeline(IObservable<IShipWorksMessage> messages)
        {
            this.messages = messages;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messages.OfType<ShipmentDeletedMessage>()
                .Where(x => x.DeletedShipmentId == viewModel.ShipmentAdapter?.Shipment?.ShipmentID)
                .Subscribe(x => viewModel.UnloadShipment());
        }
    }
}
