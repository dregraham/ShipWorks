using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Autofac;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Yahoo.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// Responsible for updating the online status of Yahoo! orders
    /// </summary>
    public class YahooEmailOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(YahooEmailOnlineUpdater));

        /// <summary>
        /// Upload the shipment details of the most recent shipment of the given order
        /// </summary>
        public async Task<IEnumerable<EmailOutboundEntity>> GenerateOrderShipmentUpdateEmail(long orderID)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);

            if (shipment == null)
            {
                log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}", orderID);
                return Enumerable.Empty<EmailOutboundEntity>();
            }

            return await GenerateShipmentUpdateEmail(shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Upload the shipment details of the given shipment
        /// </summary>
        public async Task<IEnumerable<EmailOutboundEntity>> GenerateShipmentUpdateEmail(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading shipment details for {0} since it went away.", shipmentID);
                return Enumerable.Empty<EmailOutboundEntity>();
            }

            return await GenerateShipmentUpdateEmail(shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Upload the shipment details of the given shipment
        /// </summary>
        private async Task<IEnumerable<EmailOutboundEntity>> GenerateShipmentUpdateEmail(ShipmentEntity shipment)
        {
            if (shipment.Order.IsManual)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0} because the order is manual.", shipment.ShipmentID);
                return Enumerable.Empty<EmailOutboundEntity>();
            }

            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return Enumerable.Empty<EmailOutboundEntity>();
            }

            try
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }
            catch (ObjectDeletedException)
            {
                // Shipment was deleted
                return Enumerable.Empty<EmailOutboundEntity>();
            }
            catch (SqlForeignKeyException)
            {
                // Shipment was deleted
                return Enumerable.Empty<EmailOutboundEntity>();
            }

            YahooOrderEntity order = (YahooOrderEntity) shipment.Order;
            YahooStoreEntity store = (YahooStoreEntity) StoreManager.GetStore(order.StoreID);

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                var searchProvider = lifetimeScope.Resolve<IYahooCombineOrderSearchProvider>();
                var identifiers = await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);
                return identifiers.Select(x => GenerateShipmentUpdateEmailForOrder(shipment, order, store, x)).ToList();
            }
        }

        /// <summary>
        /// Generate an email for a given order id
        /// </summary>
        private EmailOutboundEntity GenerateShipmentUpdateEmailForOrder(ShipmentEntity shipment, YahooOrderEntity order, YahooStoreEntity store, string yahooOrderID)
        {
            string emailXmlContent = GenerateEmailXmlContent(shipment, order.YahooOrderID, store.TrackingUpdatePassword);
            EmailAccountEntity account = EmailAccountManager.GetAccount(store.YahooEmailAccountID);

            // This should never happen
            if (account == null)
            {
                throw new YahooException("The email account associated with the store has been destroyed.");
            }

            // This would only happen if the user skipped setup during migration/upgrade, and they didn't have
            // Yahoo! updates configured in V2.
            if (string.IsNullOrEmpty(account.OutgoingServer))
            {
                throw new YahooException("The email account associated with the store is not configured for sending email.");
            }

            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Yahoo, "TrackingUpdate");
            logEntry.LogRequest(emailXmlContent);

            return CreateEmailOutboundEntity(shipment, order.OrderID, account, emailXmlContent, yahooOrderID);
        }

        /// <summary>
        /// Creates the email outbound entity.
        /// </summary>
        private EmailOutboundEntity CreateEmailOutboundEntity(ShipmentEntity shipment, long orderID,
            EmailAccountEntity account, string emailXmlContent, string yahooOrderID)
        {
            EmailMessageHeader header = new EmailMessageHeader
            {
                To = "tracking-update@store.yahoo.com",
                Subject = $"Tracking update for '{yahooOrderID}'",
                EmailAccountID = account.EmailAccountID,
                Visibility = EmailOutboundVisibility.OutboxOnly
            };

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                EmailOutboundEntity email = EmailOutbox.AddMessage(header, null, emailXmlContent);

                // Set the context
                email.ContextID = shipment.ShipmentID;
                email.ContextType = EntityUtility.GetEntitySeed(EntityType.ShipmentEntity);

                // Add the relations
                email.RelatedObjects.Add(new EmailOutboundRelationEntity { EntityID = shipment.ShipmentID, RelationType = (int) EmailOutboundRelationType.ContextObject });
                email.RelatedObjects.Add(new EmailOutboundRelationEntity { EntityID = orderID, RelationType = (int) EmailOutboundRelationType.RelatedObject });

                adapter.SaveAndRefetch(email);

                adapter.Commit();

                return email;
            }
        }

        /// <summary>
        /// Generate the XML content needed to update online for the given shipment
        /// </summary>
        private string GenerateEmailXmlContent(ShipmentEntity shipment, string orderID, string trackingPassword)
        {
            if (trackingPassword.Length == 0)
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
                xmlWriter.WriteAttributeString("password", SecureText.Decrypt(trackingPassword, "yahoo"));

                xmlWriter.WriteStartElement("YahooOrder");

                // Yahoo order id
                xmlWriter.WriteAttributeString("id", orderID);

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

            if (ShipmentTypeManager.IsUps((ShipmentTypeCode) shipment.ShipmentType) &&
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
                    if (UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service) && !shipment.Ups.UspsTrackingNumber.IsNullOrWhiteSpace())
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
