using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Loads shipments from a list of orders in the background with progress
    /// </summary>
    public class ShipmentsLoader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipmentsLoader));
        private BlockingCollection<ShipmentEntity> shipmentsToValidate;
        private Dictionary<long, ShipmentEntity> globalShipments;
        private bool wasCanceled;
        private bool finishedLoadingShipments;
        private const int ValidateAddressesTaskCount = 2;
        IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsLoader(IWin32Window owner)
        {
            this.owner = MethodConditions.EnsureArgumentIsNotNull(owner, nameof(owner));
            globalShipments = new Dictionary<long, ShipmentEntity>();
            shipmentsToValidate = new BlockingCollection<ShipmentEntity>();
        }

        /// <summary>
        /// The maximum number of orders that we support loading at a time.
        /// </summary>
        public static int MaxAllowedOrders => 1000;

        /// <summary>
        /// Load the shipments for the given collection of orders or shipments
        /// </summary>
        public async Task<ShipmentsLoadedEventArgs> LoadAsync(IEnumerable<long> entityIDs)
        {
            MethodConditions.EnsureArgumentIsNotNull(entityIDs, nameof(entityIDs));

            List<long> entityIDsOriginalSort = entityIDs.ToList();

            int count = entityIDs.Count();

            if (count > MaxAllowedOrders)
            {
                throw new InvalidOperationException("Too many orders trying to load at once.");
            }

            bool shouldValidate = StoreManager.DoAnyStoresHaveAutomaticValidationEnabled();

            // Progress Provider
            ProgressProvider progressProvider = new ProgressProvider();

            // Load Shipment Progress Item
            ProgressItem workProgress = new ProgressItem("Load Shipments");
            progressProvider.ProgressItems.Add(workProgress);

            // Progress Dialog
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Load Shipments";
            progressDlg.Description = "ShipWorks is loading shipments for the selected orders.";
            progressDlg.Show(owner);

            Task loadShipmentsTask = TaskEx.Run(() => LoadShipmentsInternal(workProgress, entityIDsOriginalSort));
            Task validateTask = shouldValidate ? CreateValidationTask(progressProvider, count) : TaskUtility.CompletedTask;

            try
            {
                await TaskEx.WhenAll(loadShipmentsTask, validateTask);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            progressDlg.CloseForced();

            if (wasCanceled)
            {
                globalShipments.Clear();
            }

            EntityType keyType = entityIDsOriginalSort.Any() ?
                EntityUtility.GetEntityType(entityIDsOriginalSort.First()) :
                EntityType.OrderEntity;

            // Sort the list of shipments in the original keys order.
            // During loading, we reverse the keys order so that we validate addresses in reverse order
            // from what the background process does...it validates in sequential primary key ascending order.
            List<ShipmentEntity> reorderedShipments = entityIDsOriginalSort
                .Join(globalShipments, i => i, o => GetKeyFromEntity(keyType, o), (i, o) => o.Value)
                .ToList();

            return new ShipmentsLoadedEventArgs(null, wasCanceled, null, reorderedShipments);
        }

        /// <summary>
        /// Create the address validation task
        /// </summary>
        private Task CreateValidationTask(ProgressProvider progressProvider, int count)
        {
            ProgressItem validationProgress = new ProgressItem("Validate Shipment Addresses");
            progressProvider.ProgressItems.Add(validationProgress);

            return ValidateShipmentsInternal(validationProgress, count);
        }

        /// <summary>
        /// Get the key from the given entity
        /// </summary>
        private long GetKeyFromEntity(EntityType keyType, KeyValuePair<long, ShipmentEntity> o)
        {
            return keyType == EntityType.OrderEntity ? o.Value.OrderID : o.Value.ShipmentID;
        }

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadShipmentsInternal(ProgressItem workProgress, List<long> entityIDsOriginalSort)
        {
            // We need to make sure filters are up to date so profiles being applied can be as accurate as possible.
            FilterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            int count = 0;
            int total = entityIDsOriginalSort.Count;
            EntityType keyType = entityIDsOriginalSort.Any() ? EntityUtility.GetEntityType(entityIDsOriginalSort.First()) : EntityType.OrderEntity;

            workProgress.Starting();

            IOrderedEnumerable<long> orderByDescending = entityIDsOriginalSort.OrderByDescending(id => id);

            foreach (long entityID in orderByDescending)
            {
                if (workProgress.IsCancelRequested)
                {
                    wasCanceled = true;
                    break;
                }

                workProgress.Detail = $"Loading {count + 1} of {total}";

                // Execute the work
                try
                {
                    List<ShipmentEntity> iterationShipments = new List<ShipmentEntity>();

                    if (keyType == EntityType.OrderEntity)
                    {
                        bool createIfNone = UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, entityID);

                        iterationShipments.AddRange(ShippingManager.GetShipments(entityID, createIfNone));
                    }
                    else if (keyType == EntityType.ShipmentEntity)
                    {
                        ShipmentEntity shipment = ShippingManager.GetShipment(entityID);
                        if (shipment != null)
                        {
                            iterationShipments.Add(shipment);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid key type passed to load shipments.");
                    }

                    // Queue the shipments to be validated
                    foreach (ShipmentEntity shipment in iterationShipments)
                    {
                        // Add them to the global list
                        globalShipments.Add(shipment.ShipmentID, shipment);

                        // try for a few ms to add an item
                        while (!shipmentsToValidate.TryAdd(shipment, TimeSpan.FromMilliseconds(50)) && !workProgress.IsCancelRequested)
                        {
                        }
                    }
                }
                catch (SqlForeignKeyException)
                {
                    // If the order got deleted just forget it - its not an error, the shipments just don't load.
                    log.WarnFormat("Did not load shipments for entity {0} due to FK exception.", entityID);
                }

                count++;

                workProgress.PercentComplete = (100 * count) / total;
            }

            finishedLoadingShipments = true;
            shipmentsToValidate.CompleteAdding();

            workProgress.Completed();
        }

        /// <summary>
        /// Validate all the shipments on a background thread
        /// </summary>
        private async Task ValidateShipmentsInternal(ProgressItem workProgress, int initialCount)
        {
            int count = 0;
            int total = initialCount;
            workProgress.Starting();

            // Loading orders may load more than one shipment, so the actual count of shipments to
            // validate may change during the loading process
            total = finishedLoadingShipments ? globalShipments.Count : Math.Max(total, globalShipments.Count);

            workProgress.Detail = $"Validating {count} of {total}";

            using (new LoggedStopwatch(log, "ShipmentsLoader.ValidateShipmentsInternal: COMPLETED"))
            {
                await ValidateAddressesTask(workProgress, shipmentsToValidate, (shipment) =>
                    {
                        globalShipments[shipment.ShipmentID] = shipment;

                        count++;
                        workProgress.PercentComplete = (100 * count) / total;

                        if (count != total)
                        {
                            workProgress.Detail = $"Validating {count + 1} of {total}";
                        }

                        if (workProgress.IsCancelRequested)
                        {
                            wasCanceled = true;
                        }
                    });
            }

            workProgress.Completed();
        }

        /// <summary>
        /// Validate orders with a given address validation status
        /// </summary>
        private static async Task ValidateAddressesTask(ProgressItem workProgress,
            BlockingCollection<ShipmentEntity> shipmentsQueue, Action<ShipmentEntity> updateProgress)
        {
            Stopwatch stopwatch = new Stopwatch();

            while (!shipmentsQueue.IsCompleted && !workProgress.IsCancelRequested)
            {
                stopwatch.Restart();

                int itemCount = shipmentsQueue.Count();
                log.Info($"Validating {itemCount} items on {ValidateAddressesTaskCount} thread(s)...");

                // Start a number of tasks to do address validation.
                await TaskEx
                    .WhenAll(Enumerable.Range(1, ValidateAddressesTaskCount)
                        .Select(_ => TaskEx.Run(() => ValidateShipments(workProgress, shipmentsQueue, new AddressValidator(), updateProgress)))
                    );

                stopwatch.Stop();

                if (itemCount > 0)
                {
                    long timePerItem = stopwatch.ElapsedMilliseconds / itemCount;
                    log.Info($"Validated {itemCount} items on {ValidateAddressesTaskCount} thread(s) in {stopwatch.ElapsedMilliseconds} ms ({timePerItem} ms/item)");
                }
            }
        }

        /// <summary>
        /// Validate shipments from the queue
        /// </summary>
        private static async Task ValidateShipments(ProgressItem workProgress,
            BlockingCollection<ShipmentEntity> shipmentsQueue, AddressValidator addressValidator,
            Action<ShipmentEntity> updateProgress)
        {
            while (!shipmentsQueue.IsCompleted && !workProgress.IsCancelRequested)
            {
                ShipmentEntity shipment = null;

                if (shipmentsQueue.TryTake(out shipment, TimeSpan.FromMilliseconds(100)))
                {
                    // The background process could have already validated this shipment, but shipment is in our process memory
                    // and may not match the database, so reload it.
                    shipment = (ShipmentEntity) DataProvider.GetEntity(shipment.ShipmentID);

                    await ValidatedAddressManager.ValidateShipmentAsync(shipment, addressValidator);

                    updateProgress(shipment);
                }
            }
        }
    }
}
