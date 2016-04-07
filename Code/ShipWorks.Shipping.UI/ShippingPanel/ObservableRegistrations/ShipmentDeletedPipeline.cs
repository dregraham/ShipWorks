using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Loading;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle a deleted shipment
    /// </summary>
    public class ShipmentDeletedPipeline : IShippingPanelTransientPipeline
    {
        readonly IObservable<IShipWorksMessage> messages;
        private readonly IStoreManager storeManager;
        private readonly IOrderManager orderManager;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDeletedPipeline(IObservable<IShipWorksMessage> messages, IStoreManager storeManager, IOrderManager orderManager)
        {
            this.messages = messages;
            this.storeManager = storeManager;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messages.OfType<IEntityDeletedMessage>()
                .Where(x => ShouldUnloadShipment(x, viewModel.Shipment))
                .Subscribe(x =>
                {
                    viewModel.LoadedShipmentResult = ShippingPanelLoadedShipmentResult.Deleted;
                    viewModel.UnloadShipment();
                });
        }

        /// <summary>
        /// Should the shipment be unloaded
        /// </summary>
        private bool ShouldUnloadShipment(IEntityDeletedMessage message, ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                return false;
            }

            return IsShipmentDeleted(message, shipment.ShipmentID) ||
                IsOrderDeleted(message, shipment.OrderID) ||
                IsStoreDeleted(message, shipment) ||
                IsCustomerDeleted(message, shipment);
        }

        /// <summary>
        /// Is the shipment deleted?
        /// </summary>
        private bool IsShipmentDeleted(IEntityDeletedMessage message, long shipmentID)
        {
            return message is ShipmentDeletedMessage &&
                 message.DeletedEntityID == shipmentID;
        }

        /// <summary>
        /// Is the order deleted?
        /// </summary>
        private bool IsOrderDeleted(IEntityDeletedMessage message, long orderID)
        {
            return message is OrderDeletedMessage &&
                message.DeletedEntityID == orderID;
        }

        /// <summary>
        /// Is the store deleted?
        /// </summary>
        private bool IsStoreDeleted(IEntityDeletedMessage message, ShipmentEntity shipment) =>
            IsOrderAssociationDeleted<StoreDeletedMessage>(shipment, x => x.StoreID, message, x => x.DeletedEntityID);

        /// <summary>
        /// Is the customer deleted?
        /// </summary>
        private bool IsCustomerDeleted(IEntityDeletedMessage message, ShipmentEntity shipment) =>
            IsOrderAssociationDeleted<CustomerDeletedMessage>(shipment, x => x.CustomerID, message, x => x.DeletedEntityID);

        /// <summary>
        /// Is this the deletion of an order association?
        /// </summary>
        private bool IsOrderAssociationDeleted<T>(ShipmentEntity shipment, Func<OrderEntity, long> getAssociationIdFromOrder,
            IEntityDeletedMessage message, Func<T, long> getAssociationIdFromMessage)
        {
            if (!(message is T))
            {
                return false;
            }

            long associationId = getAssociationIdFromMessage((T) message);

            if (shipment.Order != null)
            {
                return associationId == getAssociationIdFromOrder(shipment.Order);
            }

            OrderEntity order = orderManager.FetchOrder(shipment.OrderID);
            return order == null || associationId == getAssociationIdFromOrder(order);
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
