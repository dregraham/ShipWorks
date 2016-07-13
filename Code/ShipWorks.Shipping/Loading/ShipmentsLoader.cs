using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Loading
{
    [Component]
    public class ShipmentsLoader : IShipmentsLoader
    {
        private readonly IFilterHelper filterHelper;
        private readonly ILog log;
        private readonly IOrderManager orderManager;
        private readonly IShipmentFactory shipmentFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsLoader(IShipmentFactory shipmentFactory, IOrderManager orderManager,
            IFilterHelper filterHelper, Func<Type, ILog> getLogger)
        {
            this.shipmentFactory = shipmentFactory;
            this.orderManager = orderManager;
            this.filterHelper = filterHelper;
            log = getLogger(GetType());
        }

        public Task<bool> StartTask(IProgressProvider progressProvider, List<long> orderIDs,
            IDictionary<long, ShipmentEntity> globalShipments, BlockingCollection<ShipmentEntity> shipmentsToValidate)
        {
            IProgressReporter workProgress = progressProvider.AddItem("Load Shipments");

            return TaskEx.Run(() => LoadShipments(workProgress, orderIDs, globalShipments, shipmentsToValidate));
        }

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        private bool LoadShipments(IProgressReporter workProgress, List<long> entityIDsOriginalSort,
            IDictionary<long, ShipmentEntity> globalShipments, BlockingCollection<ShipmentEntity> shipmentsToValidate)
        {
            using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("LoadShipments"))
            {
                bool wasCanceled = false;

                // We need to make sure filters are up to date so profiles being applied can be as accurate as possible.
                filterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

                int needsValidationCount = 0;
                int newShipmentCount = 0;
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
                            if (shipmentFactory.AutoCreateIfNecessary(order))
                            {
                                newShipmentCount += 1;
                            }

                            // Queue the shipments to be validated
                            foreach (ShipmentEntity shipment in order.Shipments)
                            {
                                if (shipment.ShipAddressValidationStatus == (int) AddressValidationStatusType.Pending)
                                {
                                    needsValidationCount += 1;
                                }

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

                shipmentsToValidate.CompleteAdding();

                workProgress.Completed();

                trackedDurationEvent.AddMetric("Orders", entityIDsOriginalSort.Count);
                trackedDurationEvent.AddMetric("TotalShipments", globalShipments.Count);
                trackedDurationEvent.AddMetric("PendingValidation", needsValidationCount);
                trackedDurationEvent.AddMetric("ShipmentsCreated", newShipmentCount);

                return wasCanceled;
            }
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
