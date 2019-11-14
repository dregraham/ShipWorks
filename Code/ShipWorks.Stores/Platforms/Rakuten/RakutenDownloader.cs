﻿using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
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
        private readonly RakutenStoreEntity rakutenStore;
        private readonly ISqlAdapterRetry sqlAdapter;
        private DateTime downloadStartDate;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams(Justification =
        "The parameters are dependencies that were already part of the downloader, but now they are explicit")]
        public RakutenDownloader(StoreEntity store,
            IStoreTypeManager storeTypeManager,
            Func<IRakutenStoreEntity, IRakutenWebClient> webClientFactory,
            RakutenOrderLoader orderLoader,
            ISqlAdapterRetryFactory sqlAdapterRetryFactory,
            Func<Type, ILog> createLogger) :
            base(store, storeTypeManager.GetType(store))
        {
            rakutenStore = store as RakutenStoreEntity;
            this.webClient = webClientFactory(rakutenStore);
            this.orderLoader = orderLoader;
            sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "RakutenDownloader.Download");

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

                Progress.Detail = $"Downloading orders...";

                downloadStartDate = await GetOnlineLastModifiedStartingPoint().ConfigureAwait(false) ?? DateTime.UtcNow.AddDays(-30);

                RakutenOrdersResponse response = webClient.GetOrders(rakutenStore, downloadStartDate);

                while (response?.Orders?.Any() == true)
                {
                    // Save the orders and update the store's download start date
                    if (!await ProcessOrders(response).ConfigureAwait(false))
                    {
                        break;
                    }

                    Progress.Detail = $"Downloading orders...";

                    // Download more orders with the new download date
                    response = webClient.GetOrders(rakutenStore, downloadStartDate);
                }
            }
            catch (WebException ex)
            {
                throw new DownloadException(ex.Message);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }

            Progress.PercentComplete = 100;
            Progress.Detail = "Done";
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
                GenericResult<OrderEntity> result = await InstantiateOrder(new AlphaNumericOrderIdentifier(rakutenOrder.OrderNumber)).ConfigureAwait(false);

                if (result.Failure)
                {
                    log.InfoFormat("Skipping order '{0}': {1}.", rakutenOrder.OrderNumber, result.Message);
                    return;
                }

                var order = (RakutenOrderEntity) result.Value;

                order.Store = Store;

                // Create the order
                orderLoader.LoadOrder(order, rakutenOrder, this);

                // Save the downloaded order
                await sqlAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);

                // Update downloadStartDate
                if (rakutenOrder.LastModifiedDate.AddMilliseconds(1) > downloadStartDate)
                {
                    downloadStartDate = rakutenOrder.LastModifiedDate.AddMilliseconds(1);
                }
            }
        }
    }
}