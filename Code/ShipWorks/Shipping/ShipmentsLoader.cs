using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using log4net;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Filters;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Common.Threading;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Loads shipments from a list of orders in the background with progress
    /// </summary>
    public class ShipmentsLoader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentsLoader));

        ConcurrentQueue<ShipmentEntity> shipmentsToValidate; 
        List<ShipmentEntity> globalShipments;
        bool wasCanceled;
        bool finishedLoadingShipments;
        Control owner;

        /// <summary>
        /// Raised when an asyn load as completed
        /// </summary>
        public event ShipmentsLoadedEventHandler LoadCompleted;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsLoader(Control owner)
        {
            this.owner = MethodConditions.EnsureArgumentIsNotNull(owner, nameof(owner));
            globalShipments = new List<ShipmentEntity>();
            shipmentsToValidate = new ConcurrentQueue<ShipmentEntity>();
        }

        /// <summary>
        /// The maximum number of orders that we support loading at a time.
        /// </summary>
        public static int MaxAllowedOrders => 1000;

        /// <summary>
        /// User defined data that can be associated with the loader
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Load the shipments for the given collection of orders or shipments
        /// </summary>
        public async Task LoadAsync(IEnumerable<long> keys)
        {
            MethodConditions.EnsureArgumentIsNotNull(keys, nameof(keys));

            int count = keys.Count();

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
            
            Task loadShipmentsTask = TaskEx.Run(() => {
                LoadShipmentsInternal(workProgress, keys.ToList());
                finishedLoadingShipments = true;
            });
            
            Task validateTask;

            if (ShouldValidate)
            {
                // Validate Shipment Progress Item
                ProgressItem validationProgress = new ProgressItem("Validate Shipment Addresses");
                progressProvider.ProgressItems.Add(validationProgress);

                validateTask = ValidateShipmentsInternal(validationProgress, count); 
            }
            else
            {
                validateTask = TaskUtility.CompletedTask;
            }

            try
            {
                await TaskEx.WhenAll(loadShipmentsTask, validateTask);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            
            progressDlg.CloseForced();
            OnLoadShipmentsCompleted();
        }

        private bool ShouldValidate => StoreManager.DoAnyStoresHaveAutomaticValidationEnabled();

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        private void LoadShipmentsInternal(ProgressItem workProgress, IList<long> keys)
        {
            // We need to make sure filters are up to date so profiles being applied can be as accurate as possible.
            FilterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            int count = 0;
            int total = keys.Count;
            EntityType keyType = keys.Any() ? EntityUtility.GetEntityType(keys.First()) : EntityType.OrderEntity;

            workProgress.Starting();

            foreach (long entityID in keys)
            {
                if (workProgress.IsCancelRequested)
                {
                    wasCanceled = true;
                    break;
                }

                workProgress.Detail = string.Format("Loading {0} of {1}", count + 1, total);

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

                    // Add them to the global list
                    globalShipments.AddRange(iterationShipments);

                    // Queue the shipments to be validated
                    foreach (ShipmentEntity shipment in iterationShipments)
                    {
                        shipmentsToValidate.Enqueue(shipment);    
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

            workProgress.Completed();
        }

        /// <summary>
        /// Validate all the shipments on a background thread
        /// </summary>
        private async Task ValidateShipmentsInternal(ProgressItem workProgress, int initialCount)
        {
            // We need to make sure filters are up to date so profiles being applied can be as accurate as possible.
            FilterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            int count = 0;
            int total = initialCount;
            workProgress.Starting();

            AddressValidator addressValidator = new AddressValidator();
            ShipmentEntity shipment;

            // Keep trying to validate while we have shipments queued or we're not finished loading shipments
            while (shipmentsToValidate.TryDequeue(out shipment) || !finishedLoadingShipments)
            {
                if (shipment == null)
                {
                    continue;
                }

                // Loading orders may load more than one shipment, so the actual count of shipments to
                // validate may change during the loading process
                total = finishedLoadingShipments ? globalShipments.Count : Math.Max(total, globalShipments.Count);

                if (workProgress.IsCancelRequested)
                {
                    wasCanceled = true;
                    break;
                }

                workProgress.Detail = string.Format("Validating {0} of {1}", count + 1, total);

                await ValidatedAddressManager.ValidateShipmentAsync(shipment, addressValidator);

                count++;

                workProgress.PercentComplete = (100 * count) / total;
            }

            workProgress.Completed();
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
                ShipmentsLoadedEventArgs args = new ShipmentsLoadedEventArgs(null, wasCanceled, null, globalShipments);
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
