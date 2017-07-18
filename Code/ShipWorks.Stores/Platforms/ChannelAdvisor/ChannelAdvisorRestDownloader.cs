using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using RestSharp;
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
        public ChannelAdvisorRestDownloader(StoreEntity store, IChannelAdvisorRestClient restClient,
            IEncryptionProviderFactory encryptionProviderFactory,
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
                string token = GetAccessToken(restClient);

                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                DateTime start = GetOnlineLastModifiedStartingPoint() ?? DateTime.UtcNow.AddDays(-30);

                ChannelAdvisorOrderResult ordersResult = restClient.GetOrders(start, token);

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

                        // Get the products for the order to pass into the loader
                        List<ChannelAdvisorProduct> caProducts = caOrder.Items.Select(item => restClient.GetProduct(item.ProductID, token)).ToList();

                        LoadOrder(caOrder, caProducts);
                    }

                    token = GetAccessToken(restClient);

                    ordersResult = restClient.GetOrders(ordersResult.Orders.Last().CreatedDateUtc, token);
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
        /// Get the access token
        /// </summary>
        private string GetAccessToken(IChannelAdvisorRestClient client)
        {
            GenericResult<string> tokenreResult = client.GetAccessToken(refreshToken);

            if (tokenreResult.Failure)
            {
                throw new DownloadException(tokenreResult.Message);
            }

            return tokenreResult.Value;
        }

        /// <summary>
        /// Load the given ChannelAdvisor order
        /// </summary>
        private void LoadOrder(ChannelAdvisorOrder caOrder, List<ChannelAdvisorProduct> caProducts)
        {
            // Update the status
            Progress.Detail = $"Processing order {QuantitySaved + 1}...";

            // Check if it has been canceled
            if (!Progress.IsCancelRequested)
            {
                ChannelAdvisorOrderEntity order =
                    (ChannelAdvisorOrderEntity) InstantiateOrder(new OrderNumberIdentifier(caOrder.ID));

                // Required by order loader
                order.Store = Store;

                //Order loader loads the order
                orderLoader.LoadOrder(order, caOrder, caProducts, this);

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