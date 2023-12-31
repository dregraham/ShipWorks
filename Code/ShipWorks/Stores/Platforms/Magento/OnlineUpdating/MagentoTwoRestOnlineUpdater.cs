﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using log4net;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Orders.Combine;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoRestShipment;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestInvoice;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.Magento.OnlineUpdating
{
    /// <summary>
    /// Online updater for Magento 2 stores using the REST API
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.GenericModule.GenericStoreOnlineUpdater" />
    /// <seealso cref="ShipWorks.Stores.Platforms.Magento.IMagentoOnlineUpdater" />
    [Component(RegistrationType.Self)]
    public class MagentoTwoRestOnlineUpdater : GenericStoreOnlineUpdater, IMagentoOnlineUpdater
    {
        private readonly Func<MagentoStoreEntity, IMagentoTwoRestClient> webClientFactory;
        private readonly IDataProvider dataProvider;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly MagentoStoreEntity store;
        private readonly ILog log;
        private readonly ICombineOrderSearchProvider<MagentoOrderSearchEntity> combineOrderSearchProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestOnlineUpdater"/> class.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public MagentoTwoRestOnlineUpdater(MagentoStoreEntity store,
            Func<MagentoStoreEntity, IMagentoTwoRestClient> webClientFactory,
            IDataProvider dataProvider,
            IIndex<StoreTypeCode, ICombineOrderSearchProvider<MagentoOrderSearchEntity>> combineOrderSearchProviderFactory,
            Func<Type, ILog> logFactory,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory)
            : base(store)
        {
            this.webClientFactory = webClientFactory;
            this.dataProvider = dataProvider;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.store = store;
            combineOrderSearchProvider = combineOrderSearchProviderFactory[StoreTypeCode.Magento];
            log = logFactory(typeof(MagentoTwoRestOnlineUpdater));
        }

        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        public async Task UploadShipmentDetails(long orderID, MagentoUploadCommand command, string comments, bool emailCustomer)
        {
            UnitOfWork2 unitOfWork = new ManagedConnectionUnitOfWork2();
            await UploadShipmentDetails(orderID, command, comments, emailCustomer, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        public async Task UploadShipmentDetails(long orderID, MagentoUploadCommand command, string comments, bool emailCustomer, UnitOfWork2 unitOfWork)
        {
            MagentoOrderEntity orderEntity = dataProvider.GetEntity(orderID) as MagentoOrderEntity;

            if (orderEntity == null)
            {
                return;
            }

            if (orderEntity.IsManual && orderEntity.CombineSplitStatus == CombineSplitStatusType.None)
            {
                log.InfoFormat("Not executing online action since order {0} is manual.", orderEntity.OrderID);
                return;
            }

            await UploadShipmentDetails(orderEntity, command, comments, emailCustomer, unitOfWork).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        private async Task UploadShipmentDetails(MagentoOrderEntity orderEntity, MagentoUploadCommand command, string comments, bool emailCustomer, UnitOfWork2 unitOfWork)
        {
            IMagentoTwoRestClient webClient = webClientFactory(store);

            string processedComments = TemplateTokenProcessor.ProcessTokens(comments, orderEntity.OrderID);

            IEnumerable<MagentoOrderSearchEntity> orderSearchEntities = await combineOrderSearchProvider.GetOrderIdentifiers(orderEntity).ConfigureAwait(false);

            List<Exception> exceptions = new List<Exception>();

            foreach (MagentoOrderSearchEntity orderSearchEntity in orderSearchEntities)
            {
                try
                {
                    switch (command)
                    {
                        case MagentoUploadCommand.Complete:
                            UpdateAsComplete(emailCustomer, orderEntity, webClient, processedComments, orderSearchEntity);
                            break;
                        case MagentoUploadCommand.Hold:
                            UpdateAsHold(webClient, processedComments, orderSearchEntity.MagentoOrderID);
                            break;
                        case MagentoUploadCommand.Unhold:
                            UpdateAsPending(webClient, processedComments, orderSearchEntity.MagentoOrderID);
                            break;
                        case MagentoUploadCommand.Cancel:
                            UploadAsCancel(webClient, processedComments, orderSearchEntity.MagentoOrderID);
                            break;
                        case MagentoUploadCommand.Comments:
                            UploadCommentsIfPresent(webClient, processedComments, orderSearchEntity.MagentoOrderID, true);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new MagentoException(string.Join($"{ Environment.NewLine }", exceptions.Select(e => e.Message)), exceptions.First());
            }

            unitOfWork.AddForSave(orderEntity);
        }

        /// <summary>
        /// Completes the order with Magento and saves online status.
        /// </summary>
        private void UpdateAsComplete(bool emailCustomer, MagentoOrderEntity orderEntity, IMagentoTwoRestClient webClient, string processedComments, MagentoOrderSearchEntity orderSearchEntity)
        {
            Order order = webClient.GetOrder(orderSearchEntity.MagentoOrderID);
            var items = order.Items ?? Enumerable.Empty<Item>();

            string shipmentDetails = GetShipmentDetails(orderEntity, processedComments, emailCustomer, items, orderSearchEntity.OriginalOrderID);
            string invoice = GetInvoice(orderEntity, orderSearchEntity.MagentoOrderID, items, orderSearchEntity.OriginalOrderID);
            webClient.UploadShipmentDetails(shipmentDetails, invoice, orderSearchEntity.MagentoOrderID);
        }

        /// <summary>
        /// Cancels order and saves online status
        /// </summary>
        private void UploadAsCancel(IMagentoTwoRestClient webClient, string processedComments, long magentoOrderID)
        {
            UploadCommentsIfPresent(webClient, processedComments, magentoOrderID, false);
            webClient.CancelOrder(magentoOrderID);
        }
        /// <summary>
        /// Takes order off hold and saves online status
        /// </summary>
        private void UpdateAsPending(IMagentoTwoRestClient webClient, string processedComments, long magentoOrderID)
        {
            UploadCommentsIfPresent(webClient, processedComments, magentoOrderID, false);
            webClient.UnholdOrder(magentoOrderID);
        }

        /// <summary>
        /// Puts order on hold
        /// </summary>
        private void UpdateAsHold(IMagentoTwoRestClient webClient, string processedComments, long magentoOrderID)
        {
            UploadCommentsIfPresent(webClient, processedComments, magentoOrderID, false);
            webClient.HoldOrder(magentoOrderID);
        }

        /// <summary>
        /// Gets the invoice.
        /// </summary>
        private string GetInvoice(MagentoOrderEntity orderEntity, long magentoOrderID, IEnumerable<Item> items, long originalOrderID)
        {
            MagentoInvoiceRequest request = new MagentoInvoiceRequest();
            request.Invoice.MagentoOrderID = magentoOrderID;

            // Fetch the order items
            IEnumerable<(OrderItemEntity OrderItem, Item MagentoItem)> itemsToProcess = GetItemsForInvoiceAndShipmentUpload(orderEntity, items, originalOrderID);

            foreach (var item in itemsToProcess)
            {
                MagentoInvoiceItem magentoInvoiceItem = new MagentoInvoiceItem
                {
                    Qty = GetQuantity(item.OrderItem, item.MagentoItem, originalOrderID),
                    MagentoOrderItemId = item.MagentoItem.ItemId
                };
                request.Invoice.Items.Add(magentoInvoiceItem);
                request.Invoice.TotalQty += magentoInvoiceItem.Qty;
            }

            return JsonConvert.SerializeObject(request);
        }

        /// <summary>
        /// First tries to get the actual quantity from the ShipWorks order, but if it cannot
        /// match the item, return the quantity from Magento
        /// </summary>
        private double GetQuantity(OrderItemEntity orderItemEntity, Item magentoItem, long originalOrderID)
        {
            return orderItemEntity?.Quantity ?? magentoItem.QtyOrdered;
        }

        /// <summary>
        /// Tell web client to upload comments if they are present
        /// </summary>
        private void UploadCommentsIfPresent(IMagentoTwoRestClient webClient, string comments, long magentoOrderID, bool commentsOnly)
        {
            if (!string.IsNullOrWhiteSpace(comments))
            {
                webClient.UploadComments(comments, magentoOrderID, commentsOnly);
            }
        }

        /// <summary>
        /// Gets the shipment details string from the request
        /// </summary>
        private string GetShipmentDetails(MagentoOrderEntity orderEntity, string comments, bool emailCustomer, IEnumerable<Item> items, long originalOrderID)
        {
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderEntity.OrderID, false);
            if (shipment == null)
            {
                return string.Empty;
            }

            ShippingManager.EnsureShipmentLoaded(shipment);

            MagentoTwoRestUploadShipmentRequest request = new MagentoTwoRestUploadShipmentRequest();
            request.Notify = emailCustomer;

            if (!string.IsNullOrWhiteSpace(comments))
            {
                request.Comment = new Comment();
                request.Comment.Text = comments;
                request.AppendComment = true;
            }

            if (!string.IsNullOrWhiteSpace(shipment.TrackingNumber))
            {
                request.Tracks = new List<Track>();
                request.Tracks.Add(new Track
                {
                    CarrierCode = GetCarrierCode(shipment),
                    Title = GetShippingService(shipment),
                    TrackNumber = shipment.TrackingNumber
                });
            }

            // Fetch the order items
            IEnumerable<(OrderItemEntity OrderItem, Item MagentoItem)> itemsToProcess = GetItemsForInvoiceAndShipmentUpload(orderEntity, items, originalOrderID);

            request.Items = itemsToProcess.Select(i => new ShipmentItem()
            {
                Qty = GetQuantity(i.OrderItem, i.MagentoItem, originalOrderID),
                OrderItemId = i.MagentoItem.ParentItemId == null ? i.MagentoItem.ItemId : i.MagentoItem.ParentItemId.Value
            }).ToList();

            return JsonConvert.SerializeObject(request);
        }

        /// <summary>
        /// Get the magento item from the given list that corresponds with the passed in orderItem
        /// </summary>
        private Item GetMagentoItem(OrderItemEntity orderItem, IEnumerable<Item> magentoItems) =>
            magentoItems.FirstOrDefault(x => x.ItemId.ToString() == orderItem.Code) ??
            magentoItems.FirstOrDefault(x => x.Sku == orderItem.SKU || x.Name == orderItem.Name);

        /// <summary>
        /// Get a list of (OrderItemEntity, Item) for use in generating an invoice or shipment
        /// </summary>
        private IEnumerable<(OrderItemEntity, Item)> GetItemsForInvoiceAndShipmentUpload(MagentoOrderEntity orderEntity, IEnumerable<Item> items, long originalOrderID)
        {
            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(orderEntity.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == orderEntity.OrderID));
            }

            return orderEntity.OrderItems
                .Where(orderItem => !orderItem.IsManual &&
                    (orderItem.OriginalOrderID == originalOrderID || orderItem.OriginalOrderID == orderEntity.OrderID))
                .Select(orderItem => (OrderItem: orderItem, MagentoItem: GetMagentoItem(orderItem, items)))
                .Where(x => x.MagentoItem != null);
        }

        /// <summary>
        /// Gets the carrier code.
        /// </summary>
        private string GetCarrierCode(ShipmentEntity shipment)
        {
            string code;
            switch ((ShipmentTypeCode) shipment.ShipmentType)
            {
                case ShipmentTypeCode.FedEx:
                    code = "fedex";
                    break;
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    code = "ups";
                    break;
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    code = "usps";
                    break;
                default:
                    code = "other";
                    break;
            }

            return code;
        }

        /// <summary>
        /// Gets the shipping service.
        /// </summary>
        private string GetShippingService(ShipmentEntity shipment)
        {
            string service = shipmentAdapterFactory.Get(shipment).ServiceTypeName;

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(service, "");
        }
    }
}