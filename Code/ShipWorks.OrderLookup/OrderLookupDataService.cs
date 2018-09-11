using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
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
    public class OrderLookupDataService : IInitializeForCurrentUISession, INotifyPropertyChanged, IOrderLookupDataService
    {
        private readonly IMessenger messenger;
        private IDisposable subscription;
        protected readonly PropertyChangedHandler handler;
        private OrderEntity order;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupDataService(IMessenger messenger)
        {
            this.messenger = messenger;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The order that is currently in context
        /// </summary>
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
        public void RaisePropertyChanged(string propertyName) 
            => handler.RaisePropertyChanged(propertyName);

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

        /// <summary>
        /// Start listening for order found messages
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription = messenger.OfType<OrderLookupSingleScanMessage>()
                .Subscribe(orderMessage => Order = orderMessage.Order);
        }
    }
}
