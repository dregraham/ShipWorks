using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using System.Xml;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Net;
using System.Net;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using Interapptive.Shared;
using ShipWorks.Shipping.Api;
using log4net;
using ShipWorks.Data;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Entry point to the UPS API
    /// </summary>
    public static class UpsWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(UpsWebClient));

        private const string testHost = "https://wwwcie.ups.com/ups.app/xml/";
        private const string liveHost = "https://onlinetools.ups.com/ups.app/xml/";

        /// <summary>
        /// Holds all of the properties needed to build the basic XML structure
        /// and HTTP requests for the tools.
        /// </summary>
        class OnLineToolInfo
        {
            public OnLineToolInfo(UpsOnLineToolType toolType, string httpUrlPostfix, string xmlRootNode, string requestAction, string requestOption)
            {
                ToolType = toolType;
                HttpUrlPostfix = httpUrlPostfix;
                XmlRootNode = xmlRootNode;
                RequestAction = requestAction;
                RequestOption = requestOption;
            }

            public UpsOnLineToolType ToolType { get; set; }
            public string HttpUrlPostfix { get; set; }
            public string XmlRootNode { get; set; }
            public string RequestAction { get; set; }
            public string RequestOption { get; set; }
        }

        // Maps the online tool types to their information structions
        static Dictionary<UpsOnLineToolType, OnLineToolInfo> toolInfoMap = new Dictionary<UpsOnLineToolType, OnLineToolInfo>();

        /// <summary>
        /// Static constructor
        /// </summary>
        static UpsWebClient()
        {
            // License Agreement
            toolInfoMap.Add(UpsOnLineToolType.LicenseAgreement,
                new OnLineToolInfo(
                    UpsOnLineToolType.LicenseAgreement,
                    "License",
                    "AccessLicenseAgreementRequest",
                    "AccessLicense",
                    "AllTools"));

            // Access Key
            toolInfoMap.Add(UpsOnLineToolType.AccessKey,
                new OnLineToolInfo(
                    UpsOnLineToolType.AccessKey,
                    "License",
                    "AccessLicenseRequest",
                    "AccessLicense",
                    "AllTools"));

            // UserId \ Password registration
            toolInfoMap.Add(UpsOnLineToolType.Register,
                new OnLineToolInfo(
                    UpsOnLineToolType.Register,
                    "Register",
                    "RegistrationRequest",
                    "Register",
                    "none"));

            // Rates & Service Selection
            toolInfoMap.Add(UpsOnLineToolType.Rate,
                new OnLineToolInfo(
                    UpsOnLineToolType.Rate,
                    "Rate",
                    "RatingServiceSelectionRequest",
                    "Rate",
                    "shop"));

            // SurePost Rates & Service Selection
            toolInfoMap.Add(UpsOnLineToolType.SurePostRate,
                new OnLineToolInfo(
                    UpsOnLineToolType.Rate,
                    "Rate",
                    "RatingServiceSelectionRequest",
                    "Rate",
                    "Rate"));

            // Time in Transit
            toolInfoMap.Add(UpsOnLineToolType.TimeInTransit,
                new OnLineToolInfo(
                    UpsOnLineToolType.TimeInTransit,
                    "TimeInTransit",
                    "TimeInTransitRequest",
                    "TimeInTransit",
                    ""));

            // Shipment Phase 1 (ShipConfirm)
            toolInfoMap.Add(UpsOnLineToolType.ShipConfirm,
                new OnLineToolInfo(
                    UpsOnLineToolType.ShipConfirm,
                    "ShipConfirm",
                    "ShipmentConfirmRequest",
                    "ShipConfirm",
                    "validate"));

            // Shipment Phase 2 (ShipAccept)
            toolInfoMap.Add(UpsOnLineToolType.ShipAccept,
                new OnLineToolInfo(
                    UpsOnLineToolType.ShipAccept,
                    "ShipAccept",
                    "ShipmentAcceptRequest",
                    "ShipAccept",
                    ""));

            // Shipment Void
            toolInfoMap.Add(UpsOnLineToolType.ShipVoid,
                new OnLineToolInfo(
                    UpsOnLineToolType.ShipVoid,
                    "Void",
                    "VoidShipmentRequest",
                    "Void",
                    "1"));

            // Tracking
            toolInfoMap.Add(UpsOnLineToolType.Track,
                new OnLineToolInfo(
                    UpsOnLineToolType.Track,
                    "Track",
                    "TrackRequest",
                    "Track",
                    "activity"));
        }

        /// <summary>
        /// Indicates if the test server should be used instead of hte live server
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("UpsOltTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("UpsOltTestServer", value); }
        }

        /// <summary>
        /// Create a request instance for the specified tool type and shipper
        /// </summary>
        public static XmlTextWriter CreateRequest(UpsOnLineToolType toolType, UpsAccountEntity account)
        {
            return CreateRequest(toolType, account, new UpsSettingsRepository());
        }

        /// <summary>
        /// Create a request instance for the specified tool type and shipper
        /// </summary>
        public static XmlTextWriter CreateRequest(UpsOnLineToolType toolType, UpsAccountEntity account, ICarrierSettingsRepository settingsRepository)
        {
            StreamWriter writer = new StreamWriter(new MemoryStream(), StringUtility.Iso8859Encoding);
            XmlTextWriter xmlWriter = new XmlTextWriter(writer);
            xmlWriter.Formatting = Formatting.Indented;

            // Get the tool
            OnLineToolInfo tooInfo = toolInfoMap[toolType];

            // Insert the <AccessRequest> where required
            if (!IsSecurityElementTool(toolType))
            {
                AppendAccessRequest(xmlWriter, account, settingsRepository);
            }
            
            // Open
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement(tooInfo.XmlRootNode);

            // <Request> block
            xmlWriter.WriteStartElement("Request");
            xmlWriter.WriteStartElement("TransactionReference");
            xmlWriter.WriteElementString("CustomerContext", "ShipWorks");
            xmlWriter.WriteElementString("XpciVersion", "1.0001");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteElementString("RequestAction", tooInfo.RequestAction);
            xmlWriter.WriteElementString("RequestOption", tooInfo.RequestOption);
            xmlWriter.WriteEndElement();

            // Tools which require the developer ID
            if (toolType == UpsOnLineToolType.AccessKey ||
                toolType == UpsOnLineToolType.LicenseAgreement)
            {
                xmlWriter.WriteElementString("DeveloperLicenseNumber", UpsApiCore.DeveloperLicenseNumber);
            }

            return xmlWriter;
        }

        /// <summary>
        /// Process the given request and return the response
        /// </summary>
        /// <exception cref="UpsApiException">
        /// UPS does not have a record for this shipment, and therefore cannot void the shipment.
        /// or
        /// </exception>
        public static XmlDocument ProcessRequest(XmlTextWriter xmlWriter)
        {
            return ProcessRequest(xmlWriter, LogActionType.Other, new TrustingCertificateInspector());
        }

        /// <summary>
        /// Process the given request and return the response
        /// </summary>
        /// <exception cref="UpsApiException">
        /// UPS does not have a record for this shipment, and therefore cannot void the shipment.
        /// or
        /// </exception>
        public static XmlDocument ProcessRequest(XmlTextWriter xmlWriter, ICertificateInspector certificateInspector)
        {
            return ProcessRequest(xmlWriter, LogActionType.Other, certificateInspector);
        }

        /// <summary>
        /// Process the given request and return the response
        /// </summary>
        /// <exception cref="UpsApiException">
        /// UPS does not have a record for this shipment, and therefore cannot void the shipment.
        /// or
        /// </exception>
        [NDependIgnoreLongMethod]
        public static XmlDocument ProcessRequest(XmlTextWriter xmlWriter, LogActionType logActionType, ICertificateInspector certificateInspector)
        {
            // Close out the XML
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();

            // We create it as a memory stream, so we know that's what it is
            MemoryStream stream = (MemoryStream) xmlWriter.BaseStream;

            // We need it as a string so we can log it
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream, StringUtility.Iso8859Encoding);
            string requestXml = reader.ReadToEnd();

            // Determine what UPS OnLine tool it is be examining the content
            OnLineToolInfo toolInfo = DetermineOnLineTool(requestXml);

            // Log the request
            IApiLogEntry logger = (new LogEntryFactory()).GetLogEntry(ApiLogSource.UPS, toolInfo.HttpUrlPostfix, logActionType);

            logger.LogRequest(requestXml);

            string toolUrl;

            // Security Elements (License & Reg) always hit the live server
            if (IsSecurityElementTool(toolInfo.ToolType))
            {
                toolUrl = liveHost + toolInfo.HttpUrlPostfix;
            }

            // All other tools we query the preferences
            else
            {
                toolUrl = ((UseTestServer) ? testHost : liveHost) + toolInfo.HttpUrlPostfix;
            }

            HttpBinaryPostRequestSubmitter request = new HttpBinaryPostRequestSubmitter(stream.ToArray());
            request.Uri = new Uri(toolUrl);

            CertificateRequest certificateRequest = new CertificateRequest(request.Uri, certificateInspector);
            CertificateSecurityLevel certificateSecurityLevel = certificateRequest.Submit();

            if (certificateSecurityLevel != CertificateSecurityLevel.Trusted)
            {
                throw new UpsException("ShipWorks is unable to make a secure connection to UPS.");
            }

            try
            {
                using (IHttpResponseReader response = request.GetResponse())
                {
                    string responseXml = response.ReadResult(StringUtility.Iso8859Encoding);

                    // Log the response
                    logger.LogResponse(responseXml);

                    // Load the response and return it
                    XmlDocument xmlResponse = new XmlDocument();
                    xmlResponse.LoadXml(responseXml);

                    XPathNavigator xpath = xmlResponse.CreateNavigator();

                    UpsApiResponseStatus status = GetResponseStatus(xpath);
                    if (status != UpsApiResponseStatus.Success)
                    {
                        // Extract the error code
                        int errorCode = XPathUtility.Evaluate(xpath, "//Response/Error/ErrorCode", 0);

                        // Extract the offending request XML element that may be the cause 
                        string errorLocation = XPathUtility.Evaluate(xpath,
                            "//Response/Error/ErrorLocation/ErrorLocationElementName", "");

                        // Extract the error description (may not be there)
                        string errorDescription = XPathUtility.Evaluate(xpath, "//Response/Error/ErrorDescription", "");

                        // Remove extra white spaces
                        errorDescription = Regex.Replace(errorDescription, @"\s+", " ");

                        if (status == UpsApiResponseStatus.Warning)
                        {
                            log.WarnFormat("UPS retured an API warning. {0}", errorDescription);
                        }
                        else if (errorCode == UpsApiConstants.ErrorNoShipmentFoundForVoid)
                        {
                            throw new UpsApiException(status, errorCode.ToString(),
                                "UPS does not have a record for this shipment, and therefore cannot void the shipment.",
                                errorLocation);
                        }
                        else
                        {
                            throw new UpsApiException(status, errorCode.ToString(), errorDescription, errorLocation);
                        }
                    }

                    return xmlResponse;
                }
            }
            catch (XmlException ex)
            {
                throw new UpsException("UPS did not provide valid rates for this shipment.", ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UpsException));
            }
        }
        
        /// <summary>
        /// Determine the online tool based on the request data
        /// </summary>
        private static OnLineToolInfo DetermineOnLineTool(string requestXml)
        {
            foreach (OnLineToolInfo toolInfo in toolInfoMap.Values)
            {
                if (requestXml.Contains(toolInfo.XmlRootNode))
                {
                    return toolInfo;
                }
            }

            throw new InvalidOperationException("Coult not determine tool.");
        }

        /// <summary>
        /// Extract the response status from the given xpath of the UPS response
        /// </summary>
        private static UpsApiResponseStatus GetResponseStatus(XPathNavigator xpath)
        {
            int result = Convert.ToInt32(xpath.Evaluate("number(//Response/ResponseStatusCode)"));

            // Success
            if (result != 0)
            {
                return UpsApiResponseStatus.Success;
            }

            // If it was not a success
            else
            {
                string errorLevel = (string) xpath.Evaluate("string(//Response/Error/ErrorSeverity)");

                if (errorLevel == "Success") return UpsApiResponseStatus.Success;
                if (errorLevel == "Transient") return UpsApiResponseStatus.Transient;
                if (errorLevel == "Hard") return UpsApiResponseStatus.Hard;
                if (errorLevel == "Warning") return UpsApiResponseStatus.Warning;

                throw new InvalidOperationException("Unhandled UPS response severity: " + errorLevel);
            }
        }

        /// <summary>
        /// Indicates if the tool is apart of the Security Elements suite
        /// </summary>
        /// <returns>True if toolType is a security element tool.</returns>
        private static bool IsSecurityElementTool(UpsOnLineToolType toolType)
        {
            return
                (toolType == UpsOnLineToolType.LicenseAgreement ||
                 toolType == UpsOnLineToolType.AccessKey ||
                 toolType == UpsOnLineToolType.Register);
        }

        /// <summary>
        /// Append the AccessRequest block to the XML
        /// </summary>
        private static void AppendAccessRequest(XmlTextWriter xmlWriter, UpsAccountEntity shipper, ICarrierSettingsRepository upsSettingsRepository)
        {
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("AccessRequest");

            xmlWriter.WriteElementString("AccessLicenseNumber", SecureText.Decrypt(upsSettingsRepository.GetShippingSettings().UpsAccessKey, "UPS"));
            xmlWriter.WriteElementString("UserId", shipper.UserID);
            xmlWriter.WriteElementString("Password", shipper.Password);

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
        }

        /// <summary>
        /// Appends a the list of tools we license to the current XmlWriter
        /// </summary>
        public static void AppendToolList(XmlTextWriter xmlWriter)
        {
            AddToolToXml(xmlWriter, "TrackXML", "1.0");
            AddToolToXml(xmlWriter, "TimeNTransitXML", "1.0");
            AddToolToXml(xmlWriter, "RateXML", "1.0");
            AddToolToXml(xmlWriter, "ShipXML", "1.0");
        }

        /// <summary>
        /// Adds an OnLineTool to the list of tools being requested
        /// </summary>
        private static void AddToolToXml(XmlTextWriter xmlWriter, string toolID, string toolVersion)
        {
            xmlWriter.WriteStartElement("OnLineTool");
            xmlWriter.WriteElementString("ToolID", toolID);
            xmlWriter.WriteElementString("ToolVersion", toolVersion);
            xmlWriter.WriteEndElement();
        }
    }
}
