using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Common.IO.Hardware.Printers;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Fake FIMS web client for testing success/failure
    /// </summary>
    public class FimsWebClient : IFimsWebClient
    {
        private static readonly XNamespace fimsWebServiceNamespace = "http://www.fimsform.com";
        private static readonly Uri productionUri = new Uri("http://www.shipfims.com/pkgFedex3/pkgFormService");
        private static readonly XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
        private static readonly FedExShipmentTokenProcessor tokenProcessor = new FedExShipmentTokenProcessor();

        // FedEx - "labelSource for ShipWorks should be set to 5 always"
        private const string LabelSource = "5";
        private const string LabelSize = "6";
        private const string ResponseErrorMessage = "An error occurred processing the FedEx response.";
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private static ILog log;
        private IHttpRequestSubmitterFactory requestSubmitterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FimsWebClient(Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory, Func<Type, ILog> logFactory, IHttpRequestSubmitterFactory requestSubmitterFactory)
        {
            this.apiLogEntryFactory = apiLogEntryFactory;
            log = logFactory(typeof(FimsWebClient));
            this.requestSubmitterFactory = requestSubmitterFactory;
        }

        /// <summary>
        /// Ships a FIMS shipment.
        /// To simulate a successfully request, use "success" as the Username.
        /// For a failed request, send "failure" as the Username.
        /// </summary>
        public IFimsShipResponse Ship(IFimsShipRequest fimsShipRequest)
        {
            FimsShipResponse fimsShipResponse;

            if (fimsShipRequest == null)
            {
                throw new ArgumentNullException("fimsShipRequest");
            }

            XElement fimsRequestXml = BuildLabelRequest(fimsShipRequest);

            string soapRequestText = BuildSoapRequest(fimsRequestXml).ToString();

            string response = Submit(soapRequestText);

            return ProcessResponse(response);
        }

        /// <summary>
        /// Build the label request to send.
        /// </summary>
        private static XElement BuildLabelRequest(IFimsShipRequest fimsShipRequest)
        {
            ShipmentEntity shipment = fimsShipRequest.Shipment;
            FedExShipmentEntity fedExShipment = fimsShipRequest.Shipment.FedEx;

            string responseFormat = shipment.RequestedLabelFormat == (int) ThermalLanguage.ZPL ? "Z" : "I";
            string labelType = GetLabelType(shipment);

            string declaration = DetermineDeclaration(fedExShipment);

            fedExShipment.ReferenceFIMS = tokenProcessor.ProcessTokens(fedExShipment.ReferenceFIMS, shipment);

            XElement fimsRequestXml =
                new XElement("labelRequest",
                    new XElement("custCode", fimsShipRequest.Username),
                    new XElement("serviceId", fimsShipRequest.Password),
                    new XElement("labelSource", LabelSource),
                    new XElement("responseFormat", responseFormat),
                    new XElement("labelSize", LabelSize),
                    new XElement("shipperReference", fedExShipment.ReferenceFIMS),
                    new XElement("labelType", labelType),
                    new XElement("declaration", declaration),
                    new XElement("pkgWeight", shipment.TotalWeight),
                    new XElement("pkgLength", fedExShipment.Packages.FirstOrDefault()?.DimsLength),
                    new XElement("pkgWidth", fedExShipment.Packages.FirstOrDefault()?.DimsWidth),
                    new XElement("pkgHeight", fedExShipment.Packages.FirstOrDefault()?.DimsHeight),
                    new XElement("shipper",
                        new XElement("name", new PersonName(shipment.OriginFirstName, shipment.OriginMiddleName, shipment.OriginLastName)),
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
                        new XElement("name", new PersonName(shipment.ShipFirstName, shipment.ShipMiddleName, shipment.ShipLastName)),
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

            if (!string.IsNullOrEmpty(shipment.FedEx.FimsAirWaybill))
            {
                fimsRequestXml.Add(new XElement("airWaybill", shipment.FedEx.FimsAirWaybill));
            }

            return fimsRequestXml;
        }

        /// <summary>
        /// Encapsulates the logic to get the correct label type.
        /// </summary>
        private static string GetLabelType(ShipmentEntity shipment)
        {
            switch ((FedExServiceType) shipment.FedEx.Service)
            {
                case FedExServiceType.FedExFimsMailView:
                    return GetLabelTypeHelper(shipment, "41", "42");
                case FedExServiceType.FedExFimsMailViewLite:
                    return GetLabelTypeHelper(shipment, "51", "22");
                case FedExServiceType.FedExFimsStandard:
                    return GetLabelTypeHelper(shipment, "31", "22");
                case FedExServiceType.FedExFimsPremium:
                    return GetLabelTypeHelper(shipment, "21", "22");
                default:
                    throw new FedExException($"Invalid service {shipment.FedEx.Service} sent to FimsWebClient.");
            }
        }

        private static string GetLabelTypeHelper(ShipmentEntity shipment, string labelTypeBelowWeightOrValue, string labelTypeAboveWeightOrValue)
        {
            if (shipment.TotalWeight < 4.4 && shipment.CustomsValue < 400)
            {
                return labelTypeBelowWeightOrValue;
            }
            else
            {
                return labelTypeAboveWeightOrValue;
            }
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
        private string Submit(string soapRequest)
        {
            HttpRequestSubmitter requestSubmitter = requestSubmitterFactory.GetHttpTextPostRequestSubmitter(soapRequest, "");
            requestSubmitter.Uri = productionUri;

            IApiLogEntry logger = apiLogEntryFactory(ApiLogSource.FedExFims, "Ship");
            logger.LogRequest(requestSubmitter);

            try
            {
                using (IHttpResponseReader reader = requestSubmitter.GetResponse())
                {
                    string responseText = reader.ReadResult();
                    logger.LogResponse(responseText, "xml");
                    return responseText;
                }                
            }
            catch (WebException ex)
            {
                logger.LogResponse(ex);
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
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
        private static FimsShipResponse ProcessResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                throw new FedExException("FedEx FIMS shipment failed to return a response or label.");
            }

            byte[] responseBytes = Encoding.Default.GetBytes(response); 

            // Define the byte arrays of the soap xml start/stop.
            byte[] xmlResponseStartBytes = Encoding.Default.GetBytes("<SOAP-ENV:Envelope");
            byte[] xmlResponseEndBytes = Encoding.Default.GetBytes("</SOAP-ENV:Envelope");

            // Find the start and end of the soap xml response.
            int xmlResponseStart = ArrayUtility.IndexOfSequence(responseBytes, xmlResponseStartBytes, 0).LastOrDefault();
            int xmlResponseEnd = ArrayUtility.IndexOfSequence(responseBytes, xmlResponseEndBytes, 0).LastOrDefault() + xmlResponseEndBytes.Length + 1;

            // Get the soap xml response from the byte array
            byte[] xmlResponseBytes = ArrayUtility.Slice(responseBytes, xmlResponseStart, xmlResponseEnd);
            string xmlResponseText = Encoding.Default.GetString(xmlResponseBytes);
            XElement xmlResponse = XElement.Parse(xmlResponseText);

            CheckResponseForErrors(xmlResponse);

            byte[] label = Convert.FromBase64String(GetLabel(xmlResponse));

            string parcelID = GetParcelID(xmlResponse);
            string trackingNumber = GetTrackingNumber(xmlResponse);
            string responseCode = GetResponseCode(xmlResponse);
            string labelFormat = GetResponseFormat(xmlResponse);

            // Construct the ship response to return.
            FimsShipResponse fimsShipResponse = new FimsShipResponse(parcelID, responseCode)
            {
                LabelData = label,
                LabelFormat = labelFormat,
                TrackingNumber = trackingNumber
            };

            return fimsShipResponse;
        }

        /// <summary>
        /// Gets the response code.
        /// </summary>
        private static string GetResponseCode(XElement xmlResponse)
        {
            XElement responseElement = xmlResponse.Descendants(fimsWebServiceNamespace + "responseCode").FirstOrDefault();
            if (responseElement == null)
            {
                log.Error("FedEx FIMS did not return a Response Code.");
                throw new FedExException(ResponseErrorMessage);
            }

            return responseElement.Value;
        }

        /// <summary>
        /// Gets the parcel id.
        /// </summary>
        private static string GetParcelID(XElement xmlResponse)
        {
            XElement responseElement = xmlResponse.Descendants(fimsWebServiceNamespace + "parcelId").FirstOrDefault();
            if (responseElement == null)
            {
                log.Error("FedEx FIMS did not return a Parcel ID.");
                throw new FedExException(ResponseErrorMessage);
            }

            return responseElement.Value;
        }

        /// <summary>
        /// Gets the Tracking Number
        /// </summary>
        private static string GetTrackingNumber(XElement xmlResponse)
        {
            XElement responseElement = xmlResponse.Descendants(fimsWebServiceNamespace + "trackingNo").FirstOrDefault();
            if (responseElement == null)
            {
                log.Error("FedEx FIMS did not return a tracking number.");
                throw new FedExException(ResponseErrorMessage);
            }

            return responseElement.Value;
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        private static string GetLabel(XElement xmlResponse)
        {
            XElement responseElement = xmlResponse.Descendants(fimsWebServiceNamespace + "attached_label").FirstOrDefault();
            if (responseElement == null)
            {
                log.Error("FedEx FIMS did not return a label.");
                throw new FedExException(ResponseErrorMessage);
            }

            return responseElement.Value;
        }

        /// <summary>
        /// Gets the response format
        /// </summary>
        private static string GetResponseFormat(XElement xmlResponse)
        {
            XElement responseElement = xmlResponse.Descendants(fimsWebServiceNamespace + "responseFormat").FirstOrDefault();
            if (responseElement == null)
            {
                log.Error("FedEx FIMS did not return a Response Format.");
                throw new FedExException(ResponseErrorMessage);
            }

            return responseElement.Value.Equals("z", StringComparison.InvariantCultureIgnoreCase) ? "Z" : "I";
        }

        /// <summary>
        /// Check response for errors and throw if there are any.
        /// </summary>
        /// <param name="xmlResponse">The XML response.</param>
        /// <exception cref="FedExException"></exception>
        private static void CheckResponseForErrors(XElement xmlResponse)
        {
            List<XElement> errorElements = xmlResponse.Descendants(fimsWebServiceNamespace + "error").ToList();
            if (errorElements.Any())
            {
                string errorMessage = string.Join<string>(System.Environment.NewLine, errorElements.Select(e => e.Value));

                throw new FedExException(errorMessage);
            }
        }
    }
}
