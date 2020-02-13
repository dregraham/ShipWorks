using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// Base class for Generic File downloading
    /// </summary>
    public abstract class GenericFileDownloaderBase : OrderElementFactoryDownloaderBase
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileDownloaderBase));

        protected readonly GenericFileStoreType storeType;
        private readonly IWarehouseOrderClient warehouseOrderClient;
        private int fileCount = 0;
        private readonly ILicenseService licenseService;
        private List<OrderEntity> ordersToSendToHub;

        private const int UploadOrdersBatchSize = 5;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParamsAttribute]
        protected GenericFileDownloaderBase(GenericFileStoreEntity store,
            Func<StoreEntity, GenericFileStoreType> getStoreType,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory,
            IWarehouseOrderClient warehouseOrderClient,
            ILicenseService licenseService)
            : base(store, getStoreType(store), configurationData, sqlAdapterFactory)
        {
            storeType = StoreType as GenericFileStoreType;
            this.warehouseOrderClient = warehouseOrderClient;
            this.licenseService = licenseService;
        }

        /// <summary>
        /// The store being downloaded
        /// </summary>
        protected GenericFileStoreEntity GenericStoreEntity => (GenericFileStoreEntity) Store;

        /// <summary>
        /// Can be used by derived classes to perform one-time download initialization
        /// </summary>
        protected virtual void InitializeDownload()
        {

        }

        /// <summary>
        /// Import data from the given file.  Must throw GenericFileStoreException (or derived) on error.  Return false to indicate canceled before completion.
        /// </summary>
        protected abstract Task<bool> ImportFile(GenericFileInstance file);

        /// <summary>
        /// Conditionally upload orders to hub. Then download orders from hub.
        /// </summary>
        protected override async Task DownloadWarehouseOrders(Guid batchId)
        {
            if (ShouldUploadToHub())
            {
                using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
                {
                    await Download(trackedDurationEvent).ConfigureAwait(false);

                    CollectDownloadTelemetry(trackedDurationEvent);
                }
            }

            await base.DownloadWarehouseOrders(batchId).ConfigureAwait(false);
        }

        /// <summary>
        /// Should we send orders to the hub
        /// </summary>
        private bool ShouldUploadToHub()
        {
            // If this machine is configured to pull orders from warehouse then dont send orders to the hub
            if (GenericStoreEntity.FileSource == (int) GenericFileSourceTypeCode.Warehouse || !IsWarehouseAllowed())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Import from the XML file
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                InitializeDownload();

                GenericFileSourceType sourceType = GenericFileSourceTypeManager.GetFileSourceType((GenericFileSourceTypeCode) GenericStoreEntity.FileSource);

                // Prepare the file source
                using (GenericFileSource fileSource = sourceType.CreateFileSource(GenericStoreEntity))
                {
                    Progress.Detail = "Checking for files...";

                    // keep going until none are left
                    while (true)
                    {
                        bool morePages = await ImportNextFile(fileSource).ConfigureAwait(false);
                        if (!morePages)
                        {
                            if (fileCount == 0)
                            {
                                Progress.Detail = "No files to import.";
                                Progress.PercentComplete = 100;
                            }
                            else
                            {
                                Progress.Detail = "Done";
                            }

                            return;
                        }

                        // Check if it has been canceled
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }
                    }
                }
            }
            catch (GenericFileStoreException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Import the next file from the source
        /// </summary>
        private async Task<bool> ImportNextFile(GenericFileSource fileSource)
        {
            GenericFileInstance file = fileSource.GetNextFile();

            // No more files
            if (file == null)
            {
                return false;
            }

            // Increment that we've seen another file
            fileCount++;

            GenericFileStoreException importError = null;

            try
            {
                ordersToSendToHub = new List<OrderEntity>();

                // Return of false means canceled before completed
                bool continueProcessing = await ImportFile(file).ConfigureAwait(false);
                if (!continueProcessing)
                {
                    // We still return true to caller b\c there ARE more (or could be).. the caller will check the Canceled flag.
                    return false;
                }

                // orders have been added to this collection in the SaveDownloadedOrder method.
                if (ordersToSendToHub.Any())
                {
                    foreach (var batch in ordersToSendToHub.SplitIntoChunksOf(UploadOrdersBatchSize))
                    {
                        await UploadOrdersToHub(batch).ConfigureAwait(false);
                    }
                }
            }
            catch (GenericFileStoreException ex)
            {
                log.Error(string.Format("Failed to load import source '{0}'", file.Name), ex);

                importError = ex;
            }

            // Determine whether to run the success or error action
            if (importError == null)
            {
                // Let the file source do whatever success action is configured.  This has to be outside of the try\catch, b\c if the HandleSuccess throws
                // for some reason - that should NOT be handled by the error handler, but bubble all the way up immediately.
                fileSource.HandleSuccess(file);
            }
            else
            {
                // Let the file source do whatever error action is configured
                fileSource.HandleError(file, importError);
            }

            return true;
        }

        /// <summary>
        /// Stash order if warehouse store, else save it.
        /// </summary>
        protected override Task SaveDownloadedOrder(OrderEntity order)
        {
            // if the hub order id is null this order is comming
            // from a local folder and first needs to be sent to the hub

            // if we have a hub order id this order is comming directly from
            // the hub and we need to save it
            if (IsWarehouseAllowed() && order.HubOrderID == null)
            {
                ordersToSendToHub.Add(order);
                return Task.CompletedTask;
            }
            else
            {
                return base.SaveDownloadedOrder(order);
            }
        }

        /// <summary>
        /// Upload the order to the hub (if required)
        /// </summary>
        private async Task UploadOrdersToHub(IEnumerable<OrderEntity> downloadedOrders)
        {
            if (GenericStoreEntity.WarehouseStoreID.HasValue && downloadedOrders.Any())
            {
                var result =
                    await warehouseOrderClient.UploadOrders(downloadedOrders, GenericStoreEntity, false).ConfigureAwait(false);
                if (result.Failure)
                {
                    throw new DownloadException(result.Message);
                }

                foreach (WarehouseUploadOrderResponse orderResponse in result.Value)
                {
                    OrderEntity downloadedOrder = downloadedOrders.FirstOrDefault(x => x.OrderNumberComplete == orderResponse.OrderNumber);

                    downloadedOrder.HubOrderID = Guid.Parse(orderResponse.HubOrderID);
                    downloadedOrder.HubSequence = orderResponse.HubSequence;
                }
            }
        }

        /// <summary>
        /// Is warehouse allowed for the customer
        /// </summary>
        private bool IsWarehouseAllowed()
        {
            return licenseService.CheckRestriction(EditionFeature.Warehouse, null) == EditionRestrictionLevel.None &&
                    GenericStoreEntity.WarehouseStoreID.HasValue;
        }
    }
}
