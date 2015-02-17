using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using ShipWorks.Data;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Templates.Tokens;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Data.Model;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Handles performing actions on Magento orders
    /// </summary>
    public class MagentoOnlineUpdater : GenericStoreOnlineUpdater
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(MagentoOnlineUpdater));

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoOnlineUpdater(GenericModuleStoreEntity store) : base(store)
        {
        }

        /// <summary>
        /// Gets the shipping details to send to magento
        /// </summary>
        private void GetShipmentDetails(OrderEntity order, ref string carrier, ref string tracking)
        {
            string tempCarrier = "";
            string tempTracking = "";

            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
            if (shipment != null)
            {
                tempTracking = shipment.TrackingNumber;
                tempCarrier = CreateCarrierString(shipment);

                // Adjust tracking details per Mail Innovations and others
                WorldShipUtility.DetermineAlternateTracking(shipment, (track, service) =>
                    {
                        if (track.Length > 0)
                        {
                            tempCarrier = "usps";
                            tempTracking = track;
                        }
                    });
            }

            carrier = tempCarrier;
            tracking = tempTracking;
        }

        /// <summary>
        /// Gets the magento-recognized shipment carrier string for a shipment
        /// </summary>
        private string CreateCarrierString(ShipmentEntity shipment)
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

            string title = ShippingManager.GetServiceUsed(shipment);

            return string.Format("{0}|{1}", code, title);
        }

        /// <summary>
        /// Executes an action on the specified order
        /// </summary>
        public void ExecuteOrderAction(long orderID, string action, string comments, bool magentoEmails)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            ExecuteOrderAction(orderID, action, comments, magentoEmails, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Executes an action on the specified order
        /// </summary>
        public void ExecuteOrderAction(long orderID, string action, string comments, bool magentoEmails, UnitOfWork2 unitOfWork)
        {
            MagentoOrderEntity order = DataProvider.GetEntity(orderID) as MagentoOrderEntity;
            if (order != null)
            {
                if (!order.IsManual)
                {
                    string processedComments = TemplateTokenProcessor.ProcessTokens(comments, orderID);

                    MagentoWebClient webclient = (MagentoWebClient)GenericStoreType.CreateWebClient();

                    // look for any shipping information if we're Completing an order
                    string carrier = "";
                    string tracking = "";
                    if (action == "complete")
                    {
                        GetShipmentDetails(order, ref carrier, ref tracking);
                    }

                    // execute the action
                    string newStatus = webclient.ExecuteAction(order.MagentoOrderID, action, processedComments, carrier, tracking, magentoEmails);

                    // set status to what was returned
                    order.OnlineStatusCode = newStatus;
                    order.OnlineStatus = StatusCodes.GetCodeName(newStatus);

                    unitOfWork.AddForSave(order);
                }
                else
                {
                    log.InfoFormat("Not executing online action since order {0} is manual.", order.OrderID);
                }
            }
        }
    }
}
