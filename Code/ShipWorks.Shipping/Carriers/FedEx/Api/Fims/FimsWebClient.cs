using System;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Fake FIMS web client for testing success/failure
    /// </summary>
    [Component]
    public class FimsWebClient : IFimsWebClient
    {
        private static readonly Uri productionUri = new Uri("http://www.shipfims.com/pkgFedex3/pkgFormService");
        private static readonly XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
        private static readonly FedExShipmentTokenProcessor tokenProcessor = new FedExShipmentTokenProcessor();

        // FedEx - "labelSource for ShipWorks should be set to 5 always"
        private const string LabelSource = "5";
        private const string LabelSize = "6";
        private const string ResponseErrorMessage = "An error occurred processing the FedEx response.";
        private readonly ILogEntryFactory apiLogEntryFactory;
        private readonly ILog log;
        private readonly IHttpRequestSubmitterFactory requestSubmitterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FimsWebClient(
            ILogEntryFactory apiLogEntryFactory,
            Func<Type, ILog> logFactory,
            IHttpRequestSubmitterFactory requestSubmitterFactory)
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
                    return CanUseLightWeightService(shipment) ? "41" : "42";
                case FedExServiceType.FedExFimsMailViewLite:
                    return CanUseLightWeightService(shipment) ? "51" : "22";
                case FedExServiceType.FedExFimsStandard:
                    return CanUseLightWeightService(shipment) ? "31" : "22";
                case FedExServiceType.FedExFimsPremium:
                    return CanUseLightWeightService(shipment) ? "21" : "22";
                default:
                    throw new FedExException($"Invalid service {shipment.FedEx.Service} sent to FimsWebClient.");
            }
        }

        /// <summary>
        /// Fims uses a different service if package is above 4.4 lbs or valued at over 400lbs
        /// </summary>
        private static bool CanUseLightWeightService(ShipmentEntity shipment)
        {
            return (shipment.TotalWeight < 4.4 && shipment.CustomsValue < 400);
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
            IHttpRequestSubmitter requestSubmitter = requestSubmitterFactory.GetHttpTextPostRequestSubmitter(soapRequest, "");
            requestSubmitter.Uri = productionUri;

            IApiLogEntry logger = apiLogEntryFactory.GetLogEntry(ApiLogSource.FedExFims, "Ship", LogActionType.Other);
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
        private FimsShipResponse ProcessResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                throw new FedExException("FedEx FIMS shipment failed to return a response or label.");
            }

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(response);

            CheckResponseForErrors(xmlResponse);

            string parcelID = GetValueFromResponse(xmlResponse, "parcelId");
            string trackingNumber = GetValueFromResponse(xmlResponse, "trackingNo");
            string labelFormat = GetValueFromResponse(xmlResponse, "responseFormat");
            string label = GetValueFromResponse(xmlResponse, "attached_label");

            byte[] labelData = Convert.FromBase64String(label);

            // Construct the ship response to return.
            FimsShipResponse fimsShipResponse = new FimsShipResponse(parcelID, labelData, labelFormat, trackingNumber);

            return fimsShipResponse;
        }

        /// <summary>
        /// Parses XML and returns response. 
        /// </summary>
        /// <exception cref="FedExException">Response missing element.</exception>
        private string GetValueFromResponse(XmlDocument xmlResponse, string elementName)
        {
            XmlNode responseElement = xmlResponse.SelectNodes($"//*[local-name()='{elementName}']")?.Cast<XmlNode>()?.FirstOrDefault();
            if (responseElement == null)
            {
                log.Error($"FedEx FIMS ship response missing {elementName}");
                throw new FedExException(ResponseErrorMessage);
            }

            return responseElement.InnerText;
        }

        /// <summary>
        /// Check response for errors and throw if there are any.
        /// </summary>
        /// <param name="xmlResponse">The XML response.</param>
        /// <exception cref="FedExException"></exception>
        private static void CheckResponseForErrors(XmlDocument xmlResponse)
        {
            XmlNodeList errorElements = xmlResponse.SelectNodes("//*[local-name()='error']");
            if ((errorElements?.Count ?? 0) > 0)
            {
                string errorMessage = string.Join<string>(System.Environment.NewLine, errorElements.Cast<XmlNode>().Select(e => e.InnerText));

                throw new FedExException(errorMessage);
            }
        }
    }
}
