using System;
using System.IO;
using System.Text;
using System.Xml;
using Interapptive.Shared.Utility;
using log4net;
using Quartz.Util;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared;
using Interapptive.Shared.Net;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// Responsible for updating the online status of Yahoo! orders
    /// </summary>
    public class YahooEmailOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(YahooEmailOnlineUpdater));

        /// <summary>
        /// Constructor
        /// </summary>
        public YahooEmailOnlineUpdater()
        {

        }

        /// <summary>
        /// Upload the shipment details of the most recent shipment of the given order
        /// </summary>
        public EmailOutboundEntity GenerateOrderShipmentUpdateEmail(long orderID)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);

            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
                return null;
            }

            return GenerateShipmentUpdateEmail(shipment);
        }

        /// <summary>
        /// Upload the shipment details of the given shipment
        /// </summary>
        public EmailOutboundEntity GenerateShipmentUpdateEmail(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading shipment details for {0} since it went away.", shipmentID);
                return null;
            }

            return GenerateShipmentUpdateEmail(shipment);
        }

        /// <summary>
        /// Upload the shipment details of the given shipment
        /// </summary>
        [NDependIgnoreLongMethod]
        public EmailOutboundEntity GenerateShipmentUpdateEmail(ShipmentEntity shipment)
        {
            if (shipment.Order.IsManual)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0} because the order is manual.", shipment.ShipmentID);
                return null;
            }

            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return null;
            }

            try
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }
            catch (ObjectDeletedException)
            {
                // Shipment was deleted
                return null;
            }
            catch (SqlForeignKeyException)
            {
                // Shipment was deleted
                return null;
            }

            string emailXmlContent = GenerateEmailXmlContent(shipment);

            YahooOrderEntity order = (YahooOrderEntity) shipment.Order;
            YahooStoreEntity store = (YahooStoreEntity) StoreManager.GetStore(order.StoreID);
            EmailAccountEntity account = EmailAccountManager.GetAccount(store.YahooEmailAccountID);

            // This should never happen
            if (account == null)
            {
                throw new YahooException("The email account associated with the store has been destroyed.");
            }

            // This would only happen if the user skipped setup during migration/upgrade, and they didn't have
            // Yahoo! updates configured in V2.
            if (String.IsNullOrEmpty(account.OutgoingServer))
            {
                throw new YahooException("The email account associated with the store is not configured for sending email.");
            }

            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Yahoo, "TrackingUpdate");
            logEntry.LogRequest(emailXmlContent);

            EmailMessageHeader header = new EmailMessageHeader();
            header.To = "tracking-update@store.yahoo.com";
            header.Subject = string.Format("Tracking update for '{0}'", order.YahooOrderID);
            header.EmailAccountID = account.EmailAccountID;
            header.Visibility = EmailOutboundVisibility.OutboxOnly;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                EmailOutboundEntity email = EmailOutbox.AddMessage(header, null, emailXmlContent);

                // Set the context
                email.ContextID = shipment.ShipmentID;
                email.ContextType = EntityUtility.GetEntitySeed(EntityType.ShipmentEntity);

                // Add the relations
                email.RelatedObjects.Add(new EmailOutboundRelationEntity { ObjectID = shipment.ShipmentID, RelationType = (int) EmailOutboundRelationType.ContextObject });
                email.RelatedObjects.Add(new EmailOutboundRelationEntity { ObjectID = order.OrderID, RelationType = (int) EmailOutboundRelationType.RelatedObject });

                adapter.SaveAndRefetch(email);

                adapter.Commit();

                return email;
            }
        }

        /// <summary>
        /// Generate the XML content needed to update online for the given shipment
        /// </summary>
        private string GenerateEmailXmlContent(ShipmentEntity shipment)
        {
            YahooOrderEntity order = (YahooOrderEntity) shipment.Order;
            YahooStoreEntity store = (YahooStoreEntity) StoreManager.GetStore(order.StoreID);

            if (store == null)
            {
                throw new YahooException("The Yahoo! store was deleted.");
            }

            if (store.TrackingUpdatePassword.Length == 0)
            {
                throw new YahooException("You must configured the 'Email Tracking Password' in the store settings window.");
            }

            string trackingNumber;
            string shipperString;

            ShippingManager.EnsureShipmentLoaded(shipment);

            GetShipmentUploadValues(shipment, out shipperString, out trackingNumber);

            using (MemoryStream stream = new MemoryStream())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.UTF8);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteDocType("TrackingUpdate", null, "http://store.yahoo.com/lib/vw/TrackingUpdate.dtd", null);

                xmlWriter.WriteStartElement("TrackingUpdate");

                // Email tracking password
                xmlWriter.WriteAttributeString("password", SecureText.Decrypt(store.TrackingUpdatePassword, "yahoo"));

                xmlWriter.WriteStartElement("YahooOrder");

                // Yahoo order id
                xmlWriter.WriteAttributeString("id", order.YahooOrderID);

                // Shipped (Can be YES, or NO)
                xmlWriter.WriteAttributeString("shipped", "YES");

                // Package tacking number
                xmlWriter.WriteAttributeString("trackingNumber", shipment.TrackingNumber);

                // Shipper (Can be "Ups", "Fedex", "Airborne", or "Usps")
                xmlWriter.WriteAttributeString("shipper", shipperString);

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Gets the shipment values to upload to Yahoo!
        /// </summary>
        private static void GetShipmentUploadValues(ShipmentEntity shipment, out string shipperString, out string trackingNumber)
        {
            string tempTrackingNumber = shipment.TrackingNumber;
            string tempShipperString = GetShipperString(shipment);

            if (ShipmentTypeManager.IsUps((ShipmentTypeCode)shipment.ShipmentType) &&
                UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service) &&
                !shipment.Ups.UspsTrackingNumber.IsNullOrWhiteSpace())
            {
                tempTrackingNumber = shipment.Ups.UspsTrackingNumber;
            }

            shipperString = tempShipperString;
            trackingNumber = tempTrackingNumber;
        }

        /// <summary>
        /// Get the string to use as the shipper for the yahoo update
        /// </summary>
        public static string GetShipperString(ShipmentEntity shipment)
        {
            ShipmentTypeCode type = (ShipmentTypeCode) shipment.ShipmentType;

            switch (type)
            {
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    PostalServiceType service = (PostalServiceType) shipment.Postal.Service;

                    if (ShipmentTypeManager.IsDhl(service))
                    {
                        return "Dhl";
                    }

                    if (ShipmentTypeManager.IsConsolidator(service))
                    {
                        return "Consolidator";
                    }

                    return "Usps";

                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    return "Usps";

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    if (UpsUtility.IsUpsMiService((UpsServiceType)shipment.Ups.Service) && !shipment.Ups.UspsTrackingNumber.IsNullOrWhiteSpace())
                    {
                        return "Usps";
                    }

                    return "Ups";

                case ShipmentTypeCode.FedEx:
                    return "Fedex";

                case ShipmentTypeCode.iParcel:
                    return "iParcel";

                case ShipmentTypeCode.OnTrac:
                    return "OnTrac";

                case ShipmentTypeCode.Other:
                    // As of June 2014, Yahoo only allows UPS, Fedex, DHL, and Usps as the carrier by default
                    // If someone ships with 'Other', we'll try to send the right one if we can
                    if (shipment.Other.Carrier.ToLower().Contains("ups"))
                    {
                        return "Ups";
                    }

                    if (shipment.Other.Carrier.ToLower().Contains("fedex"))
                    {
                        return "Fedex";
                    }

                    if (shipment.Other.Carrier.ToLower().Contains("dhl"))
                    {
                        return "DHL";
                    }

                    return "Usps";

                default:
                    return "Usps";
            }
        }
    }
}
