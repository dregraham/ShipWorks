using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Data service for the order lookup UI Mode
    /// </summary>
    public class OrderLookupDataService : IInitializeForCurrentUISession, INotifyPropertyChanged
    {
        private readonly IMessenger messenger;
        private IDisposable subscription;
        protected readonly PropertyChangedHandler handler;

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
        public OrderEntity Order { get; private set; }

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
            subscription = messenger.OfType<OrderFoundMessage>()
                .Subscribe(x => Order = x.Order);
        }
    }
}
