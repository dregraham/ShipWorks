using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Data service for the order lookup UI Mode
    /// </summary>
    [Component(SingleInstance = true)]
    public class OrderLookupMessageBus : IInitializeForCurrentUISession, INotifyPropertyChanged, IOrderLookupMessageBus
    {
        private readonly IMessenger messenger;
        private IDisposable subscription;
        private readonly PropertyChangedHandler handler;
        private OrderEntity order;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupMessageBus(IMessenger messenger)
        {
            this.messenger = messenger;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The order that is currently in context
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OrderEntity Order
        {
            get => order;
            private set => handler.Set(nameof(Order), ref order, value);
        }

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
                .Subscribe(orderMessage => Order = orderMessage.Order);
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
