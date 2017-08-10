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
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Jet downloader
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Jet)]
    public class JetDownloader : StoreDownloader
    {
        private readonly IJetOrderLoader orderLoader;
        private readonly IJetWebClient webClient;
        private readonly IOrderRepository orderRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetDownloader(StoreEntity store,
            Func<IOrderElementFactory, IJetOrderLoader> orderLoaderFactory,
            IJetWebClient webClient,
            IOrderRepository orderRepository) 
            : base(store)
        {
            orderLoader = orderLoaderFactory(this);
            this.webClient = webClient;
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Download orders from jet
        /// </summary>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
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

                        await LoadAndAcknowledgeOrder(orderUrl).ConfigureAwait(false);
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
        private async Task LoadAndAcknowledgeOrder(string orderUrl)
        {
            GenericResult<JetOrderDetailsResult> orderDetails = webClient.GetOrderDetails(orderUrl, Store as JetStoreEntity);

            if (orderDetails.Success)
            {
                JetOrderDetailsResult jetOrder = orderDetails.Value;

                JetOrderEntity order =
                    (JetOrderEntity) await InstantiateOrder(new OrderNumberIdentifier(jetOrder.ReferenceOrderId)).ConfigureAwait(false);

                if (order.IsNew)
                {
                    orderLoader.LoadOrder(order, jetOrder, Store as JetStoreEntity);
                    await SaveDownloadedOrder(order).ConfigureAwait(false);
                }
            else
            {
                orderRepository.PopulateOrderDetails(order);
            }

                webClient.Acknowledge(order, Store as JetStoreEntity);
            }
            else
            {
                throw new DownloadException(orderDetails.Message);
            }
        }
    }
}