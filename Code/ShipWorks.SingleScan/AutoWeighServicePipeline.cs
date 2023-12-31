﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer.Collaboration;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Shipping;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Automatically weighs the shipment after scan.
    /// </summary>
    /// <seealso cref="ShipWorks.ApplicationCore.IInitializeForCurrentUISession" />
    public class AutoWeighServicePipeline : IInitializeForCurrentUISession
    {
        private readonly ISingleScanAutomationSettings singleScanAutomationSettings;
        private readonly IConnectableObservable<SingleScanFilterUpdateCompleteMessage> filterUpdateCompleteMessages;
        private readonly IDisposable filterUpdateCompleteMessagesConnection;
        private readonly IMessenger messenger;
        private readonly IAutoWeighService autoWeighService;
        private readonly IOrderLoader orderLoader;
        private readonly IShippingManager shippingManager;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public AutoWeighServicePipeline(IMessenger messenger,
            ISingleScanAutomationSettings singleScanAutomationSettings,
            IAutoWeighService autoWeighService,
            IOrderLoader orderLoader,
            IShippingManager shippingManager)
        {
            this.messenger = messenger;
            this.autoWeighService = autoWeighService;
            this.orderLoader = orderLoader;
            this.shippingManager = shippingManager;
            this.singleScanAutomationSettings = singleScanAutomationSettings;
            filterUpdateCompleteMessages = messenger.OfType<SingleScanFilterUpdateCompleteMessage>().Publish();
            filterUpdateCompleteMessagesConnection = filterUpdateCompleteMessages.Connect();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            EndSession();
        }

        /// <summary>
        /// Initialize for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            filterUpdateCompleteMessages
                .Where(ShouldWeigh)
                .SelectMany(async message => await Weigh(message))
                .Subscribe();
        }

        /// <summary>
        /// Returns true if we there is only one order in grid and autoprint isn't enabled.
        /// </summary>
        private bool ShouldWeigh(SingleScanFilterUpdateCompleteMessage message)
        {
            if (message.FilterNodeContent.Count != 1)
            {
                return false;
            }

            return !singleScanAutomationSettings.IsAutoPrintEnabled();
        }

        /// <summary>
        /// Weighs the shipment in the message. 
        /// </summary>
        private async Task<bool> Weigh(SingleScanFilterUpdateCompleteMessage message)
        {
            List<ShipmentEntity> shipments = (await GetShipments(message.OrderId)).ToList();

            using (ITrackedEvent telemetry = new TrackedEvent("SingleScan.Search.AutoWeigh"))
            {
                autoWeighService.ApplyWeight(shipments, telemetry);
            }
            
            List<ShipmentEntity> dirtyShipments = shipments.Where(s => s.IsDirty).ToList();

            IDictionary<ShipmentEntity, Exception> savedShipments = shippingManager.SaveShipmentsToDatabase(shipments, false);
            bool anyErrors = savedShipments.All(s => s.Value == null);
            SendShipmentChangedMessage(dirtyShipments);
            
            return anyErrors;
        }

        /// <summary>
        /// Sends the shipment changed message.
        /// </summary>
        private void SendShipmentChangedMessage(List<ShipmentEntity> dirtyShipments)
        {
            foreach (ShipmentEntity shipment in dirtyShipments)
            {
                messenger.Send(new ShipmentChangedMessage(this, shippingManager.GetShipmentAdapter(shipment)));
            }
        }

        /// <summary>
        /// Gets the weighted shipments.
        /// </summary>
        private async Task<IEnumerable<ShipmentEntity>> GetShipments(long? orderId)
        {
            if (!orderId.HasValue)
            {
                return null;
            }

            // Get all of the shipments for the order id that are not voided, this will add a new shipment if the order currently has no shipments
            ShipmentsLoadedEventArgs loadedOrders = await orderLoader.LoadAsync(new[] { orderId.Value }, ProgressDisplayOptions.NeverShow, singleScanAutomationSettings.AutoCreateShipments, Timeout.Infinite);

            IEnumerable<ShipmentEntity> shipments = loadedOrders.Shipments.Where(s => !s.Processed);
            return shipments;
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {
            filterUpdateCompleteMessagesConnection?.Dispose();
        }
    }
}
