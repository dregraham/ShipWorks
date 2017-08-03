using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Jet downloader
    /// </summary>
    [KeyedComponent(typeof(StoreDownloader), StoreTypeCode.Jet, ExternallyOwned = true)]
    public class JetDownloader : StoreDownloader
    {
        private readonly IJetOrderLoader orderLoader;
        private readonly IJetWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetDownloader(StoreEntity store, Func<IOrderElementFactory, IJetOrderLoader> orderLoaderFactory, IJetWebClient webClient) : base(store)
        {
            orderLoader = orderLoaderFactory(this);
            this.webClient = webClient;
        }

        /// <summary>
        /// Download orders from jet
        /// </summary>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Checking for orders...";

            try
            {
                GenericResult<JetOrderResponse> ordersResult = webClient.GetOrders(Store as JetStoreEntity);
                
                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                while (ordersResult.Success && ordersResult.Value.OrderUrls.Any())
                {
                    // Check if it has been canceled
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    foreach (string orderUrl in ordersResult.Value.OrderUrls)
                    {
                        // Update the status
                        Progress.Detail = $"Processing order {QuantitySaved + 1}...";

                        LoadAndAcknowledgeOrder(orderUrl);
                    }

                    ordersResult = webClient.GetOrders(Store as JetStoreEntity);
                }
            }
            catch (JetException ex)
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
        /// Load and acknowledge the order
        /// </summary>
        private void LoadAndAcknowledgeOrder(string orderUrl)
        {
            GenericResult<JetOrderDetailsResult> orderDetails = webClient.GetOrderDetails(orderUrl, Store as JetStoreEntity);

            if (orderDetails.Failure)
            {
                throw new DownloadException(orderDetails.Message);
            }

            JetOrderDetailsResult jetOrder = orderDetails.Value;

            JetOrderEntity order =
                (JetOrderEntity) InstantiateOrder(new OrderNumberIdentifier(jetOrder.ReferenceOrderId));

            if (order.IsNew)
            {
                orderLoader.LoadOrder(order, jetOrder, Store as JetStoreEntity);
                SaveDownloadedOrder(order);
            }

            webClient.Acknowledge(order, Store as JetStoreEntity);
        }
    }
}