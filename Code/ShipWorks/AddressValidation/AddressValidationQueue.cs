using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Queue that handles validating order shipping addresses in the background
    /// </summary>
    internal static class AddressValidationQueue
    {
        public const string SqlAppLockName = "ValidateAddresses";
        private static readonly AddressValidator addressValidator = new AddressValidator();
        private static object lockObj = new object();
        private static Task validationThread;

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

                validationThread = Task.Factory.StartNew(ValidatePendingOrders);
            }
        }

        /// <summary>
        /// Try to validate any orders that are pending validation
        /// </summary>
        private static void ValidatePendingOrders()
        {
            using (SqlConnection connection = SqlSession.Current.OpenConnection())
            {
                // Just quit if we can't get a lock because some other computer is validating
                if (SqlAppLockUtility.IsLocked(connection, SqlAppLockName) ||
                    !SqlAppLockUtility.AcquireLock(connection, SqlAppLockName))
                {
                    return;
                }

                try
                {
                    using (SqlAdapter adapter = new SqlAdapter(connection))
                    {
                        List<OrderEntity> pendingOrders;
                        LinqMetaData linqMetaData = new LinqMetaData(adapter);

                        do
                        {
                            pendingOrders = linqMetaData.Order
                                .Where(x => x.ShipAddressValidationStatus == (int) AddressValidationStatusType.Pending)
                                .Take(50)
                                .ToList();
                            pendingOrders.ForEach(x => Validate(x, adapter));
                        } while (pendingOrders.Any());
                    }
                }
                finally
                {
                    SqlAppLockUtility.ReleaseLock(connection, SqlAppLockName);
                }
            }
        }

        /// <summary>
        /// Validates the specified order identifier.
        /// </summary>
        private static void Validate(OrderEntity order, SqlAdapter adapter)
        {
            try
            {
                PersonAdapter originalShippingAddress = new PersonAdapter();
                PersonAdapter.Copy(order, "Ship", originalShippingAddress);

                if (order != null && (AddressValidationStatusType)order.ShipAddressValidationStatus == AddressValidationStatusType.Pending)
                {
                    addressValidator.Validate(order, "Ship", (originalAddress, suggestedAddresses) =>
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
        public static void OnEntityChangeDetected(object sender, EventArgs e)
        {
            PerformValidation();
        }
    }
}