using System;
using System.Collections.Generic;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Templates.Tokens;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Handles performing actions on Magento orders
    /// </summary>
    public class MagentoOnlineUpdater : GenericStoreOnlineUpdater, IMagentoOnlineUpdater
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
            carrier = "";
            tracking = "";

            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
            if (shipment != null)
            {
                tracking = shipment.TrackingNumber;
                carrier = CreateCarrierString(shipment);
            }
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

            string title = ShippingManager.GetOverriddenServiceUsed(shipment);
            // Strip out everything except for alpha numeric, period and parenthesis
            // 3dcart throws a 400 bad request when the request contains other characters
            string alphaNumericTitle = new Regex("[^a-zA-Z0-9 .()-]").Replace(title, "");

            return string.Format("{0}|{1}", code, alphaNumericTitle);
        }

        /// <summary>
        /// Executes an action on the specified order
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
        /// Executes an action on the specified order
        /// </summary>
        public async Task UploadShipmentDetails(long orderID, MagentoUploadCommand command, string comments, bool emailCustomer, UnitOfWork2 unitOfWork)
        {
            MagentoOrderEntity order = DataProvider.GetEntity(orderID) as MagentoOrderEntity;
            if (order != null)
            {
                if (order.IsManual && order.CombineSplitStatus == CombineSplitStatusType.None)
                {
                    log.InfoFormat("Not executing online action since order {0} is manual.", order.OrderID);
                }
                else
                {
                    string processedComments = TemplateTokenProcessor.ProcessTokens(comments, orderID);

                    MagentoWebClient webclient = (MagentoWebClient) GenericStoreType.CreateWebClient();

                    // look for any shipping information if we're Completing an order
                    string carrier = "";
                    string tracking = "";
                    if (command == MagentoUploadCommand.Complete)
                    {
                        GetShipmentDetails(order, ref carrier, ref tracking);
                    }

                    string newStatus = string.Empty;

                    using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                    {
                        ICombineOrderSearchProvider<MagentoOrderSearchEntity> orderSearchProvider =
                            scope.ResolveKeyed<ICombineOrderSearchProvider<MagentoOrderSearchEntity>>(StoreTypeCode.Magento);

                        IEnumerable<MagentoOrderSearchEntity> orderSearchEntities = await orderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                        foreach (MagentoOrderSearchEntity orderSearchEntity in orderSearchEntities)
                        {
                            // execute the action
                            newStatus = webclient.ExecuteAction(orderSearchEntity.MagentoOrderID, EnumHelper.GetDescription(command), processedComments, carrier, tracking, emailCustomer);
                        }
                    }

                    // set status to what was returned
                    order.OnlineStatusCode = newStatus;
                    order.OnlineStatus = StatusCodes.GetCodeName(newStatus);

                    unitOfWork.AddForSave(order);
                }
            }
        }
    }
}
