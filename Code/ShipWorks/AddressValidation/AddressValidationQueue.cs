﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.Stores;
using log4net;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Queue that handles validating order shipping addresses in the background
    /// </summary>
    internal static class AddressValidationQueue
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AddressValidationQueue));

        public const string SqlAppLockName = "ValidateAddresses";
        private static readonly AddressValidator addressValidator = new AddressValidator();
        private static object lockObj = new object();
        private static Task validationThread;
        private static CancellationToken cancellationToken;

        /// <summary>
        /// Attempt to validate any pending orders
        /// </summary>
        public static void PerformValidation()
        {
            lock (lockObj)
            {
                if (validationThread != null && !validationThread.IsCompleted)
                {
                    return;
                }

                validationThread = Task.Factory.StartNew(ValidatePendingOrders, cancellationToken);
            }
        }

        /// <summary>
        /// Attempt to validate any pending orders
        /// </summary>
        public static void PerformValidation(CancellationToken token)
        {
            cancellationToken = token;

            PerformValidation();
        }

        /// <summary>
        /// Try to validate any orders that are pending validation
        /// </summary>
        private static void ValidatePendingOrders()
        {
            if (SqlSession.Current == null)
            {
                return;
            }

            using (SqlConnection connection = SqlSession.Current.OpenConnection())
            {
                log.InfoFormat("Acquiring Lock - {0}", SqlAppLockName);
                // Just quit if we can't get a lock because some other computer is validating
                if (!SqlAppLockUtility.AcquireLock(connection, SqlAppLockName))
                {
                    log.InfoFormat("Could not acquire Lock - {0}", SqlAppLockName);
                    return;
                }

                log.InfoFormat("Lock Acquired - {0}", SqlAppLockName);

                try
                {
                    // Validate any pending orders first
                    ValidateAddresses(connection, AddressValidationStatusType.Pending, orders => orders.Any());

                    // Validate any errors, but don't continue if any of the orders in the validated batch are still errors
                    // since that would suggest that there is still something wrong with the web service
                    ValidateAddresses(connection, AddressValidationStatusType.Error, 
                        orders => orders.Any() && orders.All(x => x.ShipAddressValidationStatus != (int) AddressValidationStatusType.Error));
                }
                finally
                {
                    SqlAppLockUtility.ReleaseLock(connection, SqlAppLockName);
                }
            }
        }

        /// <summary>
        /// Validate orders with a given address validation status
        /// </summary>
        private static void ValidateAddresses(SqlConnection connection, AddressValidationStatusType statusToValidate, Func<List<OrderEntity>, bool> shouldContinue)
        {
            List<OrderEntity> pendingOrders;

            do
            {
                using (SqlAdapter adapter = new SqlAdapter(connection))
                {
                    LinqMetaData linqMetaData = new LinqMetaData(adapter);

                    pendingOrders = linqMetaData.Order
                        .Where(x => x.ShipAddressValidationStatus == (int) statusToValidate)
                        .Take(50)
                        .ToList();

                    pendingOrders.ForEach(x =>
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        Validate(x, adapter);
                    });
                }
            } while (shouldContinue(pendingOrders));
        }

        /// <summary>
        /// Validates the specified order identifier.
        /// </summary>
        private static void Validate(OrderEntity order, SqlAdapter adapter)
        {
            try
            {
                AddressAdapter originalShippingAddress = new AddressAdapter();
                AddressAdapter.Copy(order, "Ship", originalShippingAddress);

                if (order != null && (AddressValidationStatusType)order.ShipAddressValidationStatus == AddressValidationStatusType.Pending)
                {
                    StoreEntity store = StoreManager.GetRelatedStore(order.OrderID);
                    bool shouldAutomaticallyAdjustAddress = store.AddressValidationSetting != (int) AddressValidationStoreSettingType.ValidateAndNotify;

                    addressValidator.Validate(order, "Ship", shouldAutomaticallyAdjustAddress, (originalAddress, suggestedAddresses) =>
                            ValidatedAddressManager.SaveValidatedOrder(adapter, order, originalShippingAddress, originalAddress, suggestedAddresses));
                }
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
                // Don't worry about this...  The next pass through will grab this order again
            }
        }

        /// <summary>
        /// Listen for entity changes from the data provider
        /// </summary>
        public static void OnOrderEntityChangeDetected(object sender, EventArgs e)
        {
            PerformValidation();
        }
    }
}