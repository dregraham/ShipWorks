﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RestSharp.Validation;
using ShipWorks.Data;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;

namespace ShipWorks.AddressValidation
{
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

        private void Validate(long orderID)
        {

            SqlAdapter sqlAdapter = new SqlAdapter();

            OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);

            if (order != null && (AddressValidationStatusType)order.ShipAddressValidationStatus == AddressValidationStatusType.Pending)
            {
                addressValidator.Validate(order, "Ship", (originalAddress, suggestedAddresses) =>
                {
                    ValidatedAddressManager.DeleteExistingAddresses(sqlAdapter, order.OrderID);
                    ValidatedAddressManager.SaveOrderAddress(sqlAdapter, order, originalAddress, true);

                    foreach (AddressEntity address in suggestedAddresses)
                    {
                        ValidatedAddressManager.SaveOrderAddress(sqlAdapter, order, address, false);
                    }

                    order.ShipAddressValidationSuggestionCount = suggestedAddresses.Count();
                    sqlAdapter.SaveEntity(order);
                });
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)timer).Dispose();
        }
    }
}
