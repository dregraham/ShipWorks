using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
                if (ex.InnerException is WebException &&
                        ((HttpWebResponse)((WebException)ex.InnerException).Response).StatusCode == HttpStatusCode.NotFound)
                {
                    //etsy couldn't find the order, mark status as not found.
                    EtsyOrderStatusUtility.MarkOrderAsNotFound(order, unitOfWork);
                }
                else
                {
                    throw;                        
                }
            }
 
        }
    }
}
