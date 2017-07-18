using System;
using System.Data.SqlClient;
using System.Linq;
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
        private readonly ChannelAdvisorOrderLoader orderLoader;
        private readonly string refreshToken;
        private readonly ISqlAdapterRetry sqlAdapter;
        private readonly ChannelAdvisorStoreEntity caStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorRestDownloader(StoreEntity store, IChannelAdvisorRestClient restClient, IEncryptionProviderFactory encryptionProviderFactory,
        ISqlAdapterRetryFactory sqlAdapterRetryFactory, ChannelAdvisorOrderLoader orderLoader) : base(store)
        {
            this.restClient = restClient;
            this.orderLoader = orderLoader;

            sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "WalmartDownloader.Download");
            caStore = Store as ChannelAdvisorStoreEntity;
            MethodConditions.EnsureArgumentIsNotNull(caStore, "ChannelAdvisor Store");
            refreshToken = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor")
                .Decrypt(caStore.RefreshToken);
        }

        /// <summary>
        /// Download orders from ChannelAdvisor REST
        /// </summary>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Checking for orders...";

            try
            {
                string token = restClient.GetAccessToken(refreshToken);

                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                DateTime start = GetOnlineLastModifiedStartingPoint() ?? DateTime.UtcNow.AddDays(-30);

                ChannelAdvisorOrderResult ordersResult = GetOrders(start, token);

                Progress.Detail = $"Downloading {ordersResult.ResultCount} orders...";

                while (ordersResult?.ResultCount > 0)
                {
                    foreach (ChannelAdvisorOrder caOrder in ordersResult.Orders)
                    {
                        // Check if it has been canceled
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        LoadOrder(caOrder, token);
                    }

                    token = restClient.GetAccessToken(refreshToken);
                    ordersResult = GetOrders(ordersResult.Orders.Last().CreatedDateUtc, token);
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
        /// Get orders from the start
        /// </summary>
        private ChannelAdvisorOrderResult GetOrders(DateTime start, string token) =>
            restClient.GetOrders(start, token);

        /// <summary>
        /// Load the given ChannelAdvisor order
        /// </summary>
        private void LoadOrder(ChannelAdvisorOrder caOrder, string token)
        {
            // Update the status
            Progress.Detail = $"Processing order {QuantitySaved + 1}...";

            // Check if it has been canceled
            if (!Progress.IsCancelRequested)
            {
                ChannelAdvisorOrderEntity order =
                    (ChannelAdvisorOrderEntity) InstantiateOrder(new OrderNumberIdentifier(caOrder.ID));

                //Order loader loads the order
                orderLoader.LoadOrder(order, caOrder, this, token, caStore.ParsedAttributesToDownload);

                // Save the downloaded order
                sqlAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
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
        NoteEntity IOrderElementFactory.CreateNote(OrderEntity order,
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