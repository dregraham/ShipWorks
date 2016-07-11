﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Loads shipments from a list of orders in the background with progress
    /// </summary>
    public class ShipmentsLoader : IShipmentsLoader, IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipmentsLoader));
        private readonly BlockingCollection<ShipmentEntity> shipmentsToValidate;
        private readonly Dictionary<long, ShipmentEntity> globalShipments;
        private bool wasCanceled;
        private bool finishedLoadingShipments;
        private const int ValidateAddressesTaskCount = 2;
        private readonly IShipmentFactory shipmentFactory;
        private readonly IOrderManager orderManager;
        private readonly IStoreManager storeManager;
        private readonly Func<IWin32Window> ownerCreator;
        private readonly IMessageHelper messageHelper;
        private readonly ISecurityContext securityContext;
        private readonly IFilterHelper filterHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsLoader(IShipmentFactory shipmentFactory,
            IOrderManager orderManager, IStoreManager storeManager, Func<IWin32Window> ownerCreator,
            IMessageHelper messageHelper, ISecurityContext securityContext, IFilterHelper filterHelper)
        {
            this.shipmentFactory = shipmentFactory;
            this.orderManager = orderManager;
            this.storeManager = storeManager;
            this.ownerCreator = ownerCreator;
            this.messageHelper = messageHelper;
            this.securityContext = securityContext;
            this.filterHelper = filterHelper;

            globalShipments = new Dictionary<long, ShipmentEntity>();
            shipmentsToValidate = new BlockingCollection<ShipmentEntity>();
        }

        /// <summary>
        /// Load the shipments for the given collection of orders or shipments
        /// </summary>
        public async Task<ShipmentsLoadedEventArgs> LoadAsync(IEnumerable<long> entityIDs, ProgressDisplayOptions displayOptions)
        {
            MethodConditions.EnsureArgumentIsNotNull(entityIDs, nameof(entityIDs));

            List<long> entityIDsOriginalSort = entityIDs.ToList();

            int count = entityIDs.Count();

            if (count > ShipmentsLoaderConstants.MaxAllowedOrders)
            {
                throw new InvalidOperationException("Too many orders trying to load at once.");
            }

            bool shouldValidate = storeManager.DoAnyStoresHaveAutomaticValidationEnabled();

            ProgressProvider progressProvider = new ProgressProvider();
            IProgressReporter workProgress = progressProvider.AddItem("Load Shipments");

            // Progress Dialog
            using (CreateProgressDialog(displayOptions, progressProvider))
            {
                Task loadShipmentsTask = TaskEx.Run(() => LoadShipmentsInternal(workProgress, entityIDsOriginalSort));
                Task validateTask = shouldValidate ? CreateValidationTask(progressProvider, count) : TaskUtility.CompletedTask;

                try
                {
                    await TaskEx.WhenAll(loadShipmentsTask, validateTask).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return new ShipmentsLoadedEventArgs(ex, wasCanceled, null, new List<ShipmentEntity>());
                }
            }

            if (wasCanceled)
            {
                globalShipments.Clear();
            }

            // Sort the list of shipments in the original keys order.
            // During loading, we reverse the keys order so that we validate addresses in reverse order
            // from what the background process does...it validates in sequential primary key ascending order.
            List<ShipmentEntity> reorderedShipments = entityIDsOriginalSort
                .Join(globalShipments, i => i, o => o.Value.OrderID, (i, o) => o.Value)
                .ToList();

            return new ShipmentsLoadedEventArgs(null, wasCanceled, null, reorderedShipments);
        }

        /// <summary>
        /// Create the loading progress dialog
        /// </summary>
        private IDisposable CreateProgressDialog(ProgressDisplayOptions options, IProgressProvider progressProvider)
        {
            return options == ProgressDisplayOptions.NeverShow ?
                Disposable.Empty :
                messageHelper.ShowProgressDialog("Load Shipments",
                    "ShipWorks is loading shipments for the selected orders.",
                    progressProvider, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Create the address validation task
        /// </summary>
        private Task CreateValidationTask(IProgressProvider progressProvider, int count)
        {
            IProgressReporter validationProgress = progressProvider.AddItem("Validate Shipment Addresses");

            return ValidateShipmentsInternal(validationProgress, count);
        }

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadShipmentsInternal(IProgressReporter workProgress, List<long> entityIDsOriginalSort)
        {
            // We need to make sure filters are up to date so profiles being applied can be as accurate as possible.
            filterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            int count = 0;
            int total = entityIDsOriginalSort.Count;
            workProgress.Starting();

            IOrderedEnumerable<long> orderByDescending = entityIDsOriginalSort.OrderByDescending(id => id);

            foreach (IEnumerable<long> orders in orderByDescending.SplitIntoChunksOf(100))
            {
                foreach (OrderEntity order in orderManager.LoadOrders(orders, fullOrderPrefetchPath.Value))
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
                        if (securityContext.HasPermission(PermissionType.ShipmentsCreateEditProcess, order.OrderID))
                        {
                            shipmentFactory.AutoCreateIfNecessary(order);
                        }

                        // Queue the shipments to be validated
                        foreach (ShipmentEntity shipment in order.Shipments)
                        {
                            globalShipments.Add(shipment.ShipmentID, shipment);

                            while (!shipmentsToValidate.TryAdd(shipment, TimeSpan.FromMilliseconds(50)) && !workProgress.IsCancelRequested)
                            {
                                // We may need to try multiple times to successfully add to queue
                            }
                        }
                    }
                    catch (SqlForeignKeyException)
                    {
                        // If the order got deleted just forget it - its not an error, the shipments just don't load.
                        log.WarnFormat("Did not load shipments for entity {0} due to FK exception.", order.OrderID);
                    }

                    count++;

                    workProgress.PercentComplete = (100 * count) / total;
                }
            }

            finishedLoadingShipments = true;
            shipmentsToValidate.CompleteAdding();

            workProgress.Completed();
        }

        /// <summary>
        /// Validate all the shipments on a background thread
        /// </summary>
        private async Task ValidateShipmentsInternal(IProgressReporter workProgress, int initialCount)
        {
            int count = 0;
            workProgress.Starting();

            // Loading orders may load more than one shipment, so the actual count of shipments to
            // validate may change during the loading process
            int total = finishedLoadingShipments ? globalShipments.Count : Math.Max(initialCount, globalShipments.Count);

            workProgress.Detail = $"Validating {count} of {total}";

            using (new LoggedStopwatch(log, "ShipmentsLoader.ValidateShipmentsInternal: COMPLETED"))
            {
                await ValidateAddressesTask(workProgress, shipmentsToValidate, (shipment) =>
                {
                    globalShipments[shipment.ShipmentID] = shipment;

                    count++;
                    total = Math.Max(total, globalShipments.Count);

                    workProgress.PercentComplete = (100 * count) / total;

                    if (count != total)
                    {
                        workProgress.Detail = $"Validating {count + 1} of {total}";
                    }

                    if (workProgress.IsCancelRequested)
                    {
                        wasCanceled = true;
                    }
                }).ConfigureAwait(false);
            }

            workProgress.Completed();
        }

        /// <summary>
        /// Validate orders with a given address validation status
        /// </summary>
        private static async Task ValidateAddressesTask(IProgressReporter workProgress,
            BlockingCollection<ShipmentEntity> shipmentsQueue, Action<ShipmentEntity> updateProgress)
        {
            Stopwatch stopwatch = new Stopwatch();

            while (!shipmentsQueue.IsCompleted && !workProgress.IsCancelRequested)
            {
                stopwatch.Restart();

                int itemCount = shipmentsQueue.Count;
                log.Info($"Validating {itemCount} items on {ValidateAddressesTaskCount} thread(s)...");

                // Start a number of tasks to do address validation.
                await TaskEx
                    .WhenAll(Enumerable.Range(1, ValidateAddressesTaskCount)
                        .Select(_ => TaskEx.Run(() => ValidateShipments(workProgress, shipmentsQueue, new AddressValidator(), updateProgress)))
                    ).ConfigureAwait(false);

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
        private static async Task ValidateShipments(IProgressReporter workProgress,
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
                    using (SqlAdapter adapter = SqlAdapter.Create(false))
                    {
                        adapter.FetchEntity(shipment);
                    }

                    await ValidatedAddressManager.ValidateShipmentAsync(shipment, addressValidator)
                        .ConfigureAwait(false);

                    updateProgress(shipment);
                }
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            shipmentsToValidate.Dispose();
        }

        /// <summary>
        /// Create the pre-fetch path used to load an order
        /// </summary>
        private static Lazy<IPrefetchPath2> fullOrderPrefetchPath = new Lazy<IPrefetchPath2>(() =>
        {
            IPrefetchPath2 prefetchPath = new PrefetchPath2(EntityType.OrderEntity);

            prefetchPath.Add(OrderEntity.PrefetchPathStore);

            IPrefetchPathElement2 itemsPath = prefetchPath.Add(OrderEntity.PrefetchPathOrderItems);
            itemsPath.SubPath.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);

            IPrefetchPathElement2 shipmentsPath = prefetchPath.Add(OrderEntity.PrefetchPathShipments);

            IPrefetchPathElement2 upsShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathUps);
            upsShipmentPath.SubPath.Add(UpsShipmentEntity.PrefetchPathPackages);

            IPrefetchPathElement2 postalShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathPostal);
            postalShipmentPath.SubPath.Add(PostalShipmentEntity.PrefetchPathUsps);
            postalShipmentPath.SubPath.Add(PostalShipmentEntity.PrefetchPathEndicia);

            IPrefetchPathElement2 iParcelShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathIParcel);
            iParcelShipmentPath.SubPath.Add(IParcelShipmentEntity.PrefetchPathPackages);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathOnTrac);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathAmazon);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathBestRate);

            IPrefetchPathElement2 fedexShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathFedEx);
            fedexShipmentPath.SubPath.Add(FedExShipmentEntity.PrefetchPathPackages);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathOther);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathInsurancePolicy);
            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathCustomsItems);
            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathValidatedAddress);

            return prefetchPath;
        });
    }
}
