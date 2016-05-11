using System;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.OrderMotion.Udi;
using System.IO;
using System.Xml;
using Interapptive.Shared.Net;
using System.Xml.XPath;
using ShipWorks.ApplicationCore.Logging;
using System.Globalization;
using Interapptive.Shared.Security;
using log4net;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Class for communicating with the OrderMotion web service
    /// </summary>
    public class OrderMotionWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(OrderMotionWebClient));

        // store we're working for
        OrderMotionStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionWebClient(OrderMotionStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Connects to OrderMotion to test configuraiton
        /// </summary>
        public void TestConnection()
        {
            // make an AccountStatus Request
            UdiRequest accountStatusRequest = CreateRequest(UdiRequestName.AccountStatus, UdiRequest.DefaultVersion);

            ProcessRequest(accountStatusRequest);
        }

        /// <summary>
        /// Creates and configures a request
        /// </summary>
        private UdiRequest CreateRequest(UdiRequestName requestName, string version)
        {
            UdiRequest request = new UdiRequest(requestName, version);

            // add the biz Id
            string bizID = SecureText.Decrypt(store.OrderMotionBizID, "HttpBizID");
            request.Parameters.Add("HTTPBizID", bizID);

            return request;
        }

        /// <summary>
        /// Processes an ordermotion UDI Request
        /// </summary>
        private IXPathNavigable ProcessRequest(UdiRequest request)
        {
            StringWriter stringWriter = new StringWriter();

            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            request.WriteRequest(writer);

            string requestXml = stringWriter.ToString();
            requestXml = requestXml.Replace("utf-16", "UTF-8");

            byte[] requestBytes = Encoding.UTF8.GetBytes(requestXml);

            HttpBinaryPostRequestSubmitter submitter = new HttpBinaryPostRequestSubmitter(requestBytes, "text/xml; charset=utf-8");
            submitter.Uri = request.Uri;

            // log the request
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.OrderMotion, request.Name);
            logEntry.LogRequest(submitter);

            try
            {
                IHttpResponseReader responseReader = submitter.GetResponse();

                string response = responseReader.ReadResult();

                // log the response
                logEntry.LogResponse(response);

                if (response.Length == 0)
                {
                    throw new OrderMotionException("OrderMotion responded with a blank response.");
                }

                response = XmlUtility.StripInvalidXmlCharacters(response);

                try
                {
                    XmlDocument xmlResponse = new XmlDocument();
                    xmlResponse.LoadXml(response);

                    // check for errors
                    ValidateResponse(xmlResponse);

                    // everything is OK, hand the response xml off to the caller
                    return xmlResponse.CreateNavigator();
                }
                catch (XmlException)
                {
                    throw new OrderMotionException("OrderMotion responded with an invalid response.");
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(OrderMotionException));
            }
        }

        /// <summary>
        /// Locates error conditions in the OrderMotion reponse
        /// </summary>
        private void ValidateResponse(XmlDocument xmlResponse)
        {
            XmlNode successNode = xmlResponse.SelectSingleNode("//Success");
            if (successNode == null)
            {
                // not all requests follow the documentation!
                XmlNode errorDataNode = xmlResponse.SelectSingleNode("//ErrorData");
                if (errorDataNode == null)
                {
                    throw new OrderMotionException("OrderMotion returned a response ShipWorks could not understand.");
                }
                else
                {
                    if (errorDataNode.HasChildNodes)
                    {
                        XmlNode errorNode = errorDataNode.SelectSingleNode("Error");
                        if (errorNode == null)
                        {
                            throw new OrderMotionException("OrderMotion returned a response ShipWorks could not understand.");
                        }
                        else
                        {
                            throw new OrderMotionException(errorNode.InnerText);
                        }
                    }
                }
            }
            else if (String.Compare(successNode.InnerText.Trim(), "1", true, CultureInfo.InvariantCulture) != 0)
            {
                // pull out the error
                XmlNode errorNode = xmlResponse.SelectSingleNode("//ErrorData/Error");
                if (errorNode == null)
                {
                    throw new OrderMotionException("OrderMotion returned a response ShipWorks could not understand.");
                }
                else
                {
                    throw new OrderMotionException(errorNode.InnerText);
                }
            }
        }

        /// <summary>
        /// Retrieves an OrderMotion order from the UDI web service
        /// </summary>
        public IXPathNavigable GetOrder(long orderNumber)
        {
            // make an AccountStatus Request
            UdiRequest orderInformationRequest = CreateRequest(UdiRequestName.OrderInformation, UdiRequest.DefaultVersion);
            orderInformationRequest.Parameters.Add("Level", 2.ToString());
            orderInformationRequest.Parameters.Add("OrderNumber", orderNumber.ToString());

            return ProcessRequest(orderInformationRequest);
        }

        /// <summary>
        /// Makes an ItemInformationRequest to OrderMotion
        /// </summary>
        public IXPathNavigable GetItemInformation(string itemCode)
        {
            // make an AccountStatus Request
            UdiRequest itemInformationRequest = CreateRequest(UdiRequestName.ItemInformation, UdiRequest.DefaultVersion);
            itemInformationRequest.Parameters.Add("ItemCode", itemCode);

            return ProcessRequest(itemInformationRequest);
        }

        /// <summary>
        /// Upload tracking information to OrderMotion
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment)
        {
            OrderMotionOrderEntity order = shipment.Order as OrderMotionOrderEntity;

            string shipmentNumber = String.Format("{0}-{1}", order.OrderNumber, order.OrderMotionShipmentID);
            string trackingNumber = shipment.TrackingNumber;

            // Adjust tracking details per Mail Innovations and others
            WorldShipUtility.DetermineAlternateTracking(shipment, (track, service) =>
                {
                    if (track.Length > 0)
                    {
                        trackingNumber = track;
                    }
                });

            UdiRequest shipmentUpdateRequest = CreateRequest(UdiRequestName.ShipmentStatusUpdate, UdiRequest.DefaultVersion);
            shipmentUpdateRequest.WriteRequestDetails(w =>
            {
                w.WriteStartElement("ShipmentConfirmation");
                w.WriteAttributeString("type", "ShipmentNumber");
                w.WriteAttributeString("shipmentNumber", shipmentNumber);

                w.WriteElementString("Date", shipment.ShipDate.ToString("yyyy-MM-dd"));
                w.WriteElementString("TrackingNumber", trackingNumber);
                w.WriteElementString("OverwriteSHCode", "");

                w.WriteEndElement();
            });

            ProcessRequest(shipmentUpdateRequest);
        }
    }
}
