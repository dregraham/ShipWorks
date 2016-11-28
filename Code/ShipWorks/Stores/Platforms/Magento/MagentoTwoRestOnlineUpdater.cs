using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.Security;
using log4net;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.Tools.BCLExtensions.CollectionsRelated;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.DTO;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Online updater for Magento 2 stores using the REST API
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.GenericModule.GenericStoreOnlineUpdater" />
    /// <seealso cref="ShipWorks.Stores.Platforms.Magento.IMagentoOnlineUpdater" />
    [KeyedComponent(typeof(IMagentoOnlineUpdater), MagentoVersion.MagentoTwoREST, true)]
    public class MagentoTwoRestOnlineUpdater : GenericStoreOnlineUpdater, IMagentoOnlineUpdater
    {
        private readonly MagentoStoreEntity store;
        private static readonly ILog log = LogManager.GetLogger(typeof(MagentoTwoRestOnlineUpdater));

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestOnlineUpdater"/> class.
        /// </summary>
        public MagentoTwoRestOnlineUpdater(GenericModuleStoreEntity store) : base(store)
        {
            this.store = (MagentoStoreEntity) store;
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
            MagentoOrderEntity order = DataProvider.GetEntity(orderID) as MagentoOrderEntity;
            if (order == null)
            {
                return;
            }
            if (order.IsManual)
            {
                log.InfoFormat("Not executing online action since order {0} is manual.", order.OrderID);
                return;
            }

            string processedComments = TemplateTokenProcessor.ProcessTokens(comments, orderID);

            Uri storeUri = new Uri(store.ModuleUrl);

            using (var scope = IoC.BeginLifetimeScope())
            {
                IMagentoTwoRestClient webClient = scope.Resolve<IMagentoTwoRestClient>();

                string token = webClient.GetToken(storeUri, store.ModuleUsername,
                    SecureText.Decrypt(store.ModulePassword, store.ModuleUsername));

                long magentoOrderID = order.MagentoOrderID;

                switch (command)
                {
                    case MagentoUploadCommand.Complete:
                        string shipmentDetails = GetShipmentDetails(order, processedComments, emailCustomer);
                        string invoice = GetInvoice(order);
                        webClient.UploadShipmentDetails(shipmentDetails, invoice, storeUri, token, magentoOrderID);
                        SaveOnlineStatus(order, "complete");
                        break;
                    case MagentoUploadCommand.Hold:
                        webClient.HoldOrder(storeUri, token, order.MagentoOrderID);
                        UploadCommentsIfPresent(webClient, processedComments, storeUri, token, magentoOrderID);
                        SaveOnlineStatus(order, "hold");
                        break;
                    case MagentoUploadCommand.Unhold:
                        webClient.UnholdOrder(storeUri, token, magentoOrderID);
                        UploadCommentsIfPresent(webClient, processedComments, storeUri, token, magentoOrderID);
                        SaveOnlineStatus(order, "pending");
                        break;
                    case MagentoUploadCommand.Cancel:
                        webClient.CancelOrder(storeUri, token, magentoOrderID);
                        UploadCommentsIfPresent(webClient, processedComments, storeUri, token, magentoOrderID);
                        SaveOnlineStatus(order, "canceled");
                        break;
                    case MagentoUploadCommand.Comments:
                        UploadCommentsIfPresent(webClient, processedComments, storeUri, token, magentoOrderID);
                        break;
                }
            }

            unitOfWork.AddForSave(order);
        }

        /// <summary>
        /// Gets the invoice.
        /// </summary>
        private string GetInvoice(MagentoOrderEntity order)
        {
            MagentoInvoiceRequest request = new MagentoInvoiceRequest();
            request.Invoice.MagentoOrderID = order.MagentoOrderID;

            foreach (var item in order.OrderItems)
            {
                request.Invoice.Items.Add(new MagentoInvoiceItem()
                {
                    MagentoOrderItemId = Convert.ToInt64(item.Code),
                    Qty = item.Quantity
                });

                request.Invoice.TotalQty += item.Quantity;
            }

            return JsonConvert.SerializeObject(request);
        }

        /// <summary>
        /// Saves the online status to the ShipWorks OrderEntity
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="status">The status.</param>
        private void SaveOnlineStatus(OrderEntity order, string status)
        {
            // set status to what was returned
            order.OnlineStatusCode = status;
            order.OnlineStatus = status;
        }

        /// <summary>
        /// Tell web client to upload comments if they are present
        /// </summary>
        private void UploadCommentsIfPresent(IMagentoTwoRestClient webClient, string comments, Uri storeUri, string token, long magentoOrderID)
        {
            if (!string.IsNullOrWhiteSpace(comments))
            {
                webClient.UploadComments(comments, storeUri, token, magentoOrderID);
            }
        }

        /// <summary>
        /// Gets the shipment details string from the request
        /// </summary>
        private string GetShipmentDetails(MagentoOrderEntity order, string comments, bool emailCustomer)
        {
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
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
                    CarrierCode = GetCarrierCode(shipment), TrackNumber = shipment.TrackingNumber
                });
            }

            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }
            if (!order.OrderItems.IsNullOrEmpty())
            {
                request.Items = new List<ShipmentItem>();
                foreach (var item in order.OrderItems)
                {
                    request.Items.Add(new ShipmentItem()
                    {
                        OrderItemId = Convert.ToInt64(item.Code), Qty = item.Quantity
                    });
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
    }
}