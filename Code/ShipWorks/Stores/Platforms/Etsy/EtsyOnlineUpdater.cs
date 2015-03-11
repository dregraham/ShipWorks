using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.Postal;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Templates.Tokens;
using ShipWorks.Stores.Content;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Updates payment and tracking information to Etsy.
    /// </summary>
    public class EtsyOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EtsyOnlineUpdater));
        readonly EtsyStoreEntity etsyStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyOnlineUpdater(EtsyStoreEntity store)
        {
            etsyStore = store;
        }

        /// <summary>
        /// Given shipmentID, send comment, markAsPad, and markAsShipped as applicable
        /// </summary>
        public void UpdateOnlineStatus(long shipmentID, bool? markAsPaid, bool? markAsShipped, string comment, UnitOfWork2 unitOfWork)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);

            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            UpdateOnlineStatus(shipment, markAsPaid, markAsShipped, comment, unitOfWork);
        }

        /// <summary>
        /// Update the online status of the given shipment
        /// </summary>
        public void UpdateOnlineStatus(ShipmentEntity shipment, bool? markAsPaid, bool? markAsShipped, string comment)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();

            UpdateOnlineStatus(shipment, markAsPaid, markAsShipped, comment, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Upload the online status of the shpment
        /// </summary>
        public void UpdateOnlineStatus(ShipmentEntity shipment, bool? markAsPaid, bool? markAsShipped, string comment, UnitOfWork2 unitOfWork)
        {
            if (shipment == null)
            {
                log.Info("Not uploading tracking number since shipment is null.");
                return;
            }

            EtsyOrderEntity order = shipment.Order as EtsyOrderEntity;
            if (order == null)
            {
                log.Error("shipment not associated with order in EtsyOnlineUpdater.UploadStatusAndComment");
                throw new EtsyException("Shipment not associated with order");
            }

            // The comment is tokenizable
            string processedComment = string.IsNullOrEmpty(comment) ? string.Empty : TemplateTokenProcessor.ProcessTokens(comment, shipment.ShipmentID);

            UpdateOnlineStatus(order, markAsPaid, markAsShipped, processedComment, unitOfWork);
        }

        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        public void UpdateOnlineStatus(EtsyOrderEntity order, bool? markAsPaid, bool? markAsShipped)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();

            UpdateOnlineStatus(order, markAsPaid, markAsShipped, "", unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        private void UpdateOnlineStatus(EtsyOrderEntity order, bool? markAsPaid, bool? markAsShipped, string processedComment, UnitOfWork2 unitOfWork)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (order.IsManual)
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
                return;
            }

            // Update the online status
            EtsyWebClient webClient = new EtsyWebClient(etsyStore);

            try
            {
                //set the order status at etsy
                webClient.UploadStatusDetails(order.OrderNumber, processedComment, markAsPaid, markAsShipped);

                // Then update the status in our local database
                EtsyOrderStatusUtility.UpdateOrderStatus(order, markAsPaid, markAsShipped, unitOfWork);
            }
            catch (EtsyException ex)
            {
                if (!TryManageException(order, unitOfWork, ex))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        /// <param name="shipmentID">The shipment ID.</param>
        internal void UploadShipmentDetails(long shipmentID, UnitOfWork2 unitOfWork)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);

            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            EtsyOrderEntity order = shipment.Order as EtsyOrderEntity;

            if (order == null)
            {
                log.Error("shipment not associated with order in EtsyOnlineUpdater.UploadShipmentDetails");

                throw new EtsyException("Shipment not associated with order");
            }

            UploadShipmentDetails(order, shipment, unitOfWork);
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        internal void UploadShipmentDetails(long orderID)
        {
            EtsyOrderEntity order = (EtsyOrderEntity)DataProvider.GetEntity(orderID);

            if (order==null)
            {
                throw new EtsyException("Order not found");
            }

            List<ShipmentEntity> shipments = ShippingManager.GetShipments(orderID, false);
            if (shipments==null)
            {
                log.InfoFormat("No shipments associated with order {0}.", orderID);
                return;
            }

            ShipmentEntity shipment = shipments.FirstOrDefault(x => !string.IsNullOrEmpty(x.TrackingNumber));
            if (shipment==null)
            {
                log.InfoFormat("No shipment with tracking number associated with order {0}.", orderID);
            }

            UnitOfWork2 unitOfWork = new UnitOfWork2();

            UploadShipmentDetails(order, shipment, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="shipment">The shipment.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private void UploadShipmentDetails(EtsyOrderEntity order, ShipmentEntity shipment, UnitOfWork2 unitOfWork)
        {
            var webClient = new EtsyWebClient(etsyStore);
            
            try
            {
                webClient.UploadShipmentDetails(etsyStore.EtsyShopID, order.OrderNumber, shipment.TrackingNumber, GetEtsyCarrierCode(shipment));
            }
            catch (EtsyException ex)
            {
               if (!TryManageException(order, unitOfWork, ex))
               {
                   throw;
               }
            }
        }

        /// <summary>
        /// Tries to see if exception was expected from Etsy.
        /// </summary>
        private static bool TryManageException(EtsyOrderEntity order, UnitOfWork2 unitOfWork, EtsyException ex)
        {
            bool exceptionManaged = false;

            if (ex.InnerException is WebException &&
                ((HttpWebResponse) ((WebException) ex.InnerException).Response).StatusCode == HttpStatusCode.NotFound)
            {
                //etsy couldn't find the order, mark status as not found.
                EtsyOrderStatusUtility.MarkOrderAsNotFound(order, unitOfWork);
                exceptionManaged = true;
            }

            return exceptionManaged;
        }

        /// <summary>
        /// Gets the etsy carrier code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private static string GetEtsyCarrierCode(ShipmentEntity shipment)
        {
            ShippingManager.EnsureShipmentLoaded(shipment);

            ShipmentTypeCode type = (ShipmentTypeCode)shipment.ShipmentType;

            switch (type)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return "ups";

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    // The shipment is an Endicia or Usps shipment, check to see if it's DHL
                    if (ShipmentTypeManager.IsDhl((PostalServiceType)shipment.Postal.Service))
                    {
                        // The DHL carrier for Endicia is:
                        return "dhl";
                    }

                    // Use the default carrier for other Endicia types
                    return "usps";

                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                    return "usps";
                case ShipmentTypeCode.FedEx:
                    return "fedex";
                case ShipmentTypeCode.OnTrac:
                    return "ontrac";
                case ShipmentTypeCode.iParcel:
                    return "iparcel";
                case ShipmentTypeCode.Other:
                    return "other";
                case ShipmentTypeCode.None:
                    return "none";
                default:
                    return "other";
            }
        }
    }
}
