using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Queue that handles validating order shipping addresses in the background
    /// </summary>
    internal class AddressValidationQueue : IDisposable
    {
        private static readonly Lazy<ConcurrentQueue<long>> lazyOrderQueue = new Lazy<ConcurrentQueue<long>>(() => new ConcurrentQueue<long>());

        private static ConcurrentQueue<long> orderQueue;
        private static readonly object timerLock = new object();
        private Timer timer;

        private readonly AddressValidator addressValidator = new AddressValidator();

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressValidationQueue"/> class.
        /// </summary>
        public AddressValidationQueue()
        {
            orderQueue = lazyOrderQueue.Value;
            timer = new Timer(OnTimerTick, null, 0, 1000);
        }

        /// <summary>
        /// Enqueue the order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void Enqueue(OrderEntity order)
        {
            if ((AddressValidationStatusType)order.ShipAddressValidationStatus == AddressValidationStatusType.Pending)
            {
                orderQueue.Enqueue(order.OrderID);
            }
        }

        /// <summary>
        /// Called when [timer tick].
        /// </summary>
        private void OnTimerTick(object state)
        {
            if (!Monitor.TryEnter(timerLock))
            {
                // already in timer. Exit.
                return;
            }

            long orderID;

            while (orderQueue.TryDequeue(out orderID))
            {
                Validate(orderID);
            }

            // then release the lock
            Monitor.Exit(timerLock);
        }

        /// <summary>
        /// Validates the specified order identifier.
        /// </summary>
        /// <param name="orderID">The order identifier.</param>
        private void Validate(long orderID)
        {
            try
            {
                CallValidate(orderID);
            }
            catch (ObjectDeletedException)
            {
                // object has been deleted, no more need to validate.
            }
            catch (SqlForeignKeyException)
            {
                // object has been deleted, no more need to validate.
            }
            catch (ORMConcurrencyException)
            {
                Validate(orderID);
            }
        }

        /// <summary>
        /// Actually calls validation and saves.
        /// </summary>
        /// <param name="orderID">The order identifier.</param>
        private void CallValidate(long orderID)
        {
            OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);
            
            PersonAdapter originalShippingAddress = new PersonAdapter();
            PersonAdapter.Copy(order, "Ship", originalShippingAddress);

            if (order != null && (AddressValidationStatusType)order.ShipAddressValidationStatus == AddressValidationStatusType.Pending)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    addressValidator.Validate(order, "Ship", (originalAddress, suggestedAddresses) =>
                        ValidatedAddressManager.SaveValidatedOrder(adapter, order, originalShippingAddress, originalAddress, suggestedAddresses));
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            timer.Dispose();
        }
    }
}