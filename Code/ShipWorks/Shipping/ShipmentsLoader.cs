using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using Interapptive.Shared;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Filters;

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
        private Control owner;
        private object tag;
        private List<long> entityIDsOriginalSort;
        private const int ValidateAddressesTaskCount = 2;

        /// <summary>
        /// Raised when an asyn load as completed
        /// </summary>
        public event ShipmentsLoadedEventHandler LoadCompleted;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsLoader(Control owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.owner = owner;
            globalShipments = new Dictionary<long, ShipmentEntity>();
            shipmentsToValidate = new BlockingCollection<ShipmentEntity>();
        }

        /// <summary>
        /// The maximum number of orders that we support loading at a time.
        /// </summary>
        public static int MaxAllowedOrders
        {
            get { return 1000; }
        }

        /// <summary>
        /// User defined data that can be associated with the loader
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// Load the shipments for the given collection of orders or shipments
        /// </summary>
        public void LoadAsync(IEnumerable<long> entityIDs)
        {
            if (entityIDs == null)
            {
                throw new ArgumentNullException("entityIDs");
            }

            entityIDsOriginalSort = entityIDs.ToList();

            int count = entityIDsOriginalSort.Count();

            if (count > MaxAllowedOrders)
            {
                throw new InvalidOperationException("Too many orders trying to load at once.");
            }

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

            bool shouldValidate = StoreManager.DoAnyStoresHaveAutomaticValidationEnabled();

            // Start a task to load shipments
            TaskEx.Run(() =>
                       {
                          LoadShipmentsInternal(workProgress);
                       })
                  .ContinueWith(task =>
                      {
                          finishedLoadingShipments = true;

                          if (!shouldValidate)
                          {
                              FinishLoadingShipments(progressDlg);
                          }
                      });

            if (shouldValidate)
            {
                // Validate Shipment Progress Item
                ProgressItem validationProgress = new ProgressItem("Validate Shipment Addresses");
                progressProvider.ProgressItems.Add(validationProgress);

                // Start a task to validate shipments
                TaskEx.Run(async () =>
                           {
                              await ValidateShipmentsInternal(validationProgress, count);
                           })
                      .ContinueWith(task =>
                          {
                              FinishLoadingShipments(progressDlg);
                          });
            }
        }

        /// <summary>
        /// Finish the loading process
        /// </summary>
        private void FinishLoadingShipments(ProgressDlg progressDlg)
        {
            owner.Invoke((Action)(progressDlg.CloseForced));

            OnLoadShipmentsCompleted();
        }

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadShipmentsInternal(ProgressItem workProgress)
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
                        // Update the globalShipments list with the reloaded shipment so that the correct one gets sent
                        globalShipments[shipment.ShipmentID] = shipment;

                        count++;
                        workProgress.PercentComplete = (100*count)/total;

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
        private static async Task ValidateAddressesTask(ProgressItem workProgress, BlockingCollection<ShipmentEntity> shipmentsQueue, Action<ShipmentEntity> updateProgress )
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
                        .Select(x => TaskEx.Run(() =>
                            {
                                ValidateShipments(workProgress, shipmentsQueue, new AddressValidator(), updateProgress);
                            })
                        )
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
        private static void ValidateShipments(ProgressItem workProgress, BlockingCollection<ShipmentEntity> shipmentsQueue, AddressValidator addressValidator, Action<ShipmentEntity> updateProgress)
        {
            while (!shipmentsQueue.IsCompleted && !workProgress.IsCancelRequested)
            {
                ShipmentEntity shipment = null;

                if (shipmentsQueue.TryTake(out shipment, TimeSpan.FromMilliseconds(100)))
                {
                    // The background process could have already validated this shipment, but shipment is in our process memory
                    // and may not match the database, so reload it.
                    shipment = (ShipmentEntity)DataProvider.GetEntity(shipment.ShipmentID);

                    ValidatedAddressManager.ValidateShipment(shipment, addressValidator);

                    updateProgress(shipment);
                }
            }
        }

        /// <summary>
        /// The async loading of shipments for shipping has completed
        /// </summary>
        void OnLoadShipmentsCompleted()
        {
            if (wasCanceled)
            {
                globalShipments.Clear();
            }

            ShipmentsLoadedEventHandler handler = LoadCompleted;
            if (handler != null)
            {
                EntityType keyType = entityIDsOriginalSort.Any() ? EntityUtility.GetEntityType(entityIDsOriginalSort.First()) : EntityType.OrderEntity;

                // Sort the list of shipments in the original keys order.  
                // During loading, we reverse the keys order so that we validate addresses in reverse order
                // from what the background process does...it validates in sequential primary key ascending order.
                IEnumerable<ShipmentEntity> orderedByIDList = from i in entityIDsOriginalSort
                                      join o in globalShipments
                                      on i equals (keyType == EntityType.OrderEntity ? o.Value.OrderID : o.Value.ShipmentID)
                                      select o.Value;
                
                ShipmentsLoadedEventArgs args = new ShipmentsLoadedEventArgs(null, wasCanceled, null, orderedByIDList.ToList());
                if (owner.InvokeRequired)
                {
                    owner.Invoke((Action)(() => handler(this, args)));
                }
                else
                {
                    handler(this, args);   
                }
            }
        }
    }
}
