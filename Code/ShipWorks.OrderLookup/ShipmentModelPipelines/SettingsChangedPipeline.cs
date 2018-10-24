using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Settings;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup.ShipmentModelPipelines
{
    /// <summary>
    /// Pipeline for handling settings changed events (store/shipping settings/etc)
    /// </summary>
    public class SettingsChangedPipeline : IOrderLookupShipmentModelPipeline
    {
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ICurrentUserSettings currentUserSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsChangedPipeline(
            IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            ICurrentUserSettings currentUserSettings)
        {
            this.currentUserSettings = currentUserSettings;
            this.schedulerProvider = schedulerProvider;
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public IDisposable Register(IOrderLookupShipmentModel model)
        {
            return new CompositeDisposable(
                messenger.OfType<StoreChangedMessage>()
                    .Where(m => currentUserSettings.GetUIMode() == UIMode.OrderLookup)
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .Do(_ => ReloadOrder(model))
                    .Subscribe(),
                messenger.OfType<ShippingSettingsChangedMessage>()
                    .Where(m => currentUserSettings.GetUIMode() == UIMode.OrderLookup)
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .Do(_ => ReloadOrder(model))
                    .Subscribe()
                );
        }

        /// <summary>
        /// Reload the order
        /// </summary>
        private void ReloadOrder(IOrderLookupShipmentModel model)
        {
            OrderEntity order = model.SelectedOrder;
            model.Unload();
            model.LoadOrder(order);
        }
    }
}
