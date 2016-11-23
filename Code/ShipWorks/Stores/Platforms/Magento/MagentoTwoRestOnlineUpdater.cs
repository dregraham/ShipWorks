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
    [KeyedComponent(typeof(IMagentoOnlineUpdater), MagentoVersion.MagentoTwoREST)]

    public class MagentoTwoRestOnlineUpdater : GenericStoreOnlineUpdater, IMagentoOnlineUpdater
    {
        private readonly MagentoStoreEntity store;
        private static readonly ILog log = LogManager.GetLogger(typeof(MagentoTwoRestOnlineUpdater));


        public MagentoTwoRestOnlineUpdater(GenericModuleStoreEntity store) : base(store)
        {
            this.store = (MagentoStoreEntity) store;
        }

        public void UploadShipmentDetails(long orderID, string action, string comments, bool emailCustomer)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UploadShipmentDetails(orderID, action, comments, emailCustomer, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        public void UploadShipmentDetails(long orderID, string action, string comments, bool emailCustomer, UnitOfWork2 unitOfWork)
        {
            MagentoOrderEntity order = DataProvider.GetEntity(orderID) as MagentoOrderEntity;
            if (order != null)
            {
                if (!order.IsManual)
                {
                    string processedComments = TemplateTokenProcessor.ProcessTokens(comments, orderID);

                    using (var scope = IoC.BeginLifetimeScope())
                    {
                        IMagentoTwoRestClient webClient = scope.Resolve<IMagentoTwoRestClient>();
                        Uri storeUri = new Uri(store.ModuleUrl);


                        string token = webClient.GetToken(storeUri, store.ModuleUsername, SecureText.Decrypt(store.ModulePassword, store.ModuleUsername));


                        // look for any shipping information if we're Completing an order
                        string carrier = "";
                        string tracking = "";

                        string shipmentDetails = GetShipmentDetails(order, action, processedComments, emailCustomer);


                        // execute the action
                        webClient.UploadShipmentDetails(shipmentDetails, storeUri, token, order.MagentoOrderID);

                        unitOfWork.AddForSave(order);
                    }
                }
                else
                {
                    log.InfoFormat("Not executing online action since order {0} is manual.", order.OrderID);
                }
            }
        }

        private string GetShipmentDetails(MagentoOrderEntity order, string action, string comments, bool emailCustomer)
        {
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
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
                    TrackNumber = shipment.TrackingNumber
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
                        OrderItemId = Convert.ToInt64(item.Code),
                        Qty = item.Quantity
                    });
                }
            }

            return JsonConvert.SerializeObject(request);
        }

        private string GetCarrierCode(ShipmentEntity shipment)
        {
            string code = "";
            switch ((ShipmentTypeCode)shipment.ShipmentType)
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