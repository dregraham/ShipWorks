using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.Stores;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Queue that handles validating order shipping addresses in the background
    /// </summary>
    internal static class AddressValidationQueue
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AddressValidationQueue));

        static readonly List<int> validatableStatuses = new List<int>
            {
                (int) AddressValidationStatusType.Pending,
                (int) AddressValidationStatusType.Error
            };

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

                validationThread = Task.Factory.StartNew(ValidatePendingOrdersAndShipments, cancellationToken);
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
        /// Try to validate any orders and shipments that are pending validation
        /// </summary>
        private static void ValidatePendingOrdersAndShipments()
        {
            if (SqlSession.Current == null)
            {
                return;
            }

            using (SqlConnection connection = SqlSession.Current.OpenConnection())
            {
                if (!StoreManager.DoAnyStoresHaveAutomaticValidationEnabled())
                {
                    return;
                }

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
                    ValidateAddresses<OrderEntity>(new OrdersWithPendingValidationStatusPredicate(), orders => orders.Any());

                    // Validate any errors, but don't continue if any of the orders in the validated batch are still errors
                    // since that would suggest that there is still something wrong with the web service. This gets called after we attempt to validate a batch.
                    // If they are all errors, each address in the batch JUST failed.
                    ValidateAddresses<OrderEntity>(new OrdersWithErrorValidationStatusPredicate(),
                        orders => orders.Any() && orders.Any(x => x.ShipAddressValidationStatus != (int) AddressValidationStatusType.Error));

                    // Validate any pending shipments next
                    ValidatePendingShipmentAddresses();

                    // Validate any errors. See above comment about validating order errors...
                    ValidateErrorShipmentAddresses();
                }
                catch (Exception ex)
                {
                    log.Error("Error validating addresses", ex);
                }
                finally
                {
                    SqlAppLockUtility.ReleaseLock(connection, SqlAppLockName);
                }
            }
        }

        /// <summary>
        /// Validate shipments with a given address validation status
        /// </summary>
        private static void ValidatePendingShipmentAddresses()
        {
            Func<ICollection<ShipmentEntity>, bool> shouldContinue = shipments => shipments.Any();
            ValidateAddresses(new UnprocessedPendingShipmentsPredicate(), shouldContinue);
        }

        /// <summary>
        /// Validate shipments with a given address validation status
        /// </summary>
        private static void ValidateErrorShipmentAddresses()
        {
            // shouldContinue evaluates the shipments after processing them through address validation.
            // If each order in the batch still has an error (or there are no shipments) this will bail.
            Func<ICollection<ShipmentEntity>, bool> shouldContinue = shipments => shipments.Any() && shipments.Any(x => x.ShipAddressValidationStatus != (int) AddressValidationStatusType.Error);
            ValidateAddresses(new UnprocessedErrorShipmentsPredicate(), shouldContinue);
        }

        /// <summary>
        /// Validate orders with a given address validation status
        /// </summary>
        private static void ValidateAddresses<T>(IPredicateProvider predicate, Func<ICollection<T>, bool> shouldContinue) where T : EntityBase2
        {
            EntityCollection<T> pendingOrders;
            Stopwatch stopwatch = new Stopwatch();

            // The predicate gets orders in batches at a time (50 at the time of this comment).
            do
            {
                stopwatch.Restart();

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    pendingOrders = adapter.GetCollectionFromPredicate<T>(predicate);
                }

                foreach (T order in pendingOrders)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    ValidateAddressEntities(order);
                }

                stopwatch.Stop();

                int itemCount = pendingOrders.Count;

                if (itemCount > 0)
                {
                    long timePerItem = stopwatch.ElapsedMilliseconds / itemCount;
                    log.Info($"Validated {itemCount} items on 1 thread(s) in {stopwatch.ElapsedMilliseconds} ms ({timePerItem} ms/item)");
                }
            } while (shouldContinue(pendingOrders));
        }

        /// <summary>
        /// Validates the specified shipment identifier.
        /// </summary>
        private static void ValidateAddressEntities(IEntity2 entityToValidate)
        {
            try
            {
                AddressAdapter originalEntityAddress = new AddressAdapter();
                AddressAdapter.Copy(entityToValidate, "Ship", originalEntityAddress);

                if (entityToValidate != null && validatableStatuses.Contains((int) entityToValidate.Fields["ShipAddressValidationStatus"].CurrentValue))
                {
                    StoreEntity store = StoreManager.GetRelatedStore((long) entityToValidate.Fields["OrderID"].CurrentValue);
                    bool shouldAutomaticallyAdjustAddress = store.AddressValidationSetting != (int) AddressValidationStoreSettingType.ValidateAndNotify;

                    addressValidator.Validate(entityToValidate, "Ship", shouldAutomaticallyAdjustAddress,
                        (originalAddress, suggestedAddresses) =>
                        {
                            using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                            {
                                ValidatedShipAddressBase validatedAddress = CreateValidatedAddress(entityToValidate, originalAddress, suggestedAddresses, originalEntityAddress);
                                validatedAddress.Save(sqlAdapter);

                                sqlAdapter.Commit();
                            }
                        });
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
        /// A factory method to instantiate the appropriate instance of ValidatedShipAddressBase.
        /// </summary>
        private static ValidatedShipAddressBase CreateValidatedAddress(IEntity2 entity, ValidatedAddressEntity originalAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses, AddressAdapter addressAdapter)
        {
            OrderEntity order = entity as OrderEntity;
            if (order != null)
            {
                return new ValidatedOrderShipAddress(order, originalAddress, suggestedAddresses, addressAdapter);
            }

            ShipmentEntity shipment = entity as ShipmentEntity;
            if (shipment != null)
            {
                return new ValidatedShipmentShipAddress(shipment, originalAddress, suggestedAddresses);
            }

            throw new InvalidOperationException("ShipWorks could not create a validated address. An unexpected entity type was provided.");
        }

        /// <summary>
        /// Listen for entity changes from the data provider
        /// </summary>
        public static void OnOrderEntityChangeDetected(object sender, EventArgs e)
        {
            PerformValidation();
        }

        /// <summary>
        /// Listen for Shipment entity changes for the data provider.
        /// </summary>
        public static void OnShipmentEntityChangeDetected(object sender, EventArgs e)
        {
            PerformValidation();
        }
    }
}