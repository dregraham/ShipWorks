using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Stores.Platforms.GenericModule;
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
        private readonly ICombineOrderSearchProvider<MagentoOrderSearchEntity> combineOrderSearchProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestOnlineUpdater"/> class.
        /// </summary>
        public MagentoTwoRestOnlineUpdater(GenericModuleStoreEntity store,
            Func<MagentoStoreEntity, IMagentoTwoRestClient> webClientFactory, IDataProvider dataProvider,
            IIndex<StoreTypeCode, ICombineOrderSearchProvider<MagentoOrderSearchEntity>> combineOrderSearchProviderFactory,
            Func<Type, ILog> logFactory)
            : base(store)
        {
            this.webClientFactory = webClientFactory;
            this.dataProvider = dataProvider;
            this.store = (MagentoStoreEntity) store;
            combineOrderSearchProvider = combineOrderSearchProviderFactory[StoreTypeCode.Magento];
            log = logFactory(typeof(MagentoTwoRestOnlineUpdater));
        }

        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        public async Task UploadShipmentDetails(long orderID, MagentoUploadCommand command, string comments, bool emailCustomer)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
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

            IMagentoTwoRestClient webClient = webClientFactory(store);

            string processedComments = TemplateTokenProcessor.ProcessTokens(comments, orderID);

            IEnumerable<MagentoOrderSearchEntity> orderSearchEntities = await combineOrderSearchProvider.GetOrderIdentifiers(orderEntity).ConfigureAwait(false);

            List<Exception> exceptions = new List<Exception>();

            foreach (MagentoOrderSearchEntity orderSearchEntity in orderSearchEntities)
            {
                try
                {
                    await Task.Run(() =>
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
                    ).ConfigureAwait(false);
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
            string shipmentDetails = GetShipmentDetails(orderEntity, processedComments, emailCustomer, order.Items, orderSearchEntity.OriginalOrderID);
            string invoice = GetInvoice(orderEntity, orderSearchEntity.MagentoOrderID, order.Items, orderSearchEntity.OriginalOrderID);
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

            foreach (Item item in items)
            {
                MagentoInvoiceItem magentoInvoiceItem = new MagentoInvoiceItem
                {
                    Qty = GetQuantity(orderEntity, item, originalOrderID),
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
        private double GetQuantity(MagentoOrderEntity orderEntity, Item magentoItem, long originalOrderID)
        {
            return
                orderEntity.OrderItems
                    .Where(oi => oi.OriginalOrderID == originalOrderID || oi.OriginalOrderID == orderEntity.OrderID)
                    .FirstOrDefault(
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
        private string GetShipmentDetails(MagentoOrderEntity orderEntity, string comments, bool emailCustomer, IEnumerable<Item> items, long originalOrderID)
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

            if (orderEntity.OrderItems?.Any() ?? false)
            {
                request.Items = new List<ShipmentItem>();

                foreach (Item item in items)
                {
                    ShipmentItem magentoInvoiceItem = new ShipmentItem()
                    {
                        Qty = GetQuantity(orderEntity, item, originalOrderID),
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
            string service;
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

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(service, "");
        }
    }
}