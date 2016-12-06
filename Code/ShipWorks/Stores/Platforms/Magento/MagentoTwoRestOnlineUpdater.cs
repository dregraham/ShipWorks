using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.Tools.BCLExtensions.CollectionsRelated;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.DTO;
using ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoRestShipment;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestInvoice;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Online updater for Magento 2 stores using the REST API
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.GenericModule.GenericStoreOnlineUpdater" />
    /// <seealso cref="ShipWorks.Stores.Platforms.Magento.IMagentoOnlineUpdater" />
    [KeyedComponent(typeof(IMagentoOnlineUpdater), MagentoVersion.MagentoTwoREST, ExternallyOwned = false)]
    public class MagentoTwoRestOnlineUpdater : GenericStoreOnlineUpdater, IMagentoOnlineUpdater
    {
        private readonly Func<MagentoStoreEntity, IMagentoTwoRestClient> webClientFactory;
        private readonly IDataProvider dataProvider;
        private readonly MagentoStoreEntity store;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestOnlineUpdater"/> class.
        /// </summary>
        public MagentoTwoRestOnlineUpdater(GenericModuleStoreEntity store,
            Func<MagentoStoreEntity, IMagentoTwoRestClient> webClientFactory, IDataProvider dataProvider,
            Func<Type, ILog> logFactory)
            : base(store)
        {
            this.webClientFactory = webClientFactory;
            this.dataProvider = dataProvider;
            this.store = (MagentoStoreEntity) store;
            log = logFactory(typeof(MagentoTwoRestOnlineUpdater));
        }

        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        public void UploadShipmentDetails(long orderID, MagentoUploadCommand command, string comments, bool emailCustomer)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UploadShipmentDetails(orderID, command, comments, emailCustomer, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        public void UploadShipmentDetails(long orderID, MagentoUploadCommand command, string comments, bool emailCustomer, UnitOfWork2 unitOfWork)
        {
            MagentoOrderEntity orderEntity = dataProvider.GetEntity(orderID) as MagentoOrderEntity;
            if (orderEntity == null)
            {
                return;
            }
            if (orderEntity.IsManual)
            {
                log.InfoFormat("Not executing online action since order {0} is manual.", orderEntity.OrderID);
                return;
            }

            IMagentoTwoRestClient webClient = webClientFactory(store);

            string processedComments = TemplateTokenProcessor.ProcessTokens(comments, orderID);
            long magentoOrderID = orderEntity.MagentoOrderID;

            switch (command)
            {
                case MagentoUploadCommand.Complete:
                    UpdateAsComplete(emailCustomer, orderEntity, webClient, processedComments, magentoOrderID);
                    break;
                case MagentoUploadCommand.Hold:
                    UpdateAsHold(orderEntity, webClient, processedComments, magentoOrderID);
                    break;
                case MagentoUploadCommand.Unhold:
                    UpdateAsPending(webClient, processedComments, magentoOrderID);
                    break;
                case MagentoUploadCommand.Cancel:
                    UploadAsCancel(webClient, processedComments, magentoOrderID);
                    break;
                case MagentoUploadCommand.Comments:
                    UploadCommentsIfPresent(webClient, processedComments, magentoOrderID, true);
                    break;
            }

            unitOfWork.AddForSave(orderEntity);
        }

        /// <summary>
        /// Completes the order with Magento and saves online status.
        /// </summary>
        private void UpdateAsComplete(bool emailCustomer, MagentoOrderEntity orderEntity, IMagentoTwoRestClient webClient, string processedComments, long magentoOrderID)
        {
            Order order = webClient.GetOrder(orderEntity.MagentoOrderID);
            string shipmentDetails = GetShipmentDetails(orderEntity, processedComments, emailCustomer,
                order.Items);
            string invoice = GetInvoice(orderEntity, order.Items);
            webClient.UploadShipmentDetails(shipmentDetails, invoice, magentoOrderID);
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
        private void UpdateAsHold(MagentoOrderEntity orderEntity, IMagentoTwoRestClient webClient, string processedComments, long magentoOrderID)
        {
            UploadCommentsIfPresent(webClient, processedComments, magentoOrderID, false);
            webClient.HoldOrder(orderEntity.MagentoOrderID);
        }

        /// <summary>
        /// Gets the invoice.
        /// </summary>
        private string GetInvoice(MagentoOrderEntity orderEntity, IEnumerable<Item> items)
        {
            MagentoInvoiceRequest request = new MagentoInvoiceRequest();
            request.Invoice.MagentoOrderID = orderEntity.MagentoOrderID;

            foreach (Item item in items)
            {
                MagentoInvoiceItem magentoInvoiceItem = new MagentoInvoiceItem
                {
                    Qty = GetQuantity(orderEntity, item),
                    MagentoOrderItemId = item.ItemId
                };
                request.Invoice.Items.Add(magentoInvoiceItem);
                request.Invoice.TotalQty += item.QtyOrdered;
            }

            return JsonConvert.SerializeObject(request);
        }

        /// <summary>
        /// First tries to get the actual quantity from the ShipWorks order, but if it cannot
        /// match the item, return the quantity from Magento
        /// </summary>
        private double GetQuantity(MagentoOrderEntity orderEntity, Item magentoItem)
        {
            return
                orderEntity.OrderItems.FirstOrDefault(
                    x => x.Code == magentoItem.ItemId.ToString() || x.SKU == magentoItem.Sku || x.Name == magentoItem.Name)?.Quantity ??
                magentoItem.QtyOrdered;
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
        private string GetShipmentDetails(MagentoOrderEntity orderEntity, string comments, bool emailCustomer, IEnumerable<Item> items)
        {
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderEntity.OrderID);
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
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(orderEntity.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == orderEntity.OrderID));
            }

            if (!orderEntity.OrderItems.IsNullOrEmpty())
            {
                request.Items = new List<ShipmentItem>();

                foreach (Item item in items)
                {
                    ShipmentItem magentoInvoiceItem = new ShipmentItem()
                    {
                        Qty = GetQuantity(orderEntity, item),
                        OrderItemId = item.ItemId
                    };
                    request.Items.Add(magentoInvoiceItem);
                }
            }

            return JsonConvert.SerializeObject(request);
        }

        /// <summary>
        /// Gets the carrier code.
        /// </summary>
        private string GetCarrierCode(ShipmentEntity shipment)
        {
            string code = "";
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
            string service = "";
            switch ((ShipmentTypeCode)shipment.ShipmentType)
            {
                case ShipmentTypeCode.FedEx:
                    service = EnumHelper.GetDescription((FedExServiceType)shipment.FedEx.Service);
                    break;
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    service = EnumHelper.GetDescription((UpsServiceType)shipment.Ups.Service);
                    break;
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    service = EnumHelper.GetDescription((PostalServiceType)shipment.Postal.Service);
                    break;
                default:
                    service = shipment.Other.Service;
                    break;
            }

            return service;
        }
    }
}