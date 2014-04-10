using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    internal static class AddressValidationQueue
    {
        private static ConcurrentQueue<long> orderQueue = new ConcurrentQueue<long>();
        private static readonly Timer timer = new Timer(OnTimerTick, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5));
        private static readonly AddressValidator addressValidator = new AddressValidator();

        /// <summary>
        /// Enqueue the order.
        /// </summary>
        /// <param name="order">The order.</param>
        public static void Enqueue(OrderEntity order)
        {
            if ((AddressValidationStatusType)order.ShipAddressValidationStatus == AddressValidationStatusType.Pending)
            {
                orderQueue.Enqueue(order.OrderID);
            }
        }

        /// <summary>
        /// Load any pending validations so they can be processed
        /// </summary>
        public static void ReloadPendingValidations()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                // Get a list of all order ids that are pending address validation
                AdapterAddressValidationDataAccess dataAccess = new AdapterAddressValidationDataAccess(adapter);
                IEnumerable<long> orderIds = dataAccess.LinqCollections
                    .Order
                    .Where(x => x.ShipAddressValidationStatus == (int)AddressValidationStatusType.Pending)
                    .Select(x => x.OrderID);

                foreach (long id in orderIds)
                {
                    orderQueue.Enqueue(id);
                }
            }
        }

        /// <summary>
        /// Clears the queue so that processing stops immediately
        /// </summary>
        public static void Clear()
        {
            orderQueue = new ConcurrentQueue<long>();
        }

        /// <summary>
        /// Called when [timer tick].
        /// </summary>
        private static void OnTimerTick(object state)
        {
            long orderID;

            while (orderQueue.TryDequeue(out orderID))
            {
                Validate(orderID);
            }

            // Tell the timer to check again in 5 seconds
            timer.Change(5000, Timeout.Infinite);
        }

        /// <summary>
        /// Validates the specified order identifier.
        /// </summary>
        /// <param name="orderID">The order identifier.</param>
        private static void Validate(long orderID)
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
        private static void CallValidate(long orderID)
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
    }
}