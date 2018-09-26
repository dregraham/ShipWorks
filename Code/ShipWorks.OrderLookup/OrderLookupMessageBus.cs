using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Data service for the order lookup UI Mode
    /// </summary>
    [Component(SingleInstance = true)]
    public class OrderLookupMessageBus : IInitializeForCurrentUISession, INotifyPropertyChanged, IOrderLookupMessageBus
    {
        private readonly IMessenger messenger;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private IDisposable subscription;
        private readonly PropertyChangedHandler handler;
        private OrderEntity order;
        private bool shipmentAllowEditing;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupMessageBus(IMessenger messenger, ICarrierShipmentAdapterFactory shipmentAdapterFactory)
        {
            this.messenger = messenger;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The order that is currently in context
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OrderEntity Order
        {
            get => order;
            private set => handler.Set(nameof(Order), ref order, value, true);
        }

        /// <summary>
        /// Can the shipment be edited
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShipmentAllowEditing
        {
            get => shipmentAllowEditing;
            private set => handler.Set(nameof(Order), ref shipmentAllowEditing, value, true);
        }

        /// <summary>
        /// The shipment adapter for the order in context
        /// </summary>
        public ICarrierShipmentAdapter ShipmentAdapter { get; private set; }

        /// <summary>
        /// The pacakge adpaters for the order in context
        /// </summary>
        public IEnumerable<IPackageAdapter> PackageAdapters { get; private set; }

        /// <summary>
        /// Invoked when a property on the order object changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the property changed event
        /// </summary>
        public void RaisePropertyChanged(string propertyName) => handler.RaisePropertyChanged(propertyName);

        /// <summary>
        /// Start listening for order found messages
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription = messenger.OfType<OrderLookupSingleScanMessage>()
                .Subscribe(orderMessage => LoadOrder(orderMessage.Order));
        }
        
        /// <summary>
        /// Load an order
        /// </summary>
        private void LoadOrder(OrderEntity order)
        {
            if (order != null)
            {
                ShipmentAdapter = shipmentAdapterFactory.Get(order.Shipments.First());
                ShipmentAllowEditing = !ShipmentAdapter?.Shipment?.Processed ?? false;

                if (ShipmentAdapter != null)
                {
                    PackageAdapters = ShipmentAdapter.GetPackageAdapters();
                    ShipmentAdapter.Shipment.PropertyChanged += (s, e) => RaisePropertyChanged(e.PropertyName);
                }

                if (ShipmentAdapter?.Shipment?.Postal != null)
                {
                    ShipmentAdapter.Shipment.Postal.PropertyChanged += (s, e) => RaisePropertyChanged(e.PropertyName);
                }
            }
            
            Order = order;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }

        /// <summary>
        /// Stop listening for order found messages
        /// </summary>
        public void EndSession()
        {
            Dispose();
        }
    }
}
