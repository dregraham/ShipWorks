using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Fake FIMS web client for testing success/failure
    /// </summary>
    public class FimsWebClient : IFimsWebClient
    {
        private static readonly XNamespace fimsWebServiceNamespace = "http://www.fimsform.com";
        private static readonly Uri productionUri = new Uri("http://www.fimsform.com/pkgFedex/pkgFormService");
        private static readonly XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

        /// <summary>
        /// Ships a FIMS shipment.  
        /// To simulate a successfully request, use "success" as the Username.
        /// For a failed request, send "failure" as the Username.
        /// </summary>
        public IFimsShipResponse Ship(IFimsShipRequest fimsShipRequest)
        {
            FimsShipResponse fimsShipResponse;

            try
            {
                if (fimsShipRequest == null)
                {
                    throw new ArgumentNullException("fimsShipRequest");
                }

                XElement fimsRequestXml = BuildLabelRequest(fimsShipRequest);

                string soapRequestText = BuildSoapRequest(fimsRequestXml).ToString();

                byte[] responseBytes = Submit(soapRequestText);

                fimsShipResponse = ProcessResponse(responseBytes);
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }

            return fimsShipResponse;
        }

        /// <summary>
        /// Build the label request to send.
        /// </summary>
        private static XElement BuildLabelRequest(IFimsShipRequest fimsShipRequest)
        {
            ShipmentEntity shipment = fimsShipRequest.Shipment;
            FedExShipmentEntity fedExShipment = fimsShipRequest.Shipment.FedEx;

            string labelType = shipment.TotalWeight < 4.4 && shipment.CustomsValue < 350 ? "R" : "P";

            string declaration = DetermineDeclaration(fedExShipment);

            XElement fimsRequestXml =
                new XElement("labelRequest",
                    new XElement("custCode", fimsShipRequest.Username),
                    new XElement("serviceId", fimsShipRequest.Password),
                    new XElement("shipperReference", fedExShipment.ReferenceCustomer),
                    new XElement("labelType", labelType),
                    new XElement("declaration", declaration),
                    new XElement("shipper",
                        new XElement("name", shipment.OriginFirstName + " " + shipment.OriginLastName),
                        new XElement("company", shipment.OriginCompany),
                        new XElement("address1", shipment.OriginStreet1),
                        new XElement("address2", shipment.OriginStreet2),
                        new XElement("city", shipment.OriginCity),
                        new XElement("state", shipment.OriginStateProvCode),
                        new XElement("zipCode", shipment.OriginPostalCode),
                        new XElement("country", shipment.OriginCountryCode),
                        new XElement("phone", shipment.OriginPhone),
                        new XElement("email", shipment.OriginEmail)),
                    new XElement("consignee",
                        new XElement("name", shipment.ShipFirstName + " " + shipment.ShipLastName),
                        new XElement("company", shipment.ShipCompany),
                        new XElement("address1", shipment.ShipStreet1),
                        new XElement("address2", shipment.ShipStreet2),
                        new XElement("city", shipment.ShipCity),
                        new XElement("state", shipment.ShipStateProvCode),
                        new XElement("zipCode", shipment.ShipPostalCode),
                        new XElement("country", shipment.ShipCountryCode),
                        new XElement("phone", shipment.ShipPhone),
                        new XElement("email", shipment.ShipEmail)),
                        BuildCustoms(shipment)
            );

            return fimsRequestXml;
        }

        /// <summary>
        /// Determine appropriate declaration to send.
        /// </summary>
        private static string DetermineDeclaration(FedExShipmentEntity fedExShipment)
        {
            string declaration = string.Empty;
            FedExCommercialInvoicePurpose fedExCommercialInvoicePurpose = (FedExCommercialInvoicePurpose) fedExShipment.CommercialInvoicePurpose;
            switch (fedExCommercialInvoicePurpose)
            {
                case FedExCommercialInvoicePurpose.Gift:
                    declaration = "G";
                    break;
                case FedExCommercialInvoicePurpose.Sample:
                    declaration = "S";
                    break;
                default:
                    declaration = "X";
                    break;
            }
            return declaration;
        }

        /// <summary>
        /// Create the list of customs items xml
        /// </summary>
        private static XElement BuildCustoms(ShipmentEntity shipment)
        {
            XElement commodities = new XElement("commodities",
                from ci in shipment.CustomsItems
                select new XElement("commodity",
                            new XElement("description", ci.Description),
                            new XElement("value", ci.UnitValue),
                            new XElement("currency", "USD"),
                            new XElement("weight", ci.Weight),
                            new XElement("tariffNo", ci.HarmonizedCode),
                            new XElement("originCountry", ci.CountryOfOrigin)
                            )
                       );

            return commodities;
        }
        
        /// <summary>
        /// Submit the data to FIMS to get the label.
        /// </summary>
        public static byte[] Submit(string soapRequest)
        {
            byte[] response;

            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.FedExFims, "Ship");
            logger.LogRequest(soapRequest, "xml");

            using (WebClient client = new WebClient())
            {
                byte[] data = Encoding.UTF8.GetBytes(soapRequest);

                response = client.UploadData(productionUri.AbsoluteUri, data);
            }

            string responseText = ASCIIEncoding.Default.GetString(response);
            logger.LogResponse(responseText, "txt");

            return response;
        }

        /// <summary>
        /// Build the soap request to send.
        /// </summary>
        /// <param name="body">The label request specific xml</param>
        private static XDocument BuildSoapRequest(XElement body)
        {
            XDocument soapRequest = new XDocument(
               new XDeclaration("1.0", "utf-8", String.Empty),
                new XElement(soapenv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "SOAP-ENV", soapenv),
                    new XElement(soapenv + "Header"),
                    new XElement(soapenv + "Body",
                        body
                        )
                    )
                );

            return soapRequest;
        }

        /// <summary>
        /// Process the response by array
        /// </summary>
        private static FimsShipResponse ProcessResponse(byte[] responseBytes)
        {
            if (responseBytes == null)
            {
                throw new FedExException("FedEx FIMS shipment failed to return a response or label.");
            }

            // Define the byte arrays of the soap xml start/stop.
            byte[] xmlResponseStartBytes = ASCIIEncoding.Default.GetBytes("<SOAP-ENV:Envelope");
            byte[] xmlResponseEndBytes = ASCIIEncoding.Default.GetBytes("</SOAP-ENV:Envelope");
            byte[] bEOF = ASCIIEncoding.Default.GetBytes("%%EOF"); 
            byte[] bPDF = ASCIIEncoding.Default.GetBytes("%PDF");

            // Find the start and end of the soap xml response.
            int xmlResponseStart = ArrayUtility.IndexOfSequence(responseBytes, xmlResponseStartBytes, 0).LastOrDefault();
            int xmlResponseEnd = ArrayUtility.IndexOfSequence(responseBytes, xmlResponseEndBytes, 0).LastOrDefault() + xmlResponseEndBytes.Length + 1;

            // Get the soap xml response from the byte array
            byte[] xmlResponseBytes = ArrayUtility.Slice(responseBytes, xmlResponseStart, xmlResponseEnd);
            string xmlResponseText = ASCIIEncoding.Default.GetString(xmlResponseBytes);
            XElement xmlResponse = XElement.Parse(xmlResponseText);

            // Check for errors and throw if there are any.
            List<XElement> errorElements = xmlResponse.Descendants(fimsWebServiceNamespace + "error").ToList();
            if (errorElements.Any())
            {
                string errorMessage = string.Join<string>(System.Environment.NewLine, errorElements.Select(e => e.Value));

                throw new FedExException(errorMessage);
            }

            // Get the PDF 
            List<int> pdStartPOS = ArrayUtility.IndexOfSequence(responseBytes, bPDF, 0);
            List<int> pdfEndPOS = ArrayUtility.IndexOfSequence(responseBytes, bEOF, 0);
            byte[] pdf = ArrayUtility.Slice(responseBytes, pdStartPOS[0], pdfEndPOS[0] + bEOF.Length);
            
            string parcelID = string.Empty;
            string responseCode = string.Empty;

            XElement responseElement = xmlResponse.Descendants(fimsWebServiceNamespace + "parcelId").FirstOrDefault();
            if (responseElement != null)
            {
                parcelID = responseElement.Value;
            }
            else
            {
                throw new FedExException("FedEx FIMS did not return a ParcelID");
            }

            responseElement = xmlResponse.Descendants(fimsWebServiceNamespace + "responseCode").FirstOrDefault();
            if (responseElement != null)
            {
                responseCode = responseElement.Value;
            }
            else
            {
                throw new FedExException("FedEx FIMS did not return a Response Code");
            }

            // Construct the ship response to return.
            FimsShipResponse fimsShipResponse = new FimsShipResponse(parcelID, responseCode)
            {
                LabelPdfData = pdf
            };

            return fimsShipResponse;
        }
    }
}
