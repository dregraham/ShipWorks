using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Rakuten.DTO;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Downloader for downloading Rakuten orders
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Rakuten)]
    public class RakutenDownloader : StoreDownloader, IRakutenDownloader
    {
        private readonly ILog log;
        private readonly IRakutenWebClient webClient;
        private readonly RakutenOrderLoader orderLoader;
        private readonly IRakutenStoreEntity rakutenStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenDownloader(StoreEntity store,
            IStoreTypeManager storeTypeManager,
            Func<IRakutenStoreEntity, IRakutenWebClient> webClientFactory,
            RakutenOrderLoader orderLoader,
            Func<Type, ILog> createLogger) :
            base(store, storeTypeManager.GetType(store))
        {
            rakutenStore = store as RakutenStoreEntity;
            this.webClient = webClientFactory(rakutenStore);
            this.orderLoader = orderLoader;

            MethodConditions.EnsureArgumentIsNotNull(rakutenStore, "Rakuten Store");

            log = createLogger(GetType());
        }

        /// <summary>
        /// Download orders from Rakuten
        /// </summary>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Checking for orders...";

            try
            {
                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                RakutenOrdersResponse response = webClient.GetOrders(rakutenStore, rakutenStore.DownloadStartDate ?? DateTime.UtcNow.AddDays(-7));

                Progress.Detail = $"Downloading orders...";

                while (response?.Orders?.Any() == true)
                {
                    // Save the orders and update the store's download start date
                    if (!await ProcessOrders(response).ConfigureAwait(false))
                    {
                        break;
                    }

                    // Download more orders with the new download date
                    response = webClient.GetOrders(rakutenStore, rakutenStore.DownloadStartDate.Value);
                }

                if (response?.Errors != null)
                {
                    ThrowError(response.Errors);
                }
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }

            Progress.PercentComplete = 100;
            Progress.Detail = "Done";
        }

        /// <summary>
        /// Parse the download errors
        /// </summary>
        private void ThrowError(RakutenErrors errors)
        {
            RakutenError error = null;
            string resource = null;

            // Use the common error first
            if (errors.Common != null)
            {
                error = errors.Common.First();
            }
            else if (errors.Specific != null)
            {
                error = errors.Specific.First().Value.First();
                resource = errors.Specific.First().Key;
            }

            log.Error(errors);

            if (error != null)
            {
                throw new DownloadException($"An error occured when downloading from Rakuten: {error.ShortMessage} ({error.ErrorCode}) - {error.LongMessage}");
            }
            else
            {
                throw new DownloadException("An error occured when downloading from Rakuten");
            }
        }

        /// <summary>
        /// Process Rakuten orders
        /// </summary>
        private async Task<bool> ProcessOrders(RakutenOrdersResponse response)
        {
            foreach (RakutenOrder order in response.Orders)
            {
                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                await LoadOrder(order).ConfigureAwait(false);
            }

            return true;
        }

        /// <summary>
        /// Load the given Rakuten order
        /// </summary>
        private async Task LoadOrder(RakutenOrder rakutenOrder)
        {
            Progress.Detail = $"Processing order {QuantitySaved + 1}...";

            if (!Progress.IsCancelRequested)
            {
                GenericResult<OrderEntity> result = await InstantiateOrder(new RakutenOrderIdentifier(rakutenOrder.OrderNumber)).ConfigureAwait(false);

                if (result.Failure)
                {
                    log.InfoFormat("Skipping order '{0}': {1}.", rakutenOrder.OrderNumber, result.Message);
                    return;
                }

                var order = (RakutenOrderEntity) result.Value;

                order.Store = Store;

                // Save the order
                await orderLoader.LoadOrder(order, rakutenOrder, this);
            }
        }
    }
}