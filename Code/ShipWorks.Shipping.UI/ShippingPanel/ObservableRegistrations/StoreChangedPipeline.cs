using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Settings.Origin;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle the store changing
    /// </summary>
    public class StoreChangedPipeline : IShippingPanelObservableRegistration
    {
        readonly IObservable<IShipWorksMessage> messages;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messages"></param>
        public StoreChangedPipeline(IObservable<IShipWorksMessage> messages)
        {
            this.messages = messages;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messages.OfType<StoreChangedMessage>()
                .Subscribe(_ => OnStoreChanged(viewModel));
        }

        /// <summary>
        /// Handles StoreChangedMessages
        /// </summary>
        private void OnStoreChanged(ShippingPanelViewModel viewModel)
        {
            if (viewModel.OriginAddressType != (int)ShipmentOriginSource.Store)
            {
                return;
            }

            viewModel.Origin.SetAddressFromOrigin(
                viewModel.OriginAddressType,
                viewModel.ShipmentAdapter.Shipment?.OrderID ?? 0,
                viewModel.AccountId,
                viewModel.ShipmentType);
        }
    }
}
