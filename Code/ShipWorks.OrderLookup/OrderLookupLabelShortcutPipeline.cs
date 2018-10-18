using System;
using System.Reactive.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Listens for CreateLabel ShortcutMessage and sends a ProcessShipmentsMessage when it receives one
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OrderLookupLabelShortcutPipeline : IDisposable
    {
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly ISecurityContext securityContext;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupLabelShortcutPipeline(IMessenger messenger, ISchedulerProvider schedulerProvider, 
            ICurrentUserSettings currentUserSettings, ISecurityContext securityContext)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.currentUserSettings = currentUserSettings;
            this.securityContext = securityContext;
        }

        /// <summary>
        /// Register the pipeline on the shipment model
        /// </summary>
        public void Register(IOrderLookupShipmentModel orderLookupShipmentModel)
        {
            subscription = messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.CreateLabel))
                .Where(_ => currentUserSettings.GetUIMode() == UIMode.OrderLookup)
                .Where(m => orderLookupShipmentModel.ShipmentAdapter.Shipment != null)
                .Where(m => orderLookupShipmentModel.ShipmentAdapter.ShipmentTypeCode != ShipmentTypeCode.None)
                .Where(m => !orderLookupShipmentModel.ShipmentAdapter.Shipment.Processed)
                .Where(_ => orderLookupShipmentModel.SelectedOrder != null &&
                            securityContext.HasPermission(PermissionType.ShipmentsCreateEditProcess, orderLookupShipmentModel.SelectedOrder.OrderID))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Subscribe(m =>
                {
                    orderLookupShipmentModel.SaveToDatabase();
                    ShipmentEntity[] shipments = { orderLookupShipmentModel.ShipmentAdapter.Shipment };
                    messenger.Send(new ProcessShipmentsMessage(this, shipments, shipments, null));
                });
        }

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose() => subscription?.Dispose();
    }
}