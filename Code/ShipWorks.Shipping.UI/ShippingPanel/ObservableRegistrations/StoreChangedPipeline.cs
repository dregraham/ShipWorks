using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Settings.Origin;

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
            // If an order is not selected, there's no order on which to set an origin address
            // so just return.
            if (viewModel.OriginAddressType != (int) ShipmentOriginSource.Store ||
                viewModel.OrderID == null ||
                viewModel.OrderID <= 0)
            {
                return;
            }

            viewModel.Origin.SetAddressFromOrigin(
                viewModel.OriginAddressType,
                viewModel.OrderID ?? 0,
                viewModel.AccountId,
                viewModel.ShipmentType);
        }
    }
}
