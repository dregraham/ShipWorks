using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon.WebServices.SellerCentral;
using System.Net;
using System.Web.Services.Protocols;
using ShipWorks.Shipping;
using log4net;
using System.IO;
using ShipWorks.Stores.Platforms.Amazon.Dime;
using ShipWorks.ApplicationCore;
using System.Xml;
using System.Globalization;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Web client for the Amazon Feed service
    /// </summary>
    public class AmazonFeedClient
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonFeedClient));

        // store this client is interacting on behalf of
        AmazonStoreEntity store = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonFeedClient(AmazonStoreEntity amazonStore)
        {
            store = amazonStore;
        }

        /// <summary>
        /// Generate and submit the fulfillment feed for the order/shipment
        /// </summary>
        public void SubmitFulfillmentFeed(List<ShipmentEntity> shipments)
        {
            if (shipments == null || shipments.Count == 0)
            {
                return;
            }

            UploadFulfillmentFeed(CreateFulfillmentFeed(shipments));
        }

        /// <summary>
        /// Uploads the fulfillment feed located at filePath
        /// </summary>
        private void UploadFulfillmentFeed(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Unable to submit fulfillment data to Amazon, file not found.", filePath);
            }

            using (SellerCentralDimeService service = CreateDimeService("UploadShipmentFeed"))
            {
                // Setup merchant info
                Merchant merchantInfo = new Merchant();

                // the merchantIdentifier token number is supplied to you by your Amazon Engagement Manager
                merchantInfo.merchantIdentifier = store.MerchantToken;
                merchantInfo.merchantName = store.MerchantName;

                string id = Guid.NewGuid().ToString();

                // add the file as an attachment
                service.AddAttachment(id, filePath, "text/xml");
                service.ApiLogEntry.LogRequestSupplement(new FileInfo(filePath), "DimeAttachment");

                // create a pointer to the attachment by id
                ReferencedBinary outgoingDoc = new ReferencedBinary();
                outgoingDoc.location = id;

                try
                {
                    DocumentSubmissionResponse response = service.postDocument(merchantInfo, "_POST_ORDER_FULFILLMENT_DATA_", outgoingDoc);

                    log.InfoFormat("Amazon responded with document ID {0}", response.documentTransactionID);
                }
                catch (Exception ex)
                {
                    TranslateException(ex);

                    throw;
                }
            }

            try
            {
                File.Delete(filePath);
            }
            catch (IOException ex)
            {
                // Its really ok - shipworks deletes temp stuff later on a background idle watcher
                Debug.Fail("Cannot delete feed file", ex.ToString());
            }
            catch (UnauthorizedAccessException ex)
            {
                // Its really ok - shipworks deletes temp stuff later on a background idle watcher
                Debug.Fail("Cannot delete feed file", ex.ToString());
            }
        }

        /// <summary>
        /// Creates the Amazon Feed Xml for submitting and returns the path it is written to
        /// </summary>
        [NDependIgnoreLongMethod]
        private string CreateFulfillmentFeed(List<ShipmentEntity> shipments)
        {
            string path = Path.Combine(DataPath.ShipWorksTemp, Guid.NewGuid().ToString()) + ".xml";

            XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            WriteFulfillmentStart(writer);

            // Write each shipment
            foreach (ShipmentEntity shipment in shipments)
            {
                AmazonOrderEntity amazonOrder = shipment.Order as AmazonOrderEntity;
                if (amazonOrder == null)
                {
                    log.WarnFormat("Shipment '{0}' was not uploaded to Amazon because it is not for an Amazon order.", shipment.ShipmentID);
                    continue;
                }

                if (amazonOrder.IsManual)
                {
                    log.WarnFormat("Shipment '{0}' was not uploaded to Amazon because it is manual.", shipment.ShipmentID);
                    continue;
                }

                if (!shipment.Processed || shipment.Voided)
                {
                    log.WarnFormat("Shipment '{0}' was not uploaded to Amazon because it is not processed or it is voided.", shipment.ShipmentID);
                    continue;
                }

                writer.WriteStartElement("Message");
                writer.WriteElementString("MessageID", amazonOrder.OrderNumber.ToString());

                writer.WriteStartElement("OrderFulfillment");
                writer.WriteElementString("AmazonOrderID", amazonOrder.AmazonOrderID);

                DateTime shipDate = shipment.ShipDate.ToLocalTime();

                // shipdate can't be before the order was placed
                if (shipDate < shipment.Order.OrderDate)
                {
                    // set it 10 minutes after it was placed
                    shipDate = shipment.Order.OrderDate.AddMinutes(10);
                }

                // shipdate can't be in the future
                if (shipDate > DateTime.Now)
                {
                    shipDate = DateTime.Now;
                }

                writer.WriteElementString("FulfillmentDate", shipDate.ToString("yyyy-MM-ddTHH:mm:sszzzz"));

                writer.WriteStartElement("FulfillmentData");

                // Per an email on 9/11/07, Amazon will only respond correctly if the code is in upper case, and if its also apart of the method.
                ShipmentTypeCode shipmentType = (ShipmentTypeCode)shipment.ShipmentType;
                string trackingNumber = shipment.TrackingNumber;
                string serviceUsed = ShippingManager.GetServiceUsed(shipment);
                string carrier = ShippingManager.GetCarrierName(shipmentType).ToUpper(CultureInfo.InvariantCulture);

                // Adjust tracking details per Mail Innovations and others
                WorldShipUtility.DetermineAlternateTracking(shipment, (track, service) =>
                    {
                        if (track.Length > 0)
                        {
                            trackingNumber = track;
                            carrier = "UPS Mail Innovations";
                        }
                        else
                        {
                            shipmentType = ShipmentTypeCode.Other;
                        }

                    });

                writer.WriteElementString("CarrierName", carrier);
                writer.WriteElementString("ShippingMethod", serviceUsed);

                writer.WriteElementString("ShipperTrackingNumber", trackingNumber);
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            return path;
        }

        /// <summary>
        /// Write the common header information for the fulfillment report.
        /// </summary>
        private void WriteFulfillmentStart(XmlTextWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("AmazonEnvelope");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "amzn-envelope.xsd");

            writer.WriteStartElement("Header");
            writer.WriteElementString("DocumentVersion", "1.01");
            writer.WriteElementString("MerchantIdentifier", store.MerchantToken);
            writer.WriteEndElement();

            writer.WriteElementString("MessageType", "OrderFulfillment");
        }

        /// <summary>
        /// Tests connectivity with Amazon using the credentials in the store entity.
        /// </summary>
        public void TestConnection()
        {
            try
            {
                using (merchantinterfacedime service = CreateService("TestConnection"))
                {
                    // set up the merchant information
                    Merchant merchantInfo = new Merchant();

                    // the merchantidentifier token number is supplied to you by our Amazon Engagement Manager
                    merchantInfo.merchantIdentifier = store.MerchantToken;
                    merchantInfo.merchantName = store.MerchantName;

                    // just make the call, if no soap exception was raised, we're good
                    service.getLastNDocumentInfo(merchantInfo, "_GET_UNIT_METRICS_", 1);
                }
            }
            catch (Exception ex)
            {
                TranslateException(ex);

                // If it wasn't trasnlated, just rethrow it
                throw;
            }
        }

        /// <summary>
        /// Translate the given exception into an AmazonException if we know how to handle it. Otherwise, do nothing.
        /// </summary>
        private void TranslateException(Exception ex)
        {
            WebException webEx = ex as WebException;
            if (webEx != null)
            {
                string message = webEx.Message;
                if (webEx.Status == WebExceptionStatus.ProtocolError && message.Contains("Authorization Required"))
                {
                    message = "The username/password combination is invalid.";
                }

                throw new AmazonException(typeof(merchantinterfacedime), message);
            }

            SoapException soapEx = ex as SoapException;
            if (soapEx != null)
            {
                string message = soapEx.Message;

                if (message == "_UNRECOGNIZED_MERCHANT_")
                {
                    message = "Unrecognized Merchant";
                }

                throw new AmazonException(typeof(merchantinterfacedime), message);
            }

            if (WebHelper.IsWebException(ex))
            {
                throw new AmazonException(typeof(merchantinterfacedime), ex.Message);
            }
        }

        /// <summary>
        /// Creates the DIME-aware Amazon Web Client
        /// </summary>
        private SellerCentralDimeService CreateDimeService(string logName)
        {
            SellerCentralDimeService service = new SellerCentralDimeService(logName);
            service.Credentials = new NetworkCredential(store.SellerCentralUsername, SecureText.Decrypt(store.SellerCentralPassword, store.SellerCentralUsername));

            // set to https
            service.Url = service.Url.Replace("http:", "https:");

            return service;
        }

        /// <summary>
        /// Creates the web service proxy
        /// </summary>
        private merchantinterfacedime CreateService(string logName)
        {
            merchantinterfacedime service = new merchantinterfacedime(new ApiLogEntry(ApiLogSource.Amazon, logName));
            service.Credentials = new NetworkCredential(store.SellerCentralUsername, SecureText.Decrypt(store.SellerCentralPassword, store.SellerCentralUsername));

            // set to https
            service.Url = service.Url.Replace("http:", "https:");

            return service;
        }
    }
}
