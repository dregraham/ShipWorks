using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Tracking;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.Stores;
using log4net;
using ShipWorks.Stores.Content.Panels;

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
                    ValidateOrderAddresses(AddressValidationStatusType.Pending, orders => orders.Any());

                    // Validate any errors, but don't continue if any of the orders in the validated batch are still errors
                    // since that would suggest that there is still something wrong with the web service. This gets called after we attempt to validate a batch.
                    // If they are all errors, each address in the batch JUST failed.
                    ValidateOrderAddresses(AddressValidationStatusType.Error,
                        orders => orders.Any() && orders.All(x => x.ShipAddressValidationStatus != (int)AddressValidationStatusType.Error));

                    // Validate any pending shipments next
                    ValidateShipmentAddresses(AddressValidationStatusType.Pending, shipments => shipments.Any());

                    // Validate any errors. See above comment about validating order errors...
                    ValidateShipmentAddresses(AddressValidationStatusType.Error,
                        shipments => shipments.Any() && shipments.All(x => x.ShipAddressValidationStatus != (int)AddressValidationStatusType.Error));
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
        private static void ValidateOrderAddresses(AddressValidationStatusType statusToValidate, Func<List<OrderEntity>, bool> shouldContinue)
        {
            List<OrderEntity> pendingOrders;

            do
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    LinqMetaData linqMetaData = new LinqMetaData(adapter);

                    pendingOrders = linqMetaData.Order
                        .Where(x => x.ShipAddressValidationStatus == (int)statusToValidate)
                        .Take(50)
                        .ToList();
                }

                pendingOrders.ForEach(orderEntity =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    ValidateAddressEntities(orderEntity);
                });

            } while (shouldContinue(pendingOrders));
        }

        /// <summary>
        /// Validate shipments with a given address validation status
        /// </summary>
        private static void ValidateShipmentAddresses(AddressValidationStatusType statusToValidate, Func<List<ShipmentEntity>, bool> shouldContinue)
        {
            List<ShipmentEntity> pendingShipments;

            do
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    LinqMetaData linqMetaData = new LinqMetaData(adapter);

                    pendingShipments = linqMetaData.Shipment
                        .Where(x => x.ShipAddressValidationStatus == (int)statusToValidate && !x.Processed)
                        .Take(50)
                        .ToList();
                }

                pendingShipments.ForEach(shipment =>
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    ValidateAddressEntities(shipment);
                });

            } while (shouldContinue(pendingShipments));
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

                if (entityToValidate != null && validatableStatuses.Contains((int)entityToValidate.Fields["ShipAddressValidationStatus"].CurrentValue))
                {
                    StoreEntity store = StoreManager.GetRelatedStore((long)entityToValidate.Fields["OrderID"].CurrentValue);
                    bool shouldAutomaticallyAdjustAddress = store.AddressValidationSetting != (int)AddressValidationStoreSettingType.ValidateAndNotify;

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