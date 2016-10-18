using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle the store changing
    /// </summary>
    public class StoreChangedPipeline : IShippingPanelTransientPipeline
    {
        readonly IObservable<IShipWorksMessage> messages;
        IDisposable subscription;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messages"></param>
        public StoreChangedPipeline(IObservable<IShipWorksMessage> messages, ISchedulerProvider schedulerProvider)
        {
            this.messages = messages;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messages.OfType<StoreChangedMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(m => OnStoreChanged(viewModel, m));
        }

        /// <summary>
        /// Handles StoreChangedMessages
        /// </summary>
        private void OnStoreChanged(ShippingPanelViewModel viewModel, StoreChangedMessage message)
        {
            UpdateDestinationValidationControls(viewModel, message.StoreEntity);
            UpdateOriginAddress(viewModel);
        }

        /// <summary>
        /// Update the validation controls based on the store setting
        /// </summary>
        private void UpdateDestinationValidationControls(ShippingPanelViewModel viewModel, StoreEntity storeEntity)
        {
            if (storeEntity.StoreID == viewModel.ShipmentAdapter.Store.StoreID)
            {
                viewModel.Destination.IsAddressValidationEnabled = storeEntity.AddressValidationSetting != (int) AddressValidationStoreSettingType.ValidationDisabled;
            }
        }

        /// <summary>
        /// Update the origin address when the store changed
        /// </summary>
        private static void UpdateOriginAddress(ShippingPanelViewModel viewModel)
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

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
