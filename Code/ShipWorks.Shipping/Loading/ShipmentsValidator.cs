using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.AddressValidation;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Validation component for the shipments loader
    /// </summary>
    [Component]
    public class ShipmentsValidator : IShipmentsValidator
    {
        private const int ValidateAddressesTaskCount = 2;
        private readonly ILog log;
        private readonly IValidatedAddressManager validatedAddressManager;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsValidator(IValidatedAddressManager validatedAddressManager, IStoreManager storeManager,
            Func<Type, ILog> getLoggerFor)
        {
            this.storeManager = storeManager;
            this.validatedAddressManager = validatedAddressManager;
            log = getLoggerFor(typeof(ShipmentsValidator));
        }

        /// <summary>
        /// Start validating shipments as they are queued
        /// </summary>
        public Task<bool> StartTask(IProgressProvider progressProvider,
            IDictionary<long, ShipmentEntity> globalShipments, BlockingCollection<ShipmentEntity> shipmentsQueue)
        {
            bool shouldValidate = storeManager.DoAnyStoresHaveAutomaticValidationEnabled();
            if (!shouldValidate)
            {
                return Task.FromResult(false);
            }

            IProgressReporter validationProgress = progressProvider.AddItem("Validate Shipment Addresses");

            return ValidateShipmentsInternal(validationProgress, globalShipments, shipmentsQueue);
        }

        /// <summary>
        /// Validate all the shipments on a background thread
        /// </summary>
        private async Task<bool> ValidateShipmentsInternal(IProgressReporter workProgress,
            IDictionary<long, ShipmentEntity> globalShipments, BlockingCollection<ShipmentEntity> shipmentsQueue)
        {
            int count = 0;
            int total = globalShipments.Count;
            bool wasCanceled = false;

            workProgress.Starting();
            workProgress.Detail = $"Validating {count} of {total}";

            using (new LoggedStopwatch(log, "ShipmentsLoader.ValidateShipmentsInternal: COMPLETED"))
            {
                await ValidateAddressesTask(workProgress, shipmentsQueue, (shipment) =>
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
            return wasCanceled;
        }

        /// <summary>
        /// Validate orders with a given address validation status
        /// </summary>
        private async Task ValidateAddressesTask(IProgressReporter workProgress,
            BlockingCollection<ShipmentEntity> shipmentsQueue, Action<ShipmentEntity> updateProgress)
        {
            Stopwatch stopwatch = new Stopwatch();

            while (!shipmentsQueue.IsCompleted && !workProgress.IsCancelRequested)
            {
                stopwatch.Restart();

                int itemCount = shipmentsQueue.Count;
                log.Info($"Validating {itemCount} items on {ValidateAddressesTaskCount} thread(s)...");

                // Start a number of tasks to do address validation.
                await Task
                    .WhenAll(Enumerable.Range(1, ValidateAddressesTaskCount)
                        .Select(_ => Task.Run(() => ValidateShipments(workProgress, shipmentsQueue, updateProgress)))
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
        private async Task ValidateShipments(IProgressReporter workProgress,
            BlockingCollection<ShipmentEntity> shipmentsQueue,
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

                    await validatedAddressManager.ValidateShipmentAsync(shipment)
                        .ConfigureAwait(false);

                    updateProgress(shipment);
                }
            }
        }
    }
}
