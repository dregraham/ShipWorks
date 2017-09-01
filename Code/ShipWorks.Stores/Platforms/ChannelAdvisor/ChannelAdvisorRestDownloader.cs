using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Downloader for downloading orders from ChannelAdvisor via their REST api
    /// </summary>
    [Component]
    public class ChannelAdvisorRestDownloader : StoreDownloader, IChannelAdvisorRestDownloader, IOrderElementFactory
    {
        private readonly IChannelAdvisorRestClient restClient;
        private readonly Func<IEnumerable<ChannelAdvisorDistributionCenter>, ChannelAdvisorOrderLoader> orderLoaderFactory;
        private readonly string refreshToken;
        private readonly ISqlAdapterRetry sqlAdapter;
        private IEnumerable<ChannelAdvisorDistributionCenter> distributionCenters;
        private int totalOrders;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorRestDownloader(StoreEntity store, IChannelAdvisorRestClient restClient,
            IEncryptionProviderFactory encryptionProviderFactory,
            ISqlAdapterRetryFactory sqlAdapterRetryFactory,
            Func<IEnumerable<ChannelAdvisorDistributionCenter>, ChannelAdvisorOrderLoader> orderLoaderFactory) :
            base(store)
        {
            this.restClient = restClient;
            this.orderLoaderFactory = orderLoaderFactory;

            sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "ChannelAdvisorRestDownloader.Download");
            ChannelAdvisorStoreEntity caStore = Store as ChannelAdvisorStoreEntity;
            MethodConditions.EnsureArgumentIsNotNull(caStore, "ChannelAdvisor Store");
            refreshToken = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor")
                .Decrypt(caStore.RefreshToken);
        }

        /// <summary>
        /// Download orders from ChannelAdvisor REST
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

                distributionCenters = restClient.GetDistributionCenters(refreshToken).DistributionCenters;

                DateTime start = GetOrderDateStartingPoint() ?? DateTime.UtcNow.AddDays(-30);

                ChannelAdvisorOrderResult ordersResult = restClient.GetOrders(start.AddSeconds(-2), refreshToken);
                totalOrders = ordersResult.ResultCount;

                Progress.Detail = $"Downloading {ordersResult.ResultCount} orders...";

                while (ordersResult?.Orders?.Any() ?? false)
                {
                    foreach (ChannelAdvisorOrder caOrder in ordersResult.Orders)
                    {
                        // Check if it has been canceled
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        // Get the products for the order to pass into the loader
                        List<ChannelAdvisorProduct> caProducts =
                            caOrder.Items
                                .Select(item => restClient.GetProduct(item.ProductID, refreshToken))
                                .Where(p => p != null).ToList();

                        await LoadOrder(caOrder, caProducts).ConfigureAwait(false);
                    }

                    ordersResult = string.IsNullOrEmpty(ordersResult.OdataNextLink)
                        ? null
                        : restClient.GetOrders(ordersResult.OdataNextLink, refreshToken);
                }
            }
            catch (ChannelAdvisorException ex)
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
        /// Load the given ChannelAdvisor order
        /// </summary>
        private async Task LoadOrder(ChannelAdvisorOrder caOrder, List<ChannelAdvisorProduct> caProducts)
        {
            // Update the status
            Progress.Detail = $"Processing order {QuantitySaved + 1} of {totalOrders}...";
            Progress.PercentComplete = Math.Min(100 * QuantitySaved / totalOrders, 100);

            // Check if it has been canceled
            if (!Progress.IsCancelRequested)
            {
                ChannelAdvisorOrderEntity order =
                    (ChannelAdvisorOrderEntity) await InstantiateOrder(new OrderNumberIdentifier(caOrder.ID)).ConfigureAwait(false);

                // Required by order loader
                order.Store = Store;

                //Order loader loads the order
                orderLoaderFactory(distributionCenters).LoadOrder(order, caOrder, caProducts, this);

                // Save the downloaded order
                await sqlAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order));
            }
        }

        /// <summary>
        /// Create an item for the order
        /// </summary>
        OrderItemEntity IOrderElementFactory.CreateItem(OrderEntity order) => InstantiateOrderItem(order);

        /// <summary>
        /// Create an item attribute for the item
        /// </summary>
        OrderItemAttributeEntity IOrderElementFactory.CreateItemAttribute(OrderItemEntity item) =>
            InstantiateOrderItemAttribute(item);

        /// <summary>
        /// Create an item attribute for the item
        /// </summary>
        public OrderItemAttributeEntity CreateItemAttribute(OrderItemEntity item,
            string name,
            string description,
            decimal unitPrice,
            bool isManual) => InstantiateOrderItemAttribute(item, name, description, unitPrice, isManual);

        /// <summary>
        /// Crate a charge for the given order
        /// </summary>
        OrderChargeEntity IOrderElementFactory.CreateCharge(OrderEntity order) => InstantiateOrderCharge(order);

        /// <summary>
        /// Create a charge for the given order
        /// </summary>
        OrderChargeEntity IOrderElementFactory.CreateCharge(OrderEntity order,
            string type,
            string description,
            decimal amount) => InstantiateOrderCharge(order, type, description, amount);

        /// <summary>
        /// Create a note for the given order
        /// </summary>
        Task<NoteEntity> IOrderElementFactory.CreateNote(OrderEntity order,
            string noteText,
            DateTime noteDate,
            NoteVisibility noteVisibility) => InstantiateNote(order, noteText, noteDate, noteVisibility, true);

        /// <summary>
        /// Create a payment detail for the given order
        /// </summary>
        OrderPaymentDetailEntity IOrderElementFactory.CreatePaymentDetail(OrderEntity order) =>
            InstantiateOrderPaymentDetail(order);

        /// <summary>
        /// Create a payment detail for the given order
        /// </summary>
        OrderPaymentDetailEntity IOrderElementFactory.CreatePaymentDetail(OrderEntity order, string label, string value) =>
            InstantiateOrderPaymentDetail(order, label, value);
    }
}