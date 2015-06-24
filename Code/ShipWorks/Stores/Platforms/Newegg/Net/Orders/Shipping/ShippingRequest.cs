using System;
using System.Linq;
using System.Net;
using Interapptive.Shared.Net;
using System.Text;
using Interapptive.Shared.Utility;
using System.Xml;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping
{
    /// <summary>
    /// An implementation of IShippingRequest that hits the Newegg API.
    /// </summary>
    public class ShippingRequest : IShippingRequest
    {
        private const string RequestUrl = "{0}/ordermgmt/orderstatus/orders/{1}?sellerid={2}";
        
        private Credentials credentials;
        private INeweggRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingRequest"/> class. 
        /// A NeweggHttpRequest is used if no request type is given.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public ShippingRequest(Credentials credentials)
            : this(credentials, new NeweggHttpRequest())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingRequest"/> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="request">The request.</param>
        public ShippingRequest(Credentials credentials, INeweggRequest request)
        {
            this.credentials = credentials;
            this.request = request;
        }


        /// <summary>
        /// Ships the specified shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>
        /// A ShippingResult object containing the details of the response.
        /// </returns>
        public ShippingResult Ship(Shipment shipment)
        {
            if (shipment == null)
            {
                throw new InvalidOperationException("A null shipment value was provided.");
            }

            // API URL depends on which marketplace the seller selected 
            string marketplace = "";

            switch (credentials.Channel)
            {
                case NeweggChannelType.US:
                    break;
                case NeweggChannelType.Business:
                    marketplace = "/b2b";
                    break;
                case NeweggChannelType.Canada:
                    marketplace = "/can";
                    break;
                default:
                    break;
            }

            // Format our request URL with the value of the order number and seller ID and configure the request
            string formattedUrl = string.Format(RequestUrl, marketplace, shipment.Header.OrderNumber, credentials.SellerId);
            RequestConfiguration requestConfig = new RequestConfiguration("Uploading Shipment Details", formattedUrl)
            { 
                Method = HttpVerb.Put, 
                Body = GetRequestBody(shipment) 
            };

            // The shipping response data should contain the XML describing a ShippingResult
            string responseData = this.request.SubmitRequest(credentials, requestConfig);            
            NeweggResponse shippingResponse = new NeweggResponse(responseData, new ShippingResponseSerializer());

            if (shippingResponse.ResponseErrors.Count() > 0)
            {
                string errorMessage = string.Format("An error was encountered while shipping order number {0}.{1}", shipment.Header.OrderNumber, System.Environment.NewLine);
                foreach (Error error in shippingResponse.ResponseErrors)
                {
                    errorMessage += string.Format("{0} (Newegg error code {1})", error.Description, error.ErrorCode) + System.Environment.NewLine;
                }

                throw new NeweggException(errorMessage, shippingResponse);
            }

            // There weren't any errors, so the result in the response is a ShippingResult
            return shippingResponse.Result as ShippingResult;
        }


        /// <summary>
        /// Gets the body of the request to be sent to Newegg by serializing the 
        /// shipment object.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A string representing the body of the Newegg request.</returns>
        private static string GetRequestBody(Shipment shipment)
        {
            const int ShipActionValue = 2;
            string serializedShipmentXml = SerializationUtility.SerializeToXml(shipment);

            // The Newegg API requires that there are <Item> nodes within the <ItemList> node
            // when sending the request, but our shipment gets serialized into <ItemDes> nodes
            // since it was originally intended for deserializing the response from Newegg. We 
            // also need to remove the ProcessStatus node from the serialized shipment XML.
            
            // These are the only discrepancies between the request and response, so we're just going 
            // to do a string replacement on the <ItemDes> nodes and remove the ProcessStatus node. 
            // We may want to look at creating a new class that strictly pertains to sending the 
            // request, but it seemed like a lot of duplication of code at this time for something 
            // that is contained to this single spot.
            serializedShipmentXml = serializedShipmentXml.Replace("<ItemDes>", "<Item>");
            serializedShipmentXml = serializedShipmentXml.Replace("</ItemDes>", "</Item>");
            
            // Load the XML into a document to remove the unnecessary ProcessStatus node
            XmlDocument document = new XmlDocument();
            document.LoadXml(serializedShipmentXml);

            XmlNodeList packageNodes = document.SelectNodes("/Shipment/PackageList/Package");
            foreach (XmlNode node in packageNodes)
            {
                XmlNode nodeToRemove = node.SelectSingleNode("ProcessStatus");
                if (nodeToRemove != null)
                {
                    node.RemoveChild(nodeToRemove);
                }
            }

            string requestBody = string.Format(@"
                <UpdateOrderStatus>
                    <Action>{0}</Action>
                    <Value> <![CDATA[{1}]]> </Value>
                </UpdateOrderStatus>", ShipActionValue, document.OuterXml);

            return requestBody;
        }

    }
}
