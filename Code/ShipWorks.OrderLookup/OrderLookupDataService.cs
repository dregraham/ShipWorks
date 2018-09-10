using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Data service for the order lookup UI Mode
    /// </summary>
    public class OrderLookupDataService : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupDataService(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        /// <summary>
        /// The order that is currently in context
        /// </summary>
        public OrderEntity Order { get; private set; }

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
