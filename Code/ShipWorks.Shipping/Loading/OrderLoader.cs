using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Loads shipments from a list of orders in the background with progress
    /// </summary>
    [Component]
    public class OrderLoader : IOrderLoader
    {
        private static readonly AsyncSemaphore loadingSemaphore = new AsyncSemaphore(1);
        private readonly IMessageHelper messageHelper;
        private readonly IShipmentsValidator shipmentsLoaderValidator;
        private readonly IShipmentsLoader shipmentLoader;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLoader(IMessageHelper messageHelper, IShipmentsLoader shipmentLoader,
            IShipmentsValidator shipmentsLoaderValidator, Func<Type, ILog> getLogger)
        {
            this.messageHelper = messageHelper;
            this.shipmentLoader = shipmentLoader;
            this.shipmentsLoaderValidator = shipmentsLoaderValidator;
            log = getLogger(GetType());
        }

        /// <summary>
        /// Load the shipments for the given collection of orders or shipments
        /// </summary>
        public Task<ShipmentsLoadedEventArgs> LoadAsync(IEnumerable<long> entityIDs,
            ProgressDisplayOptions displayOptions, bool createIfNoShipments, int timeoutInMilliseconds) =>
            LoadAsync(entityIDs, displayOptions, createIfNoShipments, TimeSpan.FromMilliseconds(timeoutInMilliseconds));

        /// <summary>
        /// Load the shipments for the given collection of orders or shipments
        /// </summary>
        public async Task<ShipmentsLoadedEventArgs> LoadAsync(IEnumerable<long> entityIDs,
            ProgressDisplayOptions displayOptions, bool createIfNoShipments, TimeSpan timeout)
        {
            MethodConditions.EnsureArgumentIsNotNull(entityIDs, nameof(entityIDs));

            List<long> entityIDsOriginalSort = entityIDs.ToList();

            if (entityIDs.Count() > ShipmentsLoaderConstants.MaxAllowedOrders)
            {
                throw new InvalidOperationException("Too many orders trying to load at once.");
            }

            bool wasCanceled = false;
            IDictionary<long, ShipmentEntity> globalShipments = new Dictionary<long, ShipmentEntity>();

            try
            {
                var progressProvider = new ProgressProvider();
                using (CreateProgressDialog(displayOptions, progressProvider))
                {
                    if (await loadingSemaphore.WaitAsync(timeout))
                    {
                        wasCanceled = await LoadShipments(entityIDsOriginalSort, globalShipments,
                            progressProvider, createIfNoShipments);
                    }
                    else
                    {
                        return new ShipmentsLoadedEventArgs(new Exception("Could not load order. Please try again"), false, null, new List<ShipmentEntity>());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new ShipmentsLoadedEventArgs(ex, false, null, new List<ShipmentEntity>());
            }
            finally
            {
                loadingSemaphore.Release();
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
        /// Load the shipments
        /// </summary>
        private async Task<bool> LoadShipments(List<long> orderIDs, IDictionary<long, ShipmentEntity> globalShipments,
            IProgressProvider progressProvider, bool createIfNoShipments)
        {
            using (BlockingCollection<ShipmentEntity> shipmentsToValidate = new BlockingCollection<ShipmentEntity>())
            {
                Task<bool> loadShipmentsTask = shipmentLoader
                    .StartTask(progressProvider, orderIDs, globalShipments, shipmentsToValidate, createIfNoShipments);

                Task<bool> validateTask = shipmentsLoaderValidator
                    .StartTask(progressProvider, globalShipments, shipmentsToValidate);

                return await TaskEx.WhenAll(loadShipmentsTask, validateTask)
                    .ContinueWith(task => task.Result.Any(x => x));
            }
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
    }
}
